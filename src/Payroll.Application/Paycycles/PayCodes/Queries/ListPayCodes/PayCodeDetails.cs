using Payroll.Application.Paycycles.PayCodes.Common;

namespace Payroll.Application.Paycycles.PayCodes.Queries.ListPayCodes
{
    public class PayCodeDetails
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public PayCodeType Type { get; set; }
    }
}