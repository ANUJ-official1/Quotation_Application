
using QuotationApp.Helpers;
using QuotationApp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace QuotationApp
{
    public partial class SuperAdminForm : Form
    {
         private const string FilePath = "Data/admins.json";
        private List<User> _users;
        public SuperAdminForm()
        {
            InitializeComponent();
            SetupGrid();
            LoadUsers();
        }
        private void SetupGrid()
        {
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.Columns.Clear();

            dgvUsers.RowTemplate.Height = 100; // ✅ row ki height badi kar do

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Sr No.",
                DataPropertyName = "Id",
                Width = 50
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                DataPropertyName = "Name",
                Width = 150
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Username",
                DataPropertyName = "Username",
                Width = 100
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Password",
                DataPropertyName = "Password",
                Width = 100
            });

            var imgCol = new DataGridViewImageColumn
            {
                HeaderText = "Photo",
                DataPropertyName = "Photo",
                ImageLayout = DataGridViewImageCellLayout.Zoom, // ✅ maintain aspect ratio
                Width = 100                                   // ✅ column ki width bhi badi karo
            };
            dgvUsers.Columns.Add(imgCol);
        }


        private void LoadUsers()
        {
            _users = SqlHelper.LoadAdmins();
            dgvUsers.DataSource = null;
            dgvUsers.DataSource = _users;
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
             var parent = this.ParentForm as HomeForm;
            if (parent != null)
            {
                parent.LoadFormInPanel(new AdminPanelForm());
            }

        }

        private void btnAddAdmin_Click(object sender, EventArgs e)
        {
            var form = new UserForm(null, UserForm.CallerType.SuperAdmin);

            form.UserSaved += (newAdmin) =>
            {
                try
                {
                    newAdmin.UserType = "Admin"; // ✅ Set as admin

                    // Photo handling (same as before)
                    if (!string.IsNullOrWhiteSpace(newAdmin.PhotoPath) && File.Exists(newAdmin.PhotoPath))
                    {
                        Directory.CreateDirectory("Photos");
                        var dest = Path.Combine("Photos", $"{Guid.NewGuid()}{Path.GetExtension(newAdmin.PhotoPath)}");
                        File.Copy(newAdmin.PhotoPath, dest, true);
                        newAdmin.PhotoPath = dest;
                    }
                    else
                    {
                        newAdmin.PhotoPath = null;
                    }

                    // ✅ Save to SQL Server and get new ID
                    var newId = SqlHelper.SaveUser(newAdmin);
                    newAdmin.Id = newId;

                    LoadUsers(); // Refresh the list
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving admin: " + ex.Message);
                }
            };

            var parent = this.ParentForm as HomeForm;
            parent?.LoadFormInPanel(form);
        }

        private void btnDeleteAdmin_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) return;
            var selectedAdmin = (User)dgvUsers.CurrentRow.DataBoundItem; // ✅ Different name

            if (MessageBox.Show($"Delete {selectedAdmin.Name}?", "Confirm", MessageBoxButtons.YesNo)
                != DialogResult.Yes) return;

            _users.Remove(selectedAdmin);
            SqlHelper.DeleteUser(selectedAdmin.Id); // ✅ Delete from database
            LoadUsers();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvUsers.Columns[e.ColumnIndex] is DataGridViewImageColumn)
            {
                var user = (User)dgvUsers.Rows[e.RowIndex].DataBoundItem;
                if (!string.IsNullOrEmpty(user.PhotoPath) && File.Exists(user.PhotoPath))
                {
                    var preview = new Form
                    {
                        Text = "Photo Preview",
                        Width = 600,
                        Height = 600
                    };

                    var pic = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        Image = Image.FromFile(user.PhotoPath),
                        SizeMode = PictureBoxSizeMode.Zoom
                    };

                    preview.Controls.Add(pic);
                    preview.ShowDialog();
                }
            }
        }

        private void SuperAdminForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) return;
            var currentAdmin = (User)dgvUsers.CurrentRow.DataBoundItem; // ✅ Different name

            var form = new UserForm(currentAdmin, UserForm.CallerType.SuperAdmin);
            form.UserSaved += (updatedAdmin) => // ✅ Different name
            {
                if (updatedAdmin.PhotoPath != currentAdmin.PhotoPath)
                {
                    var dest = Path.Combine("Photos", $"{Guid.NewGuid()}{Path.GetExtension(updatedAdmin.PhotoPath)}");
                    File.Copy(updatedAdmin.PhotoPath, dest, true);
                    updatedAdmin.PhotoPath = dest;
                }

                currentAdmin.Name = updatedAdmin.Name;
                currentAdmin.Username = updatedAdmin.Username;
                currentAdmin.Password = updatedAdmin.Password;
                currentAdmin.PhotoPath = updatedAdmin.PhotoPath;

                SqlHelper.SaveUser(currentAdmin); // ✅ Save individual user
                LoadUsers();
            };

            var parent = this.ParentForm as HomeForm;
            parent?.LoadFormInPanel(form);
        }
    }
}
