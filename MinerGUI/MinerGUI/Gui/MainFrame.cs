using MinerGUI.Bundles;
using MinerGUI.Gui.Bundles;
using MinerGUI.Gui.Form;
using MinerGUI.Gui.LeftMenu;
using MinerGUI.Gui.Main;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinerGUI.Gui
{
    class MainFrame
    {
        public static int TopPadding = 0;
        public static int LeftPadding = 99;
        public static Double ETHIndex = 670;

        FrameForm form;

        SolidBrush leftMenuBackground;
        SolidBrush logoMenuBackground;
        PictureBox logoPicture;

        const int menuWidth = 99;
        const int logoHeight = 99;
        const int menuHeight = 60;
        const int menuTopPadding = 13;

        Color leftMenuBackgroundColor = Color.FromArgb(255, 55, 62, 72);
        public static Color TitleLabelColor = Color.FromArgb(255, 121, 143, 166);
        public static Color BackgroundColor = Color.FromArgb(255, 41, 47, 55);


        MenuPoint miningMenuPoint;
        MenuPoint hardwareMenuPoint;
        MenuPoint userMenuPoint;
        MenuPoint settingsMenuPoint;
        MenuPoint supportMenuPoint;

        MenuPoint activeMenuPoint = null;

        List<MenuPoint> menuPoints = new List<MenuPoint>();
        
        internal bool Click(int x, int y, FrameForm form, Graphics gfx)
        {
            foreach (MenuPoint menuPoint in menuPoints)
            {
                if (menuPoint.MenuRectangle.Contains(new Point(x, y)) && menuPoint != activeMenuPoint)
                {
                    this.ActivateNewMenuPoint(menuPoint, form, gfx);
                    return true;
                }
            }
            return false;
        }

        public void Draw(FrameForm form, Graphics graphics, bool firstRun)
        {
            graphics.FillRectangle(leftMenuBackground, new Rectangle(0, logoHeight, menuWidth, 345));
            graphics.FillRectangle(logoMenuBackground, new Rectangle(0, 0, menuWidth, logoHeight));

            if (!form.Controls.Contains(logoPicture))
            {
                form.Controls.Add(logoPicture);
            }

            miningMenuPoint.Draw(form, graphics);
            hardwareMenuPoint.Draw(form, graphics);
            userMenuPoint.Draw(form, graphics);
            settingsMenuPoint.Draw(form, graphics);
            supportMenuPoint.Draw(form, graphics);

            if (firstRun) this.ActivateNewMenuPoint(miningMenuPoint, form, graphics);
        }

        private void ActivateNewMenuPoint(MenuPoint menuPoint, FrameForm form, Graphics gfx)
        {
            if (activeMenuPoint != menuPoint)
            {
                if (activeMenuPoint != null)
                {
                    activeMenuPoint.Deactivate(form, gfx);
                }
                activeMenuPoint = menuPoint;
                activeMenuPoint.Activate(form, gfx);
            }

        }
        


        public MainFrame(FrameForm form, List<Bundle> bundles)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            this.form = form;
            leftMenuBackground = new System.Drawing.SolidBrush(leftMenuBackgroundColor);
            logoMenuBackground = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(255, 33, 37, 43));

            logoPicture = new PictureBox
            {
                Name = "logoPicture",
                Size = new Size(55, 55),
                Location = new Point(22, 22),
                Image = Image.FromStream(assembly.GetManifestResourceStream("MinerGUI.Resources.logo.png")),
                BackColor = Color.FromArgb(255, 33, 37, 43),
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            miningMenuPoint = new MenuPoint("mining.png", new MainContent(form, bundles), assembly, leftMenuBackgroundColor, logoHeight + menuTopPadding, menuHeight, 0, menuWidth);
            hardwareMenuPoint = new MenuPoint("hardware.png", new HardwareContent(form, bundles), assembly, leftMenuBackgroundColor, logoHeight + menuTopPadding, menuHeight, 1, menuWidth);
            userMenuPoint = new MenuPoint("user.png", new UserContent(form, new UserForm()), assembly, leftMenuBackgroundColor, logoHeight + menuTopPadding, menuHeight, 2, menuWidth);
            settingsMenuPoint = new MenuPoint("settings.png", new SettingsContent(form, new SettingsForm()), assembly, leftMenuBackgroundColor, logoHeight + menuTopPadding, menuHeight, 3, menuWidth);
            supportMenuPoint = new MenuPoint("support.png", new SupportContent(form), assembly, leftMenuBackgroundColor, logoHeight + menuTopPadding, menuHeight, 4, menuWidth);

            menuPoints.Add(miningMenuPoint);
            menuPoints.Add(hardwareMenuPoint);
            menuPoints.Add(userMenuPoint);
            menuPoints.Add(settingsMenuPoint);
            menuPoints.Add(supportMenuPoint);
        }
    }
}
