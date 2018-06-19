using MinerGUI.Gui.Form;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinerGUI.Gui.Main
{
    abstract class FrameContent
    {
        public FrameForm Form { get; }
        private List<FrameContent> childContents = new List<FrameContent>();
        public FrameContent(FrameForm form)
        {
            this.Form = form;
        }
        public void RegisterChildContent(FrameContent frameContent)
        {
            this.childContents.Add(frameContent);
        }
        public void ActivateChildContent(FrameForm form, Graphics gfx)
        {
            foreach(FrameContent childContent in childContents)
            {
                childContent.Activate(form, gfx);
            }
        }
        public void DeactivateChildContent(FrameForm form, Graphics gfx)
        {
            foreach (FrameContent childContent in childContents)
            {
                childContent.Deactivate(form, gfx);
            }
        }
        public abstract void Activate(FrameForm form, Graphics gfx);
        public abstract void Deactivate(FrameForm form, Graphics gfx);
        public abstract void Draw(FrameForm form, Graphics gfx);



        public void DrawRoundedRectangle(Graphics gfx, Rectangle Bounds, int CornerRadius, Pen DrawPen, Color FillColor)
        {
            gfx.SmoothingMode = SmoothingMode.HighQuality;
            int strokeOffset = Convert.ToInt32(Math.Ceiling(DrawPen.Width));
            Bounds = Rectangle.Inflate(Bounds, -strokeOffset, -strokeOffset);

            DrawPen.EndCap = DrawPen.StartCap = LineCap.Round;

            GraphicsPath gfxPath = new GraphicsPath();
            gfxPath.AddArc(Bounds.X, Bounds.Y, CornerRadius, CornerRadius, 180, 90);
            gfxPath.AddArc(Bounds.X + Bounds.Width - CornerRadius, Bounds.Y, CornerRadius, CornerRadius, 270, 90);
            gfxPath.AddArc(Bounds.X + Bounds.Width - CornerRadius, Bounds.Y + Bounds.Height - CornerRadius, CornerRadius, CornerRadius, 0, 90);
            gfxPath.AddArc(Bounds.X, Bounds.Y + Bounds.Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);
            gfxPath.CloseAllFigures();

            gfx.FillPath(new SolidBrush(FillColor), gfxPath);
            gfx.DrawPath(DrawPen, gfxPath);
        }

        public void DrawRectangle(Graphics gfx, Rectangle Bounds, Color FillColor)
        {
            SolidBrush myBrush = new SolidBrush(FillColor);
            gfx.FillRectangle(myBrush, Bounds);
            myBrush.Dispose();
        }
        public void DrawPause(Graphics gfx, Rectangle bound)
        {
            SolidBrush pal1 = new SolidBrush(Color.White);
            int l = (int)Math.Round(bound.Width * 0.37);
            Rectangle f1 = new Rectangle(bound.X, bound.Y, l, bound.Height);
            Rectangle f2 = new Rectangle(bound.X + bound.Width - l, bound.Y, l, bound.Height);
            gfx.FillRectangle(pal1, f1);
            gfx.FillRectangle(pal1, f2);
            pal1.Dispose();
        }
        public void DrawPlay(Graphics gfx, Rectangle bound)
        {
            Point[] points = { new Point(bound.X, bound.Y), new Point(bound.X + bound.Width, bound.Y + bound.Height/2), new Point(bound.X, bound.Y + bound.Height) };
            SolidBrush pal1 = new SolidBrush(Color.FromArgb(255, 122, 138, 160));
            gfx.FillPolygon(pal1, points);
            pal1.Dispose();
        }

        internal void DrawContent(FrameForm form, Graphics graphics)
        {
            this.Draw(form, graphics);
            foreach (FrameContent childContent in childContents)
            {
                childContent.DrawContent(form, graphics);
            }
        }

        public Rectangle GetIncreasedRectangle(Rectangle r, int px)
        {
            return new Rectangle(r.X - px, r.Y - px, r.Width + px*2, r.Height + px*2);
        }

        public void AddToFormIfNotExist(Control control, FrameForm form)
        {
            if (!form.Controls.Contains(control))
            {
                form.Controls.Add(control);
            }
        }
        public void RemoveFromFormIfExist(Control control, FrameForm form)
        {
            if (form.Controls.Contains(control))
            {
                form.Controls.Remove(control);
            }
        }
    }
}
