namespace QuotationApp
{
    partial class SuperAdminForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SuperAdminForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnAddAdmin = new System.Windows.Forms.Button();
            this.btnDeleteAdmin = new System.Windows.Forms.Button();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkSlateGray;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnBack);
            this.panel1.Controls.Add(this.btnAddAdmin);
            this.panel1.Controls.Add(this.btnDeleteAdmin);
            this.panel1.Location = new System.Drawing.Point(0, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(203, 638);
            this.panel1.TabIndex = 15;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Image = global::QuotationApp.Properties.Resources.edit1;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(36, 218);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 35);
            this.button1.TabIndex = 20;
            this.button1.Text = "      Edit";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.Transparent;
            this.btnBack.BackgroundImage = global::QuotationApp.Properties.Resources.back;
            this.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.btnBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnBack.Location = new System.Drawing.Point(19, 22);
            this.btnBack.Margin = new System.Windows.Forms.Padding(2);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(77, 51);
            this.btnBack.TabIndex = 12;
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnAddAdmin
            // 
            this.btnAddAdmin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddAdmin.FlatAppearance.BorderSize = 0;
            this.btnAddAdmin.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.btnAddAdmin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnAddAdmin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnAddAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddAdmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddAdmin.ForeColor = System.Drawing.Color.White;
            this.btnAddAdmin.Image = global::QuotationApp.Properties.Resources.add_group;
            this.btnAddAdmin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddAdmin.Location = new System.Drawing.Point(36, 146);
            this.btnAddAdmin.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddAdmin.Name = "btnAddAdmin";
            this.btnAddAdmin.Size = new System.Drawing.Size(142, 44);
            this.btnAddAdmin.TabIndex = 10;
            this.btnAddAdmin.Text = "       Add";
            this.btnAddAdmin.UseVisualStyleBackColor = false;
            this.btnAddAdmin.Click += new System.EventHandler(this.btnAddAdmin_Click);
            // 
            // btnDeleteAdmin
            // 
            this.btnDeleteAdmin.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteAdmin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDeleteAdmin.FlatAppearance.BorderSize = 0;
            this.btnDeleteAdmin.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.btnDeleteAdmin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnDeleteAdmin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnDeleteAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteAdmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteAdmin.ForeColor = System.Drawing.Color.White;
            this.btnDeleteAdmin.Image = global::QuotationApp.Properties.Resources.delete;
            this.btnDeleteAdmin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteAdmin.Location = new System.Drawing.Point(36, 271);
            this.btnDeleteAdmin.Margin = new System.Windows.Forms.Padding(2);
            this.btnDeleteAdmin.Name = "btnDeleteAdmin";
            this.btnDeleteAdmin.Size = new System.Drawing.Size(142, 41);
            this.btnDeleteAdmin.TabIndex = 11;
            this.btnDeleteAdmin.Text = "         Delete";
            this.btnDeleteAdmin.UseVisualStyleBackColor = false;
            this.btnDeleteAdmin.Click += new System.EventHandler(this.btnDeleteAdmin_Click);
            // 
            // dgvUsers
            // 
            this.dgvUsers.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(86, 31);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.RowHeadersWidth = 62;
            this.dgvUsers.Size = new System.Drawing.Size(602, 380);
            this.dgvUsers.TabIndex = 20;
            this.dgvUsers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.panel2.Controls.Add(this.dgvUsers);
            this.panel2.Location = new System.Drawing.Point(248, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(934, 503);
            this.panel2.TabIndex = 21;
            // 
            // SuperAdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1302, 566);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SuperAdminForm";
            this.Text = "SuperAdmin";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.SuperAdminForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnDeleteAdmin;
        private System.Windows.Forms.Button btnAddAdmin;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Panel panel2;
    }
}