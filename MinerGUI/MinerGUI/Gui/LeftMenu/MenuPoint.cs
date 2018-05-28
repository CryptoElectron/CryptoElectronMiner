using MinerGUI.Gui.Form;
using MinerGUI.Gui.Main;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinerGUI.Gui.LeftMenu
{
    class MenuPoint
    {
        PictureBox pointLightPicture;
        PictureBox pointDarkPicture;

        const int activePicWidth = 32;
        const int activePicHeight = 32;

        Boolean active = false;

        public Rectangle MenuRectangle { get; }
        private Rectangle menuActiveRectangle { get; }
        private SolidBrush menuActiveLine;
        private SolidBrush menuNonactiveLine;

        private String resourceName;
        private FrameContent frameContent;

        public MenuPoint(string resourceName, FrameContent content, Assembly assembly, Color backgroundColor, int logoTopMargin, int menuHeight, int position, int menuWidth)
        {
            this.resourceName = resourceName;
            this.frameContent = content;

            MenuRectangle = new Rectangle(0, logoTopMargin + menuHeight * position, menuWidth, menuHeight);

            menuActiveLine = new SolidBrush(Color.FromArgb(255, 31, 214, 208));
            menuNonactiveLine = new SolidBrush(backgroundColor);
            menuActiveRectangle = new Rectangle(0, logoTopMargin + menuHeight * position, 5, menuHeight);

            pointLightPicture = new PictureBox
            {
                Name = "light"+ resourceName,
                Size = new Size(32, 32),
                Location = new Point((menuWidth - activePicWidth) / 2, (logoTopMargin + menuHeight * position) + (menuHeight - activePicHeight) / 2),
                Image = Image.FromStream(assembly.GetManifestResourceStream("MinerGUI.Resources.Menu.Light." + resourceName)),
                BackColor = backgroundColor,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pointDarkPicture = new PictureBox
            {
                Name = "dark" + resourceName,
                Size = new Size(32, 32),
                Location = new Point((menuWidth - activePicWidth) / 2, (logoTopMargin + menuHeight * position) + (menuHeight - activePicHeight) / 2),
                Image = Image.FromStream(assembly.GetManifestResourceStream("MinerGUI.Resources.Menu.Dark." + resourceName)),
                BackColor = backgroundColor,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
        }

        public void Draw(FrameForm form, Graphics graphics)
        {
            form.Controls.Add(pointDarkPicture);
            if (active)
            {
                graphics.FillRectangle(menuActiveLine, menuActiveRectangle);
                if (form.Controls.Contains(pointDarkPicture))
                {
                    form.Controls.Remove(pointDarkPicture);
                }
                form.Controls.Add(pointLightPicture);
            } else
            {
                graphics.FillRectangle(menuNonactiveLine, menuActiveRectangle);
                if (form.Controls.Contains(pointLightPicture))
                {
                    form.Controls.Remove(pointLightPicture);
                }
                form.Controls.Add(pointDarkPicture);
            }
        }

        public void Activate(FrameForm form, Graphics graphics)
        {
            if (active) return;
            active = true;
            graphics.FillRectangle(menuActiveLine, menuActiveRectangle);
            if (form.Controls.Contains(pointDarkPicture))
            {
                form.Controls.Remove(pointDarkPicture);
            }
            form.Controls.Add(pointLightPicture);
            this.frameContent.Activate(form, graphics);
        }

        public void Deactivate(FrameForm form, Graphics graphics)
        {
            if (!active) return;
            active = false;
            graphics.FillRectangle(menuNonactiveLine, menuActiveRectangle);
            if (form.Controls.Contains(pointLightPicture))
            {
                form.Controls.Remove(pointLightPicture);
            }
            form.Controls.Add(pointDarkPicture);
            this.frameContent.Deactivate(form, graphics);
        }
    }
}
