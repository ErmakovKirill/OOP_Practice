using System.Drawing;
using System;

namespace easy
{
    internal class Tractor : Share
    {
        private Share body, cabin, wheel1, wheel2;
        private int speed = 3, currentPathSegment = 0;
        private int mlx, mly, pathCenterX, pathCenterY, pathRadius;
        private bool ride, withface = false;

        public Tractor(int x, int y, int w, int h, Color c) : base(x, y, w, h, c)
        {
            body = new Rect(x, y, w, h, c);
            cabin = new Rect(x, y - h, w / 3, h, c);
            wheel1 = new Circle(x, y + h, h, h, c);
            wheel2 = new Circle(x + w - h, y + h, h, h, c);
            pathCenterX = x;
            pathCenterY = y;
        }

        public override bool intouch(int px, int py)
        {
            return body.intouch(px, py) || cabin.intouch(px, py) || wheel1.intouch(px, py) || wheel2.intouch(px, py);
        }

        public override bool intouch(int px, int py, int xE, int yE)
        {
            return body.intouch(px, py, xE - w / 2, yE - h / 2) || cabin.intouch(px, py, xE - w / 2, yE - h - h / 2) || wheel1.intouch(px, py, xE + w - (w / 3) - w / 2, yE + h - h / 2) || wheel2.intouch(px, py, xE + w - (w / 3) - w / 2, yE + h - h / 2);
        }

        public Share TouchinEditor(int px, int py, int xE, int yE)
        {
            if (cabin.intouch(px, py, xE - w / 2, yE - h - h / 2))
                return cabin;
            else if (wheel1.intouch(px, py, xE - w / 2, yE + h - h / 2))
                return wheel1;
            else if (wheel2.intouch(px, py, xE + w - (w / 3) - w / 2, yE + h - h / 2))
                return wheel2;
            else if (body.intouch(px, py, xE - w / 2, yE - h / 2))
                return body;
            else
                return null;
        }

        public void GO()
        {
            if (!ride) return;
            int nextX = x;
            int nextY = y;
            int squareTopLeftX = pathCenterX - pathRadius;
            int squareTopLeftY = pathCenterY - pathRadius;
            int squareTopRightX = pathCenterX + pathRadius;
            int squareBottomRightY = pathCenterY + pathRadius;

            switch (currentPathSegment)
            {
                case 0:
                    nextX = x + speed;
                    nextY = squareTopLeftY;
                    if (nextX >= squareTopRightX)
                    {
                        nextX = squareTopRightX;
                        currentPathSegment = 1;
                    }
                    break;
                case 1:
                    nextY = y + speed;
                    nextX = squareTopRightX;
                    if (nextY >= squareBottomRightY)
                    {
                        nextY = squareBottomRightY;
                        currentPathSegment = 2;
                    }
                    break;
                case 2:
                    nextX = x - speed;
                    nextY = squareBottomRightY;
                    if (nextX <= squareTopLeftX)
                    {
                        nextX = squareTopLeftX;
                        currentPathSegment = 3;
                    }
                    break;
                case 3:
                    nextY = y - speed;
                    nextX = squareTopLeftX;
                    if (nextY <= squareTopLeftY)
                    {
                        nextY = squareTopLeftY;
                        currentPathSegment = 0;
                    }
                    break;
            }

            localmove(nextX, nextY);

            if (withface)
            {
                Look(mlx, mly);
            }
        }

        public override void Draw(Graphics g)
        {
            cabin.Draw(g);
            body.Draw(g);
            wheel1.Draw(g);
            wheel2.Draw(g);
        }

        public override void Draw(Graphics g, int xE, int yE)
        {
            cabin.Draw(g, xE, yE - h);
            body.Draw(g, xE, yE);
            wheel1.Draw(g, xE, yE + h);
            wheel2.Draw(g, xE + w - h, yE + h);
        }

        public override void Obvodka(Graphics g)
        {
            cabin.Obvodka(g);
            body.Obvodka(g);
            wheel1.Obvodka(g);
            wheel2.Obvodka(g);
        }

        private void localmove(int xd, int yd)
        {
            this.x = xd;
            this.y = yd;

            body.Move(xd, yd);
            cabin.Move(xd, yd - h);
            wheel1.Move(xd, yd + h);
            wheel2.Move(xd + w - h, yd + h);
        }

        public override void Move(int xd, int yd)
        {
            if (ride)
            {
                Ride = false;
            }
            localmove(xd, yd);
            this.pathCenterX = this.x;
            this.pathCenterY = this.y;
        }

        public void WheelChange()
        {
            Color c1 = wheel1.C; bool iw1 = wheel1.Inwork;
            Color c2 = wheel2.C; bool iw2 = wheel2.Inwork;

            if (wheel1 is Circle)
            {
                wheel1 = new Face(wheel1.X, wheel1.Y, wheel1.W, wheel1.H, c1);
                wheel2 = new Face(wheel2.X, wheel2.Y, wheel2.W, wheel2.H, c2);
                withface = true;
            }
            else
            {
                wheel1 = new Circle(wheel1.X, wheel1.Y, wheel1.W, wheel1.H, c1);
                wheel2 = new Circle(wheel2.X, wheel2.Y, wheel2.W, wheel2.H, c2);
                withface = false;
            }
            wheel1.Inwork = iw1;
            wheel2.Inwork = iw2;
        }

        public void LockAt(int x_mouse, int y_mouse)
        {
            mlx = x_mouse;
            mly = y_mouse;
            if (!ride && withface)
            {
                Look(mlx, mly);
            }
        }

        private void Look(int x_target, int y_target)
        {
            if (wheel1 is Face f1)
                f1.LockAt(x_target, y_target);
            if (wheel2 is Face f2)
                f2.LockAt(x_target, y_target);
        }

        public override bool Inwork
        {
            get => base.Inwork;
            set
            {
                base.Inwork = value;
                body.Inwork = value;
                cabin.Inwork = value;
                wheel1.Inwork = value;
                wheel2.Inwork = value;
                if (!value)
                    Ride = false;
            }
        }

        public bool Withface { get => withface; }

        public override Color C
        {
            get => c;
            set
            {
                c = value;
                body.C = value;
                cabin.C = value;
                wheel1.C = value;
                wheel2.C = value;
            }
        }

        public override int W
        {
            get => w;
            set
            {
                w = value;
                body.W = value;
                cabin.W = value / 3;
                localmove(x, y);
            }
        }

        public override int H
        {
            get => h;
            set
            {
                h = value;
                body.H = value;
                cabin.H = value;
                wheel1.H = value;
                wheel2.H = value;
                wheel1.W = value;
                wheel2.W = value;
                localmove(x,y);
            }
        }

        public bool Ride
        {
            get => ride;
            set
            {
                if (value)
                {
                    pathCenterX = x;
                    pathCenterY = y;
                    pathRadius = w + h;

                    int startX = pathCenterX - pathRadius;
                    int startY = pathCenterY - pathRadius;
                    localmove(startX, startY);

                    currentPathSegment = 0;
                    ride = value;
                }
                else
                {
                    localmove(pathCenterX, pathCenterY);
                    ride = false;
                }
            }
        }
    }
}