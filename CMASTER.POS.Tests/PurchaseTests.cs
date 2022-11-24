using CMASTER.POS.Business;
using Xunit;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using CMASTER.POS.Business.Interfaces;

namespace CMASTER.POS.Tests
{
    public class PurchaseTests
    {
        #region Fields

        private IPurchase _purchase;

        #endregion

        #region Properties

        public static IEnumerable<object[]> CashList()
        {
            yield return new object[] { 40, new List<Cash> { new Cash(20, 1), new Cash(10, 2), new Cash(5, 1), new Cash(1, 10), new Cash(0.10m, 10), new Cash(0.05m, 20) }, new List<Cash> { new Cash(10, 1), new Cash(5, 1), new Cash(2, 1) } };
            yield return new object[] { 152, new List<Cash> { new Cash(100, 1), new Cash(50, 2) }, new List<Cash> { new Cash(20, 2), new Cash(5, 1), new Cash(2, 1), new Cash(1, 1) } };
        }

        #endregion

        #region Constructors

        public PurchaseTests()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
            IEnumerable<decimal> currencyDenominations = configuration["currency"].Split("|").Select(decimal.Parse).OrderByDescending(c => c);
            _purchase = new Purchase(currencyDenominations);
        }

        #endregion

        #region Tests

        [Fact]
        public void Should_Correctly_Calculate_Change()
        {
            var ProvidedCash = new List<ICash>
            {
                new Cash(20, 1),
                new Cash(10, 2),
                new Cash(5, 1),
                new Cash(1, 10),
                new Cash(0.10m, 10),
                new Cash(0.05m, 20)
            };

            var Expected = new List<ICash>
            {
                new Cash(10, 1),
                new Cash(5, 1),
                new Cash(2, 1)
            };

            var Actual = _purchase.CalculateChange(ProvidedCash, 40);

            Expected.Should().BeEquivalentTo(Actual);
        }

        [Fact]
        public void Should_Fail_Calculate_Change()
        {
            var ProvidedCash = new List<ICash>
            {
                new Cash(20, 1),
                new Cash(10, 2),
                new Cash(5, 1),
                new Cash(1, 10),
                new Cash(0.10m, 10),
                new Cash(0.05m, 20)
            };

            var Expected = new List<ICash>
            {
                new Cash(10, 2),
                new Cash(5, 1),
                new Cash(2, 1)
            };

            var Actual = _purchase.CalculateChange(ProvidedCash, 40);

            Expected.Should().NotBeEquivalentTo(Actual);
        }

        [Fact]
        public void Should_Not_Return_Change()
        {
            var ProvidedCash = new List<ICash>
            {
                new Cash(20, 1),
                new Cash(10, 2),
                new Cash(5, 1),
                new Cash(1, 3),
                new Cash(0.10m, 10),
                new Cash(0.05m, 20)
            };

            var Actual = _purchase.CalculateChange(ProvidedCash, 50);

            Assert.Empty(Actual);
        }

        [Theory]
        [MemberData(nameof(CashList))]
        public void Should_Calculate_Change_Several_Inputs(decimal total, IEnumerable<ICash> provided, IEnumerable<ICash> expected)
        { 
            var Actual = _purchase.CalculateChange(provided, total);

            expected.Should().BeEquivalentTo(Actual);
        }

        [Fact]
        public void Should_Throw_Exception_Not_Enough()
        {
            var ProvidedCash = new List<ICash>
            {
                new Cash(20, 1),
                new Cash(10, 2),
                new Cash(0.10m, 10),
                new Cash(0.05m, 20)
            };

            Assert.Throws<Exception>(() => _purchase.CalculateChange(ProvidedCash, 100));
        }

        #endregion
    }
}
