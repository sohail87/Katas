using System;
using System.Collections;
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

            Assert.AreEqual(GetExpectedTotal(2,0.4), scanner.GetTotal());
        }

        [TestMethod]
        public void Buying_3_Different_Potter_Book_Gets_10Percent_Discount()
        {
            var scanner = new Scanner();

            scanner.Add(1);
            scanner.Add(2);
            scanner.Add(3);

            Assert.AreEqual(GetExpectedTotal(3, 0.8), scanner.GetTotal());
        }
        [TestMethod]
        public void Buying_4_Different_Potter_Book_Gets_20Percent_Discount()
        {
            var scanner = new Scanner();

            scanner.Add(1);
            scanner.Add(2);
            scanner.Add(3);
            scanner.Add(4);
            
            Assert.AreEqual(GetExpectedTotal(4, 1.6), scanner.GetTotal());
        }
        [TestMethod]
        public void Buying_5_Different_Potter_Book_Gets_25Percent_Discount()
        {
            var scanner = new Scanner();

            scanner.Add(1);
            scanner.Add(2);
            scanner.Add(3);
            scanner.Add(4);
            scanner.Add(5);

            Assert.AreEqual(GetExpectedTotal(5, 2), scanner.GetTotal());
        }
        [TestMethod]
        public void Buying_5_Different_Potter_Book_Gets_25Percent_Discount_And_1_Duplicate()
        {
            var scanner = new Scanner();

            scanner.Add(1);
            scanner.Add(2);
            scanner.Add(3);
            scanner.Add(4);
            scanner.Add(5);
            scanner.Add(1);


            Assert.AreEqual(GetExpectedTotal(5, 2)+8, scanner.GetTotal());
        }
        [TestMethod]
        public void Buying_2_Different_Potter_Books_Twice_Gets_5Percent_Discount_Twice()
        {
            var scanner = new Scanner();

            scanner.Add(1);
            scanner.Add(2);
            scanner.Add(1);
            scanner.Add(2);


            Assert.AreEqual(30.4, scanner.GetTotal());
        }

        private static double GetExpectedTotal(int uniqueBooks, double discount)
        {
            double expectedDiscountValue = discount * uniqueBooks;
            int expectedTotalBeforeDiscount = 8 * uniqueBooks;
            double expected = expectedTotalBeforeDiscount - expectedDiscountValue;
            return expected;
        }
    }

    public class Scanner
    {
        private const double SingleBookPrice = 8;

        private readonly Dictionary<int, Book> _books = new Dictionary<int, Book>()
        {
            {1, new Book("Harry Potter and the Sorcerer's Stone")},
            {2, new Book("Harry Potter and the Chamber of Secrets")},
            {3, new Book("Harry Potter and the Prisoner of Azkaban")},
            {4, new Book("Harry Potter and the Goblet of Fire")},
            {5, new Book("Harry Potter and the Order of the Phoenix")},
            {6, new Book("Harry Potter and the Half-Blood Prince")},
            {7, new Book("Harry Potter and the Deathly Hallows")}
        };
        
        private readonly Dictionary<int, int> _discountQuanityPercentages = new Dictionary<int, int>()
        {
            {1, 0},
            {2, 5},
            {3, 10},
            {4, 20},
            {5, 25}
        };


        private readonly Dictionary<int, Book> _basket = new Dictionary<int, Book>();

        private double _total;

        public void Add(int productKey)
        {
            _basket.Add(_basket.Count,_books[productKey]);
        }

        public double GetTotal()
        {
            _total = 0.00;
            _total += SingleBookPrice * _basket.Count();

            CalculateDiscount();

            return _total;
        }


        private void CalculateDiscount()
        {

            Dictionary<int,Book> basket = new Dictionary<int, Book>(_basket); 

            while (basket.Count > 0)
            {
                var distinctBooks = new HashSet<Book>();
                var basketItemsToRemove = new List<int>();
                foreach (var item in basket)
                {
                    if (distinctBooks.Add(item.Value))
                    {
                        basketItemsToRemove.Add(item.Key);
                        
                    }
                }
                foreach (var item in basketItemsToRemove)
                {
                    basket.Remove(item);
                }
                UpdateTotal(distinctBooks.Count);

            }

        }


        private void UpdateTotal(int numberOfBooks)
        {
            var percent = _discountQuanityPercentages[numberOfBooks];
            var percentAsDecimal = (double)percent / 100;
            var discount = percentAsDecimal * SingleBookPrice;
            Console.WriteLine($"Discount: -{discount}");
            _total -= (discount * numberOfBooks);
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