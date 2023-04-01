namespace TestApp
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnGetInfo = new Button();
            txtAuthor = new TextBox();
            txtSeriesName = new TextBox();
            txtBookNumber = new TextBox();
            label4 = new Label();
            txtJSON = new TextBox();
            txtTitle = new TextBox();
            txtFilename = new TextBox();
            label7 = new Label();
            txtNumberOfChapters = new TextBox();
            label6 = new Label();
            label5 = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            btnShowChapters = new Button();
            SuspendLayout();
            // 
            // btnGetInfo
            // 
            btnGetInfo.Location = new Point(12, 12);
            btnGetInfo.Name = "btnGetInfo";
            btnGetInfo.Size = new Size(150, 46);
            btnGetInfo.TabIndex = 0;
            btnGetInfo.Text = "Get Info";
            btnGetInfo.UseVisualStyleBackColor = true;
            btnGetInfo.Click += BtnGetInfo_Click;
            // 
            // txtAuthor
            // 
            txtAuthor.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtAuthor.Location = new Point(131, 154);
            txtAuthor.Name = "txtAuthor";
            txtAuthor.Size = new Size(444, 39);
            txtAuthor.TabIndex = 2;
            // 
            // txtSeriesName
            // 
            txtSeriesName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSeriesName.Location = new Point(131, 199);
            txtSeriesName.Name = "txtSeriesName";
            txtSeriesName.Size = new Size(444, 39);
            txtSeriesName.TabIndex = 2;
            // 
            // txtBookNumber
            // 
            txtBookNumber.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBookNumber.Location = new Point(131, 244);
            txtBookNumber.Name = "txtBookNumber";
            txtBookNumber.Size = new Size(444, 39);
            txtBookNumber.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(48, 337);
            label4.Name = "label4";
            label4.Size = new Size(77, 32);
            label4.TabIndex = 1;
            label4.Text = "JSON:";
            // 
            // txtJSON
            // 
            txtJSON.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtJSON.Location = new Point(131, 337);
            txtJSON.Multiline = true;
            txtJSON.Name = "txtJSON";
            txtJSON.ScrollBars = ScrollBars.Both;
            txtJSON.Size = new Size(444, 352);
            txtJSON.TabIndex = 2;
            // 
            // txtTitle
            // 
            txtTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTitle.Location = new Point(131, 109);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(444, 39);
            txtTitle.TabIndex = 2;
            // 
            // txtFilename
            // 
            txtFilename.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtFilename.Location = new Point(131, 64);
            txtFilename.Name = "txtFilename";
            txtFilename.Size = new Size(444, 39);
            txtFilename.TabIndex = 2;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 292);
            label7.Name = "label7";
            label7.Size = new Size(113, 32);
            label7.TabIndex = 1;
            label7.Text = "Chapters:";
            // 
            // txtNumberOfChapters
            // 
            txtNumberOfChapters.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtNumberOfChapters.Location = new Point(131, 289);
            txtNumberOfChapters.Name = "txtNumberOfChapters";
            txtNumberOfChapters.Size = new Size(118, 39);
            txtNumberOfChapters.TabIndex = 2;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(69, 67);
            label6.Name = "label6";
            label6.Size = new Size(56, 32);
            label6.TabIndex = 1;
            label6.Text = "File:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(60, 112);
            label5.Name = "label5";
            label5.Size = new Size(65, 32);
            label5.TabIndex = 1;
            label5.Text = "Title:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(33, 157);
            label1.Name = "label1";
            label1.Size = new Size(92, 32);
            label1.TabIndex = 1;
            label1.Text = "Author:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(43, 202);
            label2.Name = "label2";
            label2.Size = new Size(82, 32);
            label2.TabIndex = 1;
            label2.Text = "Series:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(31, 247);
            label3.Name = "label3";
            label3.Size = new Size(94, 32);
            label3.TabIndex = 1;
            label3.Text = "Book #:";
            // 
            // btnShowChapters
            // 
            btnShowChapters.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnShowChapters.Location = new Point(255, 285);
            btnShowChapters.Name = "btnShowChapters";
            btnShowChapters.Size = new Size(86, 46);
            btnShowChapters.TabIndex = 0;
            btnShowChapters.Text = "Show";
            btnShowChapters.UseVisualStyleBackColor = true;
            btnShowChapters.Click += BtnShowChapters_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(192F, 192F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(587, 701);
            Controls.Add(txtJSON);
            Controls.Add(label4);
            Controls.Add(txtNumberOfChapters);
            Controls.Add(label7);
            Controls.Add(txtBookNumber);
            Controls.Add(label3);
            Controls.Add(txtSeriesName);
            Controls.Add(label2);
            Controls.Add(txtFilename);
            Controls.Add(label6);
            Controls.Add(txtTitle);
            Controls.Add(label5);
            Controls.Add(txtAuthor);
            Controls.Add(label1);
            Controls.Add(btnShowChapters);
            Controls.Add(btnGetInfo);
            Name = "MainForm";
            Text = "Test App";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnGetInfo;
        private TextBox txtAuthor;
        private TextBox txtSeriesName;
        private TextBox txtBookNumber;
        private Label label4;
        private TextBox txtJSON;
        private TextBox txtTitle;
        private TextBox txtFilename;
        private Label label7;
        private TextBox txtNumberOfChapters;
        private Label label6;
        private Label label5;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button btnShowChapters;
    }
}