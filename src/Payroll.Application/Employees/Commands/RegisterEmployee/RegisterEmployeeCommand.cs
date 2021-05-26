using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Common.Interfaces;
using Payroll.Domain.Employees;
using Address = Payroll.Application.Employees.Commands.Shared.Address;

namespace Payroll.Application.Employees.Commands.RegisterEmployee
{
    public class RegisterEmployeeCommand : IRequest<string>
    {
        /// <summary>
        /// Your unique employee/staff identifier to be referenced when making subsequent payroll submission(s).
        /// </summary>
        public string EmployeeNumber { get; set; }
        /// <summary>
        /// Job title of employee
        /// </summary>
        public string JobTitle { get; set; }
        /// <summary>
        /// The employees first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// The employees middle name (if applicable)
        /// </summary>
        public string? MiddleName { get; set; }
        /// <summary>
        /// The employees last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// The employees registered address
        /// </summary>
        public Address Address { get; set; }
        /// <summary>
        /// The employees private email address
        /// </summary>
        public string PrivateEmail { get; set; }
        /// <summary>
        /// The employees corporate email address
        /// </summary>
        public string CorporateEmail { get; set; }

        /// <summary>
        /// The employees mobile phone number in E.164 format, i.e. '+442071838750'
        /// </summary>
        public string MobileNumber { get; set; }
        /// <summary>
        /// The employees nationality 
        /// </summary>
        public string Nationality { get; set; }
        /// <summary>
        /// The employees date of birth, format 1979-10-24 ("yyyy-MM-dd")
        /// </summary>
        public DateTime DateOfBirth { get; set; }
        /// <summary>
        /// The employees place of birth
        /// </summary>
        public string? PlaceOfBirth { get; set; }
        /// <summary>
        /// The date of employment
        /// </summary>
        public DateTime DateOfEmployment { get; set; }
        /// <summary>
        /// The date of termination
        /// </summary>
        public DateTime? DateOfTermination { get; set; }
        /// <summary>
        /// The employees local tax number
        /// </summary>
        public string? LocalTaxNumber { get; set; }
    }

    public class RegisterEmployeeCommandHandler : IRequestHandler<RegisterEmployeeCommand, string>
    {
        private readonly ICustomerService _customerService;
        private readonly IEmployeesContext _context;

        public RegisterEmployeeCommandHandler(IEmployeesContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        
        public async Task<string> Handle(RegisterEmployeeCommand command, CancellationToken cancellationToken)
        {
            var customerId = _customerService.GetCustomerId();
                
            var existingEmployee = await _context.Employees
                .Where(e => e.CustomerId == customerId)
                .FirstOrDefaultAsync(e => e.EmployeeNumber == command.EmployeeNumber, cancellationToken);

            if (existingEmployee != default)
            {
                throw DuplicateEmployeeNumber.ForEmployeeNumber(command.EmployeeNumber);
            }

            var employee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                CustomerId = customerId,
                EmployeeNumber = command.EmployeeNumber,
                JobTitle = command.JobTitle,
                FirstName = command.FirstName,
                MiddleName = command.MiddleName,
                LastName = command.LastName,
                CorporateEmailAddress = command.CorporateEmail,
                PrivateEmailAddress = command.PrivateEmail,
                MobileNumber = command.MobileNumber,
                Nationality = command.Nationality,
                Address = new Domain.Employees.Address()
                {
                    BuildingNumber = command.Address.BuildingNumber,
                    Street = command.Address.Street,
                    City = command.Address.City,
                    State = command.Address.State,
                    PostalCode = command.Address.PostalCode,
                    CountryCode = command.Address.CountryCode
                },
                DateOfBirth = command.DateOfBirth,
                PlaceOfBirth = command.PlaceOfBirth,
                LocalTaxNumber = command.LocalTaxNumber,
                DateOfEmployment = command.DateOfEmployment,
                DateOfTermination = command.DateOfTermination
            };
            
            await _context.Employees.AddAsync(employee, cancellationToken);
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return employee.EmployeeNumber;
        }
    }
}