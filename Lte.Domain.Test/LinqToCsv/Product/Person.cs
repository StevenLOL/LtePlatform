﻿using Lte.Domain.LinqToCsv;
using Lte.Domain.LinqToCsv.Test;
using NUnit.Framework;

namespace Lte.Domain.Test.LinqToCsv.Product
{
    public class Person : IAssertable<Person>
    {
        [CsvColumn(Name = "Name")]
        public string Name { get; set; }
        [CsvColumn(Name = "Last Name")]
        public string LastName { get; set; }
        [CsvColumn(Name = "Age")]
        public int Age { get; set; }

        public void AssertEqual(Person other)
        {
            Assert.AreEqual(other.Name, Name);
            Assert.AreEqual(other.LastName, LastName);
            Assert.AreEqual(other.Age, Age);
        }
    }
}
