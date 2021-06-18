using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payroll.Application.Common.Interfaces;
using Payroll.Application.Paycycles;
using Payroll.Domain.Common;
using Payroll.Domain.Paycycles;

namespace Payroll.Infrastructure.Persistence.Paycycles
{
    public class PaycyclesContext : DbContext, IPaycyclesContext
    {
        private readonly IDateTime _dateTime;

        public PaycyclesContext(DbContextOptions<PaycyclesContext> options, IDateTime dateTime)
            : base(options)
        {
            _dateTime = dateTime;
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

        public DbSet<Paycycle> Paycycles { get; set; }
        public DbSet<PayCode> PayCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PayCodeConfiguration());
            modelBuilder.ApplyConfiguration(new PayeeConfiguration());
            modelBuilder.ApplyConfiguration(new PayInstructionConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentOptionsConfiguration());
            modelBuilder.Entity<Paycycle>().Property(x=>x.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (PaycycleStatus) Enum.Parse(typeof(PaycycleStatus), v));
        }

        public class PayeeConfiguration : IEntityTypeConfiguration<Payee>
        {
            public void Configure(EntityTypeBuilder<Payee> builder)
            {
                builder.HasKey(x => x.EmployeeNumber);
                builder.Property(x => x.PaycycleId).IsRequired();
            }
        }

        public class PayInstructionConfiguration : IEntityTypeConfiguration<PayInstruction>
        {
            public void Configure(EntityTypeBuilder<PayInstruction> builder)
            {
                builder.HasKey(x => x.InstructionId);
                builder.Property(x=>x.InstructionId).ValueGeneratedNever();
                // builder.Property(x => x.TotalAmountCurrency).IsRequired();
                // builder.Property(x => x.TotalAmountAmount).IsRequired();
                builder.OwnsOne(x => x.TotalAmount, p => p.Property(e=>e.Currency).HasConversion<string>());
                builder.HasOne(x => x.PayCode);
                builder.Property(x => x.Description);
                // builder.Property(x => x.UnitAmountCurrency);
                // builder.Property(x => x.UnitAmountAmount);
                builder.OwnsOne(x => x.UnitAmount, p => p.Property(e=>e.Currency).HasConversion<string>());
                builder.Property(x => x.UnitQuantity);
            }
        }

        public class PaymentOptionsConfiguration : IEntityTypeConfiguration<PaymentOptions>
        {
            public void Configure(EntityTypeBuilder<PaymentOptions> builder)
            {
                builder.HasKey(x => x.EmployeeNumber);
                builder.Property(x=>x.EmployeeNumber).ValueGeneratedNever();
                builder.OwnsOne(x => x.BranchAddress);
                
                builder.HasOne(x => x.Payee)
                    .WithOne(x => x.PaymentOptions)
                    .HasForeignKey<PaymentOptions>(x => x.EmployeeNumber);

                builder.Property(x => x.AccountHolder).IsRequired();
                builder.Property(x => x.AccountNumber).IsRequired();
                builder.Property(x => x.BankName);
                builder.Property(x => x.SwiftCode);
                builder.Property(x => x.BranchCode);
                builder.Property(x => x.IsoCountryCode).IsRequired();
            }
        }
        
        public class PayCodeConfiguration : IEntityTypeConfiguration<PayCode>
        {
            public void Configure(EntityTypeBuilder<PayCode> builder)
            {
                builder
                    .HasKey(x => new { x.CustomerId, x.Code });
            }
        }
    }
}