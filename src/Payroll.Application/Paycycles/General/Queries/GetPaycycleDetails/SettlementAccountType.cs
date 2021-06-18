using AutoMapper;
using Payroll.Application.Common.Mappings;

namespace Payroll.Application.Paycycles.General.Queries.GetPaycycleDetails
{
    public class SettlementAccountType : IMapFrom<Domain.Paycycles.PaymentOptions>
    {
        public string AccountHolder { get; set; }
        public string AccountNumber { get; set; }
                
        public string? BankName { get; set; }	
        public string? SwiftCode { get; set; }
        public string? BranchCode { get; set; }

        public string? BranchAddressBuildingNumber { get; set; }
        public string? BranchAddressStreet { get; set; }
        public string? BranchAddressCity { get; set; }
        public string? BranchAddressState { get; set; }
        public string? BranchAddressPostalCode { get; set; }
        public string? BranchAddressCountryCode { get; set; }
        
        public string IsoCountryCode { get; set; }
        
        void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Paycycles.PaymentOptions, SettlementAccountType>()
                .ForMember(x=>x.BranchAddressBuildingNumber, c=>c.MapFrom(y=>y.BranchAddress.BuildingNumber))
                .ForMember(x=>x.BranchAddressStreet, c=>c.MapFrom(y=>y.BranchAddress.Street))
                .ForMember(x=>x.BranchAddressCity, c=>c.MapFrom(y=>y.BranchAddress.City))
                .ForMember(x=>x.BranchAddressState, c=>c.MapFrom(y=>y.BranchAddress.State))
                .ForMember(x=>x.BranchAddressPostalCode, c=>c.MapFrom(y=>y.BranchAddress.PostalCode))
                .ForMember(x=>x.BranchAddressCountryCode, c=>c.MapFrom(y=>y.BranchAddress.CountryCode));
        }
    }
}