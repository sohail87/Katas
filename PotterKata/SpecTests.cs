using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PotterKata
{
    [TestClass]
    public class SpecTests
    {
        /*
"Harry Potter and the Sorcerer's Stone"
"Harry Potter and the Chamber of Secrets"
"Harry Potter and the Prisoner of Azkaban"
"Harry Potter and the Goblet of Fire"
"Harry Potter and the Order of the Phoenix"
"Harry Potter and the Half-Blood Prince"
"Harry Potter and the Deathly Hallows"
*/
        [TestMethod]
        public void Buying_1_Potter_Book_Costs_8_Euro()
        {
            var basket = new Basket();
            
            basket.Add(new Book("Harry Potter and the Sorcerer's Stone"));

            Assert.AreEqual(8.00, basket.GetTotal());
        }
        [TestMethod]
        public void Buying_2_Different_Potter_Book_Gets_5Percent_Discount()
        {
            var basket = new Basket();
            
            basket.Add(new Book("Harry Potter and the Sorcerer's Stone"));
            basket.Add(new Book("Harry Potter and the Chamber of Secrets"));

            Assert.AreEqual(15.2, basket.GetTotal());
        }
    }

    public class Basket
    {
        private const double SingleBookPrice = 8;

        private double GetPercentDiscount(int percent)
        {
            double percentAsDecimal = (double)percent / 100;
            return percentAsDecimal * SingleBookPrice;
        }

        private readonly List<Book> _basket = new List<Book> {};
        private double _total;

        public void Add(Book product)
        {
            _basket.Add(product);
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

            if (uniqueList.Count() == 2)
            {
                _total -= uniqueList.Count() * GetPercentDiscount(5);
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
