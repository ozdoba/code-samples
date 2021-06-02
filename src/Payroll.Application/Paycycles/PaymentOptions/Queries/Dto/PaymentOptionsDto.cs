using Payroll.Application.Common.Mappings;

namespace Payroll.Application.Paycycles.PaymentOptions.Queries.Dto
{
    public class PaymentOptionsDto : IMapFrom<global::Payroll.Domain.Paycycles.PaymentOptions>
    {
        public string AccountHolder { get; set; }
        public string AccountNumber { get; set; }
                
        public string BankName { get; set; }	
        public string SwiftCode { get; set; }
        public string BranchCode { get; set; }
        public AddressDto BranchAddress { get; set; }
        
        public string IsoCountryCode { get; set; }
    }
}