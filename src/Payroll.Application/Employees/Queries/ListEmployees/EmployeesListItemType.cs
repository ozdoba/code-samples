using System;
using AutoMapper;
using Payroll.Application.Common.Mappings;
using Payroll.Application.Employees.Queries.Common;
using Payroll.Domain.Employees;

namespace Payroll.Application.Employees.Queries.ListEmployees
{
    public class EmployeesListItemType : IMapFrom<Employee>
    {
        public string EmployeeNumber { get; set; }

        public string JobTitle { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }

        public AddressType Address { get; set; }

        public string PrivateEmailAddress { get; set; }
        public string CorporateEmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PlaceOfBirth { get; set; }
        public string? LocalTaxNumber { get; set; }

        public DateTime? DateOfTermination { get; set; }

        public DateTime? DateOfEmployment { get; set; }
        public EmployeeStatusType Status { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Employee, EmployeesListItemType>()
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(
                        src => Enum.Parse<EmployeeStatusType>(src.Status.ToString())));
        }
    }
}