using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MinerGUI.Bundles;
using MinerGUI.Gui.Form;

namespace MinerGUI.Gui.Main.MainElements
{
    class Hardware : FrameContent
    {
        private bool active;

        private FramedEventHandler BundlesLoaded;
        private FramedEventHandler BundleStatusChanged;
        private FramedEventHandler BundleStatusChangedHidden;
        private List<GlobalMouseClickEventHander> bundleStartPauseEvents = new List<GlobalMouseClickEventHander>();


        const int topMargin = 189;
        int leftMargin = 431 + MainFrame.LeftPadding;
        int leftPrefixMargin = 77 + MainFrame.LeftPadding;
        int leftBundleNameMargin;
        int leftAlgoMargin;
        int leftHashrateMargin;
        int leftEstMargin;

        int hardwareLabelTopMargin = 16;
        int hardwareLineHeight = 46;
        int hardwareFirstLineTopMargin = 10;
        int secondLabelLine = 32;

        private Label hardwareLabel;
        private Label algorythmLabel;
        private Label hashrateLabel;
        private Label ethdayLabel;

        List<Bundle> bundles;
        Label[] bundlesPrefixes;
        Label[] bundlesNames;
        Label[] bundlesAlgos;
        Label[] bundlesHashrates;
        Label[] bundlesEst;

        private Color hardwareBackgroundLine = Color.FromArgb(255, 46, 53, 62);

        public Hardware(FrameForm form) : base(form)
        {
            leftAlgoMargin = MainFrame.LeftPadding + 555;
            leftHashrateMargin = leftAlgoMargin + 136;
            leftEstMargin = leftHashrateMargin + 153;
            leftBundleNameMargin = leftPrefixMargin + 45;
            hardwareLabel = new Label()
            {
                Text = "Hardware",
                ForeColor = Color.FromArgb(255, 98, 112, 131),
                Font = new Font("Arial", 12, FontStyle.Regular),
                Location = new Point(leftMargin, topMargin),
                Size = new Size(100, 23)
            };

            algorythmLabel = new Label()
            {
                Text = "ALGORYTHM",
                ForeColor = MainFrame.TitleLabelColor,
                Font = new Font("Arial", 8, FontStyle.Regular),
                Location = new Point(leftAlgoMargin, topMargin + secondLabelLine),
                Size = new Size(100, 15)
            };

            hashrateLabel = new Label()
            {
                Text = "HASHRATE",
                ForeColor = MainFrame.TitleLabelColor,
                Font = new Font("Arial", 8, FontStyle.Regular),
                Location = new Point(leftHashrateMargin, topMargin + secondLabelLine),
                Size = new Size(100, 15)
            };

            ethdayLabel = new Label()
            {
                Text = "ETH/DAY",
                ForeColor = MainFrame.TitleLabelColor,
                Font = new Font("Arial", 8, FontStyle.Regular),
                Location = new Point(leftEstMargin, topMargin + secondLabelLine),
                Size = new Size(100, 15)
            };

            BundlesLoaded = (name, data) =>
            {
                if (name.Equals("BundlesLoaded"))
                {

                    bundles = data as List<Bundle>;
                    form.FramedEvents -= BundlesLoaded;
                    bundlesPrefixes = new Label[bundles.Count];
                    bundlesNames = new Label[bundles.Count];
                    bundlesAlgos = new Label[bundles.Count];
                    bundlesHashrates = new Label[bundles.Count];
                    bundlesEst = new Label[bundles.Count];
                    for (int i = 0; i < bundles.Count(); i++)
                    {
                        String hashrate = bundles[i].GetHashrate().ToString();
                        if (hashrate.Contains(','))
                        {
                            int intsAfterDot = hashrate.Length - hashrate.IndexOf(',') - 1;
                            if (intsAfterDot > 3)
                            {
                                hashrate = hashrate.Substring(0, hashrate.IndexOf(',') + 3);
                            }
                            hashrate = hashrate.Replace(',', '.');

                        }
                        String estimates = bundles[i].Estimates.ToString();
                        if (estimates.Contains(','))
                        {
                            int intsAfterDot = estimates.Length - estimates.IndexOf(',') - 1;
                            if (intsAfterDot > 3)
                            {
                                estimates = estimates.Substring(0, estimates.IndexOf(',') + 3);
                            }
                            estimates = estimates.Replace(',', '.');

                        }
                        bundlesPrefixes[i] = new Label()
                        {
                            Text = bundles[i].Type == 0 ? "CPU:" : "GPU:",
                            ForeColor = Color.White,
                            Font = new Font("Arial", 11, FontStyle.Bold),
                            Location = new Point(leftPrefixMargin, topMargin + secondLabelLine + hardwareFirstLineTopMargin + hardwareLineHeight * i + hardwareLabelTopMargin),
                            Size = new Size(47, 20),
                            BackColor = hardwareBackgroundLine
                        };
                        bundlesNames[i] = new Label()
                        {
                            Text = bundles[i].Name,
                            ForeColor = Color.White,
                            Font = new Font("Arial", 11, FontStyle.Regular),
                            Location = new Point(leftBundleNameMargin, topMargin + secondLabelLine + hardwareFirstLineTopMargin + hardwareLineHeight * i + hardwareLabelTopMargin),
                            Size = new Size(400, 17),
                            BackColor = hardwareBackgroundLine
                        };
                        bundlesAlgos[i] = new Label()
                        {
                            Text = bundles[i].Algo.Name,
                            ForeColor = Color.White,
                            Font = new Font("Arial", 11, FontStyle.Regular),
                            Location = new Point(leftAlgoMargin, topMargin + secondLabelLine + hardwareFirstLineTopMargin + hardwareLineHeight * i + hardwareLabelTopMargin),
                            Size = new Size(120, 17)
                        };
                        bundlesHashrates[i] = new Label()
                        {
                            Text = hashrate + " " + bundles[i].Algo.HashrateSizer,
                            ForeColor = Color.White,
                            Font = new Font("Arial", 11, FontStyle.Regular),
                            Location = new Point(leftHashrateMargin, topMargin + secondLabelLine + hardwareFirstLineTopMargin + hardwareLineHeight * i + hardwareLabelTopMargin),
                            Size = new Size(120, 17)
                        };
                        bundlesEst[i] = new Label()
                        {
                            Text = estimates,
                            ForeColor = Color.White,
                            Font = new Font("Arial", 11, FontStyle.Regular),
                            Location = new Point(leftEstMargin, topMargin + secondLabelLine + hardwareFirstLineTopMargin + hardwareLineHeight * i + hardwareLabelTopMargin),
                            Size = new Size(120, 17)
                        };
                        int top = topMargin + secondLabelLine + hardwareFirstLineTopMargin + hardwareLineHeight * i + 4;
                        Rectangle activeRectangle = new Rectangle(MainFrame.LeftPadding + 19 + 23, top + 12, 16, 16);
                        Bundle activeBundle = bundles[i];
                        bundleStartPauseEvents.Add((o, i1) =>
                        {
                            if (activeRectangle.Contains(i1.Location))
                            {
                                if(activeBundle.IsMining())
                                {
                                    activeBundle.StopMining();
                                } else
                                {
                                    activeBundle.StartMining();
                                }
                                form.FireFrameEvent("BundleStatusChanged", bundles);
                            }
                        });
                    }
                }
            };
            BundleStatusChanged = (name, data) =>
            {
                if (name.Equals("BundleStatusChanged"))
                {
                    Graphics gfx = form.CreateGraphics();
                    for (int i = 0; i < bundles.Count(); i++)
                    {
                        if (bundles[i].ToRedraw)
                        {
                            int top = topMargin + secondLabelLine + hardwareFirstLineTopMargin + hardwareLineHeight * i + 4;
                            this.DrawRoundedRectangle(gfx, new Rectangle(MainFrame.LeftPadding + 19, top, 510, 40), 20, new Pen(Color.FromArgb(255, 69, 79, 93), 1), Color.FromArgb(255, 46, 53, 62));
                            if (bundles[i].IsMining())
                            {
                                this.DrawPause(gfx, new Rectangle(MainFrame.LeftPadding + 19 + 23, top + 12, 16, 16));
                            }
                            else
                            {
                                this.DrawPlay(gfx, new Rectangle(MainFrame.LeftPadding + 19 + 23, top + 12, 16, 16));
                            }
                            bundles[i].Redrawed();
                        }
                    }
                    gfx.Dispose();
                }
            };

            BundleStatusChangedHidden = (name, data) =>
            {
                if (name.Equals("BundleStatusChanged"))
                {
                    for (int i = 0; i < bundles.Count(); i++)
                    {

                        if (bundles[i].IsMining())
                        {
                            String hashrate = bundles[i].GetHashrate().ToString();
                            if (hashrate.Contains(','))
                            {
                                int intsAfterDot = hashrate.Length - hashrate.IndexOf(',') - 1;
                                if (intsAfterDot > 3)
                                {
                                    hashrate = hashrate.Substring(0, hashrate.IndexOf(',') + 3);
                                }
                                hashrate = hashrate.Replace(',', '.');

                            }
                            String estimates = bundles[i].Estimates.ToString();
                            if (estimates.Contains(','))
                            {
                                int intsAfterDot = estimates.Length - estimates.IndexOf(',') - 1;
                                if (intsAfterDot > 6)
                                {
                                    estimates = estimates.Substring(0, estimates.IndexOf(',') + 6);
                                }
                                estimates = estimates.Replace(',', '.');

                            }
                            bundlesAlgos[i].Text = bundles[i].Algo.Name;
                            bundlesHashrates[i].Text = hashrate + " " + bundles[i].Algo.HashrateSizer;
                            bundlesEst[i].Text = estimates;
                        }


                    }
                }
            };

            form.FramedEvents += BundlesLoaded;
            form.FramedEvents += BundleStatusChangedHidden;

        }

        public override void Activate(FrameForm form, Graphics gfx)
        {
            form.FramedEvents += BundleStatusChanged;
            this.active = true;
            this.AddToFormIfNotExist(hardwareLabel, form);
            this.AddToFormIfNotExist(algorythmLabel, form);
            this.AddToFormIfNotExist(hashrateLabel, form);
            this.AddToFormIfNotExist(ethdayLabel, form);
            for(int i = 0; i < bundles.Count(); i++)
            {
                int top = topMargin + secondLabelLine + hardwareFirstLineTopMargin + hardwareLineHeight * i+4;
                this.DrawRoundedRectangle(gfx, new Rectangle(MainFrame.LeftPadding + 19, top, 510, 40), 20, new Pen(Color.FromArgb(255, 69, 79, 93), 1), Color.FromArgb(255, 46, 53, 62));
                if(bundles[i].IsMining())
                {
                    this.DrawPause(gfx, new Rectangle(MainFrame.LeftPadding + 19 + 23, top + 12, 16, 16));
                } else
                {
                    this.DrawPlay(gfx, new Rectangle(MainFrame.LeftPadding + 19 + 23, top + 12, 16, 16));
                }
            }
            for (int i = 0; i < bundles.Count(); i++)
            {
            }
            foreach (Label l in bundlesPrefixes)
            {
                this.AddToFormIfNotExist(l, form);
            }
            foreach (Label l in bundlesNames)
            {
                this.AddToFormIfNotExist(l, form);
            }
            foreach (Label l in bundlesAlgos)
            {
                this.AddToFormIfNotExist(l, form);
            }
            foreach (Label l in bundlesHashrates)
            {
                this.AddToFormIfNotExist(l, form);
            }
            foreach (Label l in bundlesEst)
            {
                form.Controls.Add(l);
            }
            foreach(GlobalMouseClickEventHander e in bundleStartPauseEvents)
            {
                form.GlobalMouseClick += e;
            }
        }

        public override void Deactivate(FrameForm form, Graphics gfx)
        {
            this.active = false;
            form.FramedEvents -= BundleStatusChanged;
            form.Controls.Remove(hardwareLabel);
            form.Controls.Remove(algorythmLabel);
            form.Controls.Remove(hashrateLabel);
            form.Controls.Remove(ethdayLabel);
            foreach (Label l in bundlesPrefixes)
            {
                this.RemoveFromFormIfExist(l, form);
            }
            foreach (Label l in bundlesNames)
            {
                this.RemoveFromFormIfExist(l, form);
            }
            foreach (Label l in bundlesAlgos)
            {
                this.RemoveFromFormIfExist(l, form);
            }
            foreach (Label l in bundlesHashrates)
            {
                this.RemoveFromFormIfExist(l, form);
            }
            foreach (Label l in bundlesEst)
            {
                this.RemoveFromFormIfExist(l, form);
            }
            for (int i = 0; i < bundles.Count(); i++)
            {
                int top = topMargin + secondLabelLine + hardwareFirstLineTopMargin + hardwareLineHeight * i + 4;
                this.DrawRectangle(gfx, new Rectangle(MainFrame.LeftPadding + 19, top, 510, 40), MainFrame.BackgroundColor);
            }
            foreach (GlobalMouseClickEventHander e in bundleStartPauseEvents)
            {
                form.GlobalMouseClick -= e;
            }
        }

        public override void Draw(FrameForm form, Graphics gfx)
        {
            if(active)
            {

                this.AddToFormIfNotExist(hardwareLabel, form);
                this.AddToFormIfNotExist(algorythmLabel, form);
                this.AddToFormIfNotExist(hashrateLabel, form);
                this.AddToFormIfNotExist(ethdayLabel, form);
                for (int i = 0; i < bundles.Count(); i++)
                {
                    int top = topMargin + secondLabelLine + hardwareFirstLineTopMargin + hardwareLineHeight * i + 4;
                    this.DrawRoundedRectangle(gfx, new Rectangle(MainFrame.LeftPadding + 19, top, 510, 40), 20, new Pen(Color.FromArgb(255, 69, 79, 93), 1), Color.FromArgb(255, 46, 53, 62));
                    if (bundles[i].IsMining())
                    {
                        this.DrawPause(gfx, new Rectangle(MainFrame.LeftPadding + 19 + 23, top + 12, 16, 16));
                    }
                    else
                    {
                        this.DrawPlay(gfx, new Rectangle(MainFrame.LeftPadding + 19 + 23, top + 12, 16, 16));
                    }
                }
                for (int i = 0; i < bundles.Count(); i++)
                {
                }
                foreach (Label l in bundlesPrefixes)
                {
                    this.AddToFormIfNotExist(l, form);
                }
                foreach (Label l in bundlesNames)
                {
                    this.AddToFormIfNotExist(l, form);
                }
                foreach (Label l in bundlesAlgos)
                {
                    this.AddToFormIfNotExist(l, form);
                }
                foreach (Label l in bundlesHashrates)
                {
                    this.AddToFormIfNotExist(l, form);
                }
                foreach (Label l in bundlesEst)
                {
                    form.Controls.Add(l);
                }
            } else
            {
                form.Controls.Remove(hardwareLabel);
                form.Controls.Remove(algorythmLabel);
                form.Controls.Remove(hashrateLabel);
                form.Controls.Remove(ethdayLabel);
                foreach (Label l in bundlesPrefixes)
                {
                    this.RemoveFromFormIfExist(l, form);
                }
                foreach (Label l in bundlesNames)
                {
                    this.RemoveFromFormIfExist(l, form);
                }
                foreach (Label l in bundlesAlgos)
                {
                    this.RemoveFromFormIfExist(l, form);
                }
                foreach (Label l in bundlesHashrates)
                {
                    this.RemoveFromFormIfExist(l, form);
                }
                foreach (Label l in bundlesEst)
                {
                    this.RemoveFromFormIfExist(l, form);
                }
                for (int i = 0; i < bundles.Count(); i++)
                {
                    int top = topMargin + secondLabelLine + hardwareFirstLineTopMargin + hardwareLineHeight * i + 4;
                    this.DrawRectangle(gfx, new Rectangle(MainFrame.LeftPadding + 19, top, 510, 40), MainFrame.BackgroundColor);
                }
            }
        }
    }
}
