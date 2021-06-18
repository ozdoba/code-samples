using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payroll.Infrastructure.Persistence;
using Respawn;
using Xunit;

namespace Payroll.IntegrationTests
{
    public class IntegrationTest : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly Checkpoint _checkpoint = new Checkpoint()
        {
            TablesToIgnore = new[] {"__EFMigrationsHistory"}
        };

        protected readonly ApiWebApplicationFactory _factory;
        private readonly IServiceScopeFactory _scopeFactory;

        public IntegrationTest(ApiWebApplicationFactory fixture)
        {
            _factory = fixture;
            _scopeFactory = fixture.Services.GetService<IServiceScopeFactory>();

            _checkpoint.Reset(_factory.Configuration.GetConnectionString("DefaultConnection")).Wait();
        }

        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<EmployeesContext>();

                try
                {
                    await dbContext.Database.BeginTransactionAsync();

                    await action(scope.ServiceProvider);

                    await dbContext.Database.CommitTransactionAsync();
                }
                catch (Exception)
                {
                    await dbContext.Database.RollbackTransactionAsync();
                    throw;
                }
            }
        }
        
        public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<EmployeesContext>();

                try
                {
                    await dbContext.Database.BeginTransactionAsync();

                    var result = await action(scope.ServiceProvider);

                    await dbContext.Database.CommitTransactionAsync();
                    
                    return result;
                }
                catch (Exception)
                {
                    await dbContext.Database.RollbackTransactionAsync();
                    throw;
                }
            }
        }

        public Task ExecuteEmployeesContextAsync(Func<EmployeesContext, Task> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetRequiredService<EmployeesContext>()));
        }
        
        public Task<T> ExecuteEmployeesContextAsync<T>(Func<EmployeesContext, Task<T>> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetRequiredService<EmployeesContext>()));
        }

        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();

                return mediator.Send(request);
            });
        }
    }
}