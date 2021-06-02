using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payroll.Domain.Paycycles;

namespace Payroll.Application.Paycycles
{
    public interface IPaycyclesContext
    {
        DbSet<Paycycle> Paycycles { get; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}