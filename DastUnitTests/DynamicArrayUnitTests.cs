﻿using System;
using System.Linq;
using Dast;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DastUnitTests
{
    [TestClass]
    public class DynamicArrayUnitTests
    {
        [TestMethod]
        public void AddElements()
        {
            const int totalElements = 10000;
            var target = new DynamicArray<int>();
            foreach (var i in Enumerable.Range(1, totalElements))
            {
                target.Add(i);
            }
            Assert.AreEqual(totalElements, target.Count);

            for (var i = 0; i < totalElements; i++)
            {
                Assert.AreEqual(i+1, target[i]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void FailIndexToLarge()
        {
            var target = new DynamicArray<int>();
            target.Add(5);
            var badIndex = target[2];
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void FailIndexToSmall()
        {
            var target = new DynamicArray<int>();
            target.Add(5);
            var badIndex = target[-1];
        }

        [TestMethod]
        public void RandomOperation()
        {
            var r = new Random();
            const int totalOperations = 10000;
            var target = new DynamicArray<int>();
            for (var i = 0; i < totalOperations; i++)
            {
                int oldCount = target.Count;
                switch (r.Next(3))
                {
                    case 0: // Add
                        var newItem = r.Next();
                        target.Add(newItem);
                        Assert.AreEqual(oldCount + 1, target.Count);
                        Assert.AreEqual(newItem, target[oldCount]);
                        break;
                    case  1: // Remove by index
                        if(oldCount == 0) goto case 0;
                        var indexToRemove = r.Next(oldCount - 1);
                        target.RemoveAt(indexToRemove);
                        Assert.AreEqual(oldCount - 1, target.Count);
                        break;
                    case 2: // Remove by element
                        if (oldCount == 0) goto case 0;
                        var itemToRemove = target[r.Next(oldCount - 1)];
                        target.Remove(itemToRemove);
                        Assert.AreEqual(oldCount - 1, target.Count);
                        break;
                }
            }
        }
    }
}
