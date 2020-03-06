// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System.Collections.Generic;

namespace Mico.Context.Internal
{
    internal class GameObjectContextRepository : IGameObjectContextRepository
    {
        private readonly Dictionary<int, IContext> _cache = new Dictionary<int, IContext>();

        public bool HasGameObjectContext(int instanceId)
        {
            return _cache.ContainsKey(instanceId);
        }

        public IContext GetGameObjectContext(int instanceId)
        {
            return _cache[instanceId];
        }

        public void RemoveGameObjectContext(int instanceId)
        {
            _cache.Remove(instanceId);
        }

        public bool SetGameObjectContext(int instanceId, IContext context)
        {
            if (_cache.ContainsKey(instanceId)) return false;
            _cache[instanceId] = context;
            return true;
        }
    }
}