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
    class EstimatedDailyEarnings : FrameContent
    {
        private bool active;

        private FramedEventHandler DailyEarningChanged;

        private Label estimatedDailyEarningsLabel;
        private Label ethBalance;
        private Label usdBalance;

        const int topMargin = 67;
        const int rectangleTopMargin = 18;
        const int leftMargin = 267;

        const int buttonHeight = 73;
        const int buttonWidth = 230;
        const int borderWidth = 0;

        private Rectangle areaRectangle;

        private Color areaBackgroundColor = Color.FromArgb(255, 18, 96, 119);

        private Double estimatedETH = 0.67;

        public EstimatedDailyEarnings(FrameForm form) : base(form)
        {
            areaRectangle = new Rectangle(MainFrame.LeftPadding + leftMargin, topMargin + rectangleTopMargin, buttonWidth, buttonHeight);

            estimatedDailyEarningsLabel = new Label()
            {
                Text = "ESTIMATED DAILY EARNINGS",
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
                if (name.Equals("DailyETHEarningChanged"))
                {
                    String estimatedEthString;
                    String estimatedUsdString;

                    estimatedETH = Double.Parse(data.ToString());
                    Double usdEq = estimatedETH * MainFrame.ETHIndex;

                    if (estimatedETH.ToString().Contains(','))
                    {
                        int dotPos = estimatedETH.ToString().IndexOf(',');
                        estimatedEthString = estimatedETH.ToString().Replace(',', '.');
                        if (dotPos + 8 < estimatedETH.ToString().Length)
                        {
                            estimatedEthString = estimatedEthString.Substring(0, 8 + dotPos);
                        }
                    }
                    else
                    {
                        estimatedEthString = estimatedETH.ToString();
                    }


                    if (usdEq.ToString().Contains(','))
                    {
                        int usdDotPos = usdEq.ToString().IndexOf(',');
                        estimatedUsdString = usdEq.ToString().Replace(',', '.');
                        if (usdDotPos + 3 < usdEq.ToString().Length)
                        {
                            estimatedUsdString = estimatedUsdString.Substring(0, 3 + usdDotPos);
                        }
                    }
                    else
                    {
                        estimatedUsdString = usdEq.ToString();
                    }


                    ethBalance.Text = estimatedEthString + " ETH";
                    usdBalance.Text = "USD " + estimatedUsdString;
                }
            };

            form.FramedEvents += DailyEarningChanged;
        }

        public override void Activate(FrameForm form, Graphics gfx)
        {
            this.active = true;
            this.AddToFormIfNotExist(estimatedDailyEarningsLabel, form);
            this.AddToFormIfNotExist(ethBalance, form);
            this.AddToFormIfNotExist(usdBalance, form);
            this.DrawRoundedRectangle(gfx, areaRectangle, 5, new Pen(Color.Transparent), areaBackgroundColor);
        }

        public override void Deactivate(FrameForm form, Graphics gfx)
        {
            this.active = false;
            this.RemoveFromFormIfExist(estimatedDailyEarningsLabel, form);
            this.RemoveFromFormIfExist(ethBalance, form);
            this.RemoveFromFormIfExist(usdBalance, form);
            this.DrawRectangle(gfx, areaRectangle, MainFrame.BackgroundColor);
        }

        public override void Draw(FrameForm form, Graphics gfx)
        {
            if(active)
            {
                this.AddToFormIfNotExist(estimatedDailyEarningsLabel, form);
                this.AddToFormIfNotExist(ethBalance, form);
                this.AddToFormIfNotExist(usdBalance, form);
                this.DrawRoundedRectangle(gfx, areaRectangle, 5, new Pen(Color.Transparent), areaBackgroundColor);
            } else
            {
                this.RemoveFromFormIfExist(estimatedDailyEarningsLabel, form);
                this.AddToFormIfNotExist(ethBalance, form);
                this.AddToFormIfNotExist(usdBalance, form);
                this.DrawRectangle(gfx, areaRectangle, MainFrame.BackgroundColor);
            }
        }
    }
}
