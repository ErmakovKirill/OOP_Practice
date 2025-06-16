using System.Drawing;

namespace easy
{
    internal class Rect : Share
    {
        public Rect(int x, int y, int w, int h, Color c) : base(x, y, w, h, c)
        {
        }
        public override void Draw(Graphics g)
        {
            g.FillRectangle(brush, x, y, w, h);
        }
        public override void Draw(Graphics g, int xE, int yE)
        {
            g.FillRectangle(brush, xE, yE, w, h);
        }

        public override bool intouch(int px, int py)
        {
            return px >= x && px <= x + w && py >= y && py <= y + h;
        }
        public override bool intouch(int px, int py, int xE, int yE)
        {
            return px >= xE && px <= xE + w && py >= yE && py <= yE + h;
        }

        public override void Obvodka(Graphics g)
        {
            g.DrawRectangle(pen, x, y, w, h);
        }
    }
}
