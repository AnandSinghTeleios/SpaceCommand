using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AIController;

namespace AIChallenge
{
	internal sealed class GameControl:
		Control
	{
		internal static DrawingContext _drawingContext;

		public static List<IDrawable> _drawables = new List<IDrawable>();

		public static void DrawCentered(FormattedText text, Brush brush)
		{
            double x = MainWindow._instance.gameControl.ActualWidth / 2;
            double y = MainWindow._instance.gameControl.ActualHeight / 2;

            DrawOutlined(text, x, y, brush);
		}
		public static void DrawOutlined(FormattedText text, double x, double y, Brush brush)
		{
			x -= text.Width / 2;
			y -= text.Height / 2;

			text.SetForegroundBrush(Brushes.Black);
			_drawingContext.DrawText(text, new Point(x-1, y-1));
			_drawingContext.DrawText(text, new Point(x+1, y+1));
			_drawingContext.DrawText(text, new Point(x+1, y-1));
			_drawingContext.DrawText(text, new Point(x-1, y+1));

			text.SetForegroundBrush(brush);
			_drawingContext.DrawText(text, new Point(x, y));
		}

        private void GameOver()
        {
            if (!MainWindow._instance.GameProps.GameOver)
            {
                MainWindow._instance.GameProps.GameOver = true;
                MainWindow._instance.Hud.UpdateResults();
            }
        }

		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
            
            _drawingContext = drawingContext;

            foreach (var drawable in _drawables)
            {
                
                drawable.Draw();
                if (drawable.Alive && !MainWindow._instance.GameProps.GameOver)
                {                
                    foreach (var drawable2 in _drawables)
                    {
                        if (drawable2.Alive && !drawable2.Equals(drawable))
                        {
                            if (drawable is Bullet)
                            {
                                Bullet bullet = drawable as Bullet;

                                if (drawable2 is Ship)  //bullet hit ship
                                {
                                    Ship ship = drawable2 as Ship;
                                    if (bullet.CollidesWith(ship) && bullet.Owner != null && !bullet.Owner.Equals(ship))
                                    {
                                        bullet.Alive = false;

                                        if (bullet.IsScanner)
                                        {
                                            bullet.Owner.Commander.ExecuteOnScannedShip(ship);
                                        }
                                        else
                                        {
                                            bullet.Owner.Commander.ExecuteOnBulletHit(ship);
                                            bullet.Owner.ScoreProps.BulletDamage++;
                                            ship.Life -= bullet.Damage;

                                            if (ship.Life <= 0)
                                            {
                                                ship.Life = 0;
                                                ship.Alive = false;
                                                ship.Image = MainWindow._instance.DisabledImage;
                                                if (ship.Commander != null)
                                                    ship.Commander.ExecuteOnDeath(bullet.Owner);
                                                MainWindow._instance.GameProps.NumShips--;
                                                bullet.Owner.ScoreProps.KillBonus++;

                                                foreach (ShipCommander sc in MainWindow._instance.CommanderList)
                                                {
                                                    if (!sc.ship.Equals(ship))
                                                        sc.ship.ScoreProps.SurvivalScore++;
                                                }

                                                if (MainWindow._instance.GameProps.NumShips <= 1)
                                                    GameOver();
                                            }
                                            else
                                            {
                                                if (ship.Commander != null)
                                                    ship.Commander.ExecuteOnHitByBullet(bullet);
                                            }
                                        }

                                        MainWindow._instance.RemovedComponents.Add(bullet);
                                    }
                                }
                                else
                                    if (drawable2 is Bullet)    //bullet hit bullet
                                    {
                                        Bullet bullet2 = drawable2 as Bullet;
                                        if (bullet.CollidesWith(bullet2) && bullet.Owner!=null && bullet2.Owner!=null && !bullet.Owner.Equals(bullet2.Owner))
                                        {

                                            if (!bullet.IsScanner && !bullet2.IsScanner)
                                            {
                                                bullet.Owner.Commander.ExecuteOnBulletHitBullet(bullet2);
                                                bullet2.Owner.Commander.ExecuteOnBulletHitBullet(bullet);
                                                bullet.Alive = false;
                                                bullet2.Alive = false;
                                                MainWindow._instance.RemovedComponents.Add(bullet);
                                                MainWindow._instance.RemovedComponents.Add(bullet2);
                                            }
                                        }
                                    }
                            }
                            else
                                if (drawable is Ship && drawable2 is Ship)  //ship hit ship
                                {
                                    Ship ship1 = drawable as Ship;
                                    Ship ship2 = drawable2 as Ship;

                                    if (ship1.CollidesWith(ship2))
                                    {
                                        ship1.IsColliding = true;
                                        ship2.IsColliding = true;
                                        if (ship1.Controller.CurrentAction != null && ship1.Controller.CurrentAction is MoveAction)
                                            ship1.Controller.CurrentAction.Completed = true;

                                        if (ship2.Controller.CurrentAction != null && ship2.Controller.CurrentAction is MoveAction)
                                            ship2.Controller.CurrentAction.Completed = true;

                                        ship1.Commander.ExecuteOnCollideWithShip(ship2);
                                        ship2.Commander.ExecuteOnCollideWithShip(ship1);

                                        ship1.Life -= 5;
                                        ship2.Life -= 5;
                                        ship1.ScoreProps.RamDamage++;
                                        ship2.ScoreProps.RamDamage++;

                                        if (ship1.Life <= 0)
                                        {
                                            ship1.Life = 0;
                                            ship1.Alive = false;
                                            ship1.Image = MainWindow._instance.DisabledImage;
                                            if (ship1.Commander != null)
                                                ship1.Commander.ExecuteOnDeath(ship2);
                                            MainWindow._instance.GameProps.NumShips--;
                                            ship2.ScoreProps.KillBonus++;

                                            foreach (ShipCommander sc in MainWindow._instance.CommanderList)
                                            {
                                                if (!sc.ship.Equals(ship1))
                                                    sc.ship.ScoreProps.SurvivalScore++;
                                            }

                                            if (MainWindow._instance.GameProps.NumShips <= 1)
                                                GameOver();
                                        }

                                        if (ship2.Life <= 0)
                                        {
                                            ship2.Life = 0;
                                            ship2.Alive = false;
                                            ship2.Image = MainWindow._instance.DisabledImage;
                                            if (ship2.Commander != null)
                                                ship2.Commander.ExecuteOnDeath(ship1);
                                            MainWindow._instance.GameProps.NumShips--;
                                            ship1.ScoreProps.KillBonus++;

                                            foreach (ShipCommander sc in MainWindow._instance.CommanderList)
                                            {
                                                if (!sc.ship.Equals(ship2))
                                                    sc.ship.ScoreProps.SurvivalScore++;
                                            }

                                            if (MainWindow._instance.GameProps.NumShips <= 1)
                                                GameOver();
                                        }
                                    }
                                    else
                                    {
                                        ship1.IsColliding = false;
                                        ship2.IsColliding = false;
                                    }
                                }
                        }
                    }
                }
            }

            _drawingContext = null;
		}
	}
}
