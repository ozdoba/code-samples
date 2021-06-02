using System;
using System.Collections.Generic;
using Payroll.Domain.Common;

namespace Payroll.Domain.Paycycles
{
    public class Money : ValueObject
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        private Money(decimal amount, string currency) => (Amount, Currency) = (amount, currency);

        public static Money For(string currency, decimal? amount)
        {
            if (string.IsNullOrEmpty(currency) || !amount.HasValue)
            {
                return default;
            }
            return new Money(amount.Value, currency);
        }

        public Money Add(Money summand)
        {
            if (Currency != summand.Currency)
                throw new CurrencyMismatchException(
                    "Cannot sum amounts with different currencies");

            return new Money(Amount + summand.Amount, Currency);
        }

        public Money Subtract(Money subtrahend)
        {
            if (Currency != subtrahend.Currency)
                throw new CurrencyMismatchException(
                    "Cannot subtract amounts with different currencies");

            return new Money(Amount - subtrahend.Amount, Currency);
        }

        public static Money operator +(Money summand1, Money summand2) =>
            summand1.Add(summand2);

        public static Money operator -(Money minuend, Money subtrahend) =>
            minuend.Subtract(subtrahend);

        public override string ToString() => $"{Currency} {Amount}";
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Currency;
            yield return Amount;
        }
    }

    public class CurrencyMismatchException : Exception
    {
        public CurrencyMismatchException(string message) : base(message)
        {
        }
    }
}