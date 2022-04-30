namespace Audiobook_Renamer;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnGetInfo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAuthor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSeriesName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBookNumber = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtJSON = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.btnParseDirectory = new System.Windows.Forms.Button();
            this.dgvBooks = new System.Windows.Forms.DataGridView();
            this.btnRenameMissingBookNumbers = new System.Windows.Forms.Button();
            this.btnMassRename = new System.Windows.Forms.Button();
            this.ssStatusBar = new System.Windows.Forms.StatusStrip();
            this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tspbProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tsslSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslStretchAnchor = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBooks)).BeginInit();
            this.ssStatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGetInfo
            // 
            this.btnGetInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetInfo.Location = new System.Drawing.Point(425, 12);
            this.btnGetInfo.Name = "btnGetInfo";
            this.btnGetInfo.Size = new System.Drawing.Size(150, 46);
            this.btnGetInfo.TabIndex = 0;
            this.btnGetInfo.Text = "Get Info";
            this.btnGetInfo.UseVisualStyleBackColor = true;
            this.btnGetInfo.Click += new System.EventHandler(this.BtnGetInfo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "Author:";
            // 
            // txtAuthor
            // 
            this.txtAuthor.BackColor = System.Drawing.Color.Black;
            this.txtAuthor.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtAuthor.Location = new System.Drawing.Point(114, 154);
            this.txtAuthor.Name = "txtAuthor";
            this.txtAuthor.ReadOnly = true;
            this.txtAuthor.Size = new System.Drawing.Size(461, 39);
            this.txtAuthor.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 202);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 32);
            this.label2.TabIndex = 1;
            this.label2.Text = "Series:";
            // 
            // txtSeriesName
            // 
            this.txtSeriesName.BackColor = System.Drawing.Color.Black;
            this.txtSeriesName.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtSeriesName.Location = new System.Drawing.Point(114, 199);
            this.txtSeriesName.Name = "txtSeriesName";
            this.txtSeriesName.ReadOnly = true;
            this.txtSeriesName.Size = new System.Drawing.Size(461, 39);
            this.txtSeriesName.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 247);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 32);
            this.label3.TabIndex = 1;
            this.label3.Text = "Book #:";
            // 
            // txtBookNumber
            // 
            this.txtBookNumber.BackColor = System.Drawing.Color.Black;
            this.txtBookNumber.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtBookNumber.Location = new System.Drawing.Point(114, 244);
            this.txtBookNumber.Name = "txtBookNumber";
            this.txtBookNumber.ReadOnly = true;
            this.txtBookNumber.Size = new System.Drawing.Size(461, 39);
            this.txtBookNumber.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 289);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 32);
            this.label4.TabIndex = 1;
            this.label4.Text = "JSON:";
            // 
            // txtJSON
            // 
            this.txtJSON.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtJSON.BackColor = System.Drawing.Color.Black;
            this.txtJSON.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtJSON.Location = new System.Drawing.Point(114, 289);
            this.txtJSON.Multiline = true;
            this.txtJSON.Name = "txtJSON";
            this.txtJSON.ReadOnly = true;
            this.txtJSON.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtJSON.Size = new System.Drawing.Size(461, 367);
            this.txtJSON.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 32);
            this.label5.TabIndex = 1;
            this.label5.Text = "Title:";
            // 
            // txtTitle
            // 
            this.txtTitle.BackColor = System.Drawing.Color.Black;
            this.txtTitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtTitle.Location = new System.Drawing.Point(114, 109);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.ReadOnly = true;
            this.txtTitle.Size = new System.Drawing.Size(461, 39);
            this.txtTitle.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(50, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 32);
            this.label6.TabIndex = 1;
            this.label6.Text = "File:";
            // 
            // txtFilename
            // 
            this.txtFilename.BackColor = System.Drawing.Color.Black;
            this.txtFilename.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtFilename.Location = new System.Drawing.Point(114, 64);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.ReadOnly = true;
            this.txtFilename.Size = new System.Drawing.Size(461, 39);
            this.txtFilename.TabIndex = 2;
            // 
            // btnParseDirectory
            // 
            this.btnParseDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnParseDirectory.Location = new System.Drawing.Point(606, 12);
            this.btnParseDirectory.Name = "btnParseDirectory";
            this.btnParseDirectory.Size = new System.Drawing.Size(222, 46);
            this.btnParseDirectory.TabIndex = 0;
            this.btnParseDirectory.Text = "Parse Directory";
            this.btnParseDirectory.UseVisualStyleBackColor = true;
            this.btnParseDirectory.Click += new System.EventHandler(this.BtnParseDirectory_Click);
            // 
            // dgvBooks
            // 
            this.dgvBooks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvBooks.BackgroundColor = System.Drawing.Color.Black;
            this.dgvBooks.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvBooks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBooks.GridColor = System.Drawing.Color.DimGray;
            this.dgvBooks.Location = new System.Drawing.Point(606, 64);
            this.dgvBooks.Name = "dgvBooks";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBooks.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBooks.RowHeadersVisible = false;
            this.dgvBooks.RowHeadersWidth = 82;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            this.dgvBooks.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvBooks.RowTemplate.Height = 41;
            this.dgvBooks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBooks.Size = new System.Drawing.Size(1114, 540);
            this.dgvBooks.TabIndex = 3;
            this.dgvBooks.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvBooks_ColumnHeaderMouseClick);
            this.dgvBooks.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DgvBooks_DataBindingComplete);
            this.dgvBooks.SelectionChanged += new System.EventHandler(this.DgvBooks_SelectionChanged);
            // 
            // btnRenameMissingBookNumbers
            // 
            this.btnRenameMissingBookNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRenameMissingBookNumbers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenameMissingBookNumbers.Location = new System.Drawing.Point(1432, 610);
            this.btnRenameMissingBookNumbers.Name = "btnRenameMissingBookNumbers";
            this.btnRenameMissingBookNumbers.Size = new System.Drawing.Size(288, 46);
            this.btnRenameMissingBookNumbers.TabIndex = 0;
            this.btnRenameMissingBookNumbers.Text = "Rename Missing Book #";
            this.btnRenameMissingBookNumbers.UseVisualStyleBackColor = true;
            this.btnRenameMissingBookNumbers.Click += new System.EventHandler(this.BtnRenameMissingBookNumbers_Click);
            // 
            // btnMassRename
            // 
            this.btnMassRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMassRename.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMassRename.Location = new System.Drawing.Point(1498, 12);
            this.btnMassRename.Name = "btnMassRename";
            this.btnMassRename.Size = new System.Drawing.Size(222, 46);
            this.btnMassRename.TabIndex = 0;
            this.btnMassRename.Text = "Mass Rename";
            this.btnMassRename.UseVisualStyleBackColor = true;
            this.btnMassRename.Click += new System.EventHandler(this.BtnMassRename_Click);
            // 
            // ssStatusBar
            // 
            this.ssStatusBar.GripMargin = new System.Windows.Forms.Padding(0);
            this.ssStatusBar.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.ssStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslStatus,
            this.tspbProgress,
            this.tsslSpring,
            this.tsslStretchAnchor});
            this.ssStatusBar.Location = new System.Drawing.Point(0, 659);
            this.ssStatusBar.Name = "ssStatusBar";
            this.ssStatusBar.Size = new System.Drawing.Size(1732, 42);
            this.ssStatusBar.TabIndex = 4;
            this.ssStatusBar.Text = "statusStrip1";
            // 
            // tsslStatus
            // 
            this.tsslStatus.Name = "tsslStatus";
            this.tsslStatus.Size = new System.Drawing.Size(85, 32);
            this.tsslStatus.Text = "Ready!";
            // 
            // tspbProgress
            // 
            this.tspbProgress.AutoSize = false;
            this.tspbProgress.Name = "tspbProgress";
            this.tspbProgress.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tspbProgress.Size = new System.Drawing.Size(200, 30);
            this.tspbProgress.Step = 1;
            this.tspbProgress.Visible = false;
            // 
            // tsslSpring
            // 
            this.tsslSpring.Name = "tsslSpring";
            this.tsslSpring.Size = new System.Drawing.Size(1611, 32);
            this.tsslSpring.Spring = true;
            // 
            // tsslStretchAnchor
            // 
            this.tsslStretchAnchor.Name = "tsslStretchAnchor";
            this.tsslStretchAnchor.Size = new System.Drawing.Size(21, 32);
            this.tsslStretchAnchor.Text = " ";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(1348, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(144, 46);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1732, 701);
            this.Controls.Add(this.ssStatusBar);
            this.Controls.Add(this.dgvBooks);
            this.Controls.Add(this.txtJSON);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtBookNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSeriesName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtAuthor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRenameMissingBookNumbers);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnMassRename);
            this.Controls.Add(this.btnParseDirectory);
            this.Controls.Add(this.btnGetInfo);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Audiobook Renamer";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBooks)).EndInit();
            this.ssStatusBar.ResumeLayout(false);
            this.ssStatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private Button btnGetInfo;
    private Label label1;
    private TextBox txtAuthor;
    private Label label2;
    private TextBox txtSeriesName;
    private Label label3;
    private TextBox txtBookNumber;
    private Label label4;
    private TextBox txtJSON;
    private Label label5;
    private TextBox txtTitle;
    private Label label6;
    private TextBox txtFilename;
    private Button btnParseDirectory;
    private DataGridView dgvBooks;
    private Button btnRenameMissingBookNumbers;
    private Button btnMassRename;
    private StatusStrip ssStatusBar;
    private ToolStripStatusLabel tsslStatus;
    private ToolStripStatusLabel tsslSpring;
    private ToolStripProgressBar tspbProgress;
    private ToolStripStatusLabel tsslStretchAnchor;
    private Button btnCancel;
}
