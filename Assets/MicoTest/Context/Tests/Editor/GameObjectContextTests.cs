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
    public class GameObjectContextTests
    {
        private IGameObjectContextService _gameObjectContextServiceMock;

        [SetUp]
        public void Setup()
        {
            var container = new DiContainer();
#if MICO_TEST_ADD_NSUBSTITUTE
            _gameObjectContextServiceMock = Substitute.For<IGameObjectContextService>();
#endif
            container.RegisterInstance<IGameObjectContextService>(_gameObjectContextServiceMock);
            container.Compile();
            ContextContainer.Swap(container);
        }

        [Test]
        public void test_ParentContextでGameObjectContextServiceのGetGameObjectContextOrDefaultがかえること()
        {
            // setup
            var gameObjectContext = new GameObject().AddComponent<GameObjectContext>();
            var contextMock = Substitute.For<IContext>();
            _gameObjectContextServiceMock
                .GetGameObjectContextOrDefault(gameObjectContext, Arg.Any<IContext>())
                .ReturnsForAnyArgs(contextMock);
            // exercise
            var actual = gameObjectContext.ParentContext;
            // verify
            Assert.AreEqual(actual, contextMock);
        }
    }
}