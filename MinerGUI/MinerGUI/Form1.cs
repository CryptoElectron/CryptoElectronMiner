using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing.Drawing2D;
using MinerGUI.Gui;
using MinerGUI.Gui.Form;
using MinerGUI.Bundles;

namespace MinerGUI
{

    public partial class Form1 : FrameForm
    {

        MainFrame mainFrame;
        bool firstDraw = true;

        public Form1() : base()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            mainFrame.Draw(this, e.Graphics, firstDraw);
            firstDraw = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<Bundle> bundles = new List<Bundle>();
            mainFrame = new MainFrame(this, bundles);
            GlobalMouseClick += (o, i) =>
            {
                int x = 0;
                int y = 0;
                if (o.GetType() == typeof(PictureBox))
                {
                    PictureBox pic = o as PictureBox;
                    x += pic.Location.X;
                    y += pic.Location.Y;
                }
                x += i.X;
                y += i.Y;
                Graphics gfx = this.CreateGraphics();
                mainFrame.Click(x, y, this, gfx);
                gfx.Dispose();
            };
            Random rnd = new Random();
            ManagementObjectSearcher mos =
              new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject mo in mos.Get())
            {
                double es = rnd.NextDouble() / 100f;
                bundles.Add(new Bundle(mo["Name"].ToString(), new Gui.Bundles.Algo("CryptoNight", "h/s"), rnd.NextDouble() * 999, es, 0));
            }
            for (int a = 0; a < 2; a++)
            {
                bundles.Add(Bundle.getRandomBundle(rnd));
            }
            FrameForm s = this;
            Thread t = new Thread(delegate ()
            {
                s.Invoke((MethodInvoker)(() => { this.FireFrameEvent("BundlesLoaded", bundles); }));
                while (true)
                {
                    try
                    {
                        Double est = 0;
                        foreach (Bundle bundle in bundles)
                        {
                            if (bundle.IsMining())
                            {
                                Double d = 0.99 + rnd.NextDouble() / 50f;
                                bundle.Hashrate *= d;
                                bundle.Estimates *= d;
                                est += bundle.Estimates;
                            }
                        }

                        s.Invoke((MethodInvoker)(() => { this.FireFrameEvent("DailyETHEarningChanged", est); }));
                        s.Invoke((MethodInvoker)(() => { this.FireFrameEvent("ETHBalanceChanged", est / 50); }));
                        s.Invoke((MethodInvoker)(() => { this.FireFrameEvent("BundleStatusChanged", bundles); }));
                        Thread.Sleep(200);
                    }
                    catch (Exception) { }
                }
            });
            t.Start();
            s.FormClosed += (o, i) =>
            {
                t.Abort();
            };
            /*String args = "--farm-recheck 200 -U -S lb.geo.pirlpool.eu:8002 -SP 1 -O 0xddd841483e6b42c35e40164278e684c7e6e2b271.1080Ti";
            String pathToEthminer = "C:\\Users\\aki\\Desktop\\mining\\ethminer\\ethminer.exe";
            MinerExecutable.Clear("Ethash");
            String pathToExecutable = MinerExecutable.Build("Ethash", pathToEthminer, args, false);
            //MessageBox.Show();
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.FileName = Path.GetFullPath(pathToExecutable);
            p.Start();
            ChildProcessTracker.AddProcess(p);
            /*new Thread(delegate ()
            {
                while (true)
                {
                    label1.Invoke(new Action(() => { label1.Text = p.StandardOutput.ReadLine(); }));
                }
            }).Start();*
            new Thread(delegate ()
            {
                MessageBox.Show("p");
                Thread.Sleep(1000);
                KillProcessAndChildren(p.Id);
                MessageBox.Show("killed");
            }).Start();*/
        }

        /// <summary>
        /// Kill a process, and all of its children, grandchildren, etc.
        /// </summary>
        /// <param name="pid">Process ID.</param>
        private static void KillProcessAndChildren(int pid)
        {
            // Cannot close 'system idle process'.
            if (pid == 0)
            {
                return;
            }
            ManagementObjectSearcher searcher = new ManagementObjectSearcher
              ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process proc = Process.GetProcessById(pid);
                proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
        }
    }
}
