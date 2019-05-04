namespace myDewControllerPro3
{
    partial class ErrorLogPathName
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorLogPathName));
            this.CloseBtn = new System.Windows.Forms.Button();
            this.PathnameTxtBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SetDirectoryBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.filenametxtbox = new System.Windows.Forms.TextBox();
            this.foldernametxtbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // CloseBtn
            // 
            this.CloseBtn.Location = new System.Drawing.Point(400, 273);
            this.CloseBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(92, 33);
            this.CloseBtn.TabIndex = 0;
            this.CloseBtn.Text = "Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // PathnameTxtBox
            // 
            this.PathnameTxtBox.Location = new System.Drawing.Point(13, 240);
            this.PathnameTxtBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PathnameTxtBox.Name = "PathnameTxtBox";
            this.PathnameTxtBox.ReadOnly = true;
            this.PathnameTxtBox.Size = new System.Drawing.Size(477, 22);
            this.PathnameTxtBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 217);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Path";
            // 
            // SetDirectoryBtn
            // 
            this.SetDirectoryBtn.Location = new System.Drawing.Point(36, 28);
            this.SetDirectoryBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SetDirectoryBtn.Name = "SetDirectoryBtn";
            this.SetDirectoryBtn.Size = new System.Drawing.Size(423, 31);
            this.SetDirectoryBtn.TabIndex = 58;
            this.SetDirectoryBtn.Text = "Click to set the folder where the error log will be stored";
            this.SetDirectoryBtn.UseVisualStyleBackColor = true;
            this.SetDirectoryBtn.Click += new System.EventHandler(this.SetDirectoryBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 92);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 17);
            this.label2.TabIndex = 59;
            this.label2.Text = "Folder";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 165);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 17);
            this.label3.TabIndex = 60;
            this.label3.Text = "Filename";
            // 
            // filenametxtbox
            // 
            this.filenametxtbox.Location = new System.Drawing.Point(13, 188);
            this.filenametxtbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.filenametxtbox.Name = "filenametxtbox";
            this.filenametxtbox.ReadOnly = true;
            this.filenametxtbox.Size = new System.Drawing.Size(224, 22);
            this.filenametxtbox.TabIndex = 61;
            // 
            // foldernametxtbox
            // 
            this.foldernametxtbox.Location = new System.Drawing.Point(13, 112);
            this.foldernametxtbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.foldernametxtbox.Name = "foldernametxtbox";
            this.foldernametxtbox.ReadOnly = true;
            this.foldernametxtbox.Size = new System.Drawing.Size(477, 22);
            this.foldernametxtbox.TabIndex = 62;
            // 
            // ErrorLogPathName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 321);
            this.Controls.Add(this.foldernametxtbox);
            this.Controls.Add(this.filenametxtbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SetDirectoryBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PathnameTxtBox);
            this.Controls.Add(this.CloseBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ErrorLogPathName";
            this.Text = "myDewControllerPro3 ErrorLogPathName";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ErrorLogPathName_FormClosing);
            this.Load += new System.EventHandler(this.ErrorLogPathName_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.TextBox PathnameTxtBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SetDirectoryBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox filenametxtbox;
        private System.Windows.Forms.TextBox foldernametxtbox;
    }
}