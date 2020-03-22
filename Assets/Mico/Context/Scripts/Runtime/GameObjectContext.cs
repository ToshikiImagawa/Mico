// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using Mico.Context.Internal;
using UnityEngine;

namespace Mico.Context
{
    public sealed class GameObjectContext : Context
    {
        [SerializeField] private Context parentContext;

        private readonly Lazy<IGameObjectContextService> _serviceLazy =
            new Lazy<IGameObjectContextService>(ContextContainer.Resolve<IGameObjectContextService>);

        internal override IContext ParentContext =>
            _serviceLazy.Value.GetGameObjectContextOrDefault(this, parentContext);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            parentContext = null;
        }
    }
}