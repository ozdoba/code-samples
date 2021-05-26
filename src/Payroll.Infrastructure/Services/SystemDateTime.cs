using System;
using Payroll.Application.Common.Interfaces;

namespace Payroll.Infrastructure.Services
{
    public class SystemDateTime : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}