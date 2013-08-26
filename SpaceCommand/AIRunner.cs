using AIController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCommand
{
    public class AIRunner: AI
    {
        /// <summary>
        /// Put your name here.
        /// </summary>
        private static readonly String CAPTAIN_NAME = "Your Name";

        /// <summary>
        /// The ship's action controller.
        /// </summary>
        public Controller Controller { get; set; }
        
        public AIRunner(Controller c)
        {
            Controller = c;
            Controller.SetName(CAPTAIN_NAME);
        }

        /// <summary>
        ///  Commands in this method are repeated for the duration of the game
        /// </summary>
        public void Run()
        {
        }

        /// <summary>
        ///  Raised when a bullet hits a ship
        /// </summary>
        /// <param name="ship"> The ship that was hit.</param>
        public void OnBulletHit(IDrawableEntity ship)
        {
        }

        /// <summary>
        ///  Raised when a bullet hits another bullet
        /// </summary>
        /// <param name="bullet"> The bullet that was hit.</param>
        public void OnBulletHitBullet(IDrawableEntity bullet)
        {
        }

        /// <summary>
        ///  Raised when a bullet hits an edge of the screen
        /// </summary>
        public void OnBulletMissed()
        {
        }

        /// <summary>
        ///  Raised when your ship collides with the bottom of the screen
        /// </summary>
        public void OnCollideWithBottom()
        {
        }

        /// <summary>
        ///  Raised when your ship collides with the left of the screen
        /// </summary>
        public void OnCollideWithLeft()
        {
        }

        /// <summary>
        ///  Raised when your ship collides with the right of the screen
        /// </summary>
        public void OnCollideWithRight()
        {
        }

        /// <summary>
        ///  Raised when your ship collides with the top of the screen
        /// </summary>
        public void OnCollideWithTop()
        {
        }

        /// <summary>
        ///  Raised when your ship collides with another ship
        /// </summary>
        /// <param name="ship"> The ship that collided with you.</param>
        public void OnCollideWithShip(IDrawableEntity ship)
        {
        }

        /// <summary>
        ///  Raised when your ship is destroyed
        /// </summary>
        /// <param name="ship"> The ship that destroyed you.</param>
        public void OnDeath(IDrawableEntity ship)
        {
        }

        /// <summary>
        ///  Raised when a bullet hits your ship
        /// </summary>
        /// <param name="bullet"> The bullet that hit.</param>
        public void OnHitByBullet(IDrawableEntity bullet)
        {
        }

        /// <summary>
        ///  Raised when a ship has been detected
        /// </summary>
        /// <param name="ship"> The ship that was detected.</param>
        public void OnScannedShip(IDrawableEntity ship)
        {
        }

        /// <summary>
        ///  Raised when your ship wins
        /// </summary>
        public void OnWin()
        {
        }
    }
}
