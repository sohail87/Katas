using System;
using FluentAssertions;
using Xunit;

namespace HarryPotterBooks
{
    public class ScannerTests
    {
        [Fact]
        public void BuyingOneBookCostsEightPounds()
        {
            var price = new Scanner().Scan("First Book");
            price.Should().Be(8);
        }
    }

    public class Scanner
    {
        public int Scan(string bookName)
        {
            return 8;
        }
    }
}
