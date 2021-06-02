using System;
using AutoMapper;
using Payroll.Application.Common.Mappings;
using Payroll.Domain.Paycycles;

namespace Payroll.Application.Paycycles.General.Queries.GetPaycycleDetails
{
    public class PaycycleType : IMapFrom<Paycycle>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Payday { get; set; }
        
        public PaycycleStatus Status { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Paycycle, PaycycleType>()
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(
                        src => Enum.Parse<PaycycleStatus>(src.Status.ToString())));
        }
    }
}