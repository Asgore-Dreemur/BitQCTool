using Microsoft.VisualBasic.Logging;
using SharpEDL;
using SharpEDL.Auth;
using SharpEDL.DataClass;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BitQCTool
{
    public class UIHandler
    {
        public MainWindow UIWindow;
        private string? CurrentPortName;
        internal SerialPort? CurrentPort;
        private SaharaServer? SaharaServer;
        internal FirehoseServer? FirehoseServer;
        internal DateTime TimeBeforeOperation = DateTime.Now;

        internal bool PortDetectStop = false;

        public UIHandler(MainWindow window)
        {
            UIWindow = window;
            new Thread(PortDetectThread).Start();
        }

        private string? WaitForPort(string portName = "")
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%(COM%'");
            foreach(var item in searcher.Get())
            {
                string? name = item["Name"]?.ToString();
                string? deviceID = item["DeviceID"]?.ToString();
                string? pnpID = item["PNPDeviceID"]?.ToString();
                if(!string.IsNullOrEmpty(deviceID) && !string.IsNullOrEmpty(pnpID) && !string.IsNullOrEmpty(name))
                {
                    Match match = Regex.Match(pnpID, @"VID_([0-9A-F]{4})&PID_([0-9A-F]{4})", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        string vid = match.Groups[1].Value.ToLower();
                        string pid = match.Groups[2].Value.ToLower();
                        if (vid == "05c6" && pid == "9008" && (string.IsNullOrEmpty(portName) || portName == deviceID))
                        {
                            match = Regex.Match(name, @"\(COM(\d+)\)", RegexOptions.IgnoreCase);
                            if (match.Success)
                                return "COM" + match.Groups[1].Value;
                        }
                    }
                }
            }
            return null;
        }

        private void InvokeRun(Action action, Control control)
        {
            if(control.InvokeRequired)
                control.Invoke(action);
            else
                action.Invoke();
        }

        private void PortDetectThread()
        {
            while (true)
            {
                if (PortDetectStop)
                    return;
                string? newPortName = WaitForPort();
                bool newEmpty = string.IsNullOrEmpty(newPortName);
                bool empty = string.IsNullOrEmpty(CurrentPortName);
                if (!newEmpty)
                {
                    if (newPortName == CurrentPortName)
                        continue;
                    CurrentPortName = newPortName;
                    UIWindow.PortLabel.Invoke(() => UIWindow.PortLabel.Text = newPortName);
                    UIWindow.LogToWindow($"{newPortName} 已连接");
                }
                else if (!empty)
                {
                    UIWindow.PortLabel.Invoke(() => UIWindow.PortLabel.Text = "未连接");
                    UIWindow.LogToWindow($"{CurrentPortName} 已断开");
                    CurrentPortName = null;
                }
                Thread.Sleep(100);
            }
        }

        public void ParsePartitions(List<PartitionInfo> partitions)
        {
            InvokeRun(() =>
            {
                UIWindow.PartList.BeginUpdate();
                UIWindow.PartList.Items.Clear();
                foreach (var partition in partitions)
                {
                    ListViewItem item = new ListViewItem(partition.Label);
                    item.SubItems.Add(partition.Lun.ToString());
                    item.SubItems.Add(partition.StartSector);
                    item.SubItems.Add(GetFileSize(partition.SectorLen * partition.BytesPerSector));
                    item.SubItems.Add(!string.IsNullOrEmpty(partition.FilePath) ? Path.GetFileName(partition.FilePath) : "");
                    item.Tag = partition;
                    item.Checked = true;
                    UIWindow.PartList.Items.Add(item);
                }
                UIWindow.PartList.EndUpdate();
            }, UIWindow);
        }

        public void ParseFlashPack(string packPath)
        {
            List<string> programs = new List<string>();
            DirectoryInfo info = new DirectoryInfo(packPath);
            foreach (var item in info.GetFiles())
            {
                if(Regex.IsMatch(item.Name, @"rawprogram([\d]+).xml"))
                    programs.Add(item.FullName);
                if (item.Name.StartsWith("prog") && (item.Extension == ".mbn" || item.Extension == ".elf"))
                    UIWindow.LoaderTextBox.Text = item.FullName;
            }
            List<PartitionInfo> partitions = ProgramFlasher.ParseProgramFiles(packPath, programs.ToArray());
            ParsePartitions(partitions);
        }

        private string GetFileSize(long sizeInBytes)
        {
            foreach(var item in new string[] {"B", "KB", "MB", "GB" })
            {
                if (sizeInBytes / 1024 < 1)
                    return sizeInBytes.ToString() + item;
                sizeInBytes /= 1024;
            }
            return sizeInBytes.ToString() + "GB";
        }

        public void DoReadPartitions(string readPath)
        {
            Debug.Assert(FirehoseServer != null);
            if (UIWindow.PartList.Items.Count == 0)
                throw new Exception("请先读取分区表");
            UIWindow.LogToWindow("分区将被保存到:" + readPath);
            Dictionary<int, List<PartitionInfo>> partitions = new Dictionary<int, List<PartitionInfo>>();
            List<PartitionInfo?> infos = new List<PartitionInfo?>();
            UIWindow.PartList.Invoke(() => infos = UIWindow.PartList.Items
                                                    .Cast<ListViewItem>()
                                                    .Where(item => item.Checked && item.Tag != null)
                                                    .Select(item => (PartitionInfo?)item.Tag).ToList());
            Debug.Assert(infos != null);
            foreach (PartitionInfo? partition in infos)
            {
                if(partition == null) continue;
                UIWindow.LogToWindow("读取分区" + partition.Label);
                if(partition.Label == "PrimaryGPT")
                    partition.FilePath = Path.Combine(readPath, $"gpt_main{partition.Lun}.bin");
                else if(partition.Label == "BackupGPT")
                    partition.FilePath = Path.Combine(readPath, $"gpt_backup{partition.Lun}.bin");
                else
                    partition.FilePath = Path.Combine(readPath, partition.Label + ".img");
                FirehoseServer.ReadbackImage(partition).CheckAndThrow();
                if(partitions.ContainsKey(partition.Lun))
                    partitions[partition.Lun].Add(partition);
                else
                    partitions[partition.Lun] = new List<PartitionInfo>() { partition };
            }
            if (UIWindow.GenerateProgramCB.Checked)
            {
                UIWindow.LogToWindow("生成rawprogram...");
                foreach(var item in partitions)
                {
                    string rawprogramPath = Path.Combine(readPath, $"rawprogram{item.Key}.xml");
                    File.WriteAllText(rawprogramPath, ProgramFlasher.GenerateRawprogram(item.Value));
                }
            }
            UIWindow.LogToWindow("读分区完成");
        }

        public void DoWritePartitions(string programPath)
        {
            Debug.Assert(FirehoseServer != null);
            if (string.IsNullOrEmpty(programPath))
                throw new Exception("请先加载软件包");
            ProgramFlasher flasher = new ProgramFlasher(FirehoseServer, programPath);
            UIWindow.PartList.Invoke(() =>
            {
                flasher.BypassPartitions = UIWindow.PartList.Items.Cast<ListViewItem>()
                                            .Where(item => !item.Checked && item.Tag != null)
                                            .Select(item => (((PartitionInfo)item.Tag).Label, ((PartitionInfo)item.Tag).Lun))
                                            .ToList();
                if (UIWindow.SkipSafeCB.Checked)
                {
                    flasher.BypassPartitions.Add(("persist", -1));
                    flasher.BypassPartitions.Add(("modem", -1));
                    flasher.BypassPartitions.Add(("modemst1", -1));
                    flasher.BypassPartitions.Add(("modemst2", -1));
                }
                if (UIWindow.SkipDataCB.Checked)
                {
                    flasher.BypassPartitions.Add(("userdata", -1));
                    flasher.BypassPartitions.Add(("metadata", -1));
                }
            });
            flasher.ProgressChanged += UIWindow.FlashProgressChangedEvent;
            flasher.FlashPartitions().CheckAndThrow();
            UIWindow.LogToWindow("写分区完成");
        }

        public void FlashWithoutProgram()
        {
            List<PartitionInfo?> partitions = new List<PartitionInfo?>();
            UIWindow.PartList.Invoke(() =>
            {
                partitions = UIWindow.PartList.Items.Cast<ListViewItem>()
                                                    .Where(item => item.Checked)
                                                    .Select(item => (PartitionInfo?)item.Tag)
                                                    .Where(item => item != null && !string.IsNullOrEmpty(item.FilePath))
                                                    .ToList();
            });
            foreach(var item in partitions)
            {
                if (item == null) continue;
                UIWindow.LogToWindow("刷入分区" + item.Label);
                if (item.Sparse)
                    FirehoseServer?.WriteSparseImage(item).CheckAndThrow();
                else
                    FirehoseServer?.WriteUnsparseImage(item).CheckAndThrow();
            }
            UIWindow.LogToWindow("写分区完成");
        }

        public void DoEraseOperation()
        {
            List<PartitionInfo?> partitions = new List<PartitionInfo?>();
            UIWindow.PartList.Invoke(() =>
            {
                partitions = UIWindow.PartList.Items
                                .Cast<ListViewItem>()
                                .Where(item => item.Checked)
                                .Select(item => (PartitionInfo?)item.Tag)
                                .ToList();
            });
            for(int i=0;i<partitions.Count;i++)
            {
                var item = partitions[i];
                if (item == null) continue;
                UIWindow.LogToWindow("擦除分区" + item.Label);
                FirehoseServer?.ErasePartition(item);
                UpdateProgressBar(i+1, partitions.Count);
            }
            UIWindow.LogToWindow("擦分区完成");
        }

        public void UpdateProgressBar(long data1, long data2)
        {
            UIWindow.QCProgressBar.Invoke(() =>
            {
                int progress;
                if (data2 == 0)
                    progress = 0;
                else
                    progress = (int)((double)data1 / data2 * 100);
                UIWindow.QCProgressBar.Value = progress;
            });
        }

        private void DoManualAuth()
        {
            Debug.Assert(FirehoseServer != null);
            MiAuth auth = new MiAuth(FirehoseServer);
            EDLAuthWindow window = new EDLAuthWindow();
            window.BlobTextBox.Text = auth.GetBlob();
            window.DoAuthBtn.Click += (sender, e) =>
            {
                if (string.IsNullOrEmpty(window.SignTextBox.Text) || window.SignTextBox.Text.Length != 512)
                {
                    MessageBox.Show("无效的Sign,必须是长度为512的十六进制字符串", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                window.DialogResult = DialogResult.OK;
                window.Close();
                auth.SendSign(window.SignTextBox.Text.Trim());
                FirehoseServer.WaitForResponse().CheckAndThrow();
                UIWindow.LogToWindow("EDL Auth 成功");
            };
            if (window.ShowDialog() != DialogResult.OK)
                throw new Exception("用户取消操作");
        }

        private void DoNoAuth()
        {
            Debug.Assert(FirehoseServer != null);
            UIWindow.LogToWindow("进行Mi NoAuth");
            MiAuth auth = new MiAuth(FirehoseServer);
            if(auth.BypassAuth())
                UIWindow.LogToWindow("Mi NoAuth成功");
            else
                throw new Exception("Mi NoAuth失败");
        }

        public void DoPreviousOperation()
        {
            FileStream? stream = null;
            try
            {
                if (string.IsNullOrEmpty(CurrentPortName))
                    throw new Exception("没有设备连接");
                if(string.IsNullOrEmpty(UIWindow.LoaderTextBox.Text) && UIWindow.SendLoaderCB.Checked)
                    throw new Exception("未选择引导");
                CurrentPort = new SerialPort(CurrentPortName);
                CurrentPort.Open();

                SaharaServer = new SaharaServer(CurrentPort);
                FirehoseServer = new FirehoseServer(CurrentPort);

                if (UIWindow.SendLoaderCB.Checked)
                {
                    UIWindow.LogToWindow("Sahara 读信息...");
                    SaharaServer.DoHelloHandshake(SaharaMode.Command);
                    UIWindow.LogToWindow($"\nMsm HWID: {SaharaServer.GetMsmHWID()}\n" +
                        $"PKHash: {SaharaServer.GetOEMPkHash()}\n" +
                        $"SerialNum: {SaharaServer.GetSerialNum()}");
                    UIWindow.LogToWindow("发送引导...");
                    stream = new FileStream(UIWindow.LoaderTextBox.Text, FileMode.Open, FileAccess.Read);
                    SaharaServer.SwitchMode(SaharaMode.ImageTxPending);
                    var state = SaharaServer.DoHelloHandshake(SaharaMode.ImageTxPending);
                    SaharaServer.SendProgrammer(state.ImageTransfer, stream, (uint)stream.Length);
                    UIWindow.LogToWindow("引导发送成功");
                    stream.Close();
                }
                UIWindow.LogToWindow("获取设备配置");
                var response = FirehoseServer.GetDeviceConfig();
                if(response.Response != "ACK")
                {
                    if (string.Join("", response.Logs).Contains("before authentication"))
                    {
                        if (UIWindow.ManualAuthCB.Checked)
                            DoManualAuth();
                        else
                            DoNoAuth();
                        FirehoseServer.GetDeviceConfig();
                    }
                    else
                    {
                        UIWindow.LogToWindow("获取设备配置失败,按照默认配置继续", "warn");
                    }
                }
                UIWindow.Invoke(() => UIWindow.SendLoaderCB.Checked = false);
                UIWindow.LogToWindow($"\n内存类型: {FirehoseServer.MemoryName}\n" +
                    $"TargetName:{FirehoseServer.TargetName}");
            }
            catch (Exception)
            {
                if(CurrentPort != null && CurrentPort.IsOpen)
                {
                    CurrentPort.Close();
                    CurrentPort = null;
                }
                SaharaServer = null;
                FirehoseServer = null;
                if(stream != null)
                    stream.Close();
                throw;
            }
        }
    }
}
