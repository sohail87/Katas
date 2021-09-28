using System;
using System.Collections.Generic;
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

        [Fact]
        public void BuyingTwoFirstBooksCostsSixteenPounds()
        {
            var scanner = new Scanner();
            scanner.Scan("First Book");
            var price = scanner.Scan("First Book");
            price.Should().Be(16);
        }
    }

    public class Scanner
    {
        private List<string> _books = new List<string>();

        public int Scan(string bookName)
        {
            _books.Add(bookName);
            return _books.Count * 8;
        }
    }
}
