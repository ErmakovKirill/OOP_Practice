using System.Drawing;
using System.Windows.Forms;

namespace easy
{
    internal abstract class Share
    {
        protected internal int x;
        protected internal int y;
        protected internal int w;
        protected internal int h;
        protected internal Brush brush;
        protected internal Color c;
        protected internal Color d;
        protected internal Pen pen;
        protected internal bool inwork;
        public Share(int x, int y, int w, int h, Color c)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.c = c;
            d = Color.FromArgb(255, 255 - c.R, 255 - c.G, 255 - c.B);
            brush = new SolidBrush(c);
            pen = new Pen(d, 4);
            inwork = true;
        }
        public virtual void Move(int xd, int yd)
        {
            x = xd;
            y = yd;
        }
        public abstract void Draw(Graphics g);
        public abstract void Draw(Graphics g, int xE, int yE);
        public abstract bool intouch(int px, int py);
        public abstract bool intouch(int px, int py, int xE, int yE);
        public abstract void Obvodka(Graphics g);
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public virtual int W { get => w; set => w = value; }
        public virtual int H { get => h; set => h = value; }
        public virtual bool Inwork
        {
            get => inwork;
            set
            {
                inwork = value;
                if (inwork)
                    brush = new SolidBrush(Color.FromArgb(255, c.R, c.G, c.B));
                else
                    brush = new SolidBrush(Color.FromArgb(75, c.R, c.G, c.B));
            }
        }
        public virtual Color C
        {
            get => c;
            set
            {
                c = value;
                brush = new SolidBrush(c);
                pen = new Pen(Color.FromArgb(255, 255 - c.R, 255 - c.G, 255 - c.B), 4);
            }
        }
        
    }
}
