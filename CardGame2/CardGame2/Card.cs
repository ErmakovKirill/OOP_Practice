using System;
using System.Drawing;

namespace CardGame2
{
    internal class Card
    {
        private int nomer, mast, w, h, x, y;
        private string nomer_str, mast_str;
        private Brush suitBrush;
        private Font font, suitFont;
        private bool f = false;
        public Card(int nomer, int mast)
        {
            this.nomer = nomer;
            this.mast = mast;
            w = 80;
            h = 120;
            if (nomer == 11) 
                nomer_str = "J";
            else if (nomer == 12) 
                nomer_str = "Q";
            else if (nomer == 13) 
                nomer_str = "K";
            else if (nomer == 14) 
                nomer_str = "T";
            else 
                nomer_str = Convert.ToString(nomer);

            if (mast == 2 || mast == 3)
                suitBrush = Brushes.Red;
            else
                suitBrush = Brushes.Black;

            if (mast == 1)
                mast_str = "♥";
            else if (mast == 2)
                mast_str = "♦";
            else if (mast == 3)
                mast_str = "♠";
            else if (mast == 4)
                mast_str = "♣";

            font = new Font("Arial", 12, FontStyle.Bold);
            suitFont = new Font("Arial", 36, FontStyle.Bold);

        }
        public void DrawFront(Graphics g, int x, int y)
        {
            g.FillRectangle(Brushes.White, x, y, w, h); 
            g.DrawRectangle(Pens.Black, x, y, w, h); 
            g.DrawString(nomer_str, font, suitBrush, x + 5, y + 5); 
            SizeF textSize = g.MeasureString(mast_str, suitFont);
            float centerX = x + (w - textSize.Width) / 2;
            float centerY = y + (h - textSize.Height) / 2;
            g.DrawString(mast_str, suitFont, suitBrush, centerX, centerY);
            this.x = x; this.y = y;
        }
        public void DrawBack(Graphics g, int x, int y)
        {
            g.FillRectangle(Brushes.Blue, x, y, w, h);
            g.DrawRectangle(Pens.Black, x, y, w, h);
            this.x = x;
            this.y = y;
        }
        public void DrawFrontHorizontal(Graphics g, int x, int y)
        {
            g.TranslateTransform(x + h, y);
            g.RotateTransform(90);
            g.FillRectangle(Brushes.White, 0, 0, w, h);
            g.DrawRectangle(Pens.Black, 0, 0, w, h);
            g.DrawString(nomer_str, font, suitBrush, 5, 5);
            SizeF textSize = g.MeasureString(mast_str, suitFont);
            float centerX = (w - textSize.Width) / 2;
            float centerY = (h - textSize.Height) / 2;
            g.DrawString(mast_str, suitFont, suitBrush, centerX, centerY);
            g.ResetTransform();

            this.x = x; this.y = y;
        }
        public bool InTouch(int touchX, int touchY)
        {
            return touchX >= x && touchX <= x + w && touchY >= y && touchY <= y + h;
        }
        public bool Comparison(int mast_srv, int nomer_srv, int mk)
        {
            if (mast == mast_srv)
                return nomer > nomer_srv;
            else
                return mast == mk;
        }

        public int GetM { get => mast; }
        public int GetN { get => nomer; }
        public bool F { get => f; set => f = value; }
    }
}
