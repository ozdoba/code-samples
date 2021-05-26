using System.Collections.Generic;
using Payroll.Application.Employees.Queries.ExportEmployees;

namespace Payroll.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildEmployeesFile(IEnumerable<EmployeeRecord> records);
    }
}