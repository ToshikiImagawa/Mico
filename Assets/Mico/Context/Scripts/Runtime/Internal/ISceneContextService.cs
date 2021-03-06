// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using UnityEngine.SceneManagement;

namespace Mico.Context.Internal
{
    internal interface ISceneContextService
    {
        bool Boot(Scene scene, IContext sceneContext, string scenePath = null);
        void RemoveSceneContext(Scene scene);
        IContext GetSceneContextOrDefault(string scenePath);
    }
}