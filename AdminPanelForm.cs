using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using QuotationApp.Helpers;
using QuotationApp.Models;

namespace QuotationApp
{
    public partial class AdminPanelForm : Form
    {
        private const string FilePath = "Data/workers.json";
        private List<User> _users;
        public AdminPanelForm()
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
            _users = SqlHelper.LoadWorkers();
            dgvUsers.DataSource = null;
            dgvUsers.DataSource = _users;
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btnAddWorker_Click(object sender, EventArgs e)
        {
            var form = new UserForm(null, UserForm.CallerType.AdminPanel);

            form.UserSaved += (newUser) =>
            {
                newUser.UserType = "Worker"; // ✅ Set as worker

                // Photo handling (same as before)
                if (!string.IsNullOrWhiteSpace(newUser.PhotoPath) && File.Exists(newUser.PhotoPath))
                {
                    Directory.CreateDirectory("Photos");
                    var dest = Path.Combine("Photos", $"{Guid.NewGuid()}{Path.GetExtension(newUser.PhotoPath)}");
                    File.Copy(newUser.PhotoPath, dest, true);
                    newUser.PhotoPath = dest;
                }
                else
                {
                    newUser.PhotoPath = null;
                }

                // ✅ Save to SQL Server and get new ID
                var newId = SqlHelper.SaveUser(newUser);
                newUser.Id = newId;

                LoadUsers(); // Refresh the list
            };

            var parent = this.ParentForm as HomeForm;
            parent?.LoadFormInPanel(form);
        }
        private void btnDeleteWorker_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) return;
            var selectedUser = (User)dgvUsers.CurrentRow.DataBoundItem; // ✅ Different name

            if (MessageBox.Show($"Delete {selectedUser.Name}?", "Confirm", MessageBoxButtons.YesNo)
                != DialogResult.Yes) return;

            _users.Remove(selectedUser);
            // ✅ Delete from database
            SqlHelper.DeleteUser(selectedUser.Id); // You need to add this method
            LoadUsers();
        }

        private void btnEditAdmins_Click(object sender, EventArgs e)
        {
            // parent form (HomeForm) ka reference chahiye
            var parent = this.ParentForm as HomeForm;
            if (parent != null)
            {
                parent.LoadFormInPanel(new SuperAdminForm());
            }
        }

        private void AdminPanelForm_Load(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) return;
            var currentUser = (User)dgvUsers.CurrentRow.DataBoundItem; // ✅ Different name

            var form = new UserForm(currentUser, UserForm.CallerType.AdminPanel);
            form.UserSaved += (updatedUser) => // ✅ Different name
            {
                if (updatedUser.PhotoPath != currentUser.PhotoPath)
                {
                    var dest = Path.Combine("Photos", $"{Guid.NewGuid()}{Path.GetExtension(updatedUser.PhotoPath)}");
                    File.Copy(updatedUser.PhotoPath, dest, true);
                    updatedUser.PhotoPath = dest;
                }

                currentUser.Name = updatedUser.Name;
                currentUser.Username = updatedUser.Username;
                currentUser.Password = updatedUser.Password;
                currentUser.PhotoPath = updatedUser.PhotoPath;

                // ✅ Save individual user
                SqlHelper.SaveUser(currentUser);
                LoadUsers();
            };

            var parent = this.ParentForm as HomeForm;
            parent?.LoadFormInPanel(form);
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

        private void pictureBox1_Click(object sender, EventArgs e)
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
    }
}
