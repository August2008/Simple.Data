﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Data.Schema
{
    class Key
    {
        private readonly string[] _columns;
        public Key(string[] columns)
        {
            _columns = columns;
        }

        public string this[int index]
        {
            get { return _columns[index]; }
        }

        public int Length
        {
            get { return _columns.Length; }
        }
    }
}