using System;

namespace Payroll.Domain.Paycycles
{
    public class Payee
    {
        public Guid PaycycleId { get; private set; }
        public string EmployeeNumber { get; private set; }

        public PaymentOptions PaymentOptions { get; set; } = new PaymentOptions();

        // public ICollection<PayInstruction> PayInstructions { get; set; }

        public Payee(Guid paycycleId, string employeeNumber)
        {
            PaycycleId = paycycleId;
            EmployeeNumber = employeeNumber;
            // PayInstructions = new List<PayInstruction>();
        }

        // public void AddPayInstruction(PayInstruction instruction)
        // {
        //     PayInstructions.Add(instruction);
        // }
        //
        // public PayInstruction? FindInstruction(Guid instructionId)
        // {
        //     return PayInstructions.FirstOrDefault(
        //         x => x.InstructionId == instructionId);
        // }
        //
        // public bool RemovePayInstruction(PayInstruction instruction)
        // {
        //     return PayInstructions.Remove(instruction);
        // }
    }
}