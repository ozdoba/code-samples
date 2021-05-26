using System.Collections.Generic;

namespace Payroll.Application.Employees.Queries.ListIdDocuments
{
    public class IdDocumentsListVm
    {
        public string EmployeeNumber { get; set; }
        public ICollection<IdDocumentType> Documents { get; set; }
    }
}