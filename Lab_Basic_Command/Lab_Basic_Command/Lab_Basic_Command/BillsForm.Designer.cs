namespace Lab_Basic_Command
{
    partial class BillsForm
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
            this.btninbill = new System.Windows.Forms.Button();
            this.lvBills = new System.Windows.Forms.ListView();
            this.chBillid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDateCreate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.lbthucthu = new System.Windows.Forms.Label();
            this.lbtonggiam = new System.Windows.Forms.Label();
            this.lbtongtruoc = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLoadBill = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.txtTongTruocGiam = new System.Windows.Forms.TextBox();
            this.txtTongGiam = new System.Windows.Forms.TextBox();
            this.txtThucThu = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btninbill
            // 
            this.btninbill.Location = new System.Drawing.Point(696, 95);
            this.btninbill.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btninbill.Name = "btninbill";
            this.btninbill.Size = new System.Drawing.Size(112, 35);
            this.btninbill.TabIndex = 16;
            this.btninbill.Text = "In Bill";
            this.btninbill.UseVisualStyleBackColor = true;
            // 
            // lvBills
            // 
            this.lvBills.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chBillid,
            this.chDateCreate,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.lvBills.FullRowSelect = true;
            this.lvBills.HideSelection = false;
            this.lvBills.Location = new System.Drawing.Point(47, 157);
            this.lvBills.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lvBills.Name = "lvBills";
            this.lvBills.Size = new System.Drawing.Size(1056, 244);
            this.lvBills.TabIndex = 15;
            this.lvBills.UseCompatibleStateImageBehavior = false;
            this.lvBills.View = System.Windows.Forms.View.Details;
            this.lvBills.SelectedIndexChanged += new System.EventHandler(this.lvBills_SelectedIndexChanged);
            this.lvBills.DoubleClick += new System.EventHandler(this.lvBills_SelectedIndexChanged);
            // 
            // chBillid
            // 
            this.chBillid.Text = "ID";
            // 
            // chDateCreate
            // 
            this.chDateCreate.Text = "Tên hóa đơn";
            this.chDateCreate.Width = 115;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Bàn";
            this.columnHeader1.Width = 139;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Thành tiền";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Giảm giá";
            this.columnHeader3.Width = 123;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Thuế ";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Trạng thái";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Ngày thanh toán";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Tài khoản";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(340, 103);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "To:";
            // 
            // lbthucthu
            // 
            this.lbthucthu.AutoSize = true;
            this.lbthucthu.Location = new System.Drawing.Point(712, 440);
            this.lbthucthu.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbthucthu.Name = "lbthucthu";
            this.lbthucthu.Size = new System.Drawing.Size(75, 20);
            this.lbthucthu.TabIndex = 11;
            this.lbthucthu.Text = "Thực thu:";
            // 
            // lbtonggiam
            // 
            this.lbtonggiam.AutoSize = true;
            this.lbtonggiam.Location = new System.Drawing.Point(458, 440);
            this.lbtonggiam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbtonggiam.Name = "lbtonggiam";
            this.lbtonggiam.Size = new System.Drawing.Size(87, 20);
            this.lbtonggiam.TabIndex = 12;
            this.lbtonggiam.Text = "Tổng giảm:";
            // 
            // lbtongtruoc
            // 
            this.lbtongtruoc.AutoSize = true;
            this.lbtongtruoc.Location = new System.Drawing.Point(152, 440);
            this.lbtongtruoc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbtongtruoc.Name = "lbtongtruoc";
            this.lbtongtruoc.Size = new System.Drawing.Size(127, 20);
            this.lbtongtruoc.TabIndex = 13;
            this.lbtongtruoc.Text = "Tổng trước giảm:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(152, 103);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "From:";
            // 
            // btnLoadBill
            // 
            this.btnLoadBill.Location = new System.Drawing.Point(574, 94);
            this.btnLoadBill.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLoadBill.Name = "btnLoadBill";
            this.btnLoadBill.Size = new System.Drawing.Size(112, 35);
            this.btnLoadBill.TabIndex = 9;
            this.btnLoadBill.Text = "LoadBill";
            this.btnLoadBill.UseVisualStyleBackColor = true;
            this.btnLoadBill.Click += new System.EventHandler(this.btnLoadBill_Click);
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(384, 94);
            this.dtpTo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(126, 26);
            this.dtpTo.TabIndex = 8;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(211, 94);
            this.dtpFrom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(118, 26);
            this.dtpFrom.TabIndex = 7;
            this.dtpFrom.Value = new System.DateTime(2025, 10, 10, 10, 19, 0, 0);
            // 
            // txtTongTruocGiam
            // 
            this.txtTongTruocGiam.Location = new System.Drawing.Point(286, 434);
            this.txtTongTruocGiam.Name = "txtTongTruocGiam";
            this.txtTongTruocGiam.Size = new System.Drawing.Size(145, 26);
            this.txtTongTruocGiam.TabIndex = 17;
            // 
            // txtTongGiam
            // 
            this.txtTongGiam.Location = new System.Drawing.Point(541, 434);
            this.txtTongGiam.Name = "txtTongGiam";
            this.txtTongGiam.Size = new System.Drawing.Size(145, 26);
            this.txtTongGiam.TabIndex = 17;
            // 
            // txtThucThu
            // 
            this.txtThucThu.Location = new System.Drawing.Point(782, 434);
            this.txtThucThu.Name = "txtThucThu";
            this.txtThucThu.Size = new System.Drawing.Size(145, 26);
            this.txtThucThu.TabIndex = 17;
            // 
            // BillsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 555);
            this.Controls.Add(this.txtThucThu);
            this.Controls.Add(this.txtTongGiam);
            this.Controls.Add(this.txtTongTruocGiam);
            this.Controls.Add(this.btninbill);
            this.Controls.Add(this.lvBills);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbthucthu);
            this.Controls.Add(this.lbtonggiam);
            this.Controls.Add(this.lbtongtruoc);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLoadBill);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Name = "BillsForm";
            this.Text = "BillsForm";
            this.Load += new System.EventHandler(this.BillsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btninbill;
        private System.Windows.Forms.ListView lvBills;
        private System.Windows.Forms.ColumnHeader chBillid;
        private System.Windows.Forms.ColumnHeader chDateCreate;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbthucthu;
        private System.Windows.Forms.Label lbtonggiam;
        private System.Windows.Forms.Label lbtongtruoc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLoadBill;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.TextBox txtTongTruocGiam;
        private System.Windows.Forms.TextBox txtTongGiam;
        private System.Windows.Forms.TextBox txtThucThu;
    }
}