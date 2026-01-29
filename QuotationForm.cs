using System;
using System.Collections.Generic;

using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using QuotationApp.Helpers;

namespace QuotationApp
{
    public partial class QuotationForm : Form
    {
        private static string GetSafeDateString(SqlDataReader rdr, int columnIndex)
        {
            if (rdr.IsDBNull(columnIndex))
                return "";

            var value = rdr.GetValue(columnIndex);

            if (value is DateTime dt)
                return dt.ToString("dd/MM/yyyy");

            return value.ToString();
        }

        /// <summary>
        /// SqlDataReader में मौजूद columnIndex पर date या string safe रूप में DateTime में बदलकर लौटाता है.
        /// </summary>
        private static DateTime GetSafeDateTime(SqlDataReader rdr, int columnIndex)
        {
            if (rdr.IsDBNull(columnIndex))
                return DateTime.Now;

            var value = rdr.GetValue(columnIndex);

            if (value is DateTime dt)
                return dt;

            if (DateTime.TryParse(value.ToString(), out DateTime parsed))
                return parsed;

            return DateTime.Now;
        }
        private int currentQuotationId = 0; // 0 = new
        public string OpenedFrom { get; set; } // "Home" or "Database"
        public bool IsAdmin { get; set; } = false; // ✅ Add admin property
        private Dictionary<string, string> customerMap = new Dictionary<string, string>();

        public QuotationForm()
        {
            InitializeComponent();
            dgvItems.CellEndEdit += DgvItems_CellEndEdit;
            dgvItems.RowsAdded += DgvItems_RowsAdded;
            dgvItems.EditingControlShowing += DgvItems_EditingControlShowing;

            this.Load += QuotationForm_Load;
            this.VisibleChanged += QuotationForm_VisibleChanged;
            SetupDataGridViewForMultiline();
            dgvItems.UserDeletingRow += DgvItems_UserDeletingRow;
            dgvItems.UserDeletedRow += DgvItems_UserDeletedRow;
            dgvItems.KeyDown += DgvItems_KeyDown;
            this.KeyPreview = true;

        }
        private void QuotationForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                ApplyAdminRestrictions();
            }
        }
        private void ApplyAdminRestrictions()
        {
            
            // Example: someAdminOnlyButton.Visible = IsAdmin;
        }
        private void QuotationForm_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if (currentQuotationId == 0)  // ✅ sirf new ke case me auto no
            {
                txtQuotationNo.Text = GetNextQuotationNo().ToString();
                dtpDate.Value = DateTime.Now;
                rtbTerms.Text = DatabaseHelper.GetSetting("DefaultTerms");
                txtGSTNo.Text = DatabaseHelper.GetSetting("DefaultGSTNo");
            }
            // agar edit case hai to LoadQuotation() already proper QuotationNo set karega

            UpdateTotals();
            LoadAutoCompleteSources();
        }

        private void LoadSettings()
        {
            string logo = DatabaseHelper.GetSetting("LogoPath");
            if (!string.IsNullOrWhiteSpace(logo) && File.Exists(logo))
            {
                using (var fs = new FileStream(logo, FileMode.Open, FileAccess.Read))
                {
                    picLogo.Image = Image.FromStream(fs);
                }
            }

            string sig = DatabaseHelper.GetSetting("SignaturePath");
            if (!string.IsNullOrWhiteSpace(sig) && File.Exists(sig))
            {
                using (var fs = new FileStream(sig, FileMode.Open, FileAccess.Read))
                {
                    picSignature.Image = Image.FromStream(fs);
                }
            }
        }

        private void LoadAutoCompleteSources()
        {
            var acNames = new AutoCompleteStringCollection();
            customerMap.Clear();

            using (var con = DatabaseHelper.GetConnection())
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT DISTINCT CustomerName, Address FROM Quotations";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string name = rdr.IsDBNull(0) ? "" : rdr.GetString(0);
                            string addr = rdr.IsDBNull(1) ? "" : rdr.GetString(1);

                            if (!string.IsNullOrWhiteSpace(name))
                            {
                                acNames.Add(name);

                                if (!customerMap.ContainsKey(name))
                                    customerMap[name] = addr;
                            }
                        }
                    }
                }
            }

            // autocomplete sirf customer name pe
            txtCustomer.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtCustomer.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtCustomer.AutoCompleteCustomSource = acNames;
        }

        private int GetNextQuotationNo()
        {
            using (var con = DatabaseHelper.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT ISNULL(MAX(QuotationNo),0)+1 FROM Quotations", con))
                {
                    var r = cmd.ExecuteScalar();
                    return Convert.ToInt32(r);
                }
            }
        }
        private void DgvItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A) // Change this to your desired key
            {
                e.Handled = true;

                int currentRowIndex = dgvItems.CurrentCell?.RowIndex ?? -1;

                if (currentRowIndex >= 0)
                {
                    // Insert a new row at the selected position
                    dgvItems.Rows.Insert(currentRowIndex, 1);

                    // Optional: set SrNo correctly
                    for (int i = 0; i < dgvItems.Rows.Count; i++)
                    {
                        if (!dgvItems.Rows[i].IsNewRow)
                            dgvItems.Rows[i].Cells["colSrNo"].Value = i + 1;
                    }

                    // Move cursor to the first editable cell of the new row
                    dgvItems.CurrentCell = dgvItems.Rows[currentRowIndex].Cells["colParticulars"];
                    dgvItems.BeginEdit(true);
                }
            }

            // Save on Ctrl + S
            if (e.Control && e.KeyCode == Keys.S)
            {
                e.Handled = true;

                try
                {
                    // Commit any edits in DataGridView
                    if (dgvItems.IsCurrentCellInEditMode)
                        dgvItems.EndEdit();

                    dgvItems.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    // Save quotation silently
                    SaveQuotation(true); // True = keep form open
                }
                catch
                {
                    // Optionally log error silently
                }
            }
        }

        private void DgvItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // fill SrNo
            for (int i = 0; i < dgvItems.Rows.Count; i++)
            {
                if (!dgvItems.Rows[i].IsNewRow)
                    dgvItems.Rows[i].Cells["colSrNo"].Value = i + 1;
            }
        }

        private void DgvItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvItems.Columns["colQty"].Index ||
                    e.ColumnIndex == dgvItems.Columns["colRate"].Index)
                {
                    var row = dgvItems.Rows[e.RowIndex];
                    decimal qty = 0, rate = 0;
                    decimal.TryParse(Convert.ToString(row.Cells["colQty"].Value), out qty);
                    decimal.TryParse(Convert.ToString(row.Cells["colRate"].Value), out rate);
                    row.Cells["colAmount"].Value = (qty * rate).ToString("0.00");
                    UpdateTotals();
                }
                // ✅ Particulars में automatic bullet point formatting
                if (e.ColumnIndex == dgvItems.Columns["colParticulars"].Index)
                {
                    var cellValue = Convert.ToString(dgvItems.Rows[e.RowIndex].Cells["colParticulars"].Value);
                    if (!string.IsNullOrWhiteSpace(cellValue))
                    {
                        // यदि user ने comma से separate किया है तो bullet points बनाएं
                        if (cellValue.Contains(",") && !cellValue.Contains("•"))
                        {
                            var items = cellValue.Split(',');
                            var bulletPoints = string.Join(Environment.NewLine,
                                items.Select(item => "• " + item.Trim()));
                            dgvItems.Rows[e.RowIndex].Cells["colParticulars"].Value = bulletPoints;
                        }
                    }

                    // Row height को manually refresh करें
                    dgvItems.AutoResizeRow(e.RowIndex);
                }
            }
            catch { }
        }

        private void SetupDataGridViewForMultiline()
        {
            // Particulars column के लिए wrap mode enable करें
            if (dgvItems.Columns["colParticulars"] != null)
            {
                dgvItems.Columns["colParticulars"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvItems.Columns["colParticulars"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            }

            // Auto row height adjustment enable करें
            dgvItems.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Minimum row height set करें
            dgvItems.RowTemplate.Height = 40; // आप अपनी आवश्यकता के अनुसार adjust कर सकते हैं

            // DataBindingComplete event add करें
            dgvItems.DataBindingComplete += DgvItems_DataBindingComplete;
        }
        private void DgvItems_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // हर बार data load होने के बाद row height adjust करें
            dgvItems.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
        private void DgvItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Particulars column में editing के लिए multiline TextBox enable करें
            if (dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["colParticulars"].Index)
            {
                var textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.Multiline = true;
                    textBox.AcceptsReturn = true; // Enter key पर new line
                    textBox.ScrollBars = ScrollBars.Vertical;
                    textBox.WordWrap = true;
                }
            }
        }
        private void DgvItems_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            // Confirm delete
            var result = MessageBox.Show("Are you sure you want to delete this item?",
                                         "Delete Row", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void DgvItems_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            // ✅ Renumber SrNo column
            for (int i = 0; i < dgvItems.Rows.Count; i++)
            {
                if (!dgvItems.Rows[i].IsNewRow)
                    dgvItems.Rows[i].Cells["colSrNo"].Value = i + 1;
            }

            // ✅ Recalculate totals
            UpdateTotals();
        }

        private void UpdateTotals()
        {
            decimal subtotal = 0;
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.IsNewRow) continue;
                decimal amt = 0;
                decimal.TryParse(Convert.ToString(row.Cells["colAmount"].Value), out amt);
                subtotal += amt;
            }

            lblSubtotal.Text = subtotal.ToString("0.00");

        }

        private void btnChooseLogo_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string dest = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                               "logo" + Path.GetExtension(dlg.FileName));

                    // Release previous image lock if any
                    if (picLogo.Image != null)
                    {
                        picLogo.Image.Dispose();
                        picLogo.Image = null;
                    }

                    // Copy new file
                    File.Copy(dlg.FileName, dest, true);

                    // Save path to DB
                    DatabaseHelper.SaveSetting("LogoPath", dest);

                    // Load without locking the file
                    using (var fs = new FileStream(dest, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (var img = Image.FromStream(fs))
                    {
                        picLogo.Image = new Bitmap(img); // clone into memory
                    }
                }
            }
        }

        private void btnChooseSignature_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string dest = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "signature" + Path.GetExtension(dlg.FileName));
                    File.Copy(dlg.FileName, dest, true);
                    DatabaseHelper.SaveSetting("SignaturePath", dest);
                    picSignature.Image = Image.FromFile(dest);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            currentQuotationId = 0;
            txtQuotationNo.Text = GetNextQuotationNo().ToString();
            dtpDate.Value = DateTime.Now;

            enquriy.Text = txtCustomer.Text = txtAddress.Text = txtKindAttn.Text = txtGSTNo.Text = "";
            dgvItems.Rows.Clear();
            if (txtChallanNo != null) txtChallanNo.Text = "";
            if (dtpChallanDate != null) dtpChallanDate.Text = "";
            rtbTerms.Text = DatabaseHelper.GetSetting("DefaultTerms");
            UpdateTotals();
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {

            try
            {
                // ✅ First commit any pending edits in DataGridView
                if (dgvItems.IsCurrentCellInEditMode)
                {
                    dgvItems.EndEdit();
                }

                // ✅ Commit all changes to the DataGridView
                dgvItems.CommitEdit(DataGridViewDataErrorContexts.Commit);

                // Ensure quotation is saved first
                if (currentQuotationId == 0)
                {
                    SaveQuotation(true);
                }
                else
                {
                    SaveQuotation(true); // ✅ Update existing quotation
                }

              
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving quotation: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtCustomer_Leave(object sender, EventArgs e)
        {
            if (customerMap.TryGetValue(txtCustomer.Text, out var addr))
            {
                txtAddress.Text = addr;
            }
        }


        private int SaveQuotation(bool returnIdAndKeepOpen)
        {
            // Build Quotation object from form (same as before)
            var q = new Quotation();
            q.QuotationNo = int.TryParse(txtQuotationNo.Text, out int qno) ? qno : 0;
            q.Date = dtpDate.Value;
            q.Enquiry = enquriy.Text;
            q.CustomerName = txtCustomer.Text;
            q.Address = txtAddress.Text;
            q.KindAttn = txtKindAttn.Text;
            q.GSTNo = txtGSTNo.Text;
            q.Terms = rtbTerms.Text;
            q.RGPNo = txtRGPNo.Text;
            q.RGPDate = txtRGPDate.Text;
            q.ChallanNo = txtChallanNo.Text;
            q.ChallanDate = dtpChallanDate.Text;

            // Build items (same as before)
            q.Items = new List<QuotationItem>();
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.IsNewRow) continue;
                var it = new QuotationItem();
                it.SrNo = Convert.ToInt32(row.Cells["colSrNo"].Value ?? 0);
                it.Particulars = Convert.ToString(row.Cells["colParticulars"].Value);
                it.Unit = Convert.ToString(row.Cells["colUnit"].Value);
                it.Qty = decimal.TryParse(Convert.ToString(row.Cells["colQty"].Value), out decimal qv) ? qv : 0;
                it.Rate = decimal.TryParse(Convert.ToString(row.Cells["colRate"].Value), out decimal rv) ? rv : 0;
                it.Amount = decimal.TryParse(Convert.ToString(row.Cells["colAmount"].Value), out decimal av) ? av : 0;
                q.Items.Add(it);
            }

            q.Subtotal = decimal.TryParse(lblSubtotal.Text, out decimal s) ? s : 0;

            using (var con = DatabaseHelper.GetConnection())
            {
                con.Open();
                using (var tx = con.BeginTransaction())
                {
                    if (currentQuotationId == 0)
                    {
                        // INSERT NEW
                        string insertSql = @"INSERT INTO Quotations 
                    (QuotationNo, Date, Enquiry, CustomerName, Address, KindAttn, GSTNo, Terms, Subtotal, RGPNo, RGPDate, ChallanNo, ChallanDate)
                    OUTPUT INSERTED.Id
                    VALUES (@no, @date, @enqu, @cust, @addr, @attn, @gst, @terms, @sub, @rgpno, @rgpdate, @challanno, @challandate)";

                        using (var cmd = new SqlCommand(insertSql, con, tx))
                        {
                            cmd.Parameters.AddWithValue("@no", q.QuotationNo);
                            cmd.Parameters.AddWithValue("@date", q.Date);
                            cmd.Parameters.AddWithValue("@enqu", q.Enquiry ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@cust", q.CustomerName ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@addr", q.Address ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@attn", q.KindAttn ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@gst", q.GSTNo ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@terms", q.Terms ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@sub", q.Subtotal);
                            cmd.Parameters.AddWithValue("@rgpno", q.RGPNo ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@rgpdate", q.RGPDate ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@challanno", q.ChallanNo ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@challandate", q.ChallanDate ?? (object)DBNull.Value);

                            currentQuotationId = (int)cmd.ExecuteScalar();
                        }
                    }
                    else
                    {
                        // UPDATE EXISTING
                        string updateSql = @"UPDATE Quotations SET 
                    QuotationNo=@no, Date=@date, Enquiry=@enqu, CustomerName=@cust, Address=@addr,
                    KindAttn=@attn, GSTNo=@gst, Terms=@terms, Subtotal=@sub, 
                    RGPNo=@rgpno, RGPDate=@rgpdate, ChallanNo=@challanno, ChallanDate=@challandate
                    WHERE Id=@id";

                        using (var cmd = new SqlCommand(updateSql, con, tx))
                        {
                            cmd.Parameters.AddWithValue("@no", q.QuotationNo);
                            cmd.Parameters.AddWithValue("@date", q.Date);
                            cmd.Parameters.AddWithValue("@enqu", q.Enquiry ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@cust", q.CustomerName ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@addr", q.Address ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@attn", q.KindAttn ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@gst", q.GSTNo ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@terms", q.Terms ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@sub", q.Subtotal);
                            cmd.Parameters.AddWithValue("@rgpno", q.RGPNo ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@rgpdate", q.RGPDate ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@challanno", q.ChallanNo ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@challandate", q.ChallanDate ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@id", currentQuotationId);
                            cmd.ExecuteNonQuery();
                        }

                        // Delete old items
                        using (var delCmd = new SqlCommand("DELETE FROM QuotationItems WHERE QuotationId=@qid", con, tx))
                        {
                            delCmd.Parameters.AddWithValue("@qid", currentQuotationId);
                            delCmd.ExecuteNonQuery();
                        }
                    }

                    // Insert items
                    foreach (var it in q.Items)
                    {
                        string itemSql = @"INSERT INTO QuotationItems
                    (QuotationId, SrNo, Particulars, Unit, Qty, Rate, Amount)
                    VALUES (@qid, @sr, @part, @unit, @qty, @rate, @amt)";

                        using (var itemCmd = new SqlCommand(itemSql, con, tx))
                        {
                            itemCmd.Parameters.AddWithValue("@qid", currentQuotationId);
                            itemCmd.Parameters.AddWithValue("@sr", it.SrNo);
                            itemCmd.Parameters.AddWithValue("@part", it.Particulars ?? (object)DBNull.Value);
                            itemCmd.Parameters.AddWithValue("@unit", it.Unit ?? (object)DBNull.Value);
                            itemCmd.Parameters.AddWithValue("@qty", it.Qty);
                            itemCmd.Parameters.AddWithValue("@rate", it.Rate);
                            itemCmd.Parameters.AddWithValue("@amt", it.Amount);
                            itemCmd.ExecuteNonQuery();
                        }
                    }

                    tx.Commit();
                }
            }

            MessageBox.Show("Quotation saved successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadAutoCompleteSources();

            return currentQuotationId;
        }

        private void btnPrint_Click_1(object sender, EventArgs e)
        {
            try
            {
                // ✅ Commit any pending edits before saving
                if (dgvItems.IsCurrentCellInEditMode)
                {
                    dgvItems.EndEdit();
                }
                dgvItems.CommitEdit(DataGridViewDataErrorContexts.Commit);

                SaveQuotation(false);

                // ensure saved
                if (currentQuotationId == 0)
                    SaveQuotation(true);

                PrintHelper.PrintQuotationById(currentQuotationId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing quotation: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        // Method to load an existing quotation (used by DatabaseForm edit).
        public void LoadQuotation(int quotationId)
        {
            currentQuotationId = quotationId;
            using (var con = DatabaseHelper.GetConnection())
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT QuotationNo, Date, Enquiry, CustomerName, Address, KindAttn, GSTNo, Terms, Subtotal , RGPNo, RGPDate ,ChallanNo, ChallanDate FROM Quotations WHERE Id=@id";
                    cmd.Parameters.AddWithValue("@id", quotationId);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            txtQuotationNo.Text = rdr.GetInt32(0).ToString();
                            dtpDate.Value = GetSafeDateTime(rdr, 1);
                            enquriy.Text = rdr.IsDBNull(2) ? "" : rdr.GetString(2);
                            txtCustomer.Text = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
                            txtAddress.Text = rdr.IsDBNull(4) ? "" : rdr.GetString(4);
                            txtKindAttn.Text = rdr.IsDBNull(5) ? "" : rdr.GetString(5);
                            txtGSTNo.Text = rdr.IsDBNull(6) ? "" : rdr.GetString(6);
                            rtbTerms.Text = rdr.IsDBNull(7) ? "" : rdr.GetString(7);

                            // ✅ Subtotal पढ़ो
                            decimal sub = 0;
                            if (!rdr.IsDBNull(8))
                            {
                                object val = rdr.GetValue(8);
                                sub = Convert.ToDecimal(val);
                            }
                            lblSubtotal.Text = sub.ToString("0.00");
                            txtRGPNo.Text = rdr.IsDBNull(9) ? "" : rdr.GetString(9);
                            txtRGPDate.Text = rdr.IsDBNull(10) ? "" : rdr.GetString(10);
                            txtChallanNo.Text = rdr.IsDBNull(11) ? "" : rdr.GetString(11);
                            dtpChallanDate.Text = rdr.IsDBNull(12) ? "" : rdr.GetString(12);
                        }
                    }
                }

                // Items
                dgvItems.Rows.Clear();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT SrNo, Particulars,Unit, Qty,  Rate, Amount FROM QuotationItems WHERE QuotationId=@id ORDER BY SrNo";
                    cmd.Parameters.AddWithValue("@id", quotationId);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int sr = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
                            string part = rdr.IsDBNull(1) ? "" : rdr.GetString(1);
                           
                            string unit = rdr.IsDBNull(2) ? "" : rdr.GetString(2);
                            decimal qty = rdr.IsDBNull(3) ? 0 : Convert.ToDecimal(rdr.GetValue(3));
                            decimal rate = rdr.IsDBNull(4) ? 0 : Convert.ToDecimal(rdr.GetValue(4));
                            decimal amt = rdr.IsDBNull(5) ? 0 : Convert.ToDecimal(rdr.GetValue(5));

                            dgvItems.Rows.Add(sr, part, unit, qty.ToString("0.00"), rate.ToString("0.00"), amt.ToString("0.00"));
                        }
                    }
                }
            }
        }



        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        private void lblGSTAmount_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtCustomer_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var parent = this.ParentForm as HomeForm;
            if (parent != null)
            {
                if (OpenedFrom == "Database")
                {
                    // ✅ Pass admin state back to DatabaseForm
                    parent.LoadFormInPanel(new DatabaseForm(IsAdmin));
                }
                else
                {
                    // default = home
                    parent.ShowHome();
                }
            }
        }

        private void rtbTerms_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Ensure quotation is saved first
            if (currentQuotationId == 0)
            {
                SaveQuotation(true);
            }

            // Save quotation as PDF
            PrintHelper.SaveToPdfById(currentQuotationId);
        }
    }
}
