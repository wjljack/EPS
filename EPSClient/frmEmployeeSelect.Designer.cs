namespace HRSeat
{
    partial class frmEmployeeSelect
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmployeeSelect));
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tvwEmployeeSelect = new System.Windows.Forms.TreeView();
            this.ilEmployeeSelect = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSelect.Location = new System.Drawing.Point(53, 329);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(74, 28);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "确 定";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnCancel.Location = new System.Drawing.Point(148, 329);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取 消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tvwEmployeeSelect
            // 
            this.tvwEmployeeSelect.Location = new System.Drawing.Point(33, 12);
            this.tvwEmployeeSelect.Name = "tvwEmployeeSelect";
            this.tvwEmployeeSelect.Size = new System.Drawing.Size(208, 300);
            this.tvwEmployeeSelect.TabIndex = 2;
            this.tvwEmployeeSelect.DoubleClick += new System.EventHandler(this.tvwEmployeeSelect_DoubleClick);
            // 
            // ilEmployeeSelect
            // 
            this.ilEmployeeSelect.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilEmployeeSelect.ImageStream")));
            this.ilEmployeeSelect.TransparentColor = System.Drawing.Color.Transparent;
            this.ilEmployeeSelect.Images.SetKeyName(0, "Folder_12 (1)_副本.jpg");
            this.ilEmployeeSelect.Images.SetKeyName(1, "online2.png");
            this.ilEmployeeSelect.Images.SetKeyName(2, "online.png");
            // 
            // frmEmployeeSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(276, 364);
            this.Controls.Add(this.tvwEmployeeSelect);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEmployeeSelect";
            this.ShowIcon = false;
            this.Text = "选择员工";
            this.Load += new System.EventHandler(this.frmEmployeeSelect_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TreeView tvwEmployeeSelect;
        private System.Windows.Forms.ImageList ilEmployeeSelect;
    }
}