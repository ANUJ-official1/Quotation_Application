using QuotationApp.Helpers;
using System;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace QuotationApp
{
    public partial class DatabaseForm : Form
    {
        private bool isAdmin;

        public DatabaseForm(bool admin)
        {
            InitializeComponent();
            isAdmin = admin;
            ApplyAdminRestrictions();
           
           
            this.Load += DatabaseForm_Load;
        }
        private void ApplyAdminRestrictions()
        {
            
            btnDelete.Visible = isAdmin;
            btnDelete.Enabled = isAdmin;


        }
        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(value);
            if (value)
            {
                ApplyAdminRestrictions(); // Re-apply restrictions when becoming visible
            }
        }
        private void DatabaseForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                ApplyAdminRestrictions();
            }
        }

        private void DatabaseForm_Load(object sender, EventArgs e)
        {
            ApplyAdminRestrictions();
            LoadQuotations();

          
        }
        private void DatabaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose(); 
        }


        private void LoadQuotations()
        {
            dgvQuotations.Rows.Clear();
            using (var con = DatabaseHelper.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand()) // ✅ SqlCommand explicitly use करें
                {
                    cmd.Connection = con;
                    cmd.CommandText = @"
                        SELECT Id, QuotationNo, Date, 
                               Enquiry, CustomerName, Subtotal, 
                               ISNULL(RGPNo, '') AS RGPNo, 
                               ISNULL(ChallanNo, '') AS ChallanNo
                        FROM Quotations
                        ORDER BY QuotationNo DESC";

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int id = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
                            int qno = rdr.IsDBNull(1) ? 0 : rdr.GetInt32(1);

                            // ✅ SQL Server DateTime proper handling
                            string date = rdr.IsDBNull(2) ? "" : rdr.GetDateTime(2).ToString("dd/MM/yyyy");

                            string enquiry = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
                            string cust = rdr.IsDBNull(4) ? "" : rdr.GetString(4);

                            // ✅ SQL Server decimal handling
                            decimal subtotal = rdr.IsDBNull(5) ? 0 : rdr.GetDecimal(5);

                            // ✅ RGP और Challan को safely read करें
                            string rgp = rdr.IsDBNull(6) ? "" : rdr.GetString(6);
                            string challan = rdr.IsDBNull(7) ? "" : rdr.GetString(7);

                            // ✅ Enquiry को combine करें
                            string displayEnquiry = enquiry;
                            if (!string.IsNullOrWhiteSpace(rgp))
                                displayEnquiry = $"{enquiry} / {rgp}";
                            else if (!string.IsNullOrWhiteSpace(challan))
                                displayEnquiry = $"{enquiry} / {challan}";

                            // ✅ DataGridView में add करें
                            dgvQuotations.Rows.Add(id, qno, date, displayEnquiry, cust, subtotal.ToString("0.00"));
                        }
                    }
                }
            }
        }


        private int GetSelectedQuotationId()
        {
            if (dgvQuotations.SelectedRows.Count == 0) return 0;
            return Convert.ToInt32(dgvQuotations.SelectedRows[0].Cells["colId"].Value);
        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            LoadQuotations();
        }

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            int id = GetSelectedQuotationId();
            if (id == 0)
            {
                MessageBox.Show("Select a quotation.");
                return;
            }

            if (MessageBox.Show("Delete selected quotation?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            using (var con = DatabaseHelper.GetConnection())
            {
                con.Open();
                using (var tx = con.BeginTransaction()) // ✅ SQL Server transaction
                {
                    try
                    {
                        // पहले QuotationItems delete करें (foreign key constraint के लिए)
                        using (var cmd = new SqlCommand("DELETE FROM QuotationItems WHERE QuotationId=@id", con, tx))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                        }

                        // फिर main Quotation delete करें
                        using (var cmd = new SqlCommand("DELETE FROM Quotations WHERE Id=@id", con, tx))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit(); // ✅ Transaction commit करें
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback(); // ✅ Error पर rollback करें
                        MessageBox.Show($"Error deleting quotation: {ex.Message}", "Error",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            LoadQuotations();
            MessageBox.Show("Deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            int id = GetSelectedQuotationId();
            if (id == 0)
            {
                MessageBox.Show("Select a quotation.");
                return;
            }

            var form = new QuotationForm();
            form.OpenedFrom = "Database";
            form.IsAdmin = isAdmin; // ✅ yaha bhi source set karo
            form.LoadQuotation(id);

            form.FormClosed += (s, args) =>
            {
                LoadQuotations();
                ApplyAdminRestrictions();
            };

            var parent = this.ParentForm as HomeForm;
            parent?.LoadFormInPanel(form);

        }

        private void btnPrint_Click_1(object sender, EventArgs e)
        {
            int id = GetSelectedQuotationId();
            if (id == 0) { MessageBox.Show("Select a quotation."); return; }
            PrintHelper.PrintQuotationById(id);
        }

        private void dgvQuotations_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // valid row
            {
                int id = Convert.ToInt32(dgvQuotations.Rows[e.RowIndex].Cells["colId"].Value);
                if (id > 0)
                {
                    PrintHelper.PreviewQuotationById(id);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var parent = this.ParentForm as HomeForm;
            if (parent != null)
            {
                parent.ShowHome();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get selected quotation ID from database
            int id = GetSelectedQuotationId();

            // Check if any quotation is selected
            if (id == 0)
            {
                MessageBox.Show("Please select a quotation to save as PDF.", "No Selection",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Save quotation as PDF
            PrintHelper.SaveToPdfById(id);
        }
    }
}
