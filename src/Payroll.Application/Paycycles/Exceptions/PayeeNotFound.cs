using System;

namespace Payroll.Application.Paycycles.Exceptions
{
    public class PayeeNotFound : Exception
    {
        public PayeeNotFound(string msg) : base(msg)
        {
        }

        public static PayeeNotFound ForPayeeId(Guid payeeId) => new PayeeNotFound($"Payee [{payeeId}] not found");
    }
}