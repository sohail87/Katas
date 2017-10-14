using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PotterKata
{
    [TestClass]
    public class SpecTests
    {

        [TestMethod]
        public void Buying_1_Potter_Book_Costs_8_Euro()
        {
            var basket = new Basket();
            
            basket.Add(new Book("Harry Potter and the Sorcerer's Stone"));

            Assert.AreEqual(8, basket.GetTotal());
        }
    }

    public class Basket
    {
        private const int SingleBookPrice = 8;

        private readonly List<Book> _basket = new List<Book> {};

        public void Add(Book product)
        {
            _basket.Add(product);
        }

        public int GetTotal()
        {
            var total = 0;
            _basket.ForEach(p => total += SingleBookPrice);
            return total;
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
