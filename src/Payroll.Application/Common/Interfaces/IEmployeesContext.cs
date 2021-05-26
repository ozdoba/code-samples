using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payroll.Domain.Employees;

namespace Payroll.Application.Common.Interfaces
{
    public interface IEmployeesContext
    {
        DbSet<Employee> Employees { get; }
        DbSet<IdDocument> IdDocuments { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}