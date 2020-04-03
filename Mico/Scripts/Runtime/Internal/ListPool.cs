// Mico.Pool C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System.Collections.Generic;

namespace Mico.Internal
{
    internal class ListPool<T> : IPool<List<T>>
    {
        private readonly Queue<List<T>> _cache = new Queue<List<T>>();

        public List<T> Spawn()
        {
            return _cache.Count > 0 ? _cache.Dequeue() : new List<T>();
        }

        public void Despawn(object value)
        {
            if (value is List<T> registerInfo)
            {
                _cache.Enqueue(registerInfo);
                return;
            }

            MicoAssert.Throw("Assert!");
        }

        public void Clear()
        {
            _cache.Clear();
        }
    }
}