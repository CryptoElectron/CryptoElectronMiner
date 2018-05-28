using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MinerGUI.Bundles;
using MinerGUI.Gui.Form;

namespace MinerGUI.Gui.Main.AdvancedElements
{
    class AdvancedBundle : FrameContent
    {
        private FramedEventHandler BundleStatusChanged;
        private GlobalMouseClickEventHander globalMouseClickEventHander;

        private Color hardwareBackgroundLine = Color.FromArgb(255, 46, 53, 62);
        const int bundleWidth = 890;
        const int paddingLeft = 19;
        int leftPrefixMargin = MainFrame.LeftPadding + 78;
        int leftBundleNameMargin = MainFrame.LeftPadding + 125;
        int playPauseLeftPadding = 23;
        int playPauseTopPadding = 12;
        int topMargin;

        private Rectangle bundleRectangle;
        private Rectangle playPauseRectangle;
        private Bundle bundle;

        private Label bundlePrefix;
        private Label bundleName;

        public AdvancedBundle(FrameForm form, Bundle bundle, List<Bundle> bundles, int topMargin, int elementHeight) : base(form)
        {
            this.topMargin = topMargin;
            this.bundle = bundle;


            bundleRectangle = new Rectangle(MainFrame.LeftPadding + paddingLeft, topMargin, bundleWidth, elementHeight);
            playPauseRectangle = new Rectangle(MainFrame.LeftPadding + paddingLeft + playPauseLeftPadding, topMargin + playPauseTopPadding, 16, 16);

            bundlePrefix = new Label()
            {
                Text = bundle.Type == 0 ? "CPU:" : "GPU:",
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = new Point(leftPrefixMargin, topMargin + 13),
                Size = new Size(47, 20),
                BackColor = hardwareBackgroundLine
            };
            bundleName = new Label()
            {
                Text = bundle.Name,
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Regular),
                Location = new Point(leftBundleNameMargin, topMargin + 13),
                Size = new Size(400, 17),
                BackColor = hardwareBackgroundLine
            };
            globalMouseClickEventHander = (o, i1) =>
            {
                if (playPauseRectangle.Contains(i1.Location))
                {
                    if (bundle.IsMining())
                    {
                        bundle.StopMining();
                    }
                    else
                    {
                        bundle.StartMining();
                    }
                    form.FireFrameEvent("BundleStatusChanged", bundles);
                }
            };
            BundleStatusChanged = (name, data) =>
            {
                Graphics gfx = form.CreateGraphics();
                for (int i = 0; i < bundles.Count(); i++)
                {
                    if (bundles[i].Equals(bundle) && bundles[i].ToRedraw)
                    {
                        this.DrawRectangle(gfx, this.GetIncreasedRectangle(playPauseRectangle, 1), hardwareBackgroundLine);
                        if (bundles[i].IsMining())
                        {
                            this.DrawPause(gfx, playPauseRectangle);
                        }
                        else
                        {
                            this.DrawPlay(gfx, playPauseRectangle);
                        }
                        bundles[i].Redrawed();
                    }
                }
                gfx.Dispose();
            };
        }

        public override void Activate(FrameForm form, Graphics gfx)
        {
            form.GlobalMouseClick += globalMouseClickEventHander;
            form.FramedEvents += BundleStatusChanged;

            this.DrawRoundedRectangle(gfx, bundleRectangle, 20, new Pen(Color.FromArgb(255, 69, 79, 93), 1), Color.FromArgb(255, 46, 53, 62));
            form.Controls.Add(bundlePrefix);
            form.Controls.Add(bundleName);

            if (bundle.IsMining())
            {
                this.DrawPause(gfx, playPauseRectangle);
            }
            else
            {
                this.DrawPlay(gfx, playPauseRectangle);
            }
        }

        public override void Deactivate(FrameForm form, Graphics gfx)
        {
            form.GlobalMouseClick -= globalMouseClickEventHander;
            form.FramedEvents -= BundleStatusChanged;

            this.DrawRectangle(gfx, bundleRectangle, MainFrame.BackgroundColor);
            form.Controls.Remove(bundlePrefix);
            form.Controls.Remove(bundleName);
            this.DrawRectangle(gfx, new Rectangle(MainFrame.LeftPadding + paddingLeft + playPauseLeftPadding, topMargin + playPauseTopPadding, 510, 40), MainFrame.BackgroundColor);
        }
    }
}
