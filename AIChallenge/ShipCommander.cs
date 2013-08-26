using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AIController;

namespace AIChallenge
{
    public class ShipCommander
    {
        public Assembly assembly { get; set; }
        public Type type { get; set; }

        public MethodInfo runMethodInfo { get; set; }
        public MethodInfo bulletHitBulletMethodInfo { get; set; }
        public MethodInfo bulletHitMethodInfo { get; set; }
        public MethodInfo bulletMissedMethodInfo { get; set; }
        public MethodInfo hitByBulletMethodInfo { get; set; }
        public MethodInfo scannedShipMethodInfo { get; set; }
        public MethodInfo deathMethodInfo { get; set; }
        public MethodInfo winMethodInfo { get; set; }

        public MethodInfo collideWithShipMethodInfo { get; set; }
        public MethodInfo collideWithTopMethodInfo { get; set; }
        public MethodInfo collideWithBottomMethodInfo { get; set; }
        public MethodInfo collideWithLeftMethodInfo { get; set; }
        public MethodInfo collideWithRightMethodInfo { get; set; }
        
        public object classInstance { get; set; }

        public Ship ship { get; set; }
        public ShipController controller { get; set; }
        

        public ShipCommander(String filename, double startX, double startY)
        {
            controller = new ShipController();
            ship = new Ship("Data\\Ships\\ship1.png", startX, startY);
            ship.drawOrder = 200;
            ship.Commander = this;
            MainWindow._instance.AddedComponents.Add(ship);

            try
            {
                assembly = Assembly.LoadFile(filename);
                type = assembly.GetTypes()[0];
                if (type != null)
                {
                    controller.SetShip(ship);
                    ship.Controller = controller;
                    classInstance = Activator.CreateInstance(type, new object[] { controller });                    

                    runMethodInfo = type.GetMethod("Run");
                    bulletHitBulletMethodInfo = type.GetMethod("OnBulletHitBullet");
                    bulletHitMethodInfo = type.GetMethod("OnBulletHit");
                    bulletMissedMethodInfo = type.GetMethod("OnBulletMissed");
                    hitByBulletMethodInfo = type.GetMethod("OnHitByBullet");
                    scannedShipMethodInfo = type.GetMethod("OnScannedShip");
                    deathMethodInfo = type.GetMethod("OnDeath");
                    winMethodInfo = type.GetMethod("OnWin");

                    collideWithShipMethodInfo = type.GetMethod("OnCollideWithShip");
                    collideWithTopMethodInfo = type.GetMethod("OnCollideWithTop");
                    collideWithBottomMethodInfo = type.GetMethod("OnCollideWithBottom");
                    collideWithLeftMethodInfo = type.GetMethod("OnCollideWithLeft");
                    collideWithRightMethodInfo = type.GetMethod("OnCollideWithRight");
                }
            }
            catch (ReflectionTypeLoadException e)
            {

            }
        }

        public void ExecuteRun()
        {
            if (runMethodInfo != null)
            {
                controller.IsEvent = false;
                runMethodInfo.Invoke(classInstance, null);
            }
        }
        
        public void ExecuteOnBulletHitBullet(IDrawableEntity bullet)
        {
            if (bulletHitBulletMethodInfo != null)
            {
                controller.IsEvent = true;
                bulletHitBulletMethodInfo.Invoke(classInstance, new object[] { bullet });
                controller.IsEvent = false;
            }
        }

        public void ExecuteOnBulletHit(IDrawableEntity ship)
        {
            if (bulletHitMethodInfo != null)
            {
                controller.IsEvent = true;
                bulletHitMethodInfo.Invoke(classInstance, new object[] { ship });
                controller.IsEvent = false;
            }
        }

        public void ExecuteOnBulletMissed()
        {
            if (bulletMissedMethodInfo != null)
            {
                controller.IsEvent = true;
                bulletMissedMethodInfo.Invoke(classInstance, null);
                controller.IsEvent = false;
            }
        }

        public void ExecuteOnHitByBullet(IDrawableEntity bullet)
        {
            if (hitByBulletMethodInfo != null)
            {
                controller.IsEvent = true;
                hitByBulletMethodInfo.Invoke(classInstance, new object[] { bullet });
                controller.IsEvent = false;
            }
        }

        public void ExecuteOnScannedShip(IDrawableEntity ship)
        {
            if (scannedShipMethodInfo != null)
            {
                controller.IsEvent = true;
                scannedShipMethodInfo.Invoke(classInstance, new object[] { ship });
                controller.IsEvent = false;
            }
        }

        public void ExecuteOnDeath(IDrawableEntity ship)
        {
            if (deathMethodInfo != null)
            {
                controller.IsEvent = true;
                deathMethodInfo.Invoke(classInstance, new object[] { ship });
                controller.IsEvent = false;
            }
        }

        public void ExecuteOnWin()
        {
            if (winMethodInfo != null)
            {
                controller.IsEvent = true;
                winMethodInfo.Invoke(classInstance, null);
                controller.IsEvent = false;
            }
        }

        public void ExecuteOnCollideWithShip(IDrawableEntity ship)
        {
            if (collideWithShipMethodInfo != null)
            {
                controller.IsEvent = true;
                collideWithShipMethodInfo.Invoke(classInstance, new object[] { ship });
                controller.IsEvent = false;
            }
        }

        public void ExecuteOnCollideWithTop()
        {
            if (collideWithTopMethodInfo != null)
            {
                controller.IsEvent = true;
                collideWithTopMethodInfo.Invoke(classInstance, null);
                controller.IsEvent = false;
            }
        }

        public void ExecuteOnCollideWithBottom()
        {
            if (collideWithBottomMethodInfo != null)
            {
                controller.IsEvent = true;
                collideWithBottomMethodInfo.Invoke(classInstance, null);
                controller.IsEvent = false;
            }
        }

        public void ExecuteOnCollideWithLeft()
        {
            if (collideWithLeftMethodInfo != null)
            {
                controller.IsEvent = true;
                collideWithLeftMethodInfo.Invoke(classInstance, null);
                controller.IsEvent = false;
            }
        }

        public void ExecuteOnCollideWithRight()
        {
            if (collideWithRightMethodInfo != null)
            {
                controller.IsEvent = true;
                collideWithRightMethodInfo.Invoke(classInstance, null);
                controller.IsEvent = false;
            }
        }
    }
}
