using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChallenge
{
    public class RotateAction: ShipAction
    {
        public double StartValue { get; set; }
        public double EndValue { get; set; }
        public bool Clockwise { get; set; }

        public RotateAction(double start, double end, bool clockwise)
        {
            StartValue = start;
            EndValue = end;
            Clockwise = clockwise;            
        }

        public override void Execute(Ship ship)
        {
            if (!Completed)
            {
                base.Execute(ship);

                if (Math.Round(ship.GetAngle()) == Math.Round(EndValue))
                    Completed = true;
                else
                {
                    if (Clockwise)
                        ship.UpdateAngle(1);
                    else
                        ship.UpdateAngle(-1);

                    double angle = ship.GetAngle();
                    if (angle < 0) angle = 360;
                    if (angle > 360) angle = 0;
                    ship.SetAngle(angle);
                }
            }
        }
    }
}
