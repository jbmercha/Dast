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
            var r = new Random();
            const int totalElements = 10000;
            var elements = Enumerable.Range(1, totalElements).Select(x => r.Next(totalElements)).ToList();
            var target = new BinaryMinHeap<int>();
            foreach (var i in elements)
            {
                target.Add(i);
            }
            Assert.AreEqual(totalElements, target.Count);
            elements.Sort();
            for (var i = 0; i < totalElements; i++)
            {
                Assert.AreEqual(elements[i], target.RemoveMin());
            }
        }
    }
}
