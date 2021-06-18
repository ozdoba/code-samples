using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Employees.Commands.RegisterEmployee;
using Xunit;

namespace Payroll.IntegrationTests.Features.Employee
{
    [Collection("Sequential")]
    public class EmployeeRegistrationTest : IntegrationTest
    {
        public EmployeeRegistrationTest(ApiWebApplicationFactory fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task should_register_a_new_employee_command()
        {
            var employeeNumber = "TEST0002";
            var command = Helpers.CreateExampleEmployee().WithEmployeeNumber(employeeNumber);

            await SendAsync(command);
            
            var created = await ExecuteEmployeesContextAsync(db =>
                db.Employees.Where(e => e.EmployeeNumber == employeeNumber).SingleOrDefaultAsync());
            
            created.Should().NotBeNull();
            created.EmployeeNumber.Should().Be("TEST0002");
        }

        [Fact]
        public async Task should_reject_registering_employee_with_duplicate_employee_number()
        {
            var employeeNumber = "TEST0002";
            var command = Helpers.CreateExampleEmployee().WithEmployeeNumber(employeeNumber);
            await SendAsync(command);
            
            var createEmployeeWithDuplicateEmployeeNumberCommand = Helpers.CreateExampleEmployee().WithEmployeeNumber(employeeNumber);

            FluentActions
                .Invoking(async () => await SendAsync(createEmployeeWithDuplicateEmployeeNumberCommand))
                .Should().Throw<DuplicateEmployeeNumber>();
        }
    }
}