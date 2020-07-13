namespace LogMsgTest
{
    partial class example
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.BTNmainFolder = new System.Windows.Forms.Button();
            this.mainSaveFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mainLog = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.viceLog = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.viceSaveFilePath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.BTNviceFolder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BTNmainFolder
            // 
            this.BTNmainFolder.Location = new System.Drawing.Point(12, 12);
            this.BTNmainFolder.Name = "BTNmainFolder";
            this.BTNmainFolder.Size = new System.Drawing.Size(77, 34);
            this.BTNmainFolder.TabIndex = 0;
            this.BTNmainFolder.Text = "Select FolderPath";
            this.BTNmainFolder.UseVisualStyleBackColor = true;
            this.BTNmainFolder.Click += new System.EventHandler(this.button1_Click);
            // 
            // mainSaveFilePath
            // 
            this.mainSaveFilePath.Location = new System.Drawing.Point(155, 20);
            this.mainSaveFilePath.Name = "mainSaveFilePath";
            this.mainSaveFilePath.Size = new System.Drawing.Size(605, 21);
            this.mainSaveFilePath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "保存路径";
            // 
            // mainLog
            // 
            this.mainLog.Location = new System.Drawing.Point(155, 65);
            this.mainLog.Multiline = true;
            this.mainLog.Name = "mainLog";
            this.mainLog.Size = new System.Drawing.Size(605, 145);
            this.mainLog.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(156, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "主运行信息";
            // 
            // viceLog
            // 
            this.viceLog.Location = new System.Drawing.Point(155, 273);
            this.viceLog.Multiline = true;
            this.viceLog.Name = "viceLog";
            this.viceLog.Size = new System.Drawing.Size(605, 145);
            this.viceLog.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(156, 258);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "副运行信息";
            // 
            // viceSaveFilePath
            // 
            this.viceSaveFilePath.Location = new System.Drawing.Point(155, 234);
            this.viceSaveFilePath.Name = "viceSaveFilePath";
            this.viceSaveFilePath.Size = new System.Drawing.Size(605, 21);
            this.viceSaveFilePath.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(98, 239);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "保存路径";
            // 
            // BTNviceFolder
            // 
            this.BTNviceFolder.Location = new System.Drawing.Point(15, 228);
            this.BTNviceFolder.Name = "BTNviceFolder";
            this.BTNviceFolder.Size = new System.Drawing.Size(77, 34);
            this.BTNviceFolder.TabIndex = 8;
            this.BTNviceFolder.Text = "Select FolderPath";
            this.BTNviceFolder.UseVisualStyleBackColor = true;
            this.BTNviceFolder.Click += new System.EventHandler(this.BTNviceFolder_Click);
            // 
            // example
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BTNviceFolder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.viceSaveFilePath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.viceLog);
            this.Controls.Add(this.mainLog);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mainSaveFilePath);
            this.Controls.Add(this.BTNmainFolder);
            this.Name = "example";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.example_FormClosed);
            this.Load += new System.EventHandler(this.example_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BTNmainFolder;
        private System.Windows.Forms.TextBox mainSaveFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox mainLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox viceLog;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox viceSaveFilePath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BTNviceFolder;
    }
}

