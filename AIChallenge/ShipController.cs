using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIController;

namespace AIChallenge
{
    public class ShipController: Controller
    {
        private Ship Ship { get; set; }
        private List<ShipAction> ActionList { get; set; }
        public ShipAction CurrentAction { get; set; }
        private List<ShipAction> EventList { get; set; }
        public ShipAction CurrentEvent { get; set; }

        public bool ActionListFinished { get; set; }
        public bool EventListFinished { get; set; }
        public bool ActionsPaused { get; set; }

        public bool IsEvent { get; set; }

        public ShipController()
        {
            ActionList = new List<ShipAction>();
            EventList = new List<ShipAction>();
            ActionsPaused = false;
        }

        private void AddEvent(ShipAction a)
        {
            if (EventList.Count < 10)
                EventList.Add(a);
        }

        private void AddAction(ShipAction a)
        {
            if (ActionList.Count < 1000)
                ActionList.Add(a);
        }

        #region Controller Members

        /// <summary>
        ///  Rotates a ship clockwise to the specified angle
        /// </summary>
        /// <param name="angle"> The angle to rotate to.</param>
        public void RotateRight(double angle)
        {
            if (Ship != null)
            {
                RotateAction ra = new RotateAction(Ship.GetAngle(), angle, true);
                if (IsEvent)
                    AddEvent(ra);
                else
                    AddAction(ra);
            }
        }

        /// <summary>
        ///  Rotates a ship anti-clockwise to the specified angle
        /// </summary>
        /// <param name="angle"> The angle to rotate to.</param>
        public void RotateLeft(double angle)
        {
            if (Ship != null)
            {
                RotateAction ra = new RotateAction(Ship.GetAngle(), angle, false);
                if (IsEvent)
                    AddEvent(ra);
                else
                    AddAction(ra);
            }
        }

        public void Process()
        {
            if (Ship.Alive)
            {
                if (ActionList != null && ActionList.Count > 0)
                {
                    ActionListFinished = false;

                    if (!ActionsPaused)
                    {
                        if (CurrentAction == null)
                            CurrentAction = ActionList[0];

                        if (CurrentAction.Completed)
                        {
                            ActionList.Remove(CurrentAction);
                            if (ActionList.Count > 0)
                                CurrentAction = ActionList[0];
                            else
                                CurrentAction = null;
                        }

                        if (CurrentAction != null)
                            CurrentAction.Execute(Ship);
                    }
                }
                else
                    if (!ActionListFinished)
                    {
                        ActionListFinished = true;
                        if (Ship != null && Ship.Commander != null)
                            Ship.Commander.ExecuteRun();
                    }

                if (EventList != null && EventList.Count > 0)
                {
                    EventListFinished = false;

                    if (CurrentEvent == null)
                        CurrentEvent = EventList[0];

                    if (CurrentEvent.Completed)
                    {
                        if (CurrentEvent is StopAction)
                            ActionsPaused = false;

                        EventList.Remove(CurrentEvent);
                        if (EventList.Count > 0)
                            CurrentEvent = EventList[0];
                        else
                            CurrentEvent = null;
                    }

                    if (CurrentEvent != null)
                    {
                        if (CurrentEvent is StopAction)
                            ActionsPaused = true;
                        CurrentEvent.Execute(Ship);
                    }
                }
                else
                    if (!EventListFinished)
                    {
                        EventListFinished = true;
                    }
            }
        }

        public void SetShip(IDrawableEntity ship)
        {
            Ship = ship as Ship;
        }
        
        public void Forward(double amount)
        {
            if (Ship != null)
            {
                MoveAction ma = new MoveAction(Ship.GetX(), Ship.GetY(), amount, true, 2);
                if (IsEvent)
                    AddEvent(ma);
                else
                    AddAction(ma);
            }
        }

        public void Backward(double amount)
        {
            if (Ship != null)
            {
                MoveAction ma = new MoveAction(Ship.GetX(), Ship.GetY(), amount, false, 2);
                if (IsEvent)
                    AddEvent(ma);
                else
                    AddAction(ma);
            }
        }

        public void Stop(double amount)
        {
            if (Ship != null)
            {
                StopAction sa = new StopAction(amount);
                if (IsEvent)
                    AddEvent(sa);
                else
                    AddAction(sa);
            }
        }

        public void Fire(int type)
        {
            if (Ship != null && type >= 0 && type <= 2)
            {
                FireAction fa = new FireAction(type);
                if (IsEvent)
                    AddEvent(fa);
                else
                    AddAction(fa);
            }
        }

        public void Scan()
        {
            if (Ship != null)
            {
                ScanAction sa = new ScanAction();
                if (IsEvent)
                    AddEvent(sa);
                else
                    AddAction(sa);
            }
        }

        public double GetAngle()
        {
            if (Ship != null)
                return Ship.GetAngle();

            return 0;
        }

        public int GetLife()
        {
            if (Ship != null)
                return Ship.Life;

            return 0;
        }

        public double GetX()
        {
            if (Ship != null)
                return Ship.GetX();

            return 0;
        }

        public double GetY()
        {
            if (Ship != null)
                return Ship.GetY();

            return 0;
        }

        public void SetName(String name)
        {
            if (Ship != null)
                Ship.SetName(name);
        }

        public String GetName()
        {
            if (Ship != null)
                return Ship.GetName();

            return "";
        }

        public bool CanFireBullet()
        {
            if (Ship != null)
                return Ship.CanFireBullet;

            return false;
        }

        public bool CanFirePlasma()
        {
            if (Ship != null)
                return Ship.CanFirePlasma;

            return false;
        }

        public bool CanFireMissile()
        {
            if (Ship != null)
                return Ship.CanFireMissile;

            return false;
        }

        public bool CanScan()
        {
            if (Ship != null)
                return Ship.CanScan;

            return false;
        }

        #endregion
    }
}
