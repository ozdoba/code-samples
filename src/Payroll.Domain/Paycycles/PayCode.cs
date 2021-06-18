using System;

namespace Payroll.Domain.Paycycles
{
    public class PayCode
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public PayCodeType Type { get; set; }
        public Guid CustomerId { get; set; }
    }

    public enum PayCodeType
    {
        Payment,
        Deductible,
    }
}