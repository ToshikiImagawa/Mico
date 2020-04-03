// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using UnityEngine;

namespace Mico.Context.Internal
{
    internal class GameObjectContextService : IGameObjectContextService
    {
        [InjectField(Id = typeof(GameObjectContextService))]
        private IContextRepository _repository = default;

        [InjectField] private GameObjectContextHelper _helper = default;

        public IContext GetGameObjectContextOrDefault(Component component, IContext defaultContext = null)
        {
            var id = _helper.GetInstanceId(component);

            if (_repository.HasContext(id)) return _repository.GetContext(id);
            var context = _helper.GetComponentInParentOnly<IContext>(component) ?? defaultContext;
            _repository.SetContext(id, context);
            return context;
        }
    }
}