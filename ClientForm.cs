using QuotationApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuotationApp
{
    public partial class ClientForm : Form
    {
        
        public ClientForm()
        {
            InitializeComponent();
          
            LoadClients();
        }

       

        private void LoadClients()
        {
            using (var con = DatabaseHelper.GetConnection())
            {
                con.Open();
                string query = "SELECT * FROM Clients";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvClients.DataSource = dt;
            }

            // Font aur column width code same rahega
            dgvClients.DefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            dgvClients.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 13, FontStyle.Bold);

            if (dgvClients.Columns.Contains("Id"))
                dgvClients.Columns["Id"].Width = 60;
            if (dgvClients.Columns.Contains("VendorCode"))
                dgvClients.Columns["VendorCode"].Width = 150;
            if (dgvClients.Columns.Contains("CustomerName"))
                dgvClients.Columns["CustomerName"].Width = 200;
            if (dgvClients.Columns.Contains("Address"))
                dgvClients.Columns["Address"].Width = 300;
            if (dgvClients.Columns.Contains("GSTNo"))
                dgvClients.Columns["GSTNo"].Width = 150;

        }

        private void ClientForm_Load(object sender, EventArgs e)
        {

        }

        private void dgvClients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvClients.Rows[e.RowIndex];
                txtId.Text = row.Cells["Id"].Value?.ToString();
                textBox1.Text = row.Cells["VendorCode"].Value?.ToString(); // NEW
                txtCustomerName.Text = row.Cells["CustomerName"].Value?.ToString();
                rtbAddress.Text = row.Cells["Address"].Value?.ToString();
                txtGSTNo.Text = row.Cells["GSTNo"].Value?.ToString();
            }
        }

           

        

        private void rtbAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDatabase_Click(object sender, EventArgs e)
        {
            // Validation same rahegi
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                MessageBox.Show("Customer Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(rtbAddress.Text))
            {
                MessageBox.Show("Address is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtGSTNo.Text))
            {
                MessageBox.Show("GST No is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var con = DatabaseHelper.GetConnection())
            {
                con.Open();

                if (string.IsNullOrEmpty(txtId.Text))
                {
                    // INSERT
                    string insertQuery = "INSERT INTO Clients (VendorCode, CustomerName, Address, GSTNo) VALUES (@vendor, @name, @address, @gst)";
                    using (var cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@vendor", textBox1.Text);
                        cmd.Parameters.AddWithValue("@name", txtCustomerName.Text);
                        cmd.Parameters.AddWithValue("@address", rtbAddress.Text);
                        cmd.Parameters.AddWithValue("@gst", txtGSTNo.Text);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Client added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // UPDATE
                    string updateQuery = "UPDATE Clients SET VendorCode=@vendor, CustomerName=@name, Address=@address, GSTNo=@gst WHERE Id=@id";
                    using (var cmd = new SqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@vendor", textBox1.Text);
                        cmd.Parameters.AddWithValue("@name", txtCustomerName.Text);
                        cmd.Parameters.AddWithValue("@address", rtbAddress.Text);
                        cmd.Parameters.AddWithValue("@gst", txtGSTNo.Text);
                        cmd.Parameters.AddWithValue("@id", int.Parse(txtId.Text));
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Client updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            ClearForm();
            LoadClients();
        }
        

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            if (dgvClients.CurrentRow == null)
            {
                MessageBox.Show("Please select a client from the list to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Selected row ko textboxes me daalna
            DataGridViewRow row = dgvClients.CurrentRow;
            txtId.Text = row.Cells["Id"].Value?.ToString();
            textBox1.Text = row.Cells["VendorCode"].Value?.ToString();
            txtCustomerName.Text = row.Cells["CustomerName"].Value?.ToString();
            rtbAddress.Text = row.Cells["Address"].Value?.ToString();
            txtGSTNo.Text = row.Cells["GSTNo"].Value?.ToString();
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        
            {
                ClearForm();
            }

        private void ClearForm()
        {
            txtId.Clear();
            textBox1.Clear();
            txtCustomerName.Clear();
            rtbAddress.Clear();
            txtGSTNo.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (dgvClients.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgvClients.CurrentRow.Cells["Id"].Value);

                var confirm = MessageBox.Show("Are you sure you want to delete this client?",
                                              "Confirm Delete",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    using (var con = DatabaseHelper.GetConnection())
                    {
                        con.Open();
                        string deleteQuery = "DELETE FROM Clients WHERE Id=@Id";
                        using (var cmd = new SqlCommand(deleteQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    LoadClients();
                    ClearForm();
                }
            }
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

        private void txtCustomerName_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
    
}
