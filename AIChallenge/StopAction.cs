using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChallenge
{
    public class StopAction: ShipAction
    {
        public double Amount { get; set; }
        public double ElapsedTime { get; set; }

        public StopAction(double amount)
        {
            Amount = amount;
        }

        public override void Execute(Ship ship)
        {
            if (!Completed)
            {
                base.Execute(ship);
                
                ElapsedTime++;
                if (ElapsedTime >= Amount)
                    Completed = true;
            }
        }
    }
}
