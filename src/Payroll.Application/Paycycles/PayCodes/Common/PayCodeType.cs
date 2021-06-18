namespace Payroll.Application.Paycycles.PayCodes.Common
{
    public enum PayCodeType
    {
        /// <summary>
        /// Used to indicate a payment instruction as a payment, meaning that its value will be added to the paycycle
        /// </summary>
        Payment,
        /// <summary>
        /// Used to indicate a payment instruction as a deductible, meaning that its value will be subtracted
        /// from the paycycle
        /// </summary>
        Deductible,
    }
}