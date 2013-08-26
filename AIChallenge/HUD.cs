using AIController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AIChallenge
{
    public class HUD: IDrawable
    {
        private FormattedText resultText;
        private Point resultPoint = new Point(0, 0);
        private FormattedText timeText = FormattedTextCreator.Create("Time", 10);
        private Point timePoint = new Point(0, 0);
        private Rect TimeBar;
        private Pen pen = new Pen(Brushes.Black, 2);

        public HUD()
        {
        }

        public void UpdateResults()
        {
            int bulletMultiplier = 20;
            int ramMultiplier = 1;
            int survivalMultiplier = 10;
            int killMultiplier = 100;

            String winner = "";
            int highestScore = 0;
            ShipCommander shipWinner = null;

            String results = "Results\n\n";
            results += "Ship\t\t\tBullet Damage\tRam Damage\tSurvival Bonus\tKill Bonus\tTotal\n";
            results += "---------------------------------------------------------------------------------------------------------------------------------------------------------\n";
            foreach (ShipCommander sc in MainWindow._instance.CommanderList)
            {
                int total = (sc.ship.ScoreProps.BulletDamage * bulletMultiplier) + (sc.ship.ScoreProps.RamDamage * ramMultiplier) + (sc.ship.ScoreProps.SurvivalScore * survivalMultiplier) + (sc.ship.ScoreProps.KillBonus * killMultiplier);
                sc.ship.ScoreProps.Total = total;

                if (total >= highestScore)
                {
                    highestScore = total;
                    winner = sc.ship.GetName();
                    shipWinner = sc;
                }

                results += sc.ship.GetName() + "\t\t\t" + (sc.ship.ScoreProps.BulletDamage * bulletMultiplier) + "\t\t" + (sc.ship.ScoreProps.RamDamage * ramMultiplier) + "\t\t" + (sc.ship.ScoreProps.SurvivalScore * survivalMultiplier) + "\t\t" + (sc.ship.ScoreProps.KillBonus * killMultiplier) + "\t\t" + sc.ship.ScoreProps.Total + "\n";
                results += "---------------------------------------------------------------------------------------------------------------------------------------------------------\n";
            }
            results += "\nWinner: " + winner;
            resultText = FormattedTextCreator.Create(results, 15);
            Console.WriteLine(results);

            if (shipWinner != null)
                shipWinner.ExecuteOnWin();
        }

        private bool alive = true;
        public bool Alive
        {
            get
            {
                return alive;
            }
            set
            {
                alive = value;
            }
        }

        public void Draw()
        {
            if (MainWindow._instance.GameProps.GameOver)
                GameControl._drawingContext.DrawText(resultText, resultPoint);
            else
            {
                TimeBar.X = 5;
                TimeBar.Y = 15;
                TimeBar.Width = 10;
                double h = 540 - MainWindow._instance.GameProps.ElapsedTime * (double)(540.0/MainWindow._instance.GameProps.GameTime);
                if (h < 0) h = 0;
                TimeBar.Height = h;
                GameControl._drawingContext.DrawRectangle(Brushes.RoyalBlue, pen, TimeBar);
                GameControl._drawingContext.DrawText(timeText, timePoint);
            }
        }

        public int DrawOrder
        {
            get { return 300; }
        }
    }
}
