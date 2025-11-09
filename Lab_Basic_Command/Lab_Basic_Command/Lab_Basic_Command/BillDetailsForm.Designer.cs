namespace Lab_Basic_Command
{
    partial class BillDetailsForm
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
            this.lvBillDetails = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnCLose = new System.Windows.Forms.Button();
            this.btnTai = new System.Windows.Forms.Button();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvBillDetails
            // 
            this.lvBillDetails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvBillDetails.HideSelection = false;
            this.lvBillDetails.Location = new System.Drawing.Point(58, 59);
            this.lvBillDetails.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lvBillDetails.Name = "lvBillDetails";
            this.lvBillDetails.Size = new System.Drawing.Size(664, 269);
            this.lvBillDetails.TabIndex = 6;
            this.lvBillDetails.UseCompatibleStateImageBehavior = false;
            this.lvBillDetails.View = System.Windows.Forms.View.Details;
            this.lvBillDetails.SelectedIndexChanged += new System.EventHandler(this.lvBillDetails_SelectedIndexChanged);
            this.lvBillDetails.DoubleClick += new System.EventHandler(this.lvBillDetails_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Món ăn";
            this.columnHeader1.Width = 98;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Số lượng";
            this.columnHeader2.Width = 124;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Giá";
            this.columnHeader3.Width = 86;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Thành tiền";
            this.columnHeader4.Width = 120;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(54, 371);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(83, 20);
            this.lblTotal.TabIndex = 5;
            this.lblTotal.Text = "Tổng tiền: ";
            // 
            // btnCLose
            // 
            this.btnCLose.Location = new System.Drawing.Point(634, 356);
            this.btnCLose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCLose.Name = "btnCLose";
            this.btnCLose.Size = new System.Drawing.Size(112, 35);
            this.btnCLose.TabIndex = 4;
            this.btnCLose.Text = "Close";
            this.btnCLose.UseVisualStyleBackColor = true;
            // 
            // btnTai
            // 
            this.btnTai.Location = new System.Drawing.Point(487, 360);
            this.btnTai.Name = "btnTai";
            this.btnTai.Size = new System.Drawing.Size(101, 31);
            this.btnTai.TabIndex = 7;
            this.btnTai.Text = "Tải dữ liệu";
            this.btnTai.UseVisualStyleBackColor = true;
            this.btnTai.Click += new System.EventHandler(this.btnTai_Click);
            // 
            // lblTongTien
            // 
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Location = new System.Drawing.Point(144, 371);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(13, 20);
            this.lblTongTien.TabIndex = 8;
            this.lblTongTien.Text = ".";
            // 
            // BillDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 486);
            this.Controls.Add(this.lblTongTien);
            this.Controls.Add(this.btnTai);
            this.Controls.Add(this.lvBillDetails);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnCLose);
            this.Name = "BillDetailsForm";
            this.Text = "BillDetailsForm";
            this.Load += new System.EventHandler(this.BillDetailsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvBillDetails;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnCLose;
        private System.Windows.Forms.Button btnTai;
        private System.Windows.Forms.Label lblTongTien;
    }
}