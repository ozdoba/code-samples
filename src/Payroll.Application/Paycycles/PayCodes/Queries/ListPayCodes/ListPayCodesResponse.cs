using System.Collections.Generic;

namespace Payroll.Application.Paycycles.PayCodes.Queries.ListPayCodes
{
    public class ListPayCodesResponse
    {
        public string CustomerId { get; set; }
        
        public IEnumerable<PayCodeDetails> Data { get; set; }
    }
}