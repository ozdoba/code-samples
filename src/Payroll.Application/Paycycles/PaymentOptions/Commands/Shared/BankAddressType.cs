namespace Payroll.Application.Paycycles.PaymentOptions.Commands.Shared
{
    public class BankAddressType
    {
        public string BuildingNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
    }
}