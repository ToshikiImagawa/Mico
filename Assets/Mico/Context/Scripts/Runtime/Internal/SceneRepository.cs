// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Mico.Context.Internal
{
    public class SceneRepository : ISceneRepository, IDisposable
    {
        private readonly Dictionary<string, Scene> _sceneCache = new Dictionary<string, Scene>();

        public Scene? GetCacheScene(string scenePath)
        {
            if (_sceneCache.ContainsKey(scenePath)) return _sceneCache[scenePath];

            var scene = SceneManager.GetSceneByPath(scenePath);
            if (!scene.IsValid())
            {
                MicoAssert.Throw($"{scenePath} is invalid Scene!");
                return null;
            }

            _sceneCache[scenePath] = scene;
            return _sceneCache[scenePath];
        }

        public void Dispose()
        {
            _sceneCache.Clear();
        }
    }
}