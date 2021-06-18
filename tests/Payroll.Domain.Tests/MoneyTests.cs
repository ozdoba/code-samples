using Payroll.Domain.Paycycles;
using Xunit;

namespace Payroll.Domain.Tests
{
    public class MoneyTests
    {
        [Fact]
        public void Sum_of_money_gives_full_amount()
        {
            var coin1 = Money.For("EUR", 1);
            var coin2 = Money.For("EUR", 2);
            var coin3 = Money.For("EUR", 2);

            var banknote = Money.For("EUR", 5);

            Assert.Equal(banknote, coin1 + coin2 + coin3);
        }
    }
}