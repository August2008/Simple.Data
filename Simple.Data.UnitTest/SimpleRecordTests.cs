using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SODictionary = System.Collections.Generic.Dictionary<string, object>;

using NUnit.Framework;

namespace Simple.Data.UnitTest
{
    public class SimpleRecordTests
    {
        private dynamic _record;

        [SetUp]
        public void Setup()
        {
            _record = new SimpleRecord(
                new SODictionary
                {
                    { "Name", "Bob" },
                    { "Age", 32 },
                    { "Address", new SimpleRecord(new SODictionary { {"Line", "123 Way" }, {"City", "Dallas"}, {"State", "TX" }}) },
                    { "Emails", new SimpleList(new List<object> { "b@b.com", "bob@bob.com" }) },
                    { "Dependents", new SimpleList(new List<SimpleRecord> 
                        {
                            new SimpleRecord(new SODictionary { { "Name", "Alice" }, {"Age", 12 } }),
                            new SimpleRecord(new SODictionary { { "Name", "John" }, {"Age", 9 } })
                        })
                    },
                });
        }

        [Test]
        public void TestImplicitCastFromRoot()
        {
            Person person = _record;

            Assert.AreEqual("Bob", person.Name);
            Assert.AreEqual(32, person.Age);

            Assert.AreEqual("123 Way", person.Address.Line);
            Assert.AreEqual("Dallas", person.Address.City);
            Assert.AreEqual("TX", person.Address.State);

            Assert.AreEqual(2, person.Emails.Count);
            Assert.AreEqual("b@b.com", person.Emails[0]);
            Assert.AreEqual("bob@bob.com", person.Emails[1]);

            Assert.AreEqual(2, person.Dependents.Count);
            Assert.AreEqual("Alice", person.Dependents[0].Name);
            Assert.AreEqual(12, person.Dependents[0].Age);
            Assert.AreEqual("John", person.Dependents[1].Name);
            Assert.AreEqual(9, person.Dependents[1].Age);
        }

        [Test]
        public void TestDynamicAccessFromRoot()
        {
            var person = _record;

            Assert.AreEqual("Bob", person.Name);
            Assert.AreEqual(32, person.Age);

            Assert.AreEqual("123 Way", person.Address.Line);
            Assert.AreEqual("Dallas", person.Address.City);
            Assert.AreEqual("TX", person.Address.State);

            Assert.AreEqual(2, person.Emails.Count);
            Assert.AreEqual("b@b.com", person.Emails[0]);
            Assert.AreEqual("bob@bob.com", person.Emails[1]);

            Assert.AreEqual(2, person.Dependents.Count);
            Assert.AreEqual("Alice", person.Dependents[0].Name);
            Assert.AreEqual(12, person.Dependents[0].Age);
            Assert.AreEqual("John", person.Dependents[1].Name);
            Assert.AreEqual(9, person.Dependents[1].Age);
        }

    }
}