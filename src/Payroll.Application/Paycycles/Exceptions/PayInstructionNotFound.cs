using System;

namespace Payroll.Application.Paycycles.Exceptions
{
    public class PayInstructionNotFound : Exception
    {
        public PayInstructionNotFound(string msg) : base(msg)
        {
        }

        public static PayInstructionNotFound ForPayeeId(Guid payeeId) => new PayInstructionNotFound($"Payee [{payeeId}] not found");
    }
}