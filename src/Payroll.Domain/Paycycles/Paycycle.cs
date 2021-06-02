using System;
using System.Collections.Generic;
using System.Linq;

namespace Payroll.Domain.Paycycles
{
    public class Paycycle
    {
        public Guid PaycycleId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Payday { get; set; }
        
        public PaycycleStatus Status { get; set; }
        
        public ICollection<Payee> Payees { get; private set; }

        public Paycycle(Guid customerId, DateTime startDate, DateTime endDate, DateTime payday)
        {
            if (customerId == default)
            {
                throw new ArgumentNullException(nameof(customerId), "CustomerId must not be null");
            }
            if (startDate == default)
            {
                throw new ArgumentNullException(nameof(startDate), "StartDate must not be null");
            }
            if (endDate == default)
            {
                throw new ArgumentNullException(nameof(endDate), "EndDate must not be null");
            }
            if (payday == default)
            {
                throw new ArgumentNullException(nameof(payday), "Payday must not be null");
            }

            CustomerId = customerId;
            StartDate = startDate;
            EndDate = endDate;
            Payday = payday;
            Status = PaycycleStatus.Initiated;
        }

        public void AddInstruction(string employeeNumber, PayInstruction instruction)
        {
            var payee = FindOrCreatePayee(employeeNumber);

            payee.AddPayInstruction(instruction);
        }
        
        public void Confirm()
        {
            Status = PaycycleStatus.AwaitingApproval;
        }

        public Payee? FindPayee(string employeeNumber)
        {
            return Payees.FirstOrDefault(p => p.EmployeeNumber == employeeNumber);
        }
        
        private Payee FindOrCreatePayee(string employeeNumber)
        {
            var payee = FindPayee(employeeNumber);

            if (payee == default)
            {
                payee = new Payee(PaycycleId, employeeNumber);
                Payees.Add(payee);
            }

            return payee;
        }
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