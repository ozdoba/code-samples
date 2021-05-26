using System;
using Payroll.Domain.Common;

namespace Payroll.Domain.Paycycles
{
    public class Paycycle : AuditableEntity
    {
        public Guid PaycycleId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Payday { get; set; }

        public PaycycleStatus Status { get; set; } = PaycycleStatus.Initiated;
    }
    
    public enum PaycycleStatus
    {
        Initiated,
        Submitted,
        AwaitingApproval,
        AwaitingFunds,
        Funded,
        PendingProcessing,
        Processed,
    }
}