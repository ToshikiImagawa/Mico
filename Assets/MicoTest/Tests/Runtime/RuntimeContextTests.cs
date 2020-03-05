// MicoTest C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using System.Collections;
using System.Threading.Tasks;
using Mico;
using Mico.Context;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;
using Object = UnityEngine.Object;

namespace MicoTest
{
    public class RuntimeContextTests
    {
        private GameObjectContext _gameObjectContext;
        private GameObjectContext _childContext;
        private SceneContext _sceneContext;

        [SetUp]
        public void SetUp()
        {
            // setup
            _gameObjectContext = new GameObject("GameObjectContext").AddComponent<GameObjectContext>();
            _childContext = new GameObject("ChildContext").AddComponent<GameObjectContext>();
            _childContext.transform.SetParent(_gameObjectContext.transform);
            _sceneContext = new GameObject("SceneContext").AddComponent<SceneContext>();
        }

        [TearDown]
        public void TearDown()
        {
            // teardown
            Object.DestroyImmediate(_sceneContext.gameObject);
            Object.DestroyImmediate(_gameObjectContext.gameObject);
        }

        private IEnumerator WaitCompiled()
        {
            while (!_sceneContext.Container.IsCompiled) yield return null;
        }

        [UnityTest]
        public IEnumerator test_Awake時にGameObjectContextとSceneContextはCompileされること()
        {
            // exercise
            yield return WaitCompiled();
            // verify
            Assert.IsTrue(_sceneContext.Container.IsCompiled);
            Assert.IsTrue(_gameObjectContext.Container.IsCompiled);
            Assert.IsTrue(_childContext.Container.IsCompiled);
        }

        [UnityTest]
        public IEnumerator test_GameObjectContextは指定がないとき親のGameObjectContextがParentContextになること()
        {
            // exercise
            yield return WaitCompiled();
            // verify
            Assert.AreEqual(_childContext.ParentContext, _gameObjectContext);
        }

        [UnityTest]
        public IEnumerator test_SceneContextを同シーンに複数生成するとエラーになること()
        {
            // setup
            Exception exception = null;
            // exercise
            yield return WaitCompiled();
            try
            {
                var context = new GameObject("SceneContext").AddComponent<SceneContext>();
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.IsNotNull(exception);
        }
    }
}