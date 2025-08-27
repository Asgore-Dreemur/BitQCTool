using SharpEDL.DataClass;
using System.Diagnostics;
using System.Security.Permissions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BitQCTool
{
    public partial class MainWindow : Form
    {
        private UIHandler Handler;
        private string? CurrentStep;

        private EventHandler<(long, long)> NormalProgressChangedEvent;
        internal EventHandler<(string?, (long, long))> FlashProgressChangedEvent;

        public MainWindow()
        {
            InitializeComponent();
            Handler = new UIHandler(this);

            NormalProgressChangedEvent = new EventHandler<(long, long)>(new Action<object?, (long, long)>((sender, e) =>
            {
                Handler.UpdateProgressBar(e.Item1, e.Item2);
            }));
            FlashProgressChangedEvent = new EventHandler<(string?, (long, long))>(new Action<object?, (string?, (long, long))>((sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Item1) && e.Item1 != CurrentStep)
                {
                    CurrentStep = e.Item1;
                    if (CurrentStep == "Patch")
                        LogToWindow("Ӧ��patch...");
                    else
                        LogToWindow("ˢ�����" + CurrentStep);
                }
                Handler.UpdateProgressBar(e.Item2.Item1, e.Item2.Item2);
            }));
        }

        internal void LogToWindow(string msg, string level = "info")
        {
            Action action = new Action(() =>
            {
                string text = $"[{DateTime.Now.ToString()}][{level.ToUpper()}]{msg}\n";
                LogText.SelectionStart = LogText.TextLength;
                LogText.SelectionLength = 0;
                LogText.SelectionColor = level == "warn" ? Color.DarkGoldenrod : level == "error" ? Color.Red : Color.Black;
                LogText.AppendText(text);
                LogText.SelectionColor = LogText.ForeColor;
                LogText.ScrollToCaret();
            });
            if (LogText.InvokeRequired)
                LogText.Invoke(action);
            else
                action.Invoke();
        }

        private void RunActionWithTry(Action action, bool thread = false, bool printTime = true)
        {
            Action finalAction = () =>
            {
                DateTime timeBeforeOperation = DateTime.Now;
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    LogToWindow(ex.Message, "error");
                }
                finally
                {
                    Invoke(() => SetAllControls(true));
                    Handler.CurrentPort?.Close();
                    if (Handler.FirehoseServer != null)
                        Handler.FirehoseServer.ProgressChanged -= NormalProgressChangedEvent;
                }
                DateTime timeAfterOperation = DateTime.Now;
                if (printTime)
                    LogToWindow($"������ʱ:{(timeAfterOperation - timeBeforeOperation).TotalSeconds}��");
            };
            if (thread)
                new Thread(() => finalAction.Invoke()).Start();
            else
            {
                finalAction.Invoke();
            }
        }

        private void SetAllControls(bool enable)
        {
            Action action = new Action(() =>
            {
                ReadGPTBtn.Enabled = enable;
                ReadPartBtn.Enabled = enable;
                ErasePartBtn.Enabled = enable;
                WritePartBtn.Enabled = enable;
                GenerateProgramCB.Enabled = enable;
                SelectCB.Enabled = enable;
                SendLoaderCB.Enabled = enable;
                SkipSafeCB.Enabled = enable;
                SkipDataCB.Enabled = enable;
                LoaderBrowserBtn.Enabled = enable;
                FlashPackBroswerBtn.Enabled = enable;
                ResetBtn.Enabled = enable;
            });
            if (InvokeRequired)
                Invoke(action);
            else
                action.Invoke();
        }

        private void ReadPartBtn_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LogToWindow("��ǰ����: ������");
                    SetAllControls(false);
                    RunActionWithTry(() =>
                    {
                        Handler.DoPreviousOperation();
                        Debug.Assert(Handler.FirehoseServer != null);
                        Handler.FirehoseServer.ProgressChanged += NormalProgressChangedEvent;
                        Handler.DoReadPartitions(dialog.SelectedPath);
                    }, true);
                }
            }
        }

        private void DevMgrBtn_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "devmgmt.msc",
                UseShellExecute = true,
                CreateNoWindow = true
            });
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Handler.PortDetectStop = true;
        }

        private void ReadGPTBtn_Click(object sender, EventArgs e)
        {
            RunActionWithTry(() =>
            {
                LogToWindow("��ǰ����: ��ȡ������");
                SetAllControls(false);
                Handler.DoPreviousOperation();
                Debug.Assert(Handler.FirehoseServer != null); //unlikely
                List<PartitionInfo> partitions = Handler.FirehoseServer.GetPartitionsFromDevice(true);
                Handler.ParsePartitions(partitions);
            }, true);
        }

        private void FlashPackBroswerBtn_Click(object sender, EventArgs e)
        {
            RunActionWithTry(() =>
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        FlashPackText.Text = dialog.SelectedPath;
                        Handler.ParseFlashPack(dialog.SelectedPath);
                    }
                }
            }, false, false);
        }

        private void SelectCB_CheckedChanged(object sender, EventArgs e)
        {
            PartList.BeginUpdate();
            foreach (ListViewItem item in PartList.Items)
            {
                item.Checked = SelectCB.Checked;
            }
            PartList.EndUpdate();
        }

        private void LoaderBrowserBtn_Click(object sender, EventArgs e)
        {
            RunActionWithTry(() =>
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Filter = "�����ļ� (*.mbn;*.elf)|*.mbn;*.elf|�����ļ� (*.*)|*.*";
                    dialog.Multiselect = false;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        LoaderTextBox.Text = dialog.FileName;
                    }
                }
            }, false, false);
        }

        private void WritePartBtn_Click(object sender, EventArgs e)
        {
            string programPath = FlashPackText.Text;
            LogToWindow("��ǰ����:д����");
            SetAllControls(false);
            if (string.IsNullOrEmpty(programPath))
            {
                RunActionWithTry(() =>
                {
                    Debug.Assert(Handler.FirehoseServer != null);
                    Handler.DoPreviousOperation();
                    Handler.FirehoseServer.ProgressChanged += NormalProgressChangedEvent;
                    Handler.FlashWithoutProgram();
                }, true);
            }
            else
            {
                RunActionWithTry(() =>
                {
                    Handler.DoPreviousOperation();
                    Handler.DoWritePartitions(programPath);
                }, true);
            }
        }

        private void ErasePartBtn_Click(object sender, EventArgs e)
        {
            RunActionWithTry(() =>
            {
                LogToWindow("��ǰ����:������");
                SetAllControls(false);
                Handler.DoPreviousOperation();
                Handler.DoEraseOperation();
            }, true);
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            RunActionWithTry(() =>
            {
                LogToWindow("��ǰ����:�����豸");
                SetAllControls(false);
                Handler.DoPreviousOperation();
                Handler.FirehoseServer?.ResetDevice().CheckAndThrow();
                LogToWindow("�������");
            }, true);
        }

        private void PartList_ItemActivate(object sender, EventArgs e)
        {
            if (PartList.SelectedItems.Count == 0) return;
            var item = PartList.SelectedItems[0];
            var info = (PartitionInfo?)item.Tag;
            if (info == null) return;
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = $"Ϊ {item.Text} ѡ��һ���ļ�";
                dialog.Filter = "�����ļ� (*.img;*.bin;*.mbn;*.elf)|*.img;*.bin;*.mbn;*.elf|�����ļ� (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    item.SubItems[4].Text = Path.GetFileName(dialog.FileName);
                    item.Checked = true;
                    info.FilePath = dialog.FileName;
                }
            }
        }

        private void ManualAuthCB_CheckedChanged(object sender, EventArgs e)
        {
            if (ManualAuthCB.Checked)
                MessageBox.Show("��ѡ��ʹ��Mi NoAuth��,������Ҫ����EDL Auth�ĳ������ᵯ��һ���Ի���\n" +
                    "����Ҫ���и���blob��ճ��sign", "����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
