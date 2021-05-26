using Payroll.Application.Paycycles.Queries.ListPaycycles;

namespace Payroll.Application.Paycycles.Queries.GetPaycycleDetails
{
    public class PaycycleDetailsVm
    {
        public PaycycleType Paycycle { get; set; }
        public int PayeeCount { get; set; }
    }
}