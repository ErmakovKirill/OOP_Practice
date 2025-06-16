using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;

namespace easy
{
    internal class Game
    {
        private List<Share> shares = new List<Share>();
        private Random rnd = new Random();
        private Share vibraniy;
        private int fmx, fmy;
        private Editor ed;
        protected internal Point lasteditorlocation;
        public Game() 
        {
            ed = new Editor(this);
        }
        public void AddE(int x, int y)
        {
            int r = rnd.Next(50, 100);
            shares.Add(new Circle(rnd.Next(300,x - 20), rnd.Next(120,y - 20), r, r, Color.FromArgb(255,rnd.Next(256), rnd.Next(256), rnd.Next(256))));
        }
        public void AddR(int x, int y)
        {
            shares.Add(new Rect(rnd.Next(300, x - 20), rnd.Next(120, y - 20), rnd.Next(25, 100), rnd.Next(25, 100), Color.FromArgb(255,rnd.Next(256), rnd.Next(256), rnd.Next(256))));
        }
        public void AddT(int x, int y)
        {
            shares.Add(new Tractor(rnd.Next(300, x - 20), rnd.Next(120, y - 20), rnd.Next(80, 120), rnd.Next(25, 40), Color.FromArgb(255, rnd.Next(256), rnd.Next(256), rnd.Next(256))));
        }
        public void AddP(int x, int y) 
        {
            shares.Add(new Face(rnd.Next(300, x - 20), rnd.Next(120, y - 20), rnd.Next(25, 100), rnd.Next(25, 100), Color.FromArgb(255, rnd.Next(256), rnd.Next(256), rnd.Next(256))));
        }
        public void DeleteAT(int k)
        {
            if (vibraniy != shares[k])
                shares.RemoveAt(k);
            else
            {
                vibraniy = null;
                shares.RemoveAt(k);
            }
        }
        public bool Check(int mx, int my)
        {
            for (int i = shares.Count - 1; i >= 0; i--)
            {
                Share s = shares[i];
                if (s.Inwork && s.intouch(mx, my))
                {
                    vibraniy = s;
                    fmx = mx - s.X;
                    fmy = my - s.Y;
                    return true;
                }
            }
            return false;
        }
        public void FaceMove(int x, int y)
        {
            foreach (Share s in shares)
                if (s.Inwork)
                {
                    if (s is Face f)
                        f.LockAt(x, y);
                    if (s is Tractor tr)
                        if (tr.Withface)
                            tr.LockAt(x, y);
                }       
        }
        public void DrawUpd(Graphics g)
        {
            foreach (Share s in shares)
            {
                if (s is Tractor t)
                    if (t.Inwork)
                    {
                        if (t.Ride)
                            t.GO();
                    }
                s.Draw(g);
                if (s == vibraniy)
                {
                    s.Obvodka(g);
                }
            }
        }
        public void MoveOBj(int mx, int my)
        {
            vibraniy.Move(mx - fmx, my - fmy);
        }
        public void Check(int x)
        {
            shares[x].Inwork = true;
        }
        public void Uncheck(int x)
        {
            if (vibraniy != shares[x])
                shares[x].Inwork = false;
            else 
            {
                vibraniy = null;
                shares[x].Inwork = false;
            }
        }
        public void EditorUpdate()
        {
            ed = new Editor(this);
        }
        public void NewEd()
        {
            if (vibraniy != null)
            {
                ed.Share = vibraniy;
                ed.Show(); 
            }
        }
        public Point LastEditorLocation { get => lasteditorlocation; set => lasteditorlocation = value; }
    }
}
