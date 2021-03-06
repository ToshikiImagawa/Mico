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
        private IContextRepository _gameObjectContextRepositoryMock;
        private GameObjectContextHelper _gameObjectContextHelperMock;
        private IContext _contextMock;
        private IContext _defaultContextMock;

        [SetUp]
        public void Setup()
        {
            var container = new DiContainer();
#if MICO_TEST_ADD_NSUBSTITUTE
            _gameObjectContextRepositoryMock = Substitute.For<IContextRepository>();
            _gameObjectContextHelperMock = Substitute.For<GameObjectContextHelper>();
            _contextMock = Substitute.For<IContext>();
            _defaultContextMock = Substitute.For<IContext>();
            _contextMock.Container.Returns(new DiContainer());
            _defaultContextMock.Container.Returns(new DiContainer());
#endif
            container.RegisterInstance<IContextRepository>(_gameObjectContextRepositoryMock)
                .WithId(typeof(GameObjectContextService)).AsSingle();
            container.RegisterInstance<GameObjectContextHelper>(_gameObjectContextHelperMock);
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
            const int instanceId = 222;
            _gameObjectContextHelperMock
                .GetInstanceId(Arg.Any<Component>())
                .ReturnsForAnyArgs(instanceId);
            _gameObjectContextHelperMock
                .GetComponentInParentOnly<IContext>(Arg.Any<Component>())
                .ReturnsForAnyArgs(_ => null);
            _gameObjectContextRepositoryMock
                .HasContext(instanceId)
                .ReturnsForAnyArgs(true);
            _gameObjectContextRepositoryMock
                .GetContext(instanceId)
                .ReturnsForAnyArgs(_contextMock);
            var component = new GameObject().AddComponent<GameObjectContext>();
            // exercise
            var gameObjectContext =
                gameObjectContextService.GetGameObjectContextOrDefault(component, _defaultContextMock);
            // verify
            Assert.AreEqual(gameObjectContext, _contextMock);
        }

        [Test]
        public void
            test_GetGameObjectContextOrDefault実行時にHasGameObjectContextがfalse且つParentComponentにIContextがある時ParentComponentのIContextがかえること()
        {
            // setup
            var gameObjectContextService = ContextContainer.Resolve<IGameObjectContextService>();
            const int instanceId = 222;
            var component = new GameObject().AddComponent<GameObjectContext>();
            var parentComponent = new GameObject().AddComponent<GameObjectContext>();
            component.transform.SetParent(parentComponent.transform);
            _gameObjectContextHelperMock
                .GetInstanceId(Arg.Any<Component>())
                .ReturnsForAnyArgs(instanceId);
            _gameObjectContextHelperMock
                .GetComponentInParentOnly<IContext>(Arg.Any<Component>())
                .ReturnsForAnyArgs(_ => parentComponent);
            _gameObjectContextRepositoryMock
                .HasContext(instanceId)
                .Returns(false);
            _gameObjectContextRepositoryMock
                .GetContext(instanceId)
                .Returns(_contextMock);
            // exercise
            var gameObjectContext =
                gameObjectContextService.GetGameObjectContextOrDefault(component, _defaultContextMock);
            // verify
            Assert.AreEqual(gameObjectContext, parentComponent);
        }

        [Test]
        public void
            test_GetGameObjectContextOrDefault実行時にHasGameObjectContextがfalse且つParentComponentにIContextがない時defaultContextの値がかえること()
        {
            // setup
            var gameObjectContextService = ContextContainer.Resolve<IGameObjectContextService>();
            const int instanceId = 222;
            _gameObjectContextHelperMock
                .GetInstanceId(Arg.Any<Component>())
                .ReturnsForAnyArgs(instanceId);
            _gameObjectContextHelperMock
                .GetComponentInParentOnly<IContext>(Arg.Any<Component>())
                .ReturnsForAnyArgs(_ => null);
            _gameObjectContextRepositoryMock
                .HasContext(instanceId)
                .Returns(false);
            _gameObjectContextRepositoryMock
                .GetContext(instanceId)
                .Returns(_contextMock);
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