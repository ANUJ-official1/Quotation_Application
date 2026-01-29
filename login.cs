using QuotationApp.Helpers;
using QuotationApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuotationApp
{
    public partial class login : Form
    {
        private const string WorkersFile = "Data/workers.json";
        private const string AdminsFile = "Data/admins.json";
        public login()
        {
              InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
            chkShowPassword.CheckedChanged += (_, __) =>
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable; // Keep resizable border (default)
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.ControlBox = true;

            StartPosition = FormStartPosition.CenterScreen;
            AutoScaleMode = AutoScaleMode.Dpi;
            MinimumSize = new Size(1024, 700);
            this.KeyPreview = true;
            this.KeyDown += Login_KeyDown;

        }
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            // 1️⃣ Enter → Login button
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick(); // login button click
                e.Handled = true;
            }

            // 2️⃣ Ctrl + E → Exit button
            else if (e.Control && e.KeyCode == Keys.E)
            {
                button2.PerformClick(); // exit button click
                e.Handled = true;
            }

            // 3️⃣ Ctrl + S → Show Password toggle
            else if (e.Control && e.KeyCode == Keys.S)
            {
                chkShowPassword.Checked = !chkShowPassword.Checked; // toggle checkbox
                e.Handled = true;
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            // Toggle masking
            // When checked -> show text (no masking). When unchecked -> mask.
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }
        public void DoSomething()
        {
            throw new NotImplementedException();
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text.Trim();

            User user = null;
            using (var con = DatabaseHelper.GetConnection())
            {
                con.Open();
                string sql = "SELECT TOP 1 Id, Name, Username, Password, PhotoPath, UserType FROM Users WHERE Username=@uname AND Password=@pass";

                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@uname", username);
                    cmd.Parameters.AddWithValue("@pass", password);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            user = new User
                            {
                                Id = (int)rdr["Id"],
                                Name = rdr["Name"].ToString(),
                                Username = rdr["Username"].ToString(),
                                Password = rdr["Password"].ToString(),
                                PhotoPath = rdr["PhotoPath"]?.ToString(),
                                UserType = rdr["UserType"].ToString()
                            };
                        }
                    }
                }
            }

            if (user == null)
            {
                MessageBox.Show("Invalid Username or Password!");
                return;
            }

            // Check if admin (you can add admin flag in Users table)
            bool isAdmin = user.UserType.Equals("Admin", StringComparison.OrdinalIgnoreCase);

            Hide();

            if (isAdmin)
            {
                using (var home = new HomeForm(true)) // Admin panel
                    home.ShowDialog();
            }
            else
            {
                using (var home = new HomeForm(false)) // Worker panel
                    home.ShowDialog();
            }

            Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void chkShowPassword_CheckedChanged_1(object sender, EventArgs e)
        {
            // Toggle masking
            // When checked -> show text (no masking). When unchecked -> mask.
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
