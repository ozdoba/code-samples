using AutoMapper;
using Payroll.Application.Common.Mappings;
using Payroll.Domain.Paycycles;

namespace Payroll.Application.Paycycles.PayInstructions.Queries.Dto
{
    public class InstructionDetailsDto : IMapFrom<PayInstruction>
    {
        public MoneyType? UnitAmount { get; set; }
        public decimal? Quantity { get; set; }   
        public MoneyType TotalAmount { get; set; }
        public string PayCode { get; set; }
        public string Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Money, MoneyType>();
            profile.CreateMap<PayInstruction, InstructionDetailsDto>()
                .ForMember(x=>x.PayCode, c=>c.MapFrom(x=>x.PayCode.Code));
        }
    }
}