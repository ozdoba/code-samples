using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Payroll.Application.Common.Exceptions;
using Payroll.Application.Common.Interfaces;
using Payroll.Domain.Employees;

namespace Payroll.Application.Employees.Commands.AddIdDocument
{
    /// <summary>
    /// Represents an identity document
    /// </summary>
    public class AddIdDocumentCommand : IRequest
    {
        /// <summary>
        /// Your unique employee/staff identifier
        /// </summary>
        public string EmployeeNumber { get; set; }
        /// <summary>
        /// The type of the id document
        /// </summary>
        public DocumentType IdType { get; set; }
        /// <summary>
        /// The number of the id document
        /// </summary>
        public string DocumentNumber { get; set; }
        /// <summary>
        /// The issuing authority of the document
        /// </summary>
        public string IssuedBy { get; set; }
        /// <summary>
        /// Where the document was issued
        /// </summary>
        public string IssuedAt { get; set; }
        /// <summary>
        /// When the document was issued
        /// </summary>
        public DateTime IssueDate { get; set; }
        /// <summary>
        /// When does the document expire
        /// </summary>
        public DateTime ExpiryDate { get; set; }
        /// <summary>
        /// Base64 encoded copy of the document.
        /// </summary>
        public string Content { get; set; }
    }
    
    public enum DocumentType
    {
        /// <summary>
        /// Passport document
        /// </summary>
        [EnumMember(Value = "Passport")]
        Passport, 
        /// <summary>
        /// Residency card
        /// </summary>
        [EnumMember(Value = "ResidencyCard")]
        ResidencyCard,
        /// <summary>
        /// Driving License
        /// </summary>
        [EnumMember(Value = "DrivingLicence")]
        DrivingLicence
    }

    public class AddIdDocumentCommandHandler : IRequestHandler<AddIdDocumentCommand>
    {
        private readonly IEmployeesContext _context;
        private readonly ICustomerService _customerService;

        public AddIdDocumentCommandHandler(IEmployeesContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        
        public async Task<Unit> Handle(AddIdDocumentCommand request, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .Where(e => e.CustomerId == _customerService.GetCustomerId())
                .SingleOrDefaultAsync(e => e.EmployeeNumber == request.EmployeeNumber, cancellationToken);

            if (employee == default)
            {
                throw EmployeeNotFound.ForEmployeeNumber(request.EmployeeNumber);
            }
            
            _context.IdDocuments.Add(new IdDocument()
            {
                Id = Guid.NewGuid(),
                EmployeeId = employee.EmployeeId,
                IdType = request.IdType.ToString(),
                DocumentNumber = request.DocumentNumber,
                IssuedBy = request.IssuedBy,
                IssuedAt = request.IssuedAt,
                IssueDate = request.IssueDate,
                ExpiryDate = request.ExpiryDate,
                Content = request.Content
            });

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}