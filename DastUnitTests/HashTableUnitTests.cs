using System;
using System.Linq;
using Dast;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DastUnitTests
{
    [TestClass]
    public class HashTableUnitTests
    {
        [TestMethod]
        public void AddAndRetrieveItems()
        {
            const int totalElements = 10000;
            var target = new HashTable<int, string>();
            foreach (var i in Enumerable.Range(1, totalElements).OrderBy(x => Guid.NewGuid()))
            {
                target[i] = i.ToString();
            }
            Assert.AreEqual(totalElements, target.Count);

            for (var i = 0; i < totalElements; i++)
            {
                Assert.AreEqual((i + 1).ToString(), target[i + 1]);
            }
        }
    }
}
