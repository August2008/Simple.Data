using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Simple.Data.Extensions;

namespace Simple.Data
{
    internal class ConcreteTypeCreator
    {
        private static readonly ConcurrentDictionary<Type, ConcreteTypeCreator> Cache =
            new ConcurrentDictionary<Type, ConcreteTypeCreator>();

        private readonly Type _concreteType;

        private ConcreteTypeCreator(Type concreteType)
        {
            _concreteType = concreteType;
        }

        public Type ConcreteType
        {
            get { return _concreteType; }
        }

        public static ConcreteTypeCreator Get(Type concreteType)
        {
            return Cache.GetOrAdd(concreteType, type => new ConcreteTypeCreator(type));
        }

        public bool TryCreate(IDictionary<string, object> data, out object result)
        {
            return TryCreate(_concreteType, data, out result);
        }

        private bool TryCreate(Type concreteType, IDictionary<string, object> data, out object result)
        {
            bool anyPropertiesSet = false;
            object obj = Activator.CreateInstance(concreteType);
            object value;
            foreach (var propertyInfo in concreteType.GetProperties().Where(pi => CanSetProperty(pi, data)))
            {
                value = data[propertyInfo.Name.Homogenize()];
                var subData = value as IDictionary<string, object>;
                if (subData != null)
                {
                    if (!TryCreate(propertyInfo.PropertyType, subData, out value))
                        continue;
                }
                propertyInfo.SetValue(obj, value, null);
                anyPropertiesSet = true;
            }

            result = anyPropertiesSet ? obj : null;

            return anyPropertiesSet;
        }

        private static bool CanSetProperty(PropertyInfo propertyInfo, IDictionary<string, object> data)
        {
            return data.ContainsKey(propertyInfo.Name.Homogenize()) &&
                   !(propertyInfo.PropertyType.IsValueType && data[propertyInfo.Name.Homogenize()] == null);
        }
    }
}