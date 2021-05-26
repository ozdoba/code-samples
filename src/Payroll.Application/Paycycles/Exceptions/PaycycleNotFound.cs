using System;

namespace Payroll.Application.Paycycles.Exceptions
{
    public class PaycycleNotFound : Exception
    {
        public PaycycleNotFound(string msg) : base(msg)
        {
        }

        public static PaycycleNotFound ForPaycycleId(Guid paycycleId)
        {
            return new PaycycleNotFound($"Paycycle [{paycycleId}] not found.");
        }
    }
}