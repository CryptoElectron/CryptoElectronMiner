using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MinerGUI.Bundles;
using MinerGUI.Gui.Form;
using MinerGUI.Gui.Main.MainElements;

namespace MinerGUI.Gui.Main
{
    class MainContent : FrameContent
    {
        const int topMargin = 22;
        int leftMargin = 440+ MainFrame.LeftPadding;

        private Label miningLabel;

        public MainContent(FrameForm form, List<Bundle> bundles) : base(form)
        {
            this.RegisterChildContent(new StartMiningButton(form, bundles));
            this.RegisterChildContent(new EstimatedDailyEarnings(form));
            this.RegisterChildContent(new Balance(form));
            this.RegisterChildContent(new Benchmark(form));
            this.RegisterChildContent(new Hardware(form));
            miningLabel = new Label()
            {
                Text = "Mining",
                ForeColor = Color.FromArgb(255, 98, 112, 131),
                Font = new Font("Arial", 12, FontStyle.Regular),
                Location = new Point(leftMargin, topMargin),
                Size = new Size(57, 23)
            };
        }

        public override void Activate(FrameForm form, Graphics gfx)
        {
            form.Controls.Add(miningLabel);
            this.ActivateChildContent(form, gfx);
        }

        public override void Deactivate(FrameForm form, Graphics gfx)
        {
            form.Controls.Remove(miningLabel);
            this.DeactivateChildContent(form, gfx);
        }
    }
}
