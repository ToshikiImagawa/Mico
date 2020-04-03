// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mico.Context.Internal
{
    internal class SceneContextHelper
    {
        public virtual IContext GetParentContext(Component component)
        {
            return component.GetComponentInParent<IContext>();
        }

        public virtual IContext[] GetContextsInScene(Scene scene)
        {
            return scene.GetComponentsInScene<IContext>();
        }

        public virtual Component[] GetComponentsInScene(Scene scene)
        {
            return scene.GetComponentsInScene<Component>();
        }

        public virtual void Inject(DiContainer container, Component component)
        {
            container.Inject(component);
        }
    }
}