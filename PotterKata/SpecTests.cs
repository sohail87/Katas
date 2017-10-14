﻿using System.Collections.Generic;
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

            var expectedDiscountValue = .8;
            var expectedTotalBeforeDiscount = 16;
            var expected = expectedTotalBeforeDiscount - expectedDiscountValue;

            Assert.AreEqual(expected, scanner.GetTotal());
        }
        [TestMethod]
        public void Buying_3_Different_Potter_Book_Gets_10Percent_Discount()
        {
            var scanner = new Scanner();
            
            scanner.Add(1);
            scanner.Add(2);
            scanner.Add(3);

            var expectedDiscountValue = 2.4;
            var expectedTotalBeforeDiscount = 24;
            var expected = expectedTotalBeforeDiscount - expectedDiscountValue;

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

        private double GetDiscountValueForPercent(int percent)
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

        private void ApplyDiscount()
        {
            var uniqueList = _basket.Distinct();

            if (uniqueList.Count() == 3)
            {
                _total -= uniqueList.Count() * GetDiscountValueForPercent(10);
            }
            else if (uniqueList.Count() == 2)
            {
                _total -= uniqueList.Count() * GetDiscountValueForPercent(5);
            }    
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
