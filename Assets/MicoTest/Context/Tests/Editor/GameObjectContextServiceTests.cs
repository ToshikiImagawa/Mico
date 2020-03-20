// MicoContextTest C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using Mico;
using Mico.Context;
using Mico.Context.Internal;
#if MICO_TEST_ADD_NSUBSTITUTE
using NSubstitute;
#endif
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace MicoContextTest
{
    public class GameObjectContextServiceTests
    {
        private IGameObjectContextRepository _gameObjectContextRepositoryMock;
        private IContext _contextMock;
        private IContext _defaultContextMock;

        [SetUp]
        public void Setup()
        {
            var container = new DiContainer();
#if MICO_TEST_ADD_NSUBSTITUTE
            _gameObjectContextRepositoryMock = Substitute.For<IGameObjectContextRepository>();
            _contextMock = Substitute.For<IContext>();
            _defaultContextMock = Substitute.For<IContext>();
            _contextMock.Container.Returns(new DiContainer());
            _defaultContextMock.Container.Returns(new DiContainer());
#endif
            container.RegisterInstance<IGameObjectContextRepository>(_gameObjectContextRepositoryMock);
            container.RegisterNew<IGameObjectContextService, GameObjectContextService>();
            container.Compile();
            ContextContainer.Swap(container);
        }

#if MICO_TEST_ADD_NSUBSTITUTE
        [Test]
        public void test_GetGameObjectContextOrDefault実行時にHasGameObjectContextがtrueの時GetGameObjectContextの値がかえること()
        {
            // setup
            var gameObjectContextService = ContextContainer.Resolve<IGameObjectContextService>();
            _gameObjectContextRepositoryMock
                .HasGameObjectContext(Arg.Any<int>())
                .ReturnsForAnyArgs(true);
            _gameObjectContextRepositoryMock
                .GetGameObjectContext(Arg.Any<int>())
                .ReturnsForAnyArgs(_contextMock);
            var component = new GameObject().AddComponent<GameObjectContext>();
            // exercise
            var gameObjectContext =
                gameObjectContextService.GetGameObjectContextOrDefault(component, _defaultContextMock);
            // verify
            Assert.AreEqual(gameObjectContext, _contextMock);
        }
        
        [Test]
        public void test_GetGameObjectContextOrDefault実行時にHasGameObjectContextがfalse且つParentComponentにIContextがある時ParentComponentのIContextがかえること()
        {
            // setup
            var gameObjectContextService = ContextContainer.Resolve<IGameObjectContextService>();
            _gameObjectContextRepositoryMock
                .HasGameObjectContext(Arg.Any<int>())
                .ReturnsForAnyArgs(false);
            _gameObjectContextRepositoryMock
                .GetGameObjectContext(Arg.Any<int>())
                .ReturnsForAnyArgs(_contextMock);
            var component = new GameObject().AddComponent<GameObjectContext>();
            var parentComponent = new GameObject().AddComponent<GameObjectContext>();
            component.transform.SetParent(parentComponent.transform);
            // exercise
            var gameObjectContext =
                gameObjectContextService.GetGameObjectContextOrDefault(component, _defaultContextMock);
            // verify
            Assert.AreEqual(gameObjectContext, parentComponent);
        }

        [Test]
        public void test_GetGameObjectContextOrDefault実行時にHasGameObjectContextがfalse且つParentComponentにIContextがない時defaultContextの値がかえること()
        {
            // setup
            var gameObjectContextService = ContextContainer.Resolve<IGameObjectContextService>();
            _gameObjectContextRepositoryMock
                .HasGameObjectContext(Arg.Any<int>())
                .ReturnsForAnyArgs(false);
            _gameObjectContextRepositoryMock
                .GetGameObjectContext(Arg.Any<int>())
                .ReturnsForAnyArgs(_contextMock);
            var component = new GameObject().AddComponent<GameObjectContext>();
            // exercise
            var gameObjectContext =
                gameObjectContextService.GetGameObjectContextOrDefault(component, _defaultContextMock);
            // verify
            Assert.AreEqual(gameObjectContext, _defaultContextMock);
        }
#endif
    }
}