namespace MangaDl
{
    partial class Form1
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
            this.loadButton = new System.Windows.Forms.Button();
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.urlLabel = new System.Windows.Forms.Label();
            this.downloadLabel = new System.Windows.Forms.Label();
            this.dlpathTextbox = new System.Windows.Forms.TextBox();
            this.selectFolderButton = new System.Windows.Forms.Button();
            this.chaptersListview = new System.Windows.Forms.ListView();
            this.downloadButton = new System.Windows.Forms.Button();
            this.downloadAllButton = new System.Windows.Forms.Button();
            this.searchListview = new System.Windows.Forms.ListView();
            this.searchButton = new System.Windows.Forms.Button();
            this.openFolderButton = new System.Windows.Forms.Button();
            this.validateButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.siteSelectCombobox = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.threadLimitButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.threadLimitTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.queueListview = new System.Windows.Forms.ListView();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // loadButton
            // 
            this.loadButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.loadButton.Location = new System.Drawing.Point(314, 74);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(96, 23);
            this.loadButton.TabIndex = 0;
            this.loadButton.Text = "Load Chapters";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // urlTextBox
            // 
            this.urlTextBox.Location = new System.Drawing.Point(9, 19);
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(299, 20);
            this.urlTextBox.TabIndex = 1;
            this.urlTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.urlTextBox_KeyPress);
            // 
            // urlLabel
            // 
            this.urlLabel.AutoSize = true;
            this.urlLabel.Location = new System.Drawing.Point(6, 3);
            this.urlLabel.Name = "urlLabel";
            this.urlLabel.Size = new System.Drawing.Size(77, 13);
            this.urlLabel.TabIndex = 3;
            this.urlLabel.Text = "Search Manga";
            // 
            // downloadLabel
            // 
            this.downloadLabel.AutoSize = true;
            this.downloadLabel.Location = new System.Drawing.Point(413, 3);
            this.downloadLabel.Name = "downloadLabel";
            this.downloadLabel.Size = new System.Drawing.Size(67, 13);
            this.downloadLabel.TabIndex = 4;
            this.downloadLabel.Text = "Download to";
            // 
            // dlpathTextbox
            // 
            this.dlpathTextbox.Location = new System.Drawing.Point(416, 19);
            this.dlpathTextbox.Name = "dlpathTextbox";
            this.dlpathTextbox.ReadOnly = true;
            this.dlpathTextbox.Size = new System.Drawing.Size(450, 20);
            this.dlpathTextbox.TabIndex = 5;
            // 
            // selectFolderButton
            // 
            this.selectFolderButton.Location = new System.Drawing.Point(416, 45);
            this.selectFolderButton.Name = "selectFolderButton";
            this.selectFolderButton.Size = new System.Drawing.Size(96, 23);
            this.selectFolderButton.TabIndex = 6;
            this.selectFolderButton.Text = "Select folder";
            this.selectFolderButton.UseVisualStyleBackColor = true;
            this.selectFolderButton.Click += new System.EventHandler(this.selectFolderButton_Click);
            // 
            // chaptersListview
            // 
            this.chaptersListview.Location = new System.Drawing.Point(416, 74);
            this.chaptersListview.Name = "chaptersListview";
            this.chaptersListview.Size = new System.Drawing.Size(450, 445);
            this.chaptersListview.TabIndex = 7;
            this.chaptersListview.UseCompatibleStateImageBehavior = false;
            // 
            // downloadButton
            // 
            this.downloadButton.Location = new System.Drawing.Point(872, 74);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(96, 23);
            this.downloadButton.TabIndex = 8;
            this.downloadButton.Text = "Download";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // downloadAllButton
            // 
            this.downloadAllButton.Location = new System.Drawing.Point(872, 103);
            this.downloadAllButton.Name = "downloadAllButton";
            this.downloadAllButton.Size = new System.Drawing.Size(96, 23);
            this.downloadAllButton.TabIndex = 10;
            this.downloadAllButton.Text = "Download All";
            this.downloadAllButton.UseVisualStyleBackColor = true;
            this.downloadAllButton.Click += new System.EventHandler(this.downloadAllButton_Click);
            // 
            // searchListview
            // 
            this.searchListview.Location = new System.Drawing.Point(9, 74);
            this.searchListview.Name = "searchListview";
            this.searchListview.Size = new System.Drawing.Size(299, 445);
            this.searchListview.TabIndex = 11;
            this.searchListview.UseCompatibleStateImageBehavior = false;
            this.searchListview.DoubleClick += new System.EventHandler(this.searchListview_DoubleClick);
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(9, 45);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(96, 23);
            this.searchButton.TabIndex = 12;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // openFolderButton
            // 
            this.openFolderButton.Location = new System.Drawing.Point(518, 45);
            this.openFolderButton.Name = "openFolderButton";
            this.openFolderButton.Size = new System.Drawing.Size(96, 23);
            this.openFolderButton.TabIndex = 13;
            this.openFolderButton.Text = "Open Folder";
            this.openFolderButton.UseVisualStyleBackColor = true;
            this.openFolderButton.Click += new System.EventHandler(this.openFolderButton_Click);
            // 
            // validateButton
            // 
            this.validateButton.Location = new System.Drawing.Point(872, 132);
            this.validateButton.Name = "validateButton";
            this.validateButton.Size = new System.Drawing.Size(96, 23);
            this.validateButton.TabIndex = 14;
            this.validateButton.Text = "Check Chapters";
            this.validateButton.UseVisualStyleBackColor = true;
            this.validateButton.Click += new System.EventHandler(this.validateButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1216, 567);
            this.tabControl1.TabIndex = 15;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.siteSelectCombobox);
            this.tabPage1.Controls.Add(this.urlLabel);
            this.tabPage1.Controls.Add(this.validateButton);
            this.tabPage1.Controls.Add(this.loadButton);
            this.tabPage1.Controls.Add(this.openFolderButton);
            this.tabPage1.Controls.Add(this.urlTextBox);
            this.tabPage1.Controls.Add(this.searchButton);
            this.tabPage1.Controls.Add(this.downloadLabel);
            this.tabPage1.Controls.Add(this.searchListview);
            this.tabPage1.Controls.Add(this.dlpathTextbox);
            this.tabPage1.Controls.Add(this.downloadAllButton);
            this.tabPage1.Controls.Add(this.selectFolderButton);
            this.tabPage1.Controls.Add(this.downloadButton);
            this.tabPage1.Controls.Add(this.chaptersListview);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1208, 541);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Search";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // siteSelectCombobox
            // 
            this.siteSelectCombobox.FormattingEnabled = true;
            this.siteSelectCombobox.Location = new System.Drawing.Point(111, 45);
            this.siteSelectCombobox.Name = "siteSelectCombobox";
            this.siteSelectCombobox.Size = new System.Drawing.Size(197, 21);
            this.siteSelectCombobox.TabIndex = 16;
            this.siteSelectCombobox.SelectedIndexChanged += new System.EventHandler(this.siteSelectCombobox_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.threadLimitButton);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.threadLimitTextBox);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.queueListview);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1208, 541);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Download Queue";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // threadLimitButton
            // 
            this.threadLimitButton.Location = new System.Drawing.Point(574, 33);
            this.threadLimitButton.Name = "threadLimitButton";
            this.threadLimitButton.Size = new System.Drawing.Size(96, 23);
            this.threadLimitButton.TabIndex = 4;
            this.threadLimitButton.Text = "Set Thread Limit";
            this.threadLimitButton.UseVisualStyleBackColor = true;
            this.threadLimitButton.Click += new System.EventHandler(this.threadLimitButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(465, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Thread limit";
            // 
            // threadLimitTextBox
            // 
            this.threadLimitTextBox.Location = new System.Drawing.Point(468, 35);
            this.threadLimitTextBox.Name = "threadLimitTextBox";
            this.threadLimitTextBox.Size = new System.Drawing.Size(100, 20);
            this.threadLimitTextBox.TabIndex = 2;
            this.threadLimitTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.threadLimitTextBox_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Download Queue";
            // 
            // queueListview
            // 
            this.queueListview.Location = new System.Drawing.Point(9, 19);
            this.queueListview.Name = "queueListview";
            this.queueListview.Size = new System.Drawing.Size(450, 516);
            this.queueListview.TabIndex = 0;
            this.queueListview.UseCompatibleStateImageBehavior = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1240, 591);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Manga Downloader";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.Label urlLabel;
        private System.Windows.Forms.Label downloadLabel;
        private System.Windows.Forms.TextBox dlpathTextbox;
        private System.Windows.Forms.Button selectFolderButton;
        private System.Windows.Forms.ListView chaptersListview;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.Button downloadAllButton;
        private System.Windows.Forms.ListView searchListview;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Button openFolderButton;
        private System.Windows.Forms.Button validateButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView queueListview;
        private System.Windows.Forms.Button threadLimitButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox threadLimitTextBox;
        private System.Windows.Forms.ComboBox siteSelectCombobox;
    }
}

