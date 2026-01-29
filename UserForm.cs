using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuotationApp.Models;
using System.IO;

namespace QuotationApp
{
    public partial class UserForm : Form
    {
        public event Action<User> UserSaved;

        public User User { get; private set; }

        
        public enum CallerType
        {
            AdminPanel,
            SuperAdmin
        }
        public CallerType Caller { get; set; }

        public UserForm(User existing = null, CallerType caller = CallerType.AdminPanel)
        {
            InitializeComponent();
            Caller = caller;

            if (existing != null)
            {
                User = existing;
                txtName.Text = existing.Name;
                txtUsername.Text = existing.Username;
                txtPassword.Text = existing.Password;
                if (File.Exists(existing.PhotoPath))
                    picPhoto.Image = Image.FromFile(existing.PhotoPath);
            }
            else
            {
                User = new User();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Simple validation for all required fields
            var emptyFields = new List<string>();

            if (string.IsNullOrWhiteSpace(txtName.Text))
                emptyFields.Add("Name");

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
                emptyFields.Add("Username");

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
                emptyFields.Add("Password");

            // 🔹 Photo is optional, no need to add to emptyFields

            if (emptyFields.Any())
            {
                MessageBox.Show("Please fill the following fields:\n- " + string.Join("\n- ", emptyFields),
                                "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // stop here, user has to fill required fields
            }

            // Save user details
            User.Name = txtName.Text.Trim();
            User.Username = txtUsername.Text.Trim();
            User.Password = txtPassword.Text.Trim();

            // Only save PhotoPath if user actually selected a file
            if (!string.IsNullOrWhiteSpace(txtPhotoPath.Text) && File.Exists(txtPhotoPath.Text))
            {
                User.PhotoPath = txtPhotoPath.Text;
            }
            else
            {
                User.PhotoPath = null; // or empty string
            }

            // 🔥 Fire event
            UserSaved?.Invoke(User);

            // Navigate back to parent
            var parent = this.ParentForm as HomeForm;
            if (parent != null)
            {
                if (Caller == CallerType.SuperAdmin)
                    parent.LoadFormInPanel(new SuperAdminForm());
                else
                    parent.LoadFormInPanel(new AdminPanelForm());
            }
            // panel wale case me close mat karo, back button se wapas jao
        }



        private void btnUpload_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "Images|*.jpg;*.jpeg;*.png" };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            txtPhotoPath.Text = dlg.FileName;
            picPhoto.Image = Image.FromFile(dlg.FileName);
        }

        private void picPhoto_Click(object sender, EventArgs e)
        {

        }

        private void UserForm_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var parent = this.ParentForm as HomeForm;
            if (parent != null)
            {
                if (Caller == CallerType.SuperAdmin)
                    parent.LoadFormInPanel(new SuperAdminForm());
                else
                    parent.LoadFormInPanel(new AdminPanelForm());
            }
        }
    }
}
