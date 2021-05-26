using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payroll.Application.Common.Interfaces;
using Payroll.Infrastructure.Files;
using Payroll.Infrastructure.Persistence;
using Payroll.Infrastructure.Services;

namespace Payroll.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICustomerService, DefaultCustomerService>();
            services.AddScoped<ICountryLookup, CountryLookup>();
            services.AddScoped<IDateTime, SystemDateTime>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
            
            services.AddDbContext<EmployeesContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory");
                        sqlOptions.MigrationsAssembly(typeof(EmployeesContext).Assembly.FullName);
                    }));
            
            
            
            services.AddScoped<IEmployeesContext>(provider => provider.GetService<EmployeesContext>());
            
            return services;
        }
    }
}