using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myDewControllerPro3
{
    public partial class ErrorLogPathName : Form
    {
        public string fullpath;
        public string filepath;
        public string filename;

        public ErrorLogPathName()
        {
            InitializeComponent();
            // Inserted code into this for handling window resizing
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            // this.MinimizeBox = false;  // do not as user cannot minimize form!!!
        }

        private void ErrorLogPathName_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.ErrorFormLocation = this.Location;
            Properties.Settings.Default.Save();

            if (filepath == "")
            {
                MessageBox.Show("Please specify a Folder", "No folder selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // cancel the form closing
                e.Cancel = true;
            }
            else
            {
                Properties.Settings.Default.SubFormRunning = false;
                Properties.Settings.Default.Save();
            }
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            if (filepath == "")
            {
                MessageBox.Show("Please specify a Folder", "No folder selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Properties.Settings.Default.SubFormRunning = false;
                Properties.Settings.Default.Save();
                Close();
            }
        }

        private void ErrorLogPathName_Load(object sender, EventArgs e)
        {
            this.Location = Properties.Settings.Default.ErrorFormLocation;

            filepath = Properties.Settings.Default.errorlogpath;
            filename = Properties.Settings.Default.ErrorLogName;
            fullpath = filepath + "\\" + filename;
            PathnameTxtBox.Text = fullpath;
            PathnameTxtBox.Update();
            filenametxtbox.Text = filename;
            filenametxtbox.Update();
            foldernametxtbox.Text = filepath;
            foldernametxtbox.Update();
            Properties.Settings.Default.Save();
        }

        private void SetDirectoryBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd2 = new FolderBrowserDialog();
            if (fd2.ShowDialog() == DialogResult.OK)
            {
                filepath = fd2.SelectedPath;
                fullpath = filepath + "\\" + filename;
                PathnameTxtBox.Text = fullpath;
                PathnameTxtBox.Update();
                filenametxtbox.Text = filename;
                filenametxtbox.Update();
                foldernametxtbox.Text = filepath;
                foldernametxtbox.Update();

                Properties.Settings.Default.errorlogpath = filepath;
                Properties.Settings.Default.ErrorLogName = filename;
                Properties.Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("Folder not specified, default to C:\\", "No folder selected", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                filepath = "C:\\";
                fullpath = filepath + filename;
                PathnameTxtBox.Text = fullpath;
                PathnameTxtBox.Update();
                filenametxtbox.Text = filename;
                filenametxtbox.Update();
                foldernametxtbox.Text = filepath;
                foldernametxtbox.Update();

                Properties.Settings.Default.errorlogpath = filepath;
                Properties.Settings.Default.ErrorLogName = filename;
                Properties.Settings.Default.Save();
            }
        }
    }
}
