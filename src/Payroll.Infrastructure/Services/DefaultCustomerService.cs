using System;
using Payroll.Application.Common.Interfaces;

namespace Payroll.Infrastructure.Services
{
    public class DefaultCustomerService : ICustomerService
    {
        public Guid GetCustomerId()
        {
            return new Guid("30bc2c0c-7a71-4e9c-b171-097a5250811a");
        }
    }
}