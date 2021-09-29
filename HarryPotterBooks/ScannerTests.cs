using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace HarryPotterBooks
{
    public class ScannerTests
    {
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
        [InlineData(new[] {HarryPotter.One, HarryPotter.One}, 16)]
        [InlineData(new[] {HarryPotter.Two, HarryPotter.Two}, 16)]
        [InlineData(new[] {HarryPotter.Three, HarryPotter.Three, HarryPotter.Three}, 24)]
        [InlineData(new[] {HarryPotter.Four, HarryPotter.Four, HarryPotter.Four, HarryPotter.Four}, 32)]
        [InlineData(new[] {HarryPotter.Five, HarryPotter.Five, HarryPotter.Five, HarryPotter.Five, HarryPotter.Five},
            40)]
        public void BuyingMultipleOfSameBookHasNoDiscount(HarryPotter[] books, int expectedTotal)
        {
            ScanBooksAndGetTotal(books).Should().Be(expectedTotal);
        }

        [Theory]
        [InlineData(new[] {HarryPotter.One, HarryPotter.Two}, 8 * 2 * 0.95)]
        [InlineData(new[] {HarryPotter.Two, HarryPotter.Five}, 8 * 2 * 0.95)]
        [InlineData(new[] { HarryPotter.One, HarryPotter.Two, HarryPotter.Three }, 8 * 3 * 0.90)]
        [InlineData(new[] { HarryPotter.Three, HarryPotter.Four, HarryPotter.Five }, 8 * 3 * 0.90)]
        [InlineData(new[] { HarryPotter.One, HarryPotter.Two, HarryPotter.Three, HarryPotter.Four }, 8 * 4 * 0.80)]
        [InlineData(new[] { HarryPotter.Two, HarryPotter.Three, HarryPotter.Four, HarryPotter.Five }, 8 * 4 * 0.80)]
        [InlineData(new[] { HarryPotter.One, HarryPotter.Two, HarryPotter.Three, HarryPotter.Four, HarryPotter.Five }, 8 * 5 * 0.75)]
        public void BuyingDifferentBooksHasAPercentDiscount(HarryPotter[] books, double expectedTotal)
        {
            ScanBooksAndGetTotal(books).Should().Be(expectedTotal);
        }

        [Theory]
        [InlineData(new[] { HarryPotter.One, HarryPotter.Two, HarryPotter.One }, ((8 * 2 * 0.95) + 8))]
        //[InlineData(new[] { HarryPotter.One, HarryPotter.Two, HarryPotter.One, HarryPotter.Two }, (2 * (8 * 2 * 0.95)))]
        public void BuyingSeveralDifferentBooks(HarryPotter[] books, double expectedTotal)
        {
            ScanBooksAndGetTotal(books).Should().Be(expectedTotal);
        }



        private static double ScanBooksAndGetTotal(HarryPotter[] books)
        {
            var scanner = new Scanner();
            foreach (var book in books) scanner.Scan(book);

            return scanner.GetTotal();
        }

        [Fact]
        public void BuyingNotingCostsZeroPounds()
        {
            new Scanner().GetTotal().Should().Be(0);
        }
    }

    public enum HarryPotter
    {
        One,
        Two,
        Three,
        Four,
        Five
    }

    public class Scanner
    {
        private readonly List<HarryPotter> _books = new List<HarryPotter>();

        private readonly Dictionary<int, double> _discountRules = new Dictionary<int, double>
        {
            {0, 1},
            {1, 1},
            {2, 0.95},
            {3, 0.90},
            {4, 0.80},
            {5, 0.75}
        };

        public Scanner Scan(HarryPotter book)
        {
            _books.Add(book);
            return this;
        }

        public double GetTotal()
        {
            var total = 0.0;
            var uniqueBooks = new List<HarryPotter>();
            var bookGroupings = _books.GroupBy(b => b).ToDictionary(k=>k.Key, v=>v.ToList());

            foreach (var bookGroup in bookGroupings)
            {
                uniqueBooks.AddRange(bookGroup.Value.Take(1));
                bookGroupings[bookGroup.Key] = bookGroup.Value.Skip(1).ToList();
            }

            if (uniqueBooks.Any()) 
                total = uniqueBooks.Count * 8 * _discountRules[uniqueBooks.Count()];

            return bookGroupings.Aggregate(total, (current, group) => current + group.Value.Count() * 8);
        }
    }
}