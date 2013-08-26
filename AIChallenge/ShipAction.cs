using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChallenge
{
    public abstract class ShipAction
    {
        public bool Completed { get; set; }
        private int timeoutTime = 0;

        public ShipAction()
        {
            Completed = false;
        }

        public virtual void Execute(Ship ship)
        {
            if (!Completed)
            {
                timeoutTime++;
                if (timeoutTime > 500)
                {
                    timeoutTime = 0;
                    Completed = true;                    
                }
            }
        }
    }
}
