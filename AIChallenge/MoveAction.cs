using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChallenge
{
    public class MoveAction: ShipAction
    {
        public double StartX { get; set; }
        public double StartY { get; set; }
        public double Amount { get; set; }
        public double ElapsedTime { get; set; }
        public bool Forward { get; set; }
        public double Speed { get; set; }

        public MoveAction(double startX, double startY, double amount, bool forward, double speed)
        {
            StartX = startX;
            StartY = startY;
            Amount = amount;
            Forward = forward;
            Speed = speed;
        }

        public override void Execute(Ship ship)
        {
            if (!Completed)
            {
                base.Execute(ship);

                double angle = ship.GetAngle();
                double radangle = (angle - 90) * 0.017453292519943295769236907684886;
                double xdir = Math.Cos(radangle);
                double ydir = Math.Sin(radangle);

                if (Forward)
                    ship.UpdatePosition(xdir * Speed, ydir * Speed);
                else
                    ship.UpdatePosition(xdir * -Speed, ydir * -Speed);

                if (ship.GetX() < MainWindow._instance.WindowXOffset || 
                    ship.GetX() > MainWindow._instance.WindowWidthOffset-ship.Width || 
                    ship.GetY() < MainWindow._instance.WindowYOffset || 
                    ship.GetY() > MainWindow._instance.WindowHeightOffset-ship.Height)
                {
                    Completed = true;
                    if (ship.GetX() < MainWindow._instance.WindowXOffset) 
                    { 
                        ship.SetX(MainWindow._instance.WindowXOffset);
                        ship.Commander.ExecuteOnCollideWithLeft();
                    }
                    else
                    if (ship.GetX() > MainWindow._instance.WindowWidthOffset - ship.Width)
                    {
                        ship.SetX(MainWindow._instance.WindowWidthOffset - ship.Width);
                        ship.Commander.ExecuteOnCollideWithRight();
                    }
                    else
                    if (ship.GetY() < MainWindow._instance.WindowYOffset)
                    {
                        ship.SetY(MainWindow._instance.WindowYOffset);
                        ship.Commander.ExecuteOnCollideWithTop();
                    }
                    else
                    if (ship.GetY() > MainWindow._instance.WindowHeightOffset - ship.Height)
                    {
                        ship.SetY(MainWindow._instance.WindowHeightOffset - ship.Height);
                        ship.Commander.ExecuteOnCollideWithBottom();
                    }
                }

                ElapsedTime++;
                if (ElapsedTime >= Amount)
                    Completed = true;
            }
        }
    }
}
