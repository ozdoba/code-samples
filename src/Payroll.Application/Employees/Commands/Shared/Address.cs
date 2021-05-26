namespace Payroll.Application.Employees.Commands.Shared
{
    /// <summary>
    /// Represents an address
    /// </summary>
    public class Address
    {
        /// <summary>
        /// The number of the building (if applicable)
        /// </summary>
        public string BuildingNumber { get; set; }
        /// <summary>
        /// The street part of the address
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// The city
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// The state or county
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// The postal or ZIP code
        /// </summary>
        public string PostalCode { get; set; }
        /// <summary>
        /// The country code
        /// </summary>
        public string CountryCode { get; set; }
    }
}