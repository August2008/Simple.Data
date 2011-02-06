using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using System.Collections;

namespace Simple.Data.UnitTest
{
    public class ConcreteCollectionTypeCreatorTests
    {
        [TestCase(typeof(HashSet<string>))]
        [TestCase(typeof(ISet<string>))]
        public void TestCanConvertToGenericSet(Type type)
        {
            var items = new[] { "bob", "nancy", "jane" };

            Assert.IsTrue(ConcreteCollectionTypeCreator.IsCollectionType(type));

            object result;
            Assert.IsTrue(ConcreteCollectionTypeCreator.TryCreate(type, items, out result));
            Assert.IsInstanceOf<HashSet<string>>(result);

            var set = result as ISet<string>;
            Assert.IsTrue(set.Contains("bob"));
            Assert.IsTrue(set.Contains("nancy"));
            Assert.IsTrue(set.Contains("jane"));
        }

        [TestCase(typeof(List<string>))]
        [TestCase(typeof(IList<string>))]
        [TestCase(typeof(ICollection<string>))]
        [TestCase(typeof(IEnumerable<string>))]
        public void TestCanConvertToGenericList(Type type)
        {
            var items = new[] { "bob", "nancy", "jane" };

            Assert.IsTrue(ConcreteCollectionTypeCreator.IsCollectionType(type));

            object result;
            Assert.IsTrue(ConcreteCollectionTypeCreator.TryCreate(type, items, out result));
            Assert.IsInstanceOf<List<string>>(result);

            var list = result as List<string>;
            Assert.AreEqual("bob", list[0]);
            Assert.AreEqual("nancy", list[1]);
            Assert.AreEqual("jane", list[2]);
        }

        [TestCase(typeof(ArrayList))]
        [TestCase(typeof(IList))]
        [TestCase(typeof(ICollection))]
        [TestCase(typeof(IEnumerable))]
        public void TestCanConvertToList(Type type)
        {
            var items = new[] { "bob", "nancy", "jane" };

            Assert.IsTrue(ConcreteCollectionTypeCreator.IsCollectionType(type));

            object result;
            Assert.IsTrue(ConcreteCollectionTypeCreator.TryCreate(type, items, out result));
            Assert.IsInstanceOf<ArrayList>(result);

            var list = result as ArrayList;
            Assert.AreEqual("bob", list[0]);
            Assert.AreEqual("nancy", list[1]);
            Assert.AreEqual("jane", list[2]);
        }
    }
}