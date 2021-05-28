using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Employees.Commands.Shared;

namespace Payroll.Application.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeDetailsCommand : IRequest
    {
        /// <summary>
        /// Your unique employee/staff identifier
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
        // /// <summary>
        // /// The base currency the employee receives his/her pay in
        // /// </summary>
        // public string BaseCurrencyCode { get; set; }
        // /// <summary>
        // /// The transfer options for this employee
        // /// </summary>
        // public TransferOptions TransferOptions { get; set; }
    }

    public class UpdateEmployeeDetailsCommandHandler : IRequestHandler<UpdateEmployeeDetailsCommand>
    {
        private readonly IEmployeesContext _context;
        private readonly ICustomerService _customerService;

        public UpdateEmployeeDetailsCommandHandler(IEmployeesContext context, ICustomerService customerService)
        {
            _context = context; 
            _customerService = customerService;
        }
        
        public async Task<Unit> Handle(UpdateEmployeeDetailsCommand command, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
                .Where(e => e.CustomerId == _customerService.GetCustomerId())
                .SingleOrDefaultAsync(e => e.EmployeeNumber == command.EmployeeNumber, cancellationToken);

            if (employee == default)
            {
                throw EmployeeNotFound.ForEmployeeNumber(command.EmployeeNumber);
            }

            
            employee.JobTitle = command.JobTitle;
            employee.FirstName = command.FirstName;
            employee.MiddleName = command.MiddleName;
            employee.LastName = command.LastName;
            employee.Address = new Domain.Employees.Address
            {
                BuildingNumber = command.Address.BuildingNumber,
                Street = command.Address.Street,
                City = command.Address.City,
                State = command.Address.State,
                PostalCode = command.Address.PostalCode,
                CountryCode = command.Address.CountryCode 
            };
            employee.PrivateEmailAddress = command.PrivateEmail;
            employee.CorporateEmailAddress = command.CorporateEmail;
            employee.MobileNumber = command.MobileNumber;
            employee.Nationality = command.Nationality;
            employee.DateOfBirth = command.DateOfBirth;
            employee.PlaceOfBirth = command.PlaceOfBirth;
            employee.LocalTaxNumber = command.LocalTaxNumber;
            employee.DateOfEmployment = command.DateOfEmployment;
            employee.DateOfTermination = command.DateOfTermination;
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}