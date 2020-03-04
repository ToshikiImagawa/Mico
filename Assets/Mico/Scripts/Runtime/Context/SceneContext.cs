// Mico.Unity C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using Mico.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mico.Context
{
    [IgnoreInjection]
    public sealed class SceneContext : Context
    {
        [SerializeField, HideInInspector] private string parentScenePath = string.Empty;
        private SceneContext _parentSceneContext;
        private Scene? _parentScene;
#if UNITY_EDITOR
        [SerializeField] private UnityEditor.SceneAsset parentSceneAsset;
        public SceneContext ParentSceneContext => _parentSceneContext;
#endif

        private Scene _scene;
        private DiContainer _container;
        private static readonly Dictionary<int, SceneContext> Instances = new Dictionary<int, SceneContext>();
        private static SceneContext _main;

        private Scene ParentScene
        {
            get
            {
                if (_parentScene.HasValue) return _parentScene.Value;
                _parentScene = SceneManager.GetSceneByPath(parentScenePath);
                return _parentScene.Value;
            }
        }

        public override DiContainer Container
        {
            get
            {
                if (_container != null) return _container;
                if (Instances.ContainsKey(ParentScene.handle))
                {
                    _parentSceneContext = Instances[ParentScene.handle];
                    _container = new DiContainer(_parentSceneContext.Container);
                    return _container;
                }

                _container = new DiContainer();
                return _container;
            }
        }

        public void SetParentScene(Scene scene)
        {
            _parentScene = scene;
        }

        private void Awake()
        {
            _scene = gameObject.scene;
            if (!Instances.ContainsKey(_scene.handle))
            {
                Instances[_scene.handle] = this;
                var contextSceneAll = _scene.GetComponentsInScene<GameObjectContext>();
                foreach (var context in contextSceneAll) context.SetParentContext(this);
                Compile();
                foreach (var context in contextSceneAll) context.Compile();
                Inject();
                return;
            }

            MicoAssert.Throw("Assert!");
        }

        protected override void OnDestroy()
        {
            _container?.Dispose();
            if (!Instances.ContainsKey(_scene.handle)) return;
            if (Instances[_scene.handle] == this)
            {
                Instances.Remove(_scene.handle);
            }
        }

        private void Inject()
        {
            foreach (var component in _scene.GetComponentsInScene<Component>())
            {
                if (component.GetType().GetCustomAttribute<IgnoreInjectionAttribute>() != null) continue;
                var context = component.GetComponentInParent<Context>();
                if (context)
                {
                    try
                    {
                        context.Container.Inject(component);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e, context);
                    }
                }
                else Container.Inject(component);
            }
        }
    }
}