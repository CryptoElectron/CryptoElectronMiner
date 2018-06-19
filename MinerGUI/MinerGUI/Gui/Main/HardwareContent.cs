using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MinerGUI.Bundles;
using MinerGUI.Gui.Form;
using MinerGUI.Gui.Main.AdvancedElements;

namespace MinerGUI.Gui.Main
{
    class HardwareContent : FrameContent
    {
        private FramedEventHandler BundlesLoaded;
        private bool active;

        const int topMargin = 20;
        int leftMargin = 394 + MainFrame.LeftPadding;
        const int advancedBundleHeight = 40;
        const int advancedBundlesTopMargin = 60;
        const int advancedBundlesTopPadding = 6;

        private Label advancedMiningLabel;
        public HardwareContent(FrameForm form, List<Bundle> bundles) : base(form)
        {
            advancedMiningLabel = new Label()
            {
                Text = "Advanced Mining",
                ForeColor = Color.FromArgb(255, 98, 112, 131),
                Font = new Font("Arial", 12, FontStyle.Regular),
                Location = new Point(leftMargin, topMargin),
                Size = new Size(200, 23)
            };
            BundlesLoaded = (name, data) =>
            {
                if(name.Equals("BundlesLoaded"))
                {
                    bundles = data as List<Bundle>;
                    for (int i = 0; i < bundles.Count(); i++)
                    {
                        this.RegisterChildContent(new AdvancedBundle(form, bundles[i], bundles, advancedBundlesTopMargin + i * advancedBundlesTopPadding + i * advancedBundleHeight, advancedBundleHeight));
                    }
                }
                form.FramedEvents -= BundlesLoaded;
            };
            form.FramedEvents += BundlesLoaded;
        }

        public override void Activate(FrameForm form, Graphics gfx)
        {
            active = true;
            this.AddToFormIfNotExist(advancedMiningLabel, form);
            this.ActivateChildContent(form, gfx);
        }

        public override void Deactivate(FrameForm form, Graphics gfx)
        {
            active = false;
            this.RemoveFromFormIfExist(advancedMiningLabel, form);
            this.DeactivateChildContent(form, gfx);
        }

        public override void Draw(FrameForm form, Graphics gfx)
        {
            if(active)
            {
                this.AddToFormIfNotExist(advancedMiningLabel, form);
            } else
            {
                this.RemoveFromFormIfExist(advancedMiningLabel, form);
            }
        }
    }
}
