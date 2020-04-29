// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mico.Context.Internal
{
    internal class SceneContextService : ISceneContextService
    {
        [InjectField(Id = typeof(SceneContextService))]
        private IContextRepository _sceneContextRepository = default;

        [InjectField] private ISceneRepository _sceneRepository = default;
        [InjectField] private SceneContextHelper _helper = default;

        public bool Boot(Scene scene, IContext sceneContext, string scenePath = null)
        {
            if (!_sceneContextRepository.SetContext(scene.handle, sceneContext)) return false;
            var contextSceneAll = _helper.GetContextsInScene(scene).Where(_ => _ != sceneContext).ToArray();
            if (string.IsNullOrEmpty(scenePath))
            {
                sceneContext.SetContainer(new DiContainer());
            }
            else
            {
                var context = GetSceneContextOrDefault(scenePath);
                sceneContext.SetContainer(context == null ? new DiContainer() : new DiContainer(context.Container));
            }

            foreach (var context in contextSceneAll)
            {
                context.SetContainer(new DiContainer((context.ParentContext ?? sceneContext).Container));
            }

            foreach (var context in new[] {sceneContext}.Concat(contextSceneAll))
            {
                foreach (var installer in context.Installers)
                {
                    installer?.InstallRegisters(context.Container);
                }

                context.Container.Compile();
            }

            foreach (var component in _helper.GetComponentsInScene(scene))
            {
                if (component.GetType().GetCustomAttribute<IgnoreInjectionAttribute>() != null) continue;
                var parentContext = _helper.GetParentContext(component) ?? sceneContext;
                try
                {
                    _helper.Inject(parentContext.Container, component);
                }
                catch (Exception e)
                {
                    if (parentContext is Component instance)
                    {
                        Debug.LogException(e, instance);
                    }
                    else
                    {
                        Debug.LogException(e);
                    }
                }
            }

            return true;
        }

        public void RemoveSceneContext(Scene scene)
        {
            _sceneContextRepository.RemoveContext(scene.handle);
        }

        public IContext GetSceneContextOrDefault(string scenePath)
        {
            if (string.IsNullOrEmpty(scenePath)) return null;
            var parentScene = _sceneRepository.GetCacheScene(scenePath);
            if (!parentScene.HasValue) return null;
            var handle = parentScene.Value.handle;
            return _sceneContextRepository.HasContext(handle)
                ? _sceneContextRepository.GetContext(handle)
                : null;
        }
    }
}