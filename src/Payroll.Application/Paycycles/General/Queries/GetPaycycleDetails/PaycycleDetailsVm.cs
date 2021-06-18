using System.Collections.Generic;

namespace Payroll.Application.Paycycles.General.Queries.GetPaycycleDetails
{
    public class PaycycleDetailsVm
    {
        public PaycycleType Paycycle { get; set; }
        public int PayeeCount { get; set; }
        public IEnumerable<SettlementDetailsType> SettlementDetails { get; set; }
    }
}