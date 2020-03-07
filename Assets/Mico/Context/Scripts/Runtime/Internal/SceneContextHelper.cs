// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mico.Context.Internal
{
    internal class SceneContextHelper : ISceneContextHelper
    {
        public IContext GetParentContext(Component component)
        {
            return component.GetComponentInParent<IContext>();
        }

        public IContext[] GetContextsInScene(Scene scene)
        {
            return scene.GetComponentsInScene<IContext>();
        }

        public Component[] GetComponentsInScene(Scene scene)
        {
            return scene.GetComponentsInScene<Component>();
        }

        public void Inject(DiContainer container, Component component)
        {
            container.Inject(component);
        }
    }
}