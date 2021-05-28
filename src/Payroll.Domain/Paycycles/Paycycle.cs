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

        // public ICollection<Payee> Payees { get; private set; } = new List<Payee>();
        
        // public void AddInstruction(string employeeNumber, PayInstruction instruction)
        // {
        //     var payee = FindOrCreatePayee(employeeNumber);
        //
        //     payee.AddPayInstruction(instruction);
        // }
        
        public void Confirm()
        {
            Status = PaycycleStatus.AwaitingApproval;
        }

        // public Payee? FindPayee(string employeeNumber)
        // {
        //     return Payees.FirstOrDefault(p => p.EmployeeNumber == employeeNumber);
        // }
        //
        // private Payee FindOrCreatePayee(string employeeNumber)
        // {
        //     var payee = FindPayee(employeeNumber);
        //
        //     if (payee == default)
        //     {
        //         payee = new Payee(PaycycleId, employeeNumber);
        //         Payees.Add(payee);
        //     }
        //
        //     return payee;
        // }
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