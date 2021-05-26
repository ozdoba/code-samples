using System;

namespace Payroll.Application.Common.Exceptions
{
    public class EmployeeNotFound : Exception
    {
        public EmployeeNotFound(string msg) : base(msg)
        {
        }

        public static EmployeeNotFound ForEmployeeNumber(string employeeNumber)
        {
            return new EmployeeNotFound($"Employee with EmployeeNumber [{employeeNumber}] not found.");
        }
    }
}