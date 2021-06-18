using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Employees.Commands.Shared;
using Payroll.Application.Employees.Commands.UpdateAddress;
using Payroll.Application.Employees.Commands.UpdateCorporateEmail;
using Payroll.Application.Employees.Commands.UpdateDateOfBirth;
using Payroll.Application.Employees.Commands.UpdateDateOfEmployment;
using Payroll.Application.Employees.Commands.UpdateMobileNumber;
using Payroll.Application.Employees.Commands.UpdateName;
using Payroll.Application.Employees.Commands.UpdatePlaceOfBirth;
using Payroll.Application.Employees.Commands.UpdatePrivateEmail;
using Payroll.Application.Employees.Commands.UpdateTaxNumber;
using Xunit;

namespace Payroll.IntegrationTests.Features.Employee
{
    [Collection("Sequential")]
    public class UpdateNameTests : IntegrationTest
    {
        public UpdateNameTests(ApiWebApplicationFactory fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task should_fail_on_missing_employee()
        {
            const string unknownEmployeeNumber = "TEST0001";

            var command = new UpdateNameCommand
            {
                EmployeeNumber = unknownEmployeeNumber,
                FirstName = "Timo",
                LastName = "Tester"
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<EmployeeNotFound>();
        }

        [Fact]
        public async Task should_update_name_of_existing_employee()
        {
            const string employeeNumber = "TEST0001";
            const string newFirstName = "Timo";
            const string newLastName = "Tester";
            
            var createEmployeeCommand = Helpers.CreateExampleEmployee().WithEmployeeNumber(employeeNumber);
            await SendAsync(createEmployeeCommand);

            var updateCommand = new UpdateNameCommand
            {
                EmployeeNumber = employeeNumber,
                FirstName = newFirstName,
                LastName = newLastName,
            };
            await SendAsync(updateCommand);

            var entity = await ExecuteEmployeesContextAsync(db =>
                db.Employees.Where(e => e.EmployeeNumber == employeeNumber).SingleOrDefaultAsync()
            );
            entity.Should().NotBeNull();
            entity.FirstName.Should().Be(newFirstName);
            entity.LastName.Should().Be(newLastName);
        }
    }

    [Collection("Sequential")]
    public class UpdateAddressTests : IntegrationTest
    {
        public UpdateAddressTests(ApiWebApplicationFactory fixture) : base(fixture)
        {
        }
        
        [Fact]
        public async Task should_fail_on_missing_employee()
        {
            const string unknownEmployeeNumber = "TEST0001";

            var command = new UpdateAddressCommand
            {
                EmployeeNumber = unknownEmployeeNumber,
                Address = new Address()
                {
                    BuildingNumber = "10",
                    Street = "Test street",
                    PostalCode = "SW3 AB2",
                    City = "London",
                    CountryCode = "GB"
                }
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<EmployeeNotFound>();
        }
        
        [Fact]
        public async Task should_update_address_of_existing_employee()
        {
            const string employeeNumber = "TEST0001";
            const string newBuildingNumber = "10";
            const string newStreet = "Test street";
            const string newPostalCode = "SW3 AB2";
            const string newCity = "London";
            const string newCountryCode = "GB";
            
            var createEmployeeCommand = Helpers.CreateExampleEmployee().WithEmployeeNumber(employeeNumber);
            await SendAsync(createEmployeeCommand);

            var updateCommand = new UpdateAddressCommand
            {
                EmployeeNumber = employeeNumber,
                Address = new Address()
                {
                    BuildingNumber = newBuildingNumber,
                    Street = newStreet,
                    PostalCode = newPostalCode,
                    City = newCity,
                    CountryCode = newCountryCode
                }
            };
            await SendAsync(updateCommand);

            var entity = await ExecuteEmployeesContextAsync(db =>
                db.Employees.Where(e => e.EmployeeNumber == employeeNumber).SingleOrDefaultAsync()
            );
            entity.Should().NotBeNull();
            entity.Address.Should().NotBeNull();
            entity.Address.BuildingNumber.Should().Be(newBuildingNumber);
            entity.Address.Street.Should().Be(newStreet);
            entity.Address.PostalCode.Should().Be(newPostalCode);
            entity.Address.City.Should().Be(newCity);
            entity.Address.CountryCode.Should().Be(newCountryCode);
        }
    }
    
    [Collection("Sequential")]
    public class UpdatePlaceOfBirthTests : IntegrationTest
    {
        public UpdatePlaceOfBirthTests(ApiWebApplicationFactory fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task should_fail_on_missing_employee()
        {
            const string unknownEmployeeNumber = "TEST0001";

            var command = new UpdatePlaceOfBirthCommand()
            {
                EmployeeNumber = unknownEmployeeNumber,
                PlaceOfBirth = "GB",
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<EmployeeNotFound>();
        }

        [Fact]
        public async Task should_update_place_of_birth_of_existing_employee()
        {
            const string employeeNumber = "TEST0001";
            const string newPlaceOfBirth = "DE";
            
            var createEmployeeCommand = Helpers.CreateExampleEmployee().WithEmployeeNumber(employeeNumber);
            await SendAsync(createEmployeeCommand);

            var updateCommand = new UpdatePlaceOfBirthCommand
            {
                EmployeeNumber = employeeNumber,
                PlaceOfBirth = newPlaceOfBirth
            };
            await SendAsync(updateCommand);

            var entity = await ExecuteEmployeesContextAsync(db =>
                db.Employees.Where(e => e.EmployeeNumber == employeeNumber).SingleOrDefaultAsync()
            );
            entity.Should().NotBeNull();
            entity.PlaceOfBirth.Should().Be(newPlaceOfBirth);
        }
    }
    
    [Collection("Sequential")]
    public class UpdatePrivateEmailTests : IntegrationTest
    {
        public UpdatePrivateEmailTests(ApiWebApplicationFactory fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task should_fail_on_missing_employee()
        {
            const string unknownEmployeeNumber = "TEST0001";

            var command = new UpdatePrivateEmailCommand
            {
                EmployeeNumber = unknownEmployeeNumber,
                PrivateEmail = "private@email.com",
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<EmployeeNotFound>();
        }

        [Fact]
        public async Task should_update_private_email_of_existing_employee()
        {
            const string employeeNumber = "TEST0001";
            const string newPrivateEmail = "private@email.com";
            
            var createEmployeeCommand = Helpers.CreateExampleEmployee().WithEmployeeNumber(employeeNumber);
            await SendAsync(createEmployeeCommand);

            var updateCommand = new UpdatePrivateEmailCommand
            {
                EmployeeNumber = employeeNumber,
                PrivateEmail = newPrivateEmail
            };
            await SendAsync(updateCommand);

            var entity = await ExecuteEmployeesContextAsync(db =>
                db.Employees.Where(e => e.EmployeeNumber == employeeNumber).SingleOrDefaultAsync()
            );
            entity.Should().NotBeNull();
            entity.PrivateEmailAddress.Should().Be(newPrivateEmail);
        }
    }
    
    [Collection("Sequential")]
    public class UpdateCorporateEmailTests : IntegrationTest
    {
        public UpdateCorporateEmailTests(ApiWebApplicationFactory fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task should_fail_on_missing_employee()
        {
            const string unknownEmployeeNumber = "TEST0001";

            var command = new UpdateCorporateEmailCommand
            {
                EmployeeNumber = unknownEmployeeNumber,
                CorporateEmail = "corporate@email.com",
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<EmployeeNotFound>();
        }

        [Fact]
        public async Task should_update_corporate_email_of_existing_employee()
        {
            const string employeeNumber = "TEST0001";
            const string newCorporateEmail = "corporate@email.com";
            
            var createEmployeeCommand = Helpers.CreateExampleEmployee().WithEmployeeNumber(employeeNumber);
            await SendAsync(createEmployeeCommand);

            var updateCommand = new UpdateCorporateEmailCommand
            {
                EmployeeNumber = employeeNumber,
                CorporateEmail = newCorporateEmail
            };
            await SendAsync(updateCommand);

            var entity = await ExecuteEmployeesContextAsync(db =>
                db.Employees.Where(e => e.EmployeeNumber == employeeNumber).SingleOrDefaultAsync()
            );
            entity.Should().NotBeNull();
            entity.CorporateEmailAddress.Should().Be(newCorporateEmail);
        }
    }
    
    [Collection("Sequential")]
    public class UpdateTaxNumberTests : IntegrationTest
    {
        public UpdateTaxNumberTests(ApiWebApplicationFactory fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task should_fail_on_missing_employee()
        {
            const string unknownEmployeeNumber = "TEST0001";

            var command = new UpdateTaxNumberCommand
            {
                EmployeeNumber = unknownEmployeeNumber,
                LocalTaxNumber = "LOCAL/T12345",
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<EmployeeNotFound>();
        }

        [Fact]
        public async Task should_update_tax_number_of_existing_employee()
        {
            const string employeeNumber = "TEST0001";
            const string newTaxNumber = "LOCAL/T12345";
            
            var createEmployeeCommand = Helpers.CreateExampleEmployee().WithEmployeeNumber(employeeNumber);
            await SendAsync(createEmployeeCommand);

            var updateCommand = new UpdateTaxNumberCommand
            {
                EmployeeNumber = employeeNumber,
                LocalTaxNumber = newTaxNumber
            };
            await SendAsync(updateCommand);

            var entity = await ExecuteEmployeesContextAsync(db =>
                db.Employees.Where(e => e.EmployeeNumber == employeeNumber).SingleOrDefaultAsync()
            );
            entity.Should().NotBeNull();
            entity.LocalTaxNumber.Should().Be(newTaxNumber);
        }
    }
    
    
    [Collection("Sequential")]
    public class UpdateMobileNumberTests : IntegrationTest
    {
        public UpdateMobileNumberTests(ApiWebApplicationFactory fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task should_fail_on_missing_employee()
        {
            const string unknownEmployeeNumber = "TEST0001";

            var command = new UpdateMobileNumberCommand
            {
                EmployeeNumber = unknownEmployeeNumber,
                MobileNumber = "+4412345678"
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<EmployeeNotFound>();
        }

        [Fact]
        public async Task should_update_mobile_number_of_existing_employee()
        {
            const string employeeNumber = "TEST0001";
            const string newMobileNumber = "+4412345678";
            
            var createEmployeeCommand = Helpers.CreateExampleEmployee().WithEmployeeNumber(employeeNumber);
            await SendAsync(createEmployeeCommand);

            var updateCommand = new UpdateMobileNumberCommand
            {
                EmployeeNumber = employeeNumber,
                MobileNumber = newMobileNumber
            };
            await SendAsync(updateCommand);

            var entity = await ExecuteEmployeesContextAsync(db =>
                db.Employees.Where(e => e.EmployeeNumber == employeeNumber).SingleOrDefaultAsync()
            );
            entity.Should().NotBeNull();
            entity.MobileNumber.Should().Be(newMobileNumber);
        }
    }
    
    
    [Collection("Sequential")]
    public class UpdateDateOfBirthTests : IntegrationTest
    {
        public UpdateDateOfBirthTests(ApiWebApplicationFactory fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task should_fail_on_missing_employee()
        {
            const string unknownEmployeeNumber = "TEST0001";

            var command = new UpdateDateOfBirthCommand
            {
                EmployeeNumber = unknownEmployeeNumber,
                DateOfBirth = DateTime.Today
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<EmployeeNotFound>();
        }

        [Fact]
        public async Task should_update_date_of_birth_of_existing_employee()
        {
            const string employeeNumber = "TEST0001";
            DateTime newDateOfBirth = DateTime.Today;
            
            var createEmployeeCommand = Helpers.CreateExampleEmployee().WithEmployeeNumber(employeeNumber);
            await SendAsync(createEmployeeCommand);

            var updateCommand = new UpdateDateOfBirthCommand
            {
                EmployeeNumber = employeeNumber,
                DateOfBirth = newDateOfBirth
            };
            await SendAsync(updateCommand);

            var entity = await ExecuteEmployeesContextAsync(db =>
                db.Employees.Where(e => e.EmployeeNumber == employeeNumber).SingleOrDefaultAsync()
            );
            entity.Should().NotBeNull();
            entity.DateOfBirth.Should().Be(newDateOfBirth);
        }
    }
    
    
    [Collection("Sequential")]
    public class UpdateDateOfEmploymentTests : IntegrationTest
    {
        public UpdateDateOfEmploymentTests(ApiWebApplicationFactory fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task should_fail_on_missing_employee()
        {
            const string unknownEmployeeNumber = "TEST0001";

            var command = new UpdateDateOfEmploymentCommand
            {
                EmployeeNumber = unknownEmployeeNumber,
                DateOfEmployment = DateTime.Today
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<EmployeeNotFound>();
        }

        [Fact]
        public async Task should_update_date_of_employment_of_existing_employee()
        {
            const string employeeNumber = "TEST0001";
            DateTime newDateOfEmployment = DateTime.Today;
            
            var createEmployeeCommand = Helpers.CreateExampleEmployee().WithEmployeeNumber(employeeNumber);
            await SendAsync(createEmployeeCommand);

            var updateCommand = new UpdateDateOfEmploymentCommand
            {
                EmployeeNumber = employeeNumber,
                DateOfEmployment = newDateOfEmployment
            };
            await SendAsync(updateCommand);

            var entity = await ExecuteEmployeesContextAsync(db =>
                db.Employees.Where(e => e.EmployeeNumber == employeeNumber).SingleOrDefaultAsync()
            );
            entity.Should().NotBeNull();
            entity.DateOfEmployment.Should().Be(newDateOfEmployment);
        }
    }
}