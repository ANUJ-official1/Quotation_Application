using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuotationApp
{
    public partial class HomeForm : Form
    {
        
        private bool isAdmin;
        public HomeForm(bool admin)
        {
            InitializeComponent();
            isAdmin = admin;
            button2.Visible = isAdmin; // only admins see settings
           
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable; // Keep resizable border (default)
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.ControlBox = true;

            // Better scaling on different monitors / DPI
            this.AutoScaleMode = AutoScaleMode.Dpi;

            // Optional: prevent the window from becoming too small
            this.MinimumSize = new Size(1024, 700);
        }
        public void LoadFormInPanel(Form frm)
        {
            // panel साफ करो
            panelContainer.Controls.Clear();

            // form को panel में डालो
            frm.TopLevel = false; // important!
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;

            panelContainer.Controls.Add(frm);
            frm.Show();
        }
        public void ShowHome()
        {
            // पहले सारे controls हटा दो
            panelContainer.Controls.Clear();

            // Background image reset कर दो
            panelContainer.BackgroundImage = Properties.Resources.WhatsApp_Image_2025_09_24_at_12_29_42_PM__1_;
            panelContainer.BackgroundImageLayout = ImageLayout.Stretch;

            // आपका पहले से designer में बना हुआ button वापस दिखाओ
            panelContainer.Controls.Add(btnCreateQuotation);

            

            btnCreateQuotation.BringToFront();
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {

        }

        private void btnCreateQuotation_Click(object sender, EventArgs e)
        {
            var form = new QuotationForm();
            form.OpenedFrom = "Home";   // ✅ source set karo
            LoadFormInPanel(form);
        }

        private void btnDatabase_Click_1(object sender, EventArgs e)
        {

            using (var db = new DatabaseForm(isAdmin))  // ✅ हर बार नया instance
            {
                LoadFormInPanel(new DatabaseForm(isAdmin));
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();                        // hide HomeForm
            using (var login = new login())
            {
                login.ShowDialog(this);    // modal over HomeForm
            }
            this.Show();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isAdmin)
                LoadFormInPanel(new AdminPanelForm());
            else
                MessageBox.Show("Only Admins can access this!");
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new ClientForm());
        }

       

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ShowHome();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ShowHome();

        }
    }
}
