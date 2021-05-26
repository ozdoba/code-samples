using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payroll.Application.Common.Interfaces;
using Payroll.Domain.Common;
using Payroll.Domain.Employees;

namespace Payroll.Infrastructure.Persistence
{
    public class EmployeesContext : DbContext, IEmployeesContext
    {
        private readonly IDateTime _dateTime;

        public EmployeesContext(DbContextOptions<EmployeesContext> options, IDateTime dateTime)
            : base(options)
        {
            _dateTime = dateTime;
        }

        // public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<IdDocument> IdDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IdDocumentEntityTypeConfiguration());
        }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;

                    case EntityState.Modified:
                        // entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
    }

    public class IdDocumentEntityTypeConfiguration: IEntityTypeConfiguration<IdDocument>
    {
        public void Configure(EntityTypeBuilder<IdDocument> builder)
        {
            builder.HasKey(x => x.Id);
        
            builder.Property(x => x.EmployeeNumber).IsRequired();
            builder.Property(x => x.IdType).IsRequired();
            builder.Property(x => x.DocumentNumber).IsRequired();
            builder.Property(x => x.IssuedBy).IsRequired();
            builder.Property(x => x.IssuedAt).IsRequired();
            builder.Property(x => x.IssueDate).IsRequired();
            builder.Property(x => x.ExpiryDate).IsRequired();
            builder.Property(x => x.Content).IsRequired();
        }
    }

    
    public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(x => x.EmployeeId);
            builder.Property(x => x.CustomerId).IsRequired();
            builder.Property(x => x.EmployeeNumber).IsRequired();
            builder.Property(x => x.JobTitle).IsRequired();
            builder.Property(x => x.FirstName).IsRequired();
            builder.Property(x => x.MiddleName);
            builder.Property(x => x.LastName).IsRequired();
            builder.OwnsOne(x => x.Address);
            builder.Property(x => x.CorporateEmailAddress).IsRequired();
            builder.Property(x => x.PrivateEmailAddress);
            builder.Property(x => x.MobileNumber).IsRequired();
            builder.Property(x => x.Nationality).IsRequired();
            builder.Property(x=>x.DateOfBirth).IsRequired();
            builder.Property(x => x.PlaceOfBirth);
            builder.Property(x => x.LocalTaxNumber);
            builder.Property(x => x.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (Employee.EmployeeStatus) Enum.Parse(typeof(Employee.EmployeeStatus), v));
        }
    }
}