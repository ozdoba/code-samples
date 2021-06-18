using System;

namespace Payroll.Domain.Paycycles
{
    public class PayInstruction
    {
        public Guid InstructionId { get; set; }
        
        public Money? UnitAmount { get; set; }
        // public string? UnitAmountCurrency { get; set; }
        // public decimal? UnitAmountAmount { get; set; }
        public decimal? UnitQuantity { get; set; }
        
        // public string TotalAmountCurrency { get; set; }
        // public decimal TotalAmountAmount { get; set; }
        public Money TotalAmount { get; set; }
        public PayCode PayCode { get; set; }
        public string Description { get; set; }
    }
}