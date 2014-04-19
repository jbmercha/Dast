using System;
using System.Linq;
using Dast;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DastUnitTests
{
    [TestClass]
    public class BinaryMinHeapTests
    {
        [TestMethod]
        public void AddAndRemoveElements()
        {
            const int totalElements = 100000;
            var target = new BinaryMinHeap<int>();
            var r = new Random();
            foreach (var i in Enumerable.Range(1, totalElements).OrderBy(x => r.Next(totalElements)))
            {
                target.Add(i);
            }
            Assert.AreEqual(totalElements, target.Count);
            for (var i = 0; i < totalElements; i++)
            {
                Assert.AreEqual(i + 1, target.RemoveMin());
            }
        }
    }
}
