using System;
using Payroll.Application.Employees.Commands.RegisterEmployee;

namespace Payroll.IntegrationTests
{
    internal static class Helpers
    {
        internal static RegisterEmployeeCommand CreateExampleEmployee()
        {
            return new RegisterEmployeeCommand
            {
                EmployeeNumber = "TEST0002",
                JobTitle = "Crewmember",
                FirstName = "Mary",
                LastName = "Read",
                Address = new Application.Employees.Commands.Shared.Address() {
                    BuildingNumber = "1",
                    Street = "Portstreet",
                    City = "Kingston",
                    State = "Port Royale",
                    PostalCode = "JMAKN05",
                    CountryCode = "JM"
                },
                PrivateEmail = "maryread@gmail.com",
                CorporateEmail = "m.read@pirates.inc",
                MobileNumber = "+442071838750",
                Nationality = "GB",
                DateOfBirth = new DateTime(1685, 03, 08),
                DateOfEmployment = new DateTime(2021, 02, 12),
                PlaceOfBirth = "London",
            };
        }
    }
}