using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Employees.Queries.ExportEmployees;
using Payroll.Infrastructure.Files.Maps;

namespace Payroll.Infrastructure.Files
{
    public class CsvFileBuilder : ICsvFileBuilder
    {
        public byte[] BuildEmployeesFile(IEnumerable<EmployeeRecord> records)
        {
            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

                csvWriter.Context.RegisterClassMap<EmployeeRecordMap>();
                csvWriter.WriteRecords(records);
            }

            return memoryStream.ToArray();
        }
    }
}