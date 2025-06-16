using System;
using System.Drawing;


namespace easy
{
    internal class Circle : Share
    {
        public Circle(int x, int y, int w, int h, Color c) : base(x, y, w, h, c)
        {
        }
        public override void Draw(Graphics g)
        {
            g.FillEllipse(brush, x, y, w, h);
        }
        public override void Draw(Graphics g, int xE, int yE)
        {
            g.FillEllipse(brush, xE, yE, w, h);
        }
        public override void Obvodka(Graphics g)
        {
            g.DrawEllipse(pen, x, y, w, h);
        }
        public override bool intouch(int px, int py, int xE, int yE)
        {
            return Math.Pow(px - (xE + w / 2), 2) / ((w / 2) * (w / 2)) + Math.Pow(py - (yE + h / 2), 2) / (h / 2 * h / 2) <= 1;
        }
        public override bool intouch(int px, int py)
        {
            return Math.Pow(px - (x + w / 2), 2) / ((w / 2) * (w / 2)) + Math.Pow(py - (y + h / 2), 2) / (h / 2 * h / 2) <= 1;
        }
    }
}
