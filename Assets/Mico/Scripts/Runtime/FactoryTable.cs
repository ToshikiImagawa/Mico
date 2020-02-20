// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Mico
{
    internal class FactoryTable
    {
        private readonly FactoryTuple[][] _factoryTable;
        private readonly int _indexFor;

        private FactoryTable(FactoryTuple[] values)
        {
            if (values == null || values.Length == 0)
            {
                _indexFor = 1;
                _factoryTable = new FactoryTuple[0][];
                return;
            }

            var tupleGroup = values.GroupBy(value => value.GetHashCode()).ToArray();
            _indexFor = Math.Max(tupleGroup.Length - 1, 1);
            var maxIndex = tupleGroup.Max(tuple => tuple.Key % _indexFor);
            _factoryTable = new FactoryTuple[maxIndex + 1][];
            foreach (var factoryTuple in values)
            {
                var hash = factoryTuple.GetHashCode();
                var array = _factoryTable[hash % _indexFor];
                if (array == null)
                {
                    array = new FactoryTuple[1];
                    array[0] = factoryTuple;
                }
                else
                {
                    var newArray = new FactoryTuple[array.Length + 1];
                    Array.Copy(array, newArray, array.Length);
                    array = newArray;
                    array[array.Length - 1] = factoryTuple;
                }

                _factoryTable[hash % _indexFor] = array;
            }
        }

        public bool TryGet(Type type, out Func<object> factory)
        {
            return TryGet(type, typeof(DefaultId), out factory);
        }

        public Func<object> Get(Type type)
        {
            if (TryGet(type, out var factory)) return factory;
            throw new KeyNotFoundException($"Type or ID was not found. : Type = {type.FullName}");
        }

        public bool TryGet(Type type, object id, out Func<object> factory)
        {
            var hash = Util.Reflection.HashCodeCombine(id, type);
            var index = hash % _indexFor;
            if (_factoryTable.Length > index)
            {
                var array = _factoryTable[index];
                foreach (var factoryTuple in array)
                {
                    if (factoryTuple.Type != type || !factoryTuple.Id.Equals(id)) continue;
                    factory = factoryTuple.Factory;
                    return true;
                }
            }

            factory = null;
            return false;
        }

        public Func<object> Get(Type type, object id)
        {
            if (TryGet(type, id, out var factory)) return factory;
            throw new KeyNotFoundException($"Type or ID was not found. : Type = {type.FullName}, ID = {id}");
        }

        public static FactoryTable Create(FactoryTuple[] values)
        {
            return new FactoryTable(values);
        }
    }

    internal struct FactoryTuple
    {
        public readonly object Id;
        public readonly Type Type;
        public readonly Func<object> Factory;

        private FactoryTuple(object id, Type type, Func<object> factory)
        {
            Id = id;
            Type = type;
            Factory = factory;
        }

        private FactoryTuple(Type type, Func<object> factory)
        {
            Id = typeof(DefaultId);
            Type = type;
            Factory = factory;
        }

        public override int GetHashCode()
        {
            return Util.Reflection.HashCodeCombine(Id, Type);
        }

        public static FactoryTuple Create(Type type, Func<object> factory)
        {
            return new FactoryTuple(type, factory);
        }

        public static FactoryTuple Create(Type type, object id, Func<object> factory)
        {
            return new FactoryTuple(id, type, factory);
        }

        public static FactoryTuple Create<TValue>(Func<TValue> factory) where TValue : class
        {
            return new FactoryTuple(typeof(TValue), factory);
        }

        public static FactoryTuple Create<TValue>(object id, Func<TValue> factory) where TValue : class
        {
            return new FactoryTuple(id, typeof(TValue), factory);
        }
    }

    internal class DefaultId
    {
        public override string ToString()
        {
            return "<Default ID>";
        }
    }
}