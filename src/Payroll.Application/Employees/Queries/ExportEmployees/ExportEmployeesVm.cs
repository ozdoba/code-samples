namespace Payroll.Application.Employees.Queries.ExportEmployees
{
    public class ExportEmployeesVm
    {
        public string FileName { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }
    }
}