using Payroll.Application.Employees.Commands.RegisterEmployee;

namespace Payroll.IntegrationTests
{
    public static class EmployeeCommandExtensions
    {
        public static RegisterEmployeeCommand WithEmployeeNumber(this RegisterEmployeeCommand command, string employeeNumber)
        {
            command.EmployeeNumber = employeeNumber;
            return command;
        }
    }
}