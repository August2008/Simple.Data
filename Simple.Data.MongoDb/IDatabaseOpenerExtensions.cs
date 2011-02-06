using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Data.MongoDb
{
    public static class IDatabaseOpenerExtensions
    {
        public static Database OpenMongo(this IDatabaseOpener opener, string connectionString)
        {
            return opener.Open("MongoDb", new { ConnectionString = connectionString });
        }
    }
}