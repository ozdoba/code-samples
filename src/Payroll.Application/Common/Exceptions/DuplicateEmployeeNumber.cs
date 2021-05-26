using System;

namespace Payroll.Application.Common.Exceptions
{
    public class DuplicateEmployeeNumber : Exception
    {
        public DuplicateEmployeeNumber(string msg) : base(msg)
        {
        }

        public static DuplicateEmployeeNumber ForEmployeeNumber(string employeeNumber)
        {
            return new DuplicateEmployeeNumber($"Duplicate EmployeeNumber [{employeeNumber}]");
        }
    }
}