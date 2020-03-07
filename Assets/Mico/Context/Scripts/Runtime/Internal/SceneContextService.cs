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
        [InjectField] private ISceneContextRepository _sceneContextRepository = default;
        [InjectField] private ISceneRepository _sceneRepository = default;
        [InjectField] private ISceneContextHelper _helper = default;

        public bool Boot(Scene scene, IContext sceneContext, string scenePath = null)
        {
            if (!_sceneContextRepository.SetSceneContext(scene.handle, sceneContext)) return false;
            var contextSceneAll = _helper.GetContextsInScene(scene).Where(_ => _ != sceneContext).ToArray();
            if (string.IsNullOrEmpty(scenePath))
            {
                sceneContext.SetContainer(new DiContainer());
            }
            else
            {
                var parentScene = GetScene(scenePath);
                sceneContext.SetContainer(_sceneContextRepository.HasSceneContext(parentScene.handle)
                    ? new DiContainer(_sceneContextRepository.GetSceneContext(parentScene.handle).Container)
                    : new DiContainer());
            }

            foreach (var context in contextSceneAll)
            {
                SetContainer(context, context.ParentContext ?? sceneContext);
            }

            foreach (var context in new[] {sceneContext}.Concat(contextSceneAll))
            {
                Compile(context);
            }

            Inject(sceneContext, _helper.GetComponentsInScene(scene));
            return true;
        }

        public void RemoveSceneContext(Scene scene)
        {
            _sceneContextRepository.RemoveSceneContext(scene.handle);
        }

        public IContext GetSceneContextOrDefault(string scenePath)
        {
            var parentScene = GetScene(scenePath);
            return !parentScene.IsValid() ? null : GetSceneContextOrDefault(parentScene);
        }

        private IContext GetSceneContextOrDefault(Scene scene)
        {
            return _sceneContextRepository.HasSceneContext(scene.handle)
                ? _sceneContextRepository.GetSceneContext(scene.handle)
                : null;
        }

        private Scene GetScene(string path)
        {
            return _sceneRepository.GetCacheScene(path);
        }

        private void SetContainer(IContext context, IContext parentContext)
        {
            context.SetContainer(parentContext != null ? new DiContainer(parentContext.Container) : new DiContainer());
        }

        private void Compile(IContext context)
        {
            foreach (var installer in context.Installers)
            {
                installer?.InstallRegisters(context.Container);
            }

            context.Container.Compile();
        }

        private void Inject(IContext context, IEnumerable<Component> componentsInScene)
        {
            foreach (var component in componentsInScene)
            {
                if (component.GetType().GetCustomAttribute<IgnoreInjectionAttribute>() != null) continue;
                var parentContext = _helper.GetParentContext(component) ?? context;
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
        }
    }
}