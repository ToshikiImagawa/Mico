// MicoContextTest C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using Mico;
using Mico.Context;
using Mico.Context.Internal;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace MicoContextTest
{
    public class SceneContextTests
    {
        private ISceneContextService _sceneContextServiceMock;

        [SetUp]
        public void Setup()
        {
            var container = new DiContainer();
#if MICO_TEST_ADD_NSUBSTITUTE
            _sceneContextServiceMock = Substitute.For<ISceneContextService>();
#endif
            container.RegisterInstance<ISceneContextService>(_sceneContextServiceMock);
            container.Compile();
            ContextContainer.Swap(container);
        }

        [Test]
        public void test_ParentContextでGameObjectContextServiceのGetSceneContextOrDefaultがかえること()
        {
            // setup
            var sceneContext = new GameObject().AddComponent<SceneContext>();
            var context = new GameObject().AddComponent<SceneContext>();
            _sceneContextServiceMock.GetSceneContextOrDefault(Arg.Any<string>())
                .ReturnsForAnyArgs(context);
            // exercise
            var actual = sceneContext.ParentContext;
            // verify
            Assert.AreEqual(actual, context);
        }

        [Test]
        public void test_ParentSceneContextでGameObjectContextServiceのGetSceneContextOrDefaultがかえること()
        {
            // setup
            var sceneContext = new GameObject().AddComponent<SceneContext>();
            var context = new GameObject().AddComponent<SceneContext>();
            _sceneContextServiceMock.GetSceneContextOrDefault(Arg.Any<string>())
                .ReturnsForAnyArgs(context);
            // exercise
            var actual = sceneContext.ParentSceneContext;
            // verify
            Assert.AreEqual(actual, context);
        }
    }
}