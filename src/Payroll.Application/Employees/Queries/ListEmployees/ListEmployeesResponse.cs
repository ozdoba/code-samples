using System;
using System.Collections.Generic;

namespace Payroll.Application.Employees.Queries.ListEmployees
{
    public class ListEmployeesResponse
    {
        public Guid CustomerId { get; set; }
        
        public IEnumerable<EmployeesListItemType> Employees { get; set; }
    }
}