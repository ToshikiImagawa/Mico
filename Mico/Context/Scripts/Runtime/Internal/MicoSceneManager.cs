// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using UnityEngine.SceneManagement;

namespace Mico.Context.Internal
{
    public class MicoSceneManager
    {
        public virtual bool GetSceneByPath(string scenePath, out Scene scene)
        {
            scene = SceneManager.GetSceneByPath(scenePath);
            return scene.IsValid();
        }
    }
}