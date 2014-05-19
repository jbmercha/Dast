using System;
using System.Collections.Generic;
using System.Linq;
using Dast;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DastUnitTests
{
    [TestClass]
    public class QueueUnitTests
    {

        [TestMethod]
        public void CircularBufferQueueBasicRandomOperations()
        {
            RandomOperations(new CircularBufferQueue<int>());
        }

        public void RandomOperations(IQueue<int> target)
        {
            var r = new Random();
            var frameworkQueue = new Queue<int>();
            var operations = Enumerable.Range(0, 100000).Select(x => r.Next(2)).ToArray();
            foreach (var operation in operations)
            {
                int oldCount = target.Count;
                switch (operation)
                {
                    case 0: // Add
                        var newItem = r.Next();
                        target.Enqueue(newItem);
                        frameworkQueue.Enqueue(newItem);
                        Assert.AreEqual(oldCount + 1, target.Count);
                        break;
                    case 1: // Remove by index
                        if (oldCount == 0) goto case 0;
                        var item = target.Dequeue();
                        var frameworkItem = frameworkQueue.Dequeue();
                        Assert.AreEqual(oldCount - 1, target.Count);
                        Assert.AreEqual(frameworkItem, item);
                        break;
                }
            }
        }
    }
}
