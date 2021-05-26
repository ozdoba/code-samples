using System.Globalization;
using CsvHelper.Configuration;
using Payroll.Application.Employees.Queries.ExportEmployees;

namespace Payroll.Infrastructure.Files.Maps
{
    public class EmployeeRecordMap : ClassMap<EmployeeRecord>
    {
        public EmployeeRecordMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.DateOfTermination).Optional();
            Map(m => m.DateOfEmployment).Optional();
        }
    }
    
    
}