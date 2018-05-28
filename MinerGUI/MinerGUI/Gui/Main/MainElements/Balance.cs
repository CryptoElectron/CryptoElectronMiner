using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MinerGUI.Gui.Form;

namespace MinerGUI.Gui.Main.MainElements
{
    class Balance : FrameContent
    {
        private FramedEventHandler DailyEarningChanged;

        private Label yourBalance;
        private Label ethBalance;
        private Label usdBalance;

        const int topMargin = 67;
        const int rectangleTopMargin = 18;
        const int leftMargin = 267+240;

        const int buttonHeight = 73;
        const int buttonWidth = 230;
        const int borderWidth = 0;

        private Rectangle areaRectangle;

        private Color areaBackgroundColor = Color.FromArgb(255, 124, 171, 63);

        private Double ETHBalance = 2.35458;

        public Balance(FrameForm form) : base(form)
        {
            areaRectangle = new Rectangle(MainFrame.LeftPadding + leftMargin, topMargin + rectangleTopMargin, buttonWidth, buttonHeight);

            yourBalance = new Label()
            {
                Text = "YOUR BALANCE",
                ForeColor = MainFrame.TitleLabelColor,
                Font = new Font("Arial", 8, FontStyle.Regular),
                Location = new Point(MainFrame.LeftPadding + leftMargin, topMargin),
                Size = new Size(200, 11)
            };
            ethBalance = new Label()
            {
                Text = "0.02 ETH",
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = new Point(MainFrame.LeftPadding + leftMargin + 20, topMargin + rectangleTopMargin + 17),
                Size = new Size(150, 19),
                BackColor = areaBackgroundColor
            };
            usdBalance = new Label()
            {
                Text = "USD 0.01",
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Regular),
                Location = new Point(MainFrame.LeftPadding + leftMargin + 20, topMargin + rectangleTopMargin + 41),
                Size = new Size(150, 19),
                BackColor = areaBackgroundColor
            };

            DailyEarningChanged = (name, data) =>
            {
                if (name.Equals("ETHBalanceChanged"))
                {
                    String ethStrBalance;
                    String usdStrBalance;

                    ETHBalance += Double.Parse(data.ToString());
                    if (ETHBalance.ToString().Contains(','))
                    {
                        int dotPos = ETHBalance.ToString().IndexOf(',');
                        ethStrBalance = ETHBalance.ToString().Replace(',', '.');
                        if (8 + dotPos < ETHBalance.ToString().Length)
                        {
                            ethBalance.Text = ethStrBalance.Substring(0, 8 + dotPos);
                        }
                    }
                    else
                    {
                        ethStrBalance = ethBalance.ToString();
                    }
                    Double usdEq = ETHBalance * MainFrame.ETHIndex;

                    if (usdEq.ToString().Contains(','))
                    {
                        int usdDotPos = usdEq.ToString().IndexOf(',');
                        usdStrBalance = usdEq.ToString().Replace(',', '.');
                        if (3 + usdDotPos < usdEq.ToString().Length)
                        {
                            usdStrBalance = usdStrBalance.Substring(0, 3 + usdDotPos);
                        }
                    }
                    else
                    {
                        usdStrBalance = usdEq.ToString();
                    }


                    ethBalance.Text = ethStrBalance + " ETH";
                    usdBalance.Text = "USD " + usdStrBalance;

                }
            };

            form.FramedEvents += DailyEarningChanged;
        }

        public override void Activate(FrameForm form, Graphics gfx)
        {
            form.Controls.Add(yourBalance);
            form.Controls.Add(ethBalance);
            form.Controls.Add(usdBalance);
            this.DrawRoundedRectangle(gfx, areaRectangle, 5, new Pen(Color.Transparent), areaBackgroundColor);
        }

        public override void Deactivate(FrameForm form, Graphics gfx)
        {
            form.Controls.Remove(yourBalance);
            form.Controls.Remove(ethBalance);
            form.Controls.Remove(usdBalance);
            this.DrawRectangle(gfx, areaRectangle, MainFrame.BackgroundColor);
        }
    }
}
