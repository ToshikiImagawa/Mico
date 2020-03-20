// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using UnityEngine;

namespace Mico.Context.Internal
{
    internal class GameObjectContextService : IGameObjectContextService
    {
        [InjectField] private IGameObjectContextRepository _repository = default;
        [InjectField] private IGameObjectContextHelper _helper = default;

        public IContext GetGameObjectContextOrDefault(Component component, IContext defaultContext = null)
        {
            var id = _helper.GetInstanceId(component);

            if (_repository.HasGameObjectContext(id)) return _repository.GetGameObjectContext(id);
            var context = _helper.GetComponentInParentOnly<IContext>(component) ?? defaultContext;
            _repository.SetGameObjectContext(id, context);
            return context;
        }
    }
}