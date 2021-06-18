using Payroll.Application.Common.Mappings;
using Payroll.Application.Paycycles.PayInstructions.Queries.Dto;
using Payroll.Domain.Paycycles;

namespace Payroll.Application.Paycycles.General.Queries.GetPaycycleDetails
{
    public class SettlementDetailsType : IMapFrom<Payee>
    {
        public string EmployeeNumber { get; set; }
        public MoneyType SettlementAmount { get; set; }
        public SettlementAccountType SettlementAccount { get; set; }
    }
}