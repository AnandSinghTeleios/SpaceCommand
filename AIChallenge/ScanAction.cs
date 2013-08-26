using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChallenge
{
    public class ScanAction: ShipAction
    {
        public override void Execute(Ship ship)
        {
            base.Execute(ship);
            if (ship != null && ship.CanScan)
            {
                ship.CanScan = false;
                Bullet b = new Bullet("Data\\scanBullet.png", ship.GetX() + ship.Width / 2 - 30, ship.GetY() + ship.Height / 2 - 5, ship);
                b.SetAngle(ship.GetAngle());
                b.Speed = 15;
                b.drawOrder = 250;
                b.IsScanner = true;
                MainWindow._instance.AddedComponents.Add(b);
            }

            Completed = true;
        }
    }
}
