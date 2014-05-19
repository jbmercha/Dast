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
        public void LinearProbingAddAndRetrieveElements()
        {
            var target = new HashTableWithLinearProbing<int, string>();
            AddAndRetrieveElements(target);
        }

        [TestMethod]
        public void LinearProbingRandomOperations()
        {
            var target = new HashTableWithLinearProbing<int, string>();
            RandomOperations(target);
        }

        [TestMethod]
        public void ChainingAddAndRetrieveElements()
        {
            var target = new HashTableWithChaining<int, string>();
            AddAndRetrieveElements(target);
        }

        [TestMethod]
        public void ChainingRandomOperations()
        {
            var target = new HashTableWithChaining<int, string>();
            RandomOperations(target);
        }


        public void AddAndRetrieveElements(IHashTable<int, string> target)
        {
            var seed = new Random().Next();
            var r = new Random(seed);
            var items = Enumerable.Range(1, 1000000).Select(x => r.Next()).Distinct().Select(x => new { Key = x, Value = x.ToString()}) .ToArray();
            var sw = new Stopwatch();
            sw.Start();
            foreach (var i in items)
            {
                target[i.Key] = i.Value;
            }
            Assert.AreEqual(items.Length, target.Count);
            foreach (var i in items)
            {
                Assert.AreEqual(i.Value, target[i.Key]);
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            r = new Random(seed);
            var t2 = new Dictionary<int, string>();
            sw = new Stopwatch();
            sw.Start();
            foreach (var i in items)
            {
                t2[i.Key] = i.Value;
            }
            Assert.AreEqual(items.Length, t2.Count);
            foreach (var i in items)
            {
                Assert.AreEqual(i.Value, t2[i.Key]);
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }

        public void RandomOperations(IHashTable<int, string> target)
        {
            var r = new Random(123);
            const int totalOperations = 10000;
            var itemsInTarget = new List<int>();

            for (var i = 0; i < totalOperations; i++)
            {
                var oldCount = target.Count;
                switch (r.Next(2))
                {
                    case 0: // Add
                        var newItem = r.Next();
                        target[newItem] = newItem.ToString();
                        itemsInTarget.Add(newItem);
                        Assert.AreEqual(oldCount + 1, target.Count);
                        Assert.AreEqual(newItem.ToString(), target[newItem]);
                        break;
                    case 1: // Remove by index
                        if (oldCount == 0) goto case 0;
                        var indexToRemove = r.Next(itemsInTarget.Count - 1);
                        var itemToRemove = itemsInTarget[indexToRemove];
                        target.Remove(itemToRemove);
                        itemsInTarget.RemoveAt(indexToRemove);
                        Assert.AreEqual(oldCount - 1, target.Count);
                        break;
                }
            }
        }
    }
}
