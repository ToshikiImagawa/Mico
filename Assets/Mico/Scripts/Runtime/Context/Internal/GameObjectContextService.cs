// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using UnityEngine;

namespace Mico.Context.Internal
{
    internal class GameObjectContextService : IGameObjectContextService
    {
        [InjectField] private IGameObjectContextRepository _repository = default;

        public IContext GetGameObjectContextOrDefault(Component component, IContext defaultContext = null)
        {
            var id = component.GetInstanceID();

            if (_repository.HasGameObjectContext(id)) return _repository.GetGameObjectContext(id);
            var context = component.GetComponentInParentOnly<IContext>() ?? defaultContext;
            _repository.SetGameObjectContext(id, context);
            return context;
        }
    }
}