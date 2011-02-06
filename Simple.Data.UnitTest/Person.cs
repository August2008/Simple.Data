using System;
using System.Collections.Generic;

namespace Simple.Data.UnitTest
{
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Address Address { get; set; }

        public List<string> Emails { get; set; }

        public IList<Dependent> Dependents { get; set; }
    }

    class Address
    {
        public string Line { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }

    class Dependent
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
