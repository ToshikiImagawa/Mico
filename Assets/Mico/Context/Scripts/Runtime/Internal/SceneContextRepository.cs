// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using System.Collections.Generic;

namespace Mico.Context.Internal
{
    internal class SceneContextRepository : ISceneContextRepository, IDisposable
    {
        private readonly Dictionary<int, IContext> _cache = new Dictionary<int, IContext>();

        public bool HasSceneContext(int handle)
        {
            return _cache.ContainsKey(handle);
        }

        public IContext GetSceneContext(int handle)
        {
            return _cache[handle];
        }

        public void RemoveSceneContext(int handle)
        {
            _cache.Remove(handle);
        }

        public bool SetSceneContext(int handle, IContext context)
        {
            if (_cache.ContainsKey(handle)) return false;
            _cache[handle] = context;
            return true;
        }

        public void Dispose()
        {
            _cache.Clear();
        }
    }
}