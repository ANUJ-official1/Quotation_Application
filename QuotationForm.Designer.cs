using System;
using System.Windows.Forms;

namespace QuotationApp
{
    partial class QuotationForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuotationForm));
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.txtQuotationNo = new System.Windows.Forms.TextBox();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.txtKindAttn = new System.Windows.Forms.TextBox();
            this.txtGSTNo = new System.Windows.Forms.TextBox();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.colSrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colParticulars = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rtbTerms = new System.Windows.Forms.RichTextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblSubtotal = new System.Windows.Forms.Label();
            this.enquriy = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnChooseLogo = new System.Windows.Forms.Button();
            this.btnChooseSignature = new System.Windows.Forms.Button();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.picSignature = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dtpChallanDate = new System.Windows.Forms.TextBox();
            this.txtChallanNo = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRGPDate = new System.Windows.Forms.TextBox();
            this.txtRGPNo = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSignature)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtpDate
            // 
            this.dtpDate.CalendarTitleBackColor = System.Drawing.SystemColors.ControlText;
            this.dtpDate.CalendarTitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dtpDate.Location = new System.Drawing.Point(807, 40);
            this.dtpDate.Margin = new System.Windows.Forms.Padding(2);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(162, 20);
            this.dtpDate.TabIndex = 0;
            // 
            // txtQuotationNo
            // 
            this.txtQuotationNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtQuotationNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuotationNo.Location = new System.Drawing.Point(807, 4);
            this.txtQuotationNo.Margin = new System.Windows.Forms.Padding(2);
            this.txtQuotationNo.Name = "txtQuotationNo";
            this.txtQuotationNo.Size = new System.Drawing.Size(161, 26);
            this.txtQuotationNo.TabIndex = 1;
            // 
            // txtCustomer
            // 
            this.txtCustomer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomer.Location = new System.Drawing.Point(163, 38);
            this.txtCustomer.Margin = new System.Windows.Forms.Padding(2);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(278, 26);
            this.txtCustomer.TabIndex = 2;
            this.txtCustomer.TextChanged += new System.EventHandler(this.txtCustomer_TextChanged);
            this.txtCustomer.Leave += new System.EventHandler(this.txtCustomer_Leave);
            // 
            // txtKindAttn
            // 
            this.txtKindAttn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKindAttn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKindAttn.Location = new System.Drawing.Point(163, 94);
            this.txtKindAttn.Margin = new System.Windows.Forms.Padding(2);
            this.txtKindAttn.Name = "txtKindAttn";
            this.txtKindAttn.Size = new System.Drawing.Size(804, 26);
            this.txtKindAttn.TabIndex = 3;
            // 
            // txtGSTNo
            // 
            this.txtGSTNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGSTNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGSTNo.Location = new System.Drawing.Point(88, 12);
            this.txtGSTNo.Margin = new System.Windows.Forms.Padding(2);
            this.txtGSTNo.Name = "txtGSTNo";
            this.txtGSTNo.Size = new System.Drawing.Size(134, 26);
            this.txtGSTNo.TabIndex = 4;
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.dgvItems.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvItems.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSrNo,
            this.colParticulars,
            this.colUnit,
            this.colQty,
            this.colRate,
            this.colAmount});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItems.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvItems.GridColor = System.Drawing.Color.Silver;
            this.dgvItems.Location = new System.Drawing.Point(260, 142);
            this.dgvItems.Margin = new System.Windows.Forms.Padding(2);
            this.dgvItems.Name = "dgvItems";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.LawnGreen;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItems.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvItems.RowHeadersWidth = 62;
            this.dgvItems.RowTemplate.Height = 28;
            this.dgvItems.Size = new System.Drawing.Size(975, 291);
            this.dgvItems.TabIndex = 7;
            this.dgvItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellContentClick);
            // 
            // colSrNo
            // 
            this.colSrNo.HeaderText = "Sr.No";
            this.colSrNo.MinimumWidth = 8;
            this.colSrNo.Name = "colSrNo";
            this.colSrNo.Width = 60;
            // 
            // colParticulars
            // 
            this.colParticulars.HeaderText = "Particulars";
            this.colParticulars.MinimumWidth = 8;
            this.colParticulars.Name = "colParticulars";
            this.colParticulars.Width = 350;
            // 
            // colUnit
            // 
            this.colUnit.HeaderText = "Unit";
            this.colUnit.MinimumWidth = 8;
            this.colUnit.Name = "colUnit";
            this.colUnit.Width = 90;
            // 
            // colQty
            // 
            this.colQty.HeaderText = "Qty";
            this.colQty.MinimumWidth = 8;
            this.colQty.Name = "colQty";
            this.colQty.Width = 90;
            // 
            // colRate
            // 
            this.colRate.HeaderText = "Rate";
            this.colRate.MinimumWidth = 8;
            this.colRate.Name = "colRate";
            this.colRate.Width = 150;
            // 
            // colAmount
            // 
            this.colAmount.HeaderText = "Amount";
            this.colAmount.MinimumWidth = 8;
            this.colAmount.Name = "colAmount";
            this.colAmount.Width = 150;
            // 
            // rtbTerms
            // 
            this.rtbTerms.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbTerms.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbTerms.Location = new System.Drawing.Point(2, 40);
            this.rtbTerms.Margin = new System.Windows.Forms.Padding(2);
            this.rtbTerms.Name = "rtbTerms";
            this.rtbTerms.Size = new System.Drawing.Size(221, 60);
            this.rtbTerms.TabIndex = 11;
            this.rtbTerms.Text = "";
            this.rtbTerms.TextChanged += new System.EventHandler(this.rtbTerms_TextChanged);
            // 
            // txtAddress
            // 
            this.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress.Location = new System.Drawing.Point(163, 67);
            this.txtAddress.Margin = new System.Windows.Forms.Padding(2);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(278, 26);
            this.txtAddress.TabIndex = 21;
            // 
            // lblSubtotal
            // 
            this.lblSubtotal.AutoSize = true;
            this.lblSubtotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblSubtotal.Location = new System.Drawing.Point(857, 12);
            this.lblSubtotal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new System.Drawing.Size(49, 20);
            this.lblSubtotal.TabIndex = 8;
            this.lblSubtotal.Text = "Total";
            // 
            // enquriy
            // 
            this.enquriy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.enquriy.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enquriy.Location = new System.Drawing.Point(807, 68);
            this.enquriy.Margin = new System.Windows.Forms.Padding(2);
            this.enquriy.Name = "enquriy";
            this.enquriy.Size = new System.Drawing.Size(161, 26);
            this.enquriy.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 24);
            this.label1.TabIndex = 23;
            this.label1.Text = "Kind Attn. :";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(2, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 24);
            this.label2.TabIndex = 24;
            this.label2.Text = "Address";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(2, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 24);
            this.label3.TabIndex = 25;
            this.label3.Text = "Companyname";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(720, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 24);
            this.label4.TabIndex = 26;
            this.label4.Text = "QutNo  ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(720, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 24);
            this.label5.TabIndex = 27;
            this.label5.Text = "Date     ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(720, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 24);
            this.label6.TabIndex = 28;
            this.label6.Text = "Enquiry";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(1, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 24);
            this.label7.TabIndex = 29;
            this.label7.Text = "GSTNO";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkSlateGray;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnChooseLogo);
            this.panel1.Controls.Add(this.btnChooseSignature);
            this.panel1.Location = new System.Drawing.Point(3, -5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(194, 753);
            this.panel1.TabIndex = 30;
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
            this.button1.Image = global::QuotationApp.Properties.Resources.folder;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(29, 173);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 42);
            this.button1.TabIndex = 34;
            this.button1.Text = "       PDF";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::QuotationApp.Properties.Resources.back;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(0, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 57);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Image = global::QuotationApp.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(29, 107);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(145, 42);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "          Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.btnPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Image = global::QuotationApp.Properties.Resources.printer1;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(29, 236);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(2);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(145, 42);
            this.btnPrint.TabIndex = 19;
            this.btnPrint.Text = "          Print";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click_1);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Image = global::QuotationApp.Properties.Resources.dust;
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.Location = new System.Drawing.Point(29, 311);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(145, 39);
            this.btnClear.TabIndex = 17;
            this.btnClear.Text = "          clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnChooseLogo
            // 
            this.btnChooseLogo.BackColor = System.Drawing.Color.Transparent;
            this.btnChooseLogo.FlatAppearance.BorderSize = 0;
            this.btnChooseLogo.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.btnChooseLogo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnChooseLogo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnChooseLogo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseLogo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseLogo.ForeColor = System.Drawing.Color.White;
            this.btnChooseLogo.Image = global::QuotationApp.Properties.Resources.photo;
            this.btnChooseLogo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChooseLogo.Location = new System.Drawing.Point(29, 378);
            this.btnChooseLogo.Margin = new System.Windows.Forms.Padding(2);
            this.btnChooseLogo.Name = "btnChooseLogo";
            this.btnChooseLogo.Size = new System.Drawing.Size(145, 38);
            this.btnChooseLogo.TabIndex = 15;
            this.btnChooseLogo.Text = "          Logo";
            this.btnChooseLogo.UseVisualStyleBackColor = false;
            this.btnChooseLogo.Click += new System.EventHandler(this.btnChooseLogo_Click);
            // 
            // btnChooseSignature
            // 
            this.btnChooseSignature.BackColor = System.Drawing.Color.Transparent;
            this.btnChooseSignature.FlatAppearance.BorderSize = 0;
            this.btnChooseSignature.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.btnChooseSignature.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnChooseSignature.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnChooseSignature.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseSignature.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseSignature.ForeColor = System.Drawing.Color.White;
            this.btnChooseSignature.Image = global::QuotationApp.Properties.Resources.signature;
            this.btnChooseSignature.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChooseSignature.Location = new System.Drawing.Point(29, 442);
            this.btnChooseSignature.Margin = new System.Windows.Forms.Padding(2);
            this.btnChooseSignature.Name = "btnChooseSignature";
            this.btnChooseSignature.Size = new System.Drawing.Size(145, 42);
            this.btnChooseSignature.TabIndex = 20;
            this.btnChooseSignature.Text = "          sig";
            this.btnChooseSignature.UseVisualStyleBackColor = false;
            this.btnChooseSignature.Click += new System.EventHandler(this.btnChooseSignature_Click);
            // 
            // picLogo
            // 
            this.picLogo.Location = new System.Drawing.Point(315, 28);
            this.picLogo.Margin = new System.Windows.Forms.Padding(2);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(145, 108);
            this.picLogo.TabIndex = 13;
            this.picLogo.TabStop = false;
            // 
            // picSignature
            // 
            this.picSignature.Location = new System.Drawing.Point(835, 40);
            this.picSignature.Margin = new System.Windows.Forms.Padding(2);
            this.picSignature.Name = "picSignature";
            this.picSignature.Size = new System.Drawing.Size(133, 58);
            this.picSignature.TabIndex = 12;
            this.picSignature.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.panel2.Controls.Add(this.dtpDate);
            this.panel2.Controls.Add(this.txtQuotationNo);
            this.panel2.Controls.Add(this.enquriy);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtAddress);
            this.panel2.Controls.Add(this.txtCustomer);
            this.panel2.Controls.Add(this.txtKindAttn);
            this.panel2.Location = new System.Drawing.Point(260, 18);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(975, 120);
            this.panel2.TabIndex = 31;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.dtpChallanDate);
            this.panel3.Controls.Add(this.txtChallanNo);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.txtRGPDate);
            this.panel3.Controls.Add(this.txtRGPNo);
            this.panel3.Controls.Add(this.rtbTerms);
            this.panel3.Controls.Add(this.txtGSTNo);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.picSignature);
            this.panel3.Controls.Add(this.lblSubtotal);
            this.panel3.Location = new System.Drawing.Point(260, 437);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(975, 116);
            this.panel3.TabIndex = 32;
            // 
            // dtpChallanDate
            // 
            this.dtpChallanDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dtpChallanDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpChallanDate.Location = new System.Drawing.Point(638, 71);
            this.dtpChallanDate.Margin = new System.Windows.Forms.Padding(2);
            this.dtpChallanDate.Name = "dtpChallanDate";
            this.dtpChallanDate.Size = new System.Drawing.Size(134, 26);
            this.dtpChallanDate.TabIndex = 35;
            // 
            // txtChallanNo
            // 
            this.txtChallanNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtChallanNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChallanNo.Location = new System.Drawing.Point(638, 34);
            this.txtChallanNo.Margin = new System.Windows.Forms.Padding(2);
            this.txtChallanNo.Name = "txtChallanNo";
            this.txtChallanNo.Size = new System.Drawing.Size(134, 26);
            this.txtChallanNo.TabIndex = 34;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(246, 73);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 24);
            this.label9.TabIndex = 33;
            this.label9.Text = "RGP.Date";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(246, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 24);
            this.label8.TabIndex = 32;
            this.label8.Text = "RGP.NO";
            // 
            // txtRGPDate
            // 
            this.txtRGPDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRGPDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRGPDate.Location = new System.Drawing.Point(360, 71);
            this.txtRGPDate.Margin = new System.Windows.Forms.Padding(2);
            this.txtRGPDate.Name = "txtRGPDate";
            this.txtRGPDate.Size = new System.Drawing.Size(134, 26);
            this.txtRGPDate.TabIndex = 31;
            // 
            // txtRGPNo
            // 
            this.txtRGPNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRGPNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRGPNo.Location = new System.Drawing.Point(360, 34);
            this.txtRGPNo.Margin = new System.Windows.Forms.Padding(2);
            this.txtRGPNo.Name = "txtRGPNo";
            this.txtRGPNo.Size = new System.Drawing.Size(134, 26);
            this.txtRGPNo.TabIndex = 30;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(510, 76);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(128, 24);
            this.label10.TabIndex = 37;
            this.label10.Text = "Challan.Date";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(510, 36);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(113, 24);
            this.label11.TabIndex = 36;
            this.label11.Text = "Challan.No";
            // 
            // QuotationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1239, 621);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dgvItems);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.picLogo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "QuotationForm";
            this.Text = "Quot";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.QuotationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSignature)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Example: Check if a button column was clicked
            if (e.ColumnIndex == 2 && e.RowIndex >= 0) // Assuming button is in column index 2
            {
                MessageBox.Show("Button clicked in row " + e.RowIndex);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.TextBox txtQuotationNo;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.TextBox txtKindAttn;
        private System.Windows.Forms.TextBox txtGSTNo;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.RichTextBox rtbTerms;
        private System.Windows.Forms.PictureBox picSignature;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnChooseLogo;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnChooseSignature;
        private TextBox txtAddress;
        private Label lblSubtotal;
        private TextBox enquriy;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Panel panel1;
        private PictureBox picLogo;
        private Button btnCancel;
        private DataGridViewTextBoxColumn colSrNo;
        private DataGridViewTextBoxColumn colParticulars;
        private DataGridViewTextBoxColumn colUnit;
        private DataGridViewTextBoxColumn colQty;
        private DataGridViewTextBoxColumn colRate;
        private DataGridViewTextBoxColumn colAmount;
        private Panel panel2;
        private Panel panel3;
        private Button button1;
        private TextBox txtRGPDate;
        private TextBox txtRGPNo;
        private Label label9;
        private Label label8;
        private TextBox dtpChallanDate;
        private TextBox txtChallanNo;
        private Label label10;
        private Label label11;
    }
}