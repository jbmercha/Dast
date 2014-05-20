using System;
using System.Collections.Generic;
using System.Linq;
using Dast;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DastUnitTests
{
    [TestClass]
    public class StackUnitTests
    {
        [TestMethod]
        public void ArrayStackRandomOperations()
        {
            var target = new ArrayStack<int>();
            RandomOperations(target);
        }

       
        public void RandomOperations(IStack<int> target)
        {
            var r = new Random();
            var frameworkStack = new Stack<int>();
            var operations = Enumerable.Range(0, 100000).Select(x => r.Next(2)).ToArray();
            foreach (var operation in operations)
            {
                int oldCount = target.Count;
                switch (operation)
                {
                    case 0: // Add
                        var newItem = r.Next();
                        target.Push(newItem);
                        frameworkStack.Push(newItem);
                        Assert.AreEqual(oldCount + 1, target.Count);
                        break;
                    case 1: // Remove
                        if (oldCount == 0) goto case 0;
                        var item = target.Pop();
                        var frameworkItem = frameworkStack.Pop();
                        Assert.AreEqual(oldCount - 1, target.Count);
                        Assert.AreEqual(frameworkItem, item);
                        break;
                }
            }
        }
    }
}
