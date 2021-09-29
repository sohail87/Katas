using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace HarryPotterBooks
{
    public class ScannerTests
    {
        [Theory]
        [InlineData(HP.One, 8)]
        [InlineData(HP.Two, 8)]
        [InlineData(HP.Three, 8)]
        [InlineData(HP.Four, 8)]
        [InlineData(HP.Five, 8)]
        public void BuyingOneBookCostsEightPounds(HP book, int expectedTotal)
        {
            new Scanner().Scan(book).GetTotal().Should().Be(expectedTotal);
        }

        [Theory]
        [InlineData(new[] {HP.One, HP.One}, 16)]
        [InlineData(new[] {HP.Two, HP.Two}, 16)]
        [InlineData(new[] {HP.Three, HP.Three, HP.Three}, 24)]
        [InlineData(new[] {HP.Four, HP.Four, HP.Four, HP.Four}, 32)]
        [InlineData(new[] {HP.Five, HP.Five, HP.Five, HP.Five, HP.Five},
            40)]
        public void BuyingMultipleOfSameBookHasNoDiscount(HP[] books, int expectedTotal)
        {
            ScanBooksAndGetTotal(books).Should().Be(expectedTotal);
        }

        [Theory]
        [InlineData(new[] {HP.One, HP.Two}, 8 * 2 * 0.95)]
        [InlineData(new[] {HP.Two, HP.Five}, 8 * 2 * 0.95)]
        [InlineData(new[] {HP.One, HP.Two, HP.Three}, 8 * 3 * 0.90)]
        [InlineData(new[] {HP.Three, HP.Four, HP.Five}, 8 * 3 * 0.90)]
        [InlineData(new[] {HP.One, HP.Two, HP.Three, HP.Four}, 8 * 4 * 0.80)]
        [InlineData(new[] {HP.Two, HP.Three, HP.Four, HP.Five}, 8 * 4 * 0.80)]
        [InlineData(new[] {HP.One, HP.Two, HP.Three, HP.Four, HP.Five}, 8 * 5 * 0.75)]
        public void BuyingDifferentBooksHasAPercentDiscount(HP[] books, double expectedTotal)
        {
            ScanBooksAndGetTotal(books).Should().Be(expectedTotal);
        }

        [Theory]
        [InlineData(new[] {HP.One, HP.Two, HP.One}, 8 * 2 * 0.95 + 8)]
        [InlineData(new[] {HP.One, HP.Two, HP.One, HP.Two}, 2 * (8 * 2 * 0.95))]
        [InlineData(new[] {HP.One, HP.One, HP.Two, HP.Three, HP.Three, HP.Four}, 8 * 4 * 0.8 + 8 * 2 * 0.95)]
        [InlineData(new[] {HP.One, HP.Two, HP.Two, HP.Three, HP.Four, HP.Five}, 8 + 8 * 5 * 0.75)]
        public void BuyingSeveralDifferentBooks(HP[] books, double expectedTotal)
        {
            ScanBooksAndGetTotal(books).Should().Be(expectedTotal);
        }

        [Theory]
        [InlineData(new[] {HP.One, HP.One, HP.Two, HP.Two, HP.Three, HP.Three, HP.Four, HP.Five}
            , 2 * (8 * 4 * 0.8)
        )]
        [InlineData(new[]
            {
                HP.One, HP.One, HP.One, HP.One, HP.One,
                HP.Two, HP.Two, HP.Two, HP.Two, HP.Two,
                HP.Three, HP.Three, HP.Three, HP.Three,
                HP.Four, HP.Four, HP.Four, HP.Four, HP.Four,
                HP.Five, HP.Five, HP.Five, HP.Five
            }, 3 * (8 * 5 * 0.75) + 2 * (8 * 4 * 0.8)
        )]
        public void EdgeCases(HP[] books, double expectedTotal)
        {
            ScanBooksAndGetTotal(books).Should().Be(expectedTotal);
        }

        private static double ScanBooksAndGetTotal(HP[] books)
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

    public enum HP
    {
        One,
        Two,
        Three,
        Four,
        Five
    }

    public class Scanner
    {
        private const int BookPrice = 8;
        private readonly List<HP> _books = new List<HP>();

        private readonly Dictionary<int, double> _discountRules = new Dictionary<int, double>
        {
            {0, 1},
            {1, 1},
            {2, 0.95},
            {3, 0.90},
            {4, 0.80},
            {5, 0.75}
        };

        public Scanner Scan(HP book)
        {
            _books.Add(book);
            return this;
        }

        public double GetTotal()
        {
            var discountBags = new Dictionary<int, List<HP>>();
            var booksGroupedByTitle = _books.GroupBy(b => b).ToDictionary(k => k.Key, v => v.ToList());

            foreach (var sameTitleBooks in booksGroupedByTitle)
            foreach (var sameTitleBookItem in sameTitleBooks.Value.Select((value, index) => new {index, value}))
            {
                if (discountBags.ContainsKey(sameTitleBookItem.index)) discountBags[sameTitleBookItem.index].Add(sameTitleBookItem.value);
                else discountBags[sameTitleBookItem.index] = new List<HP> {sameTitleBookItem.value};
            }

            return discountBags.Sum(bag => bag.Value.Count * BookPrice * _discountRules[bag.Value.Count]);
        }
    }
}