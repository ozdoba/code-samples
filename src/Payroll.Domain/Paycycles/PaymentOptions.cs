namespace Payroll.Domain.Paycycles
{
    public class PaymentOptions
    {
        public string EmployeeNumber { get; set; }
        public Payee Payee { get; set; }

        public string AccountHolder { get; set; }
        public string AccountNumber { get; set; }
                
        public string? BankName { get; set; }	
        public string? SwiftCode { get; set; }
        public string? BranchCode { get; set; }
        public Address? BranchAddress { get; set; }
        
        public string IsoCountryCode { get; set; }
    }
    
    public class Address
    {
        public string? BuildingNumber { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? CountryCode { get; set; }
    }
}