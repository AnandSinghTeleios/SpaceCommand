using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChallenge
{
    public class FireAction: ShipAction
    {
        public int Type { get; set; }

        public FireAction(int type)
        {
            Type = type;
        }

        public override void Execute(Ship ship)
        {
            base.Execute(ship);
            if (ship != null)
            {
                if (Type == 0 && ship.CanFireBullet)
                {
                    ship.CanFireBullet = false;
                    Bullet b = new Bullet("Data\\Ships\\bullet1.png", ship.GetX() + ship.Width / 2 - 15, ship.GetY() + ship.Height / 2 - 15, ship);
                    b.SetAngle(ship.GetAngle());
                    b.Speed = 5;
                    b.Damage = 5;
                    b.drawOrder = ship.DrawOrder + 100;
                    MainWindow._instance.AddedComponents.Add(b);
                }
                else
                    if (Type == 1 && ship.CanFirePlasma)
                    {
                        ship.CanFirePlasma = false;
                        Bullet b = new Bullet("Data\\Ships\\bullet2.png", ship.GetX() + ship.Width / 2 - 18, ship.GetY() + ship.Height / 2 - 18, ship);
                        b.SetAngle(ship.GetAngle());
                        b.Speed = 6;
                        b.Damage = 10;
                        b.drawOrder = ship.DrawOrder + 100;
                        MainWindow._instance.AddedComponents.Add(b);
                    }
                    else
                        if (Type == 2 && ship.CanFireMissile)
                        {
                            ship.CanFireMissile = false;
                            Bullet b = new Bullet("Data\\Ships\\bullet3.png", ship.GetX() + ship.Width / 2 - 15, ship.GetY() + ship.Height / 2 - 21, ship);
                            b.SetAngle(ship.GetAngle());
                            b.Speed = 4;
                            b.Damage = 15;
                            b.drawOrder = ship.DrawOrder + 100;
                            MainWindow._instance.AddedComponents.Add(b);
                        }
            }

            Completed = true;
        }
    }
}
