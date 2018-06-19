using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MinerGUI.Bundles;
using MinerGUI.Gui.Form;

namespace MinerGUI.Gui.Main.MainElements
{
    class StartMiningButton : FrameContent
    {
        private bool active;

        private FramedEventHandler BundleStatusChanged;
        private FramedEventHandler BundleStatusChangedHidden;
        private GlobalMouseClickEventHander globalMouseClickEventHander;

        private bool mining = false;

        int leftPadding = MainFrame.LeftPadding;
        int topPadding = MainFrame.TopPadding;

        const int topMargin = 85;
        const int leftMargin = 17;

        const int buttonHeight = 74;
        const int buttonWidth = 208;
        const int borderWidth = 2;

        private Label statusLabel;
        private Rectangle buttonRectangle;
        private Rectangle playPauseButtonRectangle;
        private Color buttonBackground;

        public StartMiningButton(FrameForm form, List<Bundle> bundles) : base(form)
        {
            StartMiningButton s = this;

            buttonBackground = Color.FromArgb(255, 39, 52, 62);
            statusLabel = new Label()
            {
                Text = "START",
                ForeColor = Color.White,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(leftPadding + leftMargin + 86, topPadding + topMargin + 28),
                BackColor = buttonBackground
            };

            buttonRectangle = new Rectangle(leftPadding + leftMargin, topPadding + topMargin, buttonWidth, buttonHeight);
            playPauseButtonRectangle = new Rectangle(buttonRectangle.X + 22, buttonRectangle.Y + 23, 27, 30);

            BundleStatusChangedHidden = (name, data) =>
            {
                if (name.Equals("BundleStatusChanged"))
                {
                    mining = IsMining(data as List<Bundle>);
                }
            };
            BundleStatusChanged = (name, data) =>
            {
                if (name.Equals("BundleStatusChanged"))
                {
                    bool newStatus = IsMining(data as List<Bundle>);
                    if(newStatus != mining)
                    {

                        mining = newStatus;
                        Graphics gfx = this.Form.CreateGraphics();
                        gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        statusLabel.Text = mining ? "STOP" : "START";

                        this.ClearPlayPause(gfx, playPauseButtonRectangle, buttonBackground);
                        if (mining)
                        {
                            this.DrawPause(gfx, playPauseButtonRectangle);
                        } else
                        {
                            this.DrawPlay(gfx, playPauseButtonRectangle);
                        }
                        gfx.Dispose();
                    }
                }
            };
            statusLabel.Click += (o, i) =>
            {
                if (IsMining(bundles))
                {
                    foreach (Bundle bundle in bundles)
                    {
                        bundle.StopMining();
                    }
                }
                else
                {
                    foreach (Bundle bundle in bundles)
                    {
                        bundle.StartMining();
                    }
                }
                form.FireFrameEvent("BundleStatusChanged", bundles);
            };
            globalMouseClickEventHander = (o, i) =>
            {
                if(buttonRectangle.Contains(i.Location))
                {
                    if (IsMining(bundles)) {
                        foreach(Bundle bundle in bundles)
                        {
                            bundle.StopMining();   
                        }
                    } else
                    {
                        foreach (Bundle bundle in bundles)
                        {
                            bundle.StartMining();
                        }
                    }
                    form.FireFrameEvent("BundleStatusChanged", bundles);
                }
            };

            form.FramedEvents += BundleStatusChangedHidden;
        }

        public override void Activate(FrameForm form, Graphics gfx)
        {
            this.active = true;
            form.FramedEvents -= BundleStatusChangedHidden;
            form.FramedEvents += BundleStatusChanged;
            form.GlobalMouseClick += globalMouseClickEventHander;
            this.AddToFormIfNotExist(statusLabel, form);
            this.DrawRoundedRectangle(gfx, buttonRectangle, 5, new Pen(Color.FromArgb(255, 31, 214, 208), 2), buttonBackground);
            if(mining)
            {
                this.DrawPause(gfx, playPauseButtonRectangle);
            } else
            {
                this.DrawPlay(gfx, playPauseButtonRectangle);
            }
        }

        public override void Deactivate(FrameForm form, Graphics gfx)
        {
            this.active = false;
            form.FramedEvents -= BundleStatusChanged;
            form.FramedEvents += BundleStatusChangedHidden;
            form.GlobalMouseClick -= globalMouseClickEventHander;
            this.RemoveFromFormIfExist(statusLabel, form);
            this.DrawRectangle(gfx, buttonRectangle, MainFrame.BackgroundColor);
        }

        private bool IsMining(List<Bundle> bundles)
        {
            foreach (Bundle bundle in bundles)
            {
                if (bundle.IsMining())
                {
                    return true;
                }
            }
            return false;
        }
        private void ClearPlayPause(Graphics gfx, Rectangle r, Color c)
        {
            this.DrawRectangle(gfx, this.GetIncreasedRectangle(r, 2), c);
        }

        public override void Draw(FrameForm form, Graphics gfx)
        {
            if(active)
            {
                this.AddToFormIfNotExist(statusLabel, form);
                this.DrawRoundedRectangle(gfx, buttonRectangle, 5, new Pen(Color.FromArgb(255, 31, 214, 208), 2), buttonBackground);
                if (mining)
                {
                    this.DrawPause(gfx, playPauseButtonRectangle);
                }
                else
                {
                    this.DrawPlay(gfx, playPauseButtonRectangle);
                }
            } else
            {
                this.RemoveFromFormIfExist(statusLabel, form);
                this.DrawRectangle(gfx, buttonRectangle, MainFrame.BackgroundColor);
            }
        }
    }
}
