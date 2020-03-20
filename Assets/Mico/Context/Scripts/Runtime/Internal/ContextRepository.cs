// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using System.Collections.Generic;

namespace Mico.Context.Internal
{
    internal class ContextRepository : IContextRepository, IDisposable
    {
        private readonly Dictionary<int, IContext> _cache = new Dictionary<int, IContext>();

        public bool HasContext(int instanceId)
        {
            return _cache.ContainsKey(instanceId);
        }

        public IContext GetContext(int instanceId)
        {
            return _cache[instanceId];
        }

        public void RemoveContext(int instanceId)
        {
            _cache.Remove(instanceId);
        }

        public bool SetContext(int instanceId, IContext context)
        {
            if (_cache.ContainsKey(instanceId)) return false;
            _cache[instanceId] = context;
            return true;
        }

        public void Dispose()
        {
            _cache.Clear();
        }
    }
}