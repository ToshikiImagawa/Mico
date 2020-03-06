// Mico.Unity C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using Mico.Context.Internal;
using Mico.Internal;
using UnityEngine;

namespace Mico.Context
{
    public sealed class SceneContext : Context
    {
        [SerializeField, HideInInspector] private string parentScenePath = string.Empty;

        private readonly Lazy<ISceneContextService> _serviceLazy =
            new Lazy<ISceneContextService>(ContextContainer.Resolve<ISceneContextService>);

#if UNITY_EDITOR
        [SerializeField] private UnityEditor.SceneAsset parentSceneAsset;
        public SceneContext ParentSceneContext => ParentContext as SceneContext;
#endif

        internal override IContext ParentContext => _serviceLazy.Value.GetSceneContextOrDefault(parentScenePath);

        private void Awake()
        {
            if (!_serviceLazy.Value.Boot(gameObject.scene, this, parentScenePath))
            {
                MicoAssert.Throw("Assert!");
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _serviceLazy.Value.RemoveSceneContext(gameObject.scene);
            parentScenePath = null;
        }
    }
}