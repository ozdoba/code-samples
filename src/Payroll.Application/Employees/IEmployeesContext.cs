using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payroll.Domain.Employees;

namespace Payroll.Application.Employees
{
    public interface IEmployeesContext
    {
        DbSet<Employee> Employees { get; }
        DbSet<IdDocument> IdDocuments { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}