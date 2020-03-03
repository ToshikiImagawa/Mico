// MicoSample C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using UnityEngine.SceneManagement;

namespace MicoSample
{
    public class MicoSceneLoader
    {
        public MicoSceneLoader(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }
}