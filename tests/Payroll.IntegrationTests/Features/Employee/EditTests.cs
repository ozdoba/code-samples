using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Employees.Commands.EditEmployee;
using Xunit;

namespace Payroll.IntegrationTests.Features.Employee
{
    [Collection("Sequential")]
    public class EditEmployeeTest : IntegrationTest
    {
        public EditEmployeeTest(ApiWebApplicationFactory fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task should_fail_on_unknown_employee()
        {
            var unknownEmployeeNumber = "INVALID";
            var command = new EditEmployeeCommand
            {
                EmployeeNumber = unknownEmployeeNumber,
                JobTitle = "Captain",
                FirstName = "Timo",
                LastName = "Tester",
                Address = new Application.Employees.Commands.Shared.Address()
                {
                    BuildingNumber = "1",
                    Street = "Portstreet",
                    City = "Kingston",
                    State = "Port Royale",
                    PostalCode = "JMAKN05",
                    CountryCode = "JM"
                },
                PrivateEmail = "timo.tester@gmail.com",
                CorporateEmail = "t.read@pirates.inc",
                MobileNumber = "+442071838750",
                Nationality = "GB",
                DateOfBirth = new DateTime(1984, 03, 08),
                DateOfEmployment = new DateTime(2020, 02, 12),
                PlaceOfBirth = "London"
            };


            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<EmployeeNotFound>();
        }

        [Fact]
        public async Task should_edit_existing_employee()
        {
            var employeeNumber = "TEST0002";
            var command = Helpers.CreateExampleEmployee().WithEmployeeNumber(employeeNumber);
            await SendAsync(command);

            var editCommand = new EditEmployeeCommand
            {
                EmployeeNumber = employeeNumber,
                JobTitle = "Captain",
                FirstName = "Timo",
                LastName = "Tester",
                Address = new Application.Employees.Commands.Shared.Address()
                {
                    BuildingNumber = "1",
                    Street = "Portstreet",
                    City = "Kingston",
                    State = "Port Royale",
                    PostalCode = "JMAKN05",
                    CountryCode = "JM"
                },
                PrivateEmail = "timo.tester@gmail.com",
                CorporateEmail = "t.read@pirates.inc",
                MobileNumber = "+442071838750",
                Nationality = "GB",
                DateOfBirth = new DateTime(1984, 03, 08),
                DateOfEmployment = new DateTime(2020, 02, 12),
                PlaceOfBirth = "London"
            };

            await SendAsync(editCommand);

            var entity = await ExecuteEmployeesContextAsync(db =>
                db.Employees.Where(e => e.EmployeeNumber == employeeNumber).SingleOrDefaultAsync());

            entity.Should().NotBeNull();
            entity.JobTitle.Should().Be("Captain");
            entity.FirstName.Should().Be("Timo");
            entity.LastName.Should().Be("Tester");
            entity.Address.Should().NotBeNull();
            entity.Address.BuildingNumber.Should().Be("1");
            entity.Address.Street.Should().Be("Portstreet");
            entity.Address.City.Should().Be("Kingston");
            entity.Address.State.Should().Be("Port Royale");
            entity.Address.PostalCode.Should().Be("JMAKN05");
            entity.Address.CountryCode.Should().Be("JM");
            entity.PrivateEmailAddress.Should().Be("timo.tester@gmail.com");
            entity.CorporateEmailAddress.Should().Be("t.read@pirates.inc");
            entity.MobileNumber.Should().Be("+442071838750");
            entity.Nationality.Should().Be("GB");
            entity.DateOfBirth.Should().Be(new DateTime(1984, 03, 08));
            entity.DateOfEmployment.Should().Be(new DateTime(2020, 02, 12));
            entity.PlaceOfBirth.Should().Be("London");
        }
    }
}