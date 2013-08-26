using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AIChallenge
{
    public class Ship: Sprite
    {
        public double Speed { get; set; }
        public ShipController Controller { get; set; }
        public ShipCommander Commander { get; set; }

        public int BulletCooldown { get; set; }
        public int PlasmaCooldown { get; set; }
        public int MissileCooldown { get; set; }
        public int ScanCooldown { get; set; }

        public bool CanFireBullet { get; set; }
        public bool CanFirePlasma { get; set; }
        public bool CanFireMissile { get; set; }
        public bool CanScan { get; set; }

        public int Life { get; set; }
        private Rect LifeBar;
        private Rect BulletCooldownBar;
        private Rect PlasmaCooldownBar;
        private Rect MissileCooldownBar;
        private Pen pen = new Pen(Brushes.Black, 2);
        private Pen pen2 = new Pen(Brushes.Black, 1);

        private FormattedText nameText;
        private Point namePoint = new Point(0, 0);
        public ScoreProperties ScoreProps { get; set; }
        
        public Ship(string filename, double x, double y): base(filename, x, y)
        {
            Alive = true;
            IsColliding = false;

            ScoreProps = new ScoreProperties();

            CanFireBullet = true;
            CanFireMissile = true;
            CanFirePlasma = true;
            CanScan = true;

            Life = 100;
            LifeBar = new Rect(GetX(), GetY(), 50, 5);
            BulletCooldownBar = new Rect(GetX(), GetY(), 50, 2);
            PlasmaCooldownBar = new Rect(GetX(), GetY(), 50, 2);
            MissileCooldownBar = new Rect(GetX(), GetY(), 50, 2);
        }

        public void SetName(String name)
        {
            if (this.name == null)
            {
                this.name = name;
                nameText = FormattedTextCreator.Create(name, 10);
            }
        }

        public override void Update()
        {
            if (nameText != null)
            {
                namePoint.X = GetX() +Width/2- nameText.Width / 2;
                namePoint.Y = GetY() + Height + 5;

                if (!Alive)
                    GameControl._drawingContext.PushOpacity(0.4);
                GameControl._drawingContext.DrawText(nameText, namePoint);
                if (!Alive)
                    GameControl._drawingContext.Pop();
            }

            if (Alive)
            {
                LifeBar.X = GetX();
                LifeBar.Y = GetY() - 10;
                if (Life > 0)
                    LifeBar.Width = Life / 2;
                GameControl._drawingContext.DrawRectangle(Brushes.Red, pen, LifeBar);

                BulletCooldownBar.X = GetX();
                BulletCooldownBar.Y = GetY() - 15;
                BulletCooldownBar.Width = 50 - BulletCooldown / 2;
                GameControl._drawingContext.DrawRectangle(Brushes.Green, pen2, BulletCooldownBar);

                PlasmaCooldownBar.X = GetX();
                PlasmaCooldownBar.Y = GetY() - 20;
                PlasmaCooldownBar.Width = 50 - PlasmaCooldown / 10;
                GameControl._drawingContext.DrawRectangle(Brushes.Yellow, pen2, PlasmaCooldownBar);

                MissileCooldownBar.X = GetX();
                MissileCooldownBar.Y = GetY() - 25;
                MissileCooldownBar.Width = 50 - MissileCooldown / 20;
                GameControl._drawingContext.DrawRectangle(Brushes.Purple, pen2, MissileCooldownBar);

                if (Controller != null)
                    Controller.Process();

                if (!CanFireBullet)
                {
                    BulletCooldown++;
                    if (BulletCooldown > 100)
                    {
                        BulletCooldown = 0;
                        CanFireBullet = true;
                    }
                }

                if (!CanFirePlasma)
                {
                    PlasmaCooldown++;
                    if (PlasmaCooldown > 500)
                    {
                        PlasmaCooldown = 0;
                        CanFirePlasma = true;
                    }
                }

                if (!CanFireMissile)
                {
                    MissileCooldown++;
                    if (MissileCooldown > 1000)
                    {
                        MissileCooldown = 0;
                        CanFireMissile = true;
                    }
                }

                if (!CanScan)
                {
                    ScanCooldown++;
                    if (ScanCooldown > 10)
                    {
                        ScanCooldown = 0;
                        CanScan = true;
                    }
                }
            }
        }
    }
}
