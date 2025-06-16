using System;
using System.Drawing;

namespace easy
{
    internal class Face : Share
    {
        private Circle left, rigt;
        private Brush eye;
        public Face(int x, int y, int w, int h, Color c) : base(x, y, w, h, c)
        {
            this.h = w;
            left = new Circle(x + w / 10, y + w / 6, w / 6, w / 6, Color.Black);
            rigt = new Circle(x + w / 2, y + w / 6, w / 6, w / 6, Color.Black);
            eye = new SolidBrush(Color.White);
        }
        public override int W
        {
            get => w;
            set
            {
                w = value;
                left.W = value / 6;
                rigt.W = value / 6;
            }
        }
        public override void Draw(Graphics g)
        {
            g.FillEllipse(brush, x, y, w, h);
            g.FillEllipse(eye, x + w / 10, y + w / 6, w / 3, h / 3);
            g.FillEllipse(eye, x + w / 2, y + w / 6, w / 3, h / 3);
            left.Draw(g);
            rigt.Draw(g);
        }
        public override void Obvodka(Graphics g)
        {
            g.DrawEllipse(pen, x, y, w, h);
        }
        public override void Draw(Graphics g, int xE, int yE)
        {
            g.FillEllipse(brush, xE, yE, w, h);
            g.FillEllipse(eye, xE + w / 10, yE + w / 6, w / 3, h / 3);
            g.FillEllipse(eye, xE + w / 2, yE + w / 6, w / 3, h / 3);
            left.Draw(g, xE + w / 10 + w / 6, yE + w / 6 + h / 6);
            rigt.Draw(g, xE + w / 2 + w / 6, yE + w / 6 + h / 6);
        }
        public override bool intouch(int px, int py)
        {
            return Math.Pow(px - (x + w / 2), 2) / ((w / 2) * (w / 2)) + Math.Pow(py - (y + h / 2), 2) / (h / 2 * h / 2) <= 1;
        }
        public override bool intouch(int px, int py, int xE, int yE)
        {
            return Math.Pow(px - (xE + w / 2), 2) / ((w / 2) * (w / 2)) + Math.Pow(py - (yE + h / 2), 2) / (h / 2 * h / 2) <= 1;
        }
        public override void Move(int xd, int yd)
        {
            base.Move(xd, yd);
            left.Move(xd, yd);
            rigt.Move(xd, yd);
        }
        public void LockAt(int mouseX, int mouseY)
        {
            double leftEyeCenterX = x + w / 10 + w / 6; 
            double leftEyeCenterY = y + w / 6 + h / 6; 
            double rightEyeCenterX = x + w / 2 + w / 6; 
            double rightEyeCenterY = y + w / 6 + h / 6; 
            double eyeA = w / 6; 
            double eyeB = h / 6; 
            double pupilA = left.W / 2; 
            double pupilB = left.H / 2; 
            if (Math.Pow(mouseX - leftEyeCenterX, 2) / (eyeA * eyeA) + Math.Pow(mouseY - leftEyeCenterY, 2) / (eyeB * eyeB) <= 1)
            {
                left.X = (int)Math.Max(leftEyeCenterX - (eyeA - pupilA), Math.Min(leftEyeCenterX + (eyeA - pupilA), mouseX)) - left.W / 2;
                left.Y = (int)Math.Max(leftEyeCenterY - (eyeB - pupilB), Math.Min(leftEyeCenterY + (eyeB - pupilB), mouseY)) - left.H / 2;
            }
            else
            {
                double dx = mouseX - leftEyeCenterX;
                double dy = mouseY - leftEyeCenterY;
                double length = Math.Sqrt(dx * dx + dy * dy);
                if (length == 0)
                    length = 1; 
                left.X = (int)((leftEyeCenterX + (dx / length) * (eyeA - pupilA)) - left.W / 2);
                left.Y = (int)((leftEyeCenterY + (dy / length) * (eyeB - pupilB)) - left.H / 2);
            }
            if (Math.Pow(mouseX - rightEyeCenterX, 2) / (eyeA * eyeA) + Math.Pow(mouseY - rightEyeCenterY, 2) / (eyeB * eyeB) <= 1)
            {
                rigt.X = (int)Math.Max(rightEyeCenterX - (eyeA - pupilA), Math.Min(rightEyeCenterX + (eyeA - pupilA), mouseX)) - rigt.W / 2;
                rigt.Y = (int)Math.Max(rightEyeCenterY - (eyeB - pupilB), Math.Min(rightEyeCenterY + (eyeB - pupilB), mouseY)) - rigt.H / 2;
            }
            else
            {
                double dx = mouseX - rightEyeCenterX;
                double dy = mouseY - rightEyeCenterY;
                double length = Math.Sqrt(dx * dx + dy * dy);
                if (length == 0)
                    length = 1;
                rigt.X = (int)((rightEyeCenterX + (dx / length) * (eyeA - pupilA)) - rigt.W / 2);
                rigt.Y = (int)((rightEyeCenterY + (dy / length) * (eyeB - pupilB)) - rigt.H / 2);
            }
        }
        public override bool Inwork 
        { 
            get => inwork; 
            set
            {
                inwork = value;
                if (value)
                {
                    brush = new SolidBrush(Color.FromArgb(255, c.R, c.G, c.B));
                    eye = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
                }
                else
                {
                    brush = new SolidBrush(Color.FromArgb(75, c.R, c.G, c.B));
                    eye = new SolidBrush(Color.FromArgb(75, 255, 255, 255));
                }
                left.Inwork = value;
            }
        }
    }
}
