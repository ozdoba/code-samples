using System;
using System.Collections.Generic;
using Payroll.Domain.Common;

namespace Payroll.Domain.Employees
{
    public class Employee : AuditableEntity
    {
        public Employee() { }

        public Guid CustomerId { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }

        public string JobTitle { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }

        public Address Address { get; set; } = new Address();

        public string PrivateEmailAddress { get; set; }
        public string CorporateEmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PlaceOfBirth { get; set; }
        public string? LocalTaxNumber { get; set; }

        public DateTime? DateOfTermination { get; set; }

        public DateTime DateOfEmployment { get; set; }
        public EmployeeStatus Status { get; set; }

        public ICollection<IdDocument> IdDocuments { get; private set; } = new List<IdDocument>();

        public enum EmployeeStatus
        {
            /// <summary>
            /// The employee is registered and awaiting approval of the
            /// identity documents
            /// </summary>
            AwaitingApproval,

            /// <summary>
            /// The employee has been approved
            /// </summary>
            Approved,

            /// <summary>
            /// The employee is marked as active
            /// </summary>
            Active,

            /// <summary>
            /// The employee has been deactivated
            /// </summary>
            Inactive,

            /// <summary>
            /// The employee has been terminated
            /// </summary>
            Terminated,
        }
    }
}