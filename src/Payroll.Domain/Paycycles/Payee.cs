using System;
using System.Collections.Generic;
using System.Linq;

namespace Payroll.Domain.Paycycles
{
    public class Payee
    {
        public Guid PaycycleId { get; private set; }
        public string EmployeeNumber { get; private set; }
        
        public PaymentOptions? PaymentOptions { get; set; }

        public ICollection<PayInstruction> PayInstructions { get; set; }

        public Payee(Guid paycycleId, string employeeNumber)
        {
            PaycycleId = paycycleId;
            EmployeeNumber = employeeNumber;
            PayInstructions = new List<PayInstruction>();
        }

        public void UpdatePaymentOptions(
            string accountHolder,
            string accountNumber,
            string? bankName,
            string? swiftCode,
            string? branchCode,
            string? branchAddressBuildingNumber,
            string? branchAddressStreet,
            string? branchAddressCity,
            string? branchAddressState,
            string? branchAddressPostalCode,
            string? branchAddressCountryCode,
            string isoCountryCode)
        {
            PaymentOptions = new PaymentOptions();
            PaymentOptions.AccountHolder = accountHolder;
            PaymentOptions.AccountNumber = accountNumber;
            PaymentOptions.BankName = bankName;
            PaymentOptions.SwiftCode = swiftCode;
            PaymentOptions.BranchCode = branchCode;
            PaymentOptions.BranchAddress = new Address()
            {
                BuildingNumber = branchAddressBuildingNumber,
                Street = branchAddressStreet,
                City = branchAddressCity,
                State = branchAddressState,
                PostalCode = branchAddressPostalCode,
                CountryCode = branchAddressCountryCode
            };
            PaymentOptions.IsoCountryCode = isoCountryCode;
        }

        public void AddPayInstruction(PayInstruction instruction)
        {
            PayInstructions.Add(instruction);
        }

        public PayInstruction? FindInstruction(Guid instructionId)
        {
            return PayInstructions.FirstOrDefault(
                x => x.InstructionId == instructionId);
        }

        public bool RemovePayInstruction(PayInstruction instruction)
        {
            return PayInstructions.Remove(instruction);
        }
    }
}