namespace BitQCTool
{
    partial class MainWindow
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
            LogGB = new GroupBox();
            LogText = new RichTextBox();
            PartGB = new GroupBox();
            PartList = new ListView();
            LabelCH = new ColumnHeader();
            LunCH = new ColumnHeader();
            StartSectorCH = new ColumnHeader();
            PartSizeCH = new ColumnHeader();
            FilePathCH = new ColumnHeader();
            LoaderBrowserBtn = new Button();
            FlashPackBroswerBtn = new Button();
            LoaderTextBox = new TextBox();
            FlashPackText = new TextBox();
            _ProgrammerPath = new Label();
            _FlashPackPath = new Label();
            SendLoaderCB = new CheckBox();
            SkipSafeCB = new CheckBox();
            SkipDataCB = new CheckBox();
            ReadPartBtn = new Button();
            WritePartBtn = new Button();
            ErasePartBtn = new Button();
            OperationCG = new GroupBox();
            ManualAuthCB = new CheckBox();
            ResetBtn = new Button();
            GenerateProgramCB = new CheckBox();
            SelectCB = new CheckBox();
            ReadGPTBtn = new Button();
            ProgressGB = new GroupBox();
            QCProgressBar = new ProgressBar();
            DevMgrBtn = new Button();
            _PortLabel = new Label();
            PortLabel = new Label();
            LogGB.SuspendLayout();
            PartGB.SuspendLayout();
            OperationCG.SuspendLayout();
            ProgressGB.SuspendLayout();
            SuspendLayout();
            // 
            // LogGB
            // 
            LogGB.Controls.Add(LogText);
            LogGB.Location = new Point(12, 12);
            LogGB.Name = "LogGB";
            LogGB.Size = new Size(346, 588);
            LogGB.TabIndex = 1;
            LogGB.TabStop = false;
            LogGB.Text = "日志窗口";
            // 
            // LogText
            // 
            LogText.BorderStyle = BorderStyle.None;
            LogText.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            LogText.Location = new Point(6, 22);
            LogText.Name = "LogText";
            LogText.ReadOnly = true;
            LogText.Size = new Size(334, 550);
            LogText.TabIndex = 0;
            LogText.Text = "";
            // 
            // PartGB
            // 
            PartGB.Controls.Add(PartList);
            PartGB.Controls.Add(LoaderBrowserBtn);
            PartGB.Controls.Add(FlashPackBroswerBtn);
            PartGB.Controls.Add(LoaderTextBox);
            PartGB.Controls.Add(FlashPackText);
            PartGB.Controls.Add(_ProgrammerPath);
            PartGB.Controls.Add(_FlashPackPath);
            PartGB.Location = new Point(364, 12);
            PartGB.Name = "PartGB";
            PartGB.Size = new Size(608, 371);
            PartGB.TabIndex = 2;
            PartGB.TabStop = false;
            PartGB.Text = "分区表";
            // 
            // PartList
            // 
            PartList.CheckBoxes = true;
            PartList.Columns.AddRange(new ColumnHeader[] { LabelCH, LunCH, StartSectorCH, PartSizeCH, FilePathCH });
            PartList.FullRowSelect = true;
            PartList.Location = new Point(6, 22);
            PartList.MultiSelect = false;
            PartList.Name = "PartList";
            PartList.Size = new Size(596, 268);
            PartList.TabIndex = 1;
            PartList.UseCompatibleStateImageBehavior = false;
            PartList.View = View.Details;
            PartList.ItemActivate += PartList_ItemActivate;
            // 
            // LabelCH
            // 
            LabelCH.Text = "分区名";
            LabelCH.Width = 120;
            // 
            // LunCH
            // 
            LunCH.Text = "LUN";
            LunCH.Width = 70;
            // 
            // StartSectorCH
            // 
            StartSectorCH.Text = "起始扇区";
            StartSectorCH.Width = 110;
            // 
            // PartSizeCH
            // 
            PartSizeCH.Text = "分区大小";
            PartSizeCH.Width = 90;
            // 
            // FilePathCH
            // 
            FilePathCH.Text = "文件名";
            FilePathCH.Width = 130;
            // 
            // LoaderBrowserBtn
            // 
            LoaderBrowserBtn.Location = new Point(534, 328);
            LoaderBrowserBtn.Name = "LoaderBrowserBtn";
            LoaderBrowserBtn.Size = new Size(69, 23);
            LoaderBrowserBtn.TabIndex = 6;
            LoaderBrowserBtn.Text = "浏览";
            LoaderBrowserBtn.UseVisualStyleBackColor = true;
            LoaderBrowserBtn.Click += LoaderBrowserBtn_Click;
            // 
            // FlashPackBroswerBtn
            // 
            FlashPackBroswerBtn.Location = new Point(534, 296);
            FlashPackBroswerBtn.Name = "FlashPackBroswerBtn";
            FlashPackBroswerBtn.Size = new Size(69, 23);
            FlashPackBroswerBtn.TabIndex = 5;
            FlashPackBroswerBtn.Text = "浏览";
            FlashPackBroswerBtn.UseVisualStyleBackColor = true;
            FlashPackBroswerBtn.Click += FlashPackBroswerBtn_Click;
            // 
            // LoaderTextBox
            // 
            LoaderTextBox.Location = new Point(109, 327);
            LoaderTextBox.Name = "LoaderTextBox";
            LoaderTextBox.ReadOnly = true;
            LoaderTextBox.Size = new Size(419, 23);
            LoaderTextBox.TabIndex = 4;
            // 
            // FlashPackText
            // 
            FlashPackText.Location = new Point(109, 296);
            FlashPackText.Name = "FlashPackText";
            FlashPackText.ReadOnly = true;
            FlashPackText.Size = new Size(419, 23);
            FlashPackText.TabIndex = 3;
            // 
            // _ProgrammerPath
            // 
            _ProgrammerPath.AutoSize = true;
            _ProgrammerPath.Location = new Point(22, 332);
            _ProgrammerPath.Name = "_ProgrammerPath";
            _ProgrammerPath.Size = new Size(56, 17);
            _ProgrammerPath.TabIndex = 2;
            _ProgrammerPath.Text = "引导路径";
            // 
            // _FlashPackPath
            // 
            _FlashPackPath.AutoSize = true;
            _FlashPackPath.Location = new Point(22, 300);
            _FlashPackPath.Name = "_FlashPackPath";
            _FlashPackPath.Size = new Size(68, 17);
            _FlashPackPath.TabIndex = 1;
            _FlashPackPath.Text = "刷机包路径";
            // 
            // SendLoaderCB
            // 
            SendLoaderCB.AutoSize = true;
            SendLoaderCB.Checked = true;
            SendLoaderCB.CheckState = CheckState.Checked;
            SendLoaderCB.Location = new Point(13, 22);
            SendLoaderCB.Name = "SendLoaderCB";
            SendLoaderCB.Size = new Size(75, 21);
            SendLoaderCB.TabIndex = 7;
            SendLoaderCB.Text = "发送引导";
            SendLoaderCB.UseVisualStyleBackColor = true;
            // 
            // SkipSafeCB
            // 
            SkipSafeCB.AutoSize = true;
            SkipSafeCB.Checked = true;
            SkipSafeCB.CheckState = CheckState.Checked;
            SkipSafeCB.Location = new Point(13, 49);
            SkipSafeCB.Name = "SkipSafeCB";
            SkipSafeCB.Size = new Size(99, 21);
            SkipSafeCB.TabIndex = 8;
            SkipSafeCB.Text = "跳过安全分区";
            SkipSafeCB.UseVisualStyleBackColor = true;
            // 
            // SkipDataCB
            // 
            SkipDataCB.AutoSize = true;
            SkipDataCB.Checked = true;
            SkipDataCB.CheckState = CheckState.Checked;
            SkipDataCB.Location = new Point(490, 49);
            SkipDataCB.Name = "SkipDataCB";
            SkipDataCB.Size = new Size(99, 21);
            SkipDataCB.TabIndex = 9;
            SkipDataCB.Text = "跳过数据分区";
            SkipDataCB.UseVisualStyleBackColor = true;
            // 
            // ReadPartBtn
            // 
            ReadPartBtn.Location = new Point(150, 79);
            ReadPartBtn.Name = "ReadPartBtn";
            ReadPartBtn.Size = new Size(86, 29);
            ReadPartBtn.TabIndex = 10;
            ReadPartBtn.Text = "读分区";
            ReadPartBtn.UseVisualStyleBackColor = true;
            ReadPartBtn.Click += ReadPartBtn_Click;
            // 
            // WritePartBtn
            // 
            WritePartBtn.Location = new Point(261, 79);
            WritePartBtn.Name = "WritePartBtn";
            WritePartBtn.Size = new Size(86, 29);
            WritePartBtn.TabIndex = 11;
            WritePartBtn.Text = "写分区";
            WritePartBtn.UseVisualStyleBackColor = true;
            WritePartBtn.Click += WritePartBtn_Click;
            // 
            // ErasePartBtn
            // 
            ErasePartBtn.Location = new Point(373, 79);
            ErasePartBtn.Name = "ErasePartBtn";
            ErasePartBtn.Size = new Size(86, 29);
            ErasePartBtn.TabIndex = 12;
            ErasePartBtn.Text = "擦分区";
            ErasePartBtn.UseVisualStyleBackColor = true;
            ErasePartBtn.Click += ErasePartBtn_Click;
            // 
            // OperationCG
            // 
            OperationCG.Controls.Add(ManualAuthCB);
            OperationCG.Controls.Add(ResetBtn);
            OperationCG.Controls.Add(GenerateProgramCB);
            OperationCG.Controls.Add(SelectCB);
            OperationCG.Controls.Add(ReadGPTBtn);
            OperationCG.Controls.Add(SendLoaderCB);
            OperationCG.Controls.Add(SkipSafeCB);
            OperationCG.Controls.Add(SkipDataCB);
            OperationCG.Controls.Add(ErasePartBtn);
            OperationCG.Controls.Add(ReadPartBtn);
            OperationCG.Controls.Add(WritePartBtn);
            OperationCG.Location = new Point(364, 389);
            OperationCG.Name = "OperationCG";
            OperationCG.Size = new Size(608, 126);
            OperationCG.TabIndex = 14;
            OperationCG.TabStop = false;
            OperationCG.Text = "操作台";
            // 
            // ManualAuthCB
            // 
            ManualAuthCB.AutoSize = true;
            ManualAuthCB.Location = new Point(241, 22);
            ManualAuthCB.Name = "ManualAuthCB";
            ManualAuthCB.Size = new Size(126, 21);
            ManualAuthCB.TabIndex = 20;
            ManualAuthCB.Text = "不使用Mi NoAuth";
            ManualAuthCB.UseVisualStyleBackColor = true;
            ManualAuthCB.CheckedChanged += ManualAuthCB_CheckedChanged;
            // 
            // ResetBtn
            // 
            ResetBtn.Location = new Point(484, 79);
            ResetBtn.Name = "ResetBtn";
            ResetBtn.Size = new Size(80, 29);
            ResetBtn.TabIndex = 19;
            ResetBtn.Text = "重启";
            ResetBtn.UseVisualStyleBackColor = true;
            ResetBtn.Click += ResetBtn_Click;
            // 
            // GenerateProgramCB
            // 
            GenerateProgramCB.AutoSize = true;
            GenerateProgramCB.Checked = true;
            GenerateProgramCB.CheckState = CheckState.Checked;
            GenerateProgramCB.Location = new Point(241, 49);
            GenerateProgramCB.Name = "GenerateProgramCB";
            GenerateProgramCB.Size = new Size(124, 21);
            GenerateProgramCB.TabIndex = 15;
            GenerateProgramCB.Text = "生成rawprogram";
            GenerateProgramCB.UseVisualStyleBackColor = true;
            // 
            // SelectCB
            // 
            SelectCB.AutoSize = true;
            SelectCB.Location = new Point(490, 22);
            SelectCB.Name = "SelectCB";
            SelectCB.Size = new Size(92, 21);
            SelectCB.TabIndex = 14;
            SelectCB.Text = "全选/全不选";
            SelectCB.UseVisualStyleBackColor = true;
            SelectCB.CheckedChanged += SelectCB_CheckedChanged;
            // 
            // ReadGPTBtn
            // 
            ReadGPTBtn.Location = new Point(35, 79);
            ReadGPTBtn.Name = "ReadGPTBtn";
            ReadGPTBtn.Size = new Size(86, 29);
            ReadGPTBtn.TabIndex = 13;
            ReadGPTBtn.Text = "读分区表";
            ReadGPTBtn.UseVisualStyleBackColor = true;
            ReadGPTBtn.Click += ReadGPTBtn_Click;
            // 
            // ProgressGB
            // 
            ProgressGB.Controls.Add(QCProgressBar);
            ProgressGB.Location = new Point(364, 522);
            ProgressGB.Name = "ProgressGB";
            ProgressGB.Size = new Size(608, 79);
            ProgressGB.TabIndex = 15;
            ProgressGB.TabStop = false;
            ProgressGB.Text = "操作进度";
            // 
            // QCProgressBar
            // 
            QCProgressBar.Location = new Point(6, 33);
            QCProgressBar.MarqueeAnimationSpeed = 0;
            QCProgressBar.Name = "QCProgressBar";
            QCProgressBar.Size = new Size(596, 30);
            QCProgressBar.Style = ProgressBarStyle.Continuous;
            QCProgressBar.TabIndex = 0;
            // 
            // DevMgrBtn
            // 
            DevMgrBtn.Location = new Point(874, 607);
            DevMgrBtn.Name = "DevMgrBtn";
            DevMgrBtn.Size = new Size(100, 30);
            DevMgrBtn.TabIndex = 16;
            DevMgrBtn.Text = "设备管理器";
            DevMgrBtn.UseVisualStyleBackColor = true;
            DevMgrBtn.Click += DevMgrBtn_Click;
            // 
            // _PortLabel
            // 
            _PortLabel.AutoSize = true;
            _PortLabel.Location = new Point(10, 614);
            _PortLabel.Name = "_PortLabel";
            _PortLabel.Size = new Size(35, 17);
            _PortLabel.TabIndex = 17;
            _PortLabel.Text = "端口:";
            // 
            // PortLabel
            // 
            PortLabel.AutoSize = true;
            PortLabel.Location = new Point(51, 614);
            PortLabel.Name = "PortLabel";
            PortLabel.Size = new Size(44, 17);
            PortLabel.TabIndex = 18;
            PortLabel.Text = "未连接";
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 643);
            Controls.Add(PortLabel);
            Controls.Add(_PortLabel);
            Controls.Add(DevMgrBtn);
            Controls.Add(ProgressGB);
            Controls.Add(OperationCG);
            Controls.Add(PartGB);
            Controls.Add(LogGB);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainWindow";
            Text = "BitQCTool";
            FormClosed += MainWindow_FormClosed;
            LogGB.ResumeLayout(false);
            PartGB.ResumeLayout(false);
            PartGB.PerformLayout();
            OperationCG.ResumeLayout(false);
            OperationCG.PerformLayout();
            ProgressGB.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private GroupBox LogGB;
        private GroupBox PartGB;
        private Label _ProgrammerPath;
        private Label _FlashPackPath;
        private GroupBox OperationCG;
        private GroupBox ProgressGB;
        private Button DevMgrBtn;
        private Label _PortLabel;
        internal TextBox LoaderTextBox;
        internal TextBox FlashPackText;
        internal CheckBox SkipSafeCB;
        internal CheckBox SendLoaderCB;
        private Button LoaderBrowserBtn;
        private Button FlashPackBroswerBtn;
        internal Button ErasePartBtn;
        internal Button WritePartBtn;
        internal Button ReadPartBtn;
        internal CheckBox SkipDataCB;
        internal Button ReadGPTBtn;
        internal ProgressBar QCProgressBar;
        internal Label PortLabel;
        internal RichTextBox LogText;
        private ColumnHeader LabelCH;
        private ColumnHeader LunCH;
        private ColumnHeader StartSectorCH;
        private ColumnHeader PartSizeCH;
        private ColumnHeader FilePathCH;
        internal ListView PartList;
        private CheckBox SelectCB;
        internal CheckBox GenerateProgramCB;
        private Button ResetBtn;
        internal CheckBox ManualAuthCB;
    }
}
