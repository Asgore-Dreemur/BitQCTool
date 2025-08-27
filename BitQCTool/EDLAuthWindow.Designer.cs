namespace BitQCTool
{
    partial class EDLAuthWindow
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
            _BlobLabel = new Label();
            BlobTextBox = new TextBox();
            _SignLabel = new Label();
            SignTextBox = new TextBox();
            DoAuthBtn = new Button();
            SuspendLayout();
            // 
            // _BlobLabel
            // 
            _BlobLabel.AutoSize = true;
            _BlobLabel.Location = new Point(12, 59);
            _BlobLabel.Name = "_BlobLabel";
            _BlobLabel.Size = new Size(35, 17);
            _BlobLabel.TabIndex = 0;
            _BlobLabel.Text = "Blob";
            // 
            // BlobTextBox
            // 
            BlobTextBox.Location = new Point(53, 12);
            BlobTextBox.Multiline = true;
            BlobTextBox.Name = "BlobTextBox";
            BlobTextBox.ReadOnly = true;
            BlobTextBox.Size = new Size(319, 108);
            BlobTextBox.TabIndex = 1;
            // 
            // _SignLabel
            // 
            _SignLabel.AutoSize = true;
            _SignLabel.Location = new Point(12, 199);
            _SignLabel.Name = "_SignLabel";
            _SignLabel.Size = new Size(33, 17);
            _SignLabel.TabIndex = 2;
            _SignLabel.Text = "Sign";
            // 
            // SignTextBox
            // 
            SignTextBox.Location = new Point(53, 154);
            SignTextBox.Multiline = true;
            SignTextBox.Name = "SignTextBox";
            SignTextBox.Size = new Size(319, 108);
            SignTextBox.TabIndex = 3;
            // 
            // DoAuthBtn
            // 
            DoAuthBtn.Location = new Point(143, 304);
            DoAuthBtn.Name = "DoAuthBtn";
            DoAuthBtn.Size = new Size(82, 30);
            DoAuthBtn.TabIndex = 4;
            DoAuthBtn.Text = "Do Auth";
            DoAuthBtn.UseVisualStyleBackColor = true;
            // 
            // EDLAuthWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 361);
            Controls.Add(DoAuthBtn);
            Controls.Add(SignTextBox);
            Controls.Add(_SignLabel);
            Controls.Add(BlobTextBox);
            Controls.Add(_BlobLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "EDLAuthWindow";
            Text = "Mi -- EDL Auth";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label _BlobLabel;
        internal TextBox BlobTextBox;
        private Label _SignLabel;
        internal TextBox SignTextBox;
        internal Button DoAuthBtn;
    }
}