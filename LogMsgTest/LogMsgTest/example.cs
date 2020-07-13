using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using FormLog;

namespace LogMsgTest
{
    public partial class example : Form
    {
        System.Timers.Timer mytimer = new System.Timers.Timer(100);

        public example()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            if(folderBrowserDialog.SelectedPath!=null)
            {
                mainSaveFilePath.Text = folderBrowserDialog.SelectedPath;
                FormLogMsg.RedirectFilePath(folderBrowserDialog.SelectedPath);
            }
        }

        private void example_Load(object sender, EventArgs e)
        {
            FormLog.FormLogMsg.AddCtrl(mainLog, null);
            FormLog.FormLogMsg.AddCtrl(viceLog, "second", null);
            mytimer.Elapsed += Mytimer_Elapsed;
            mytimer.AutoReset = true;
            mytimer.Enabled = true;
        }

        private void Mytimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            FormLogMsg.LogMsgShow(DateTime.Now.ToLongDateString() + "Hello,World!");
            FormLogMsg.LogMsgShow("second", DateTime.Now.ToLongDateString() + "Hello,World!");
        }

        private void BTNviceFolder_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            //folderBrowserDialog.ShowDialog();
            //if (folderBrowserDialog.SelectedPath != null)
            //{
            //    viceSaveFilePath.Text = folderBrowserDialog.SelectedPath;
            //    FormLogMsg.RedirectFilePath("second", folderBrowserDialog.SelectedPath);
            //}
            System.Diagnostics.Process.Start("explorer.exe", mainSaveFilePath.Text);

        }

        private void example_FormClosed(object sender, FormClosedEventArgs e)
        {
            mytimer.Close();
            FormLogMsg.Dispose();
        }
    }
}
