using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace HarryPotterBooks
{
    public class ScannerTests
    {
        [Fact]
        public void BuyingNotingCostsZeroPounds()
        {
            new Scanner().GetTotal().Should().Be(0);
        }

        [Theory]
        [InlineData(HarryPotter.One, 8)]
        [InlineData(HarryPotter.Two, 8)]
        [InlineData(HarryPotter.Three, 8)]
        [InlineData(HarryPotter.Four, 8)]
        [InlineData(HarryPotter.Five, 8)]
        public void BuyingOneBookCostsEightPounds(HarryPotter book, int expectedTotal)
        {
            new Scanner().Scan(book).GetTotal().Should().Be(expectedTotal);
        }

        [Theory]
        [InlineData(new[]{ HarryPotter.One, HarryPotter.One }, 16)]
        [InlineData(new[] { HarryPotter.Two, HarryPotter.Two }, 16)]
        [InlineData(new[] { HarryPotter.Three, HarryPotter.Three, HarryPotter.Three }, 24)]
        [InlineData(new[] { HarryPotter.Four, HarryPotter.Four, HarryPotter.Four, HarryPotter.Four }, 32)]
        [InlineData(new[] { HarryPotter.Five, HarryPotter.Five, HarryPotter.Five, HarryPotter.Five, HarryPotter.Five }, 40)]

        public void BuyingMultipleOfSameBookHasNoDiscount(HarryPotter[] books, int expectedTotal)
        {
            var scanner = new Scanner();
            foreach (var book in books)
            {
                scanner.Scan(book);
            }
            scanner.GetTotal().Should().Be(expectedTotal);
        }

        [Theory]
        [InlineData(new[] { HarryPotter.One, HarryPotter.Two }, 8 * 2 * 0.95)]
        [InlineData(new[] { HarryPotter.Two, HarryPotter.Five }, 8 * 2 * 0.95)]
        public void BuyingTwoDifferentBooksHasFivePercentDiscount(HarryPotter[] books, double expectedTotal)
        {
            var scanner = new Scanner();
            foreach (var book in books)
            {
                scanner.Scan(book);
            }
            scanner.GetTotal().Should().Be(expectedTotal);
        }
    }

    public enum HarryPotter
    {
        One,
        Two,
        Three,
        Four,
        Five,
    }

    public class Scanner
    {
        private List<HarryPotter> _books = new List<HarryPotter>();

        public Scanner Scan(HarryPotter book)
        {
            _books.Add(book);
            return this;
        }

        public double GetTotal()
        {

            var basket = new List<HarryPotter>();
            var discountBag = _books.GroupBy(b => b);
            foreach (var bookGroup in discountBag)
            {
                basket.AddRange(bookGroup.Take(1));
            }
            if (discountBag.Count() == 2) return _books.Count * 8 * 0.95;
            return _books.Count * 8;
        }
    }
}
