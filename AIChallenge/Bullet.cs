using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChallenge
{
    class Bullet: Sprite
    {
        public double Speed { get; set; }
        public Ship Owner { get; set; }
        public int Damage { get; set; }
        public bool IsScanner { get; set; }

        public Bullet(string filename, double x, double y, Ship owner): base(filename, x, y)
        {
            Alive = true;
            Owner = owner;
        }

        public override void Update()
        {
            if (Alive)
            {
                double angle = GetAngle();
                if (angle < 0) angle = 360;
                if (angle > 360) angle = 0;
                SetAngle(angle);
                double radangle = (angle - 90) * 0.017453292519943295769236907684886;

                double xdir = Math.Cos(radangle);
                double ydir = Math.Sin(radangle);
                UpdatePosition(xdir * Speed, ydir * Speed);

                if (GetX() < -Width || GetX() > 800 || GetY() < -Height || GetY() > 600)
                {
                    if (Owner != null && !IsScanner)
                        Owner.Commander.ExecuteOnBulletMissed();
                    MainWindow._instance.RemovedComponents.Add(this);
                    Alive = false;
                }
            }
        }
    }
}
