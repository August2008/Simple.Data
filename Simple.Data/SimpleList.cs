using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

using Simple.Data.Extensions;

namespace Simple.Data
{
    public class SimpleList : List<object>
    {
        public SimpleList(IEnumerable<object> other)
            : base(other)
        { }

        public dynamic ElementAt(int index)
        {
            return Enumerable.ElementAt(this, index);
        }

        public dynamic ElementAtOrDefault(int index)
        {
            return Enumerable.ElementAtOrDefault(this, index);
        }

        public dynamic First()
        {
            return Enumerable.First(this);
        }

        public dynamic FirstOrDefault()
        {
            return Enumerable.FirstOrDefault(this);
        }

        public dynamic Last()
        {
            return Enumerable.Last(this);
        }

        public dynamic LastOrDefault()
        {
            return Enumerable.LastOrDefault(this);
        }

        public dynamic Single()
        {
            return Enumerable.Single(this);
        }

        public dynamic SingleOrDefault()
        {
            return Enumerable.SingleOrDefault(this);
        }
    }
}