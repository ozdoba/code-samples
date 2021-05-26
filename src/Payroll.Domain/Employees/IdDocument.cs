using System;

namespace Payroll.Domain.Employees
{
    public class IdDocument
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string IdType { get; set; }
        public string DocumentNumber { get; set; }
        public string IssuedBy { get; set; }
        public string IssuedAt { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Content { get; set; }
    }
}