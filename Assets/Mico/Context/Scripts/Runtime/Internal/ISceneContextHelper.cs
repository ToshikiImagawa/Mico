// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mico.Context.Internal
{
    internal interface ISceneContextHelper
    {
        IContext GetParentContext(Component component);
        IContext[] GetContextsInScene(Scene scene);
        Component[] GetComponentsInScene(Scene scene);
        void Inject(DiContainer container, Component component);
    }
}