using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PotterKata
{
    [TestClass]
    public class SpecTests
    {
        [TestMethod]
        public void Buying_1_Potter_Book_Costs_8_Euro()
        {
            var basket = new Scanner();
            
            basket.Add(1);

            Assert.AreEqual(8.00, basket.GetTotal());
        }
        [TestMethod]
        public void Buying_2_Different_Potter_Book_Gets_5Percent_Discount()
        {
            var scanner = new Scanner();
            
            scanner.Add(1);
            scanner.Add(2);

            const double expectedDiscountValue = 0.8;
            const int expectedTotalBeforeDiscount = 16;
            const double expected = expectedTotalBeforeDiscount - expectedDiscountValue;
            Assert.AreEqual(expected, scanner.GetTotal());
        }
        [TestMethod]
        public void Buying_3_Different_Potter_Book_Gets_10Percent_Discount()
        {
            var scanner = new Scanner();
            
            scanner.Add(1);
            scanner.Add(2);
            scanner.Add(3);

            const double expectedDiscountValue = 2.4;
            const int expectedTotalBeforeDiscount = 24;
            const double expected = expectedTotalBeforeDiscount - expectedDiscountValue;
            Assert.AreEqual(expected, scanner.GetTotal());
        }
    }

    public class Scanner
    {
        private Dictionary<int, Book> books = new Dictionary<int, Book>()
        {
            {1,new Book("Harry Potter and the Sorcerer's Stone")},
            {2,new Book("Harry Potter and the Chamber of Secrets")},
            {3,new Book("Harry Potter and the Prisoner of Azkaban")},
            {4,new Book("Harry Potter and the Goblet of Fire")},
            {5,new Book("Harry Potter and the Order of the Phoenix")},
            {6,new Book("Harry Potter and the Half-Blood Prince")},
            {7,new Book("Harry Potter and the Deathly Hallows")}
        };
        private const double SingleBookPrice = 8;

        private double GetDiscountAmountPerBook(int percent)
        {
            var percentAsDecimal = (double)percent / 100;
            return percentAsDecimal * SingleBookPrice;
        }

        private readonly List<Book> _basket = new List<Book>();

        private double _total;

        public void Add(int productKey)
        {
            _basket.Add(books[productKey]);
        }

        public double GetTotal()
        {
            _total = 0.00;

            _basket.ForEach(p => _total += SingleBookPrice);

            ApplyDiscount();

            return _total;
        }

        private Dictionary<int,int> DiscountQuanityPercentages = new Dictionary<int, int>()
        {
             {1,0 },{2,5 },{3,10 }, {4,20 }, {5,25 }
        };

        private void ApplyDiscount()
        {
            var uniqueBooks = _basket.Distinct().Count();

            _total -= uniqueBooks * GetDiscountAmountPerBook(DiscountQuanityPercentages[uniqueBooks]);
        }
    }

    public class Book
    {
        public string Title { get; set; }
        public Book(string title)
        {
            Title = title;
        }
    }
}
