using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var seed = new Random().Next();
            var r = new Random(seed);
            var items = Enumerable.Range(1, 1000000).Select(x => r.Next()).Distinct().ToArray();
            var target = new HashTableWithLinearProbing<int, string>();
            var sw = new Stopwatch();
            sw.Start();
            foreach (var i in items)
            {
                target[i] = i.ToString();
            }
            Assert.AreEqual(items.Length, target.Count);
            foreach (var i in items)
            {
                Assert.AreEqual(i.ToString(), target[i]);
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            r = new Random(seed);
            var t2 = new Dictionary<int, string>();
            sw = new Stopwatch();
            sw.Start();
            foreach (var i in items)
            {
                t2[i] = i.ToString();
            }
            Assert.AreEqual(items.Length, t2.Count);
            foreach (var i in items)
            {
                Assert.AreEqual(i.ToString(), t2[i]);
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
    }
}
