using System;
using Payroll.Application.Common.Mappings;
using Payroll.Application.Employees.Queries.Common;
using Payroll.Domain.Employees;

namespace Payroll.Application.Employees.Queries.GetEmployeeDetails
{
    public class EmployeeDetailsType : IMapFrom<Employee>
    {
        public Guid CustomerId { get; set; }
        public Guid EmployeeId { get; set; }
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
        public Employee.EmployeeStatus Status { get; set; }
    }
}