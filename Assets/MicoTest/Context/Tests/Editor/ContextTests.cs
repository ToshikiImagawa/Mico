// MicoTest C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using Mico;
using Mico.Context.Internal;
using NUnit.Framework;

namespace MicoTest
{
    public class ContextTests
    {
#if MICO_TEST_ADD_MOQ
        private readonly Moq.Mock<ISceneRepository> _sceneRepositoryMock =
            new Moq.Mock<ISceneRepository>();

        private readonly Moq.Mock<ISceneContextRepository> _sceneContextRepositoryMock =
            new Moq.Mock<ISceneContextRepository>();

        private readonly Moq.Mock<ISceneContextService> _sceneContextServiceMock =
            new Moq.Mock<ISceneContextService>();

        private readonly Moq.Mock<IGameObjectContextRepository> _gameObjectContextRepositoryMock =
            new Moq.Mock<IGameObjectContextRepository>();

        private readonly Moq.Mock<IGameObjectContextService> _gameObjectContextServiceMock =
            new Moq.Mock<IGameObjectContextService>();
#endif
        [SetUp]
        public void Setup()
        {
            var mockContainer = new DiContainer();
#if MICO_TEST_ADD_MOQ
            mockContainer.RegisterInstance<ISceneRepository>(_sceneRepositoryMock.Object).AsSingle();
            mockContainer.RegisterInstance<ISceneContextRepository>(_sceneContextRepositoryMock.Object).AsSingle();
            mockContainer.RegisterInstance<ISceneContextService>(_sceneContextServiceMock.Object);
            mockContainer.RegisterInstance<IGameObjectContextRepository>(_gameObjectContextRepositoryMock.Object);
            mockContainer.RegisterInstance<IGameObjectContextService>(_gameObjectContextServiceMock.Object);
#endif
            mockContainer.Compile();
            ContextContainer.Swap(mockContainer);
        }

#if MICO_TEST_ADD_MOQ
        [Test]
        public void test_SceneContextServiceのBoot時にSetSceneContextが失敗した時falseが返ること()
        {
            // setup
            _sceneContextRepositoryMoq
                .Setup(_ => _.SetSceneContext(0, _contextMoq.Object))
                .Returns(false);
            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            var isSuccessful = sceneContextService.Boot(default, _contextMoq.Object, "");
            // verify
            Assert.IsFalse(isSuccessful);
        }

        [Test]
        public void test_SceneContextServiceのBoot時にSetSceneContextが成功した時trueが返ること()
        {
            // setup
            _sceneContextRepositoryMoq
                .Setup(_ => _.SetSceneContext(0, _contextMoq.Object))
                .Returns(true);
            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            var isSuccessful = sceneContextService.Boot(default, _contextMoq.Object, "");
            // verify
            Assert.IsTrue(isSuccessful);
        }

        [Test]
        public void test_SceneContextServiceのBoot時にscenePathがnullの時SetContainerが呼ばれること()
        {
            // setup
            _sceneContextRepositoryMoq
                .Setup(_ => _.SetSceneContext(0, _contextMoq.Object))
                .Returns(true);
            _sceneContextHelper
                .Setup(_ => _.GetContextsInScene(default))
                .Returns(new[] {_contextMoq.Object});
            _sceneContextHelper
                .Setup(_ => _.GetComponentsInScene(default))
                .Returns(new Component[0]);

            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            sceneContextService.Boot(default, _contextMoq.Object);
            // verify
            _contextMoq.Verify(_ => _.SetContainer(It.IsAny<DiContainer>()), Times.AtLeastOnce);
        }

        [Test]
        public void test_SceneContextServiceのBoot時にscenePathが有効の時SetContainerとGetCacheSceneが呼ばれること()
        {
            // setup
            _sceneContextRepositoryMoq
                .Setup(_ => _.SetSceneContext(0, _contextMoq.Object))
                .Returns(true);
            _sceneContextHelper
                .Setup(_ => _.GetContextsInScene(default))
                .Returns(new[] {_contextMoq.Object});
            _sceneContextHelper
                .Setup(_ => _.GetComponentsInScene(default))
                .Returns(new Component[0]);
            _sceneContextRepositoryMoq
                .Setup(_ => _.GetSceneContext(0))
                .Returns(_parentContextMoq.Object);

            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            sceneContextService.Boot(default, _contextMoq.Object, "mockScene");
            // verify
            _sceneRepositoryMoq.Verify(_ => _.GetCacheScene("mockScene"), Times.AtLeastOnce);
            _contextMoq.Verify(_ => _.SetContainer(It.IsAny<DiContainer>()), Times.AtLeastOnce);
        }

        [Test]
        public void test_SceneContextServiceのBoot時HasSceneContextがtrueの時GetSceneContextが呼ばれること()
        {
            // setup
            _sceneContextRepositoryMoq
                .Setup(_ => _.SetSceneContext(0, _contextMoq.Object))
                .Returns(true);
            _sceneContextHelper
                .Setup(_ => _.GetContextsInScene(default))
                .Returns(new[] {_contextMoq.Object});
            _sceneContextHelper
                .Setup(_ => _.GetComponentsInScene(default))
                .Returns(new Component[0]);
            _sceneContextRepositoryMoq
                .Setup(_ => _.HasSceneContext(0))
                .Returns(true);
            _sceneContextRepositoryMoq
                .Setup(_ => _.GetSceneContext(0))
                .Returns(_parentContextMoq.Object);

            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            sceneContextService.Boot(default, _contextMoq.Object, "mockScene");
            // verify
            _sceneContextRepositoryMoq.Verify(_ => _.GetSceneContext(It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Test]
        public void test_SceneContextServiceのBoot時HasSceneContextがfalseの時GetSceneContextが呼ばれないこと()
        {
            // setup
            _sceneContextRepositoryMoq
                .Setup(_ => _.SetSceneContext(0, _contextMoq.Object))
                .Returns(true);
            _sceneContextHelper
                .Setup(_ => _.GetContextsInScene(default))
                .Returns(new[] {_contextMoq.Object});
            _sceneContextHelper
                .Setup(_ => _.GetComponentsInScene(default))
                .Returns(new Component[0]);
            _sceneContextRepositoryMoq
                .Setup(_ => _.HasSceneContext(0))
                .Returns(false);
            _sceneContextRepositoryMoq
                .Setup(_ => _.GetSceneContext(0))
                .Returns(_parentContextMoq.Object);

            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            sceneContextService.Boot(default, _contextMoq.Object, "mockScene");
            // verify
            _sceneContextRepositoryMoq.Verify(_ => _.GetSceneContext(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void test_SceneContextServiceのBoot時にGetContextsInSceneが複数返す時各ContextのSetContainerが呼ばれること()
        {
            // setup
            var childrenContextMoq = new[]
                {new Mock<IContext>(), new Mock<IContext>(), new Mock<IContext>()};
            foreach (var mock in childrenContextMoq)
            {
                mock.SetupGet(_ => _.Container)
                    .Returns(new DiContainer());
            }

            _sceneContextRepositoryMoq
                .Setup(_ => _.SetSceneContext(0, _contextMoq.Object))
                .Returns(true);
            _sceneContextHelper
                .Setup(_ => _.GetContextsInScene(default))
                .Returns(childrenContextMoq.Select(_ => _.Object).Concat(new[] {_contextMoq.Object}).ToArray);
            _sceneContextHelper
                .Setup(_ => _.GetComponentsInScene(default))
                .Returns(new Component[0]);

            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            sceneContextService.Boot(default, _contextMoq.Object);
            // verify
            foreach (var mock in childrenContextMoq)
            {
                mock.Verify(_ => _.SetContainer(It.IsAny<DiContainer>()), Times.AtLeastOnce);
            }
        }

        [Test]
        public void test_SceneContextServiceのBoot時にInstallersが複数返す時各InstallerのInstallRegistersが呼ばれること()
        {
            // setup
            _sceneContextRepositoryMoq
                .Setup(_ => _.SetSceneContext(0, _contextMoq.Object))
                .Returns(true);
            _sceneContextHelper
                .Setup(_ => _.GetContextsInScene(default))
                .Returns(new[] {_contextMoq.Object});
            _sceneContextHelper
                .Setup(_ => _.GetComponentsInScene(default))
                .Returns(new Component[0]);
            _contextMoq.SetupGet(_ => _.Installers)
                .Returns(_installersMoq.Select(_ => _.Object));

            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            sceneContextService.Boot(default, _contextMoq.Object);
            // verify
            foreach (var mock in _installersMoq)
            {
                mock.Verify(_ => _.InstallRegisters(_contextMoq.Object.Container), Times.AtLeastOnce);
            }
        }

        [Test]
        public void test_SceneContextServiceのBoot時にGetComponentsInSceneが複数返す時各InstallerのInstallRegistersが呼ばれること()
        {
            // setup
            var isInjectSceneContainer = false;
            var sceneContextMoq = new Mock<IContext>();
            var sceneContainerMoq = new Mock<DiContainer>();
            // sceneContainerMoq
            //     .As<IDiContainer>()
            //     .Setup(_ => _.Inject(It.IsAny<Component>()))
            //     .Callback(() =>
            //     {
            //         isInjectSceneContainer = true; 
            //         Debug.Log("sceneContainerMoq Inject");
            //     });
            sceneContextMoq
                .SetupGet(_ => _.Container)
                .Returns(sceneContainerMoq.Object)
                .Callback(() => { Debug.Log("return sceneContainerMoq"); });
            // _sceneContextRepositoryMoq
            //     .Setup(_ => _.GetParentContext(It.IsAny<Component>()))
            //     .Returns(() => null);
            var components = new[]
            {
                (Component) new GameObject().AddComponent<IgnoreComponent>(),
                new GameObject().AddComponent<MockComponent>(),
                new GameObject().AddComponent<MockComponent>(),
            };

            _sceneContextRepositoryMoq
                .Setup(_ => _.SetSceneContext(0, sceneContextMoq.Object))
                .Returns(true);
            // _sceneRepositoryMoq
            //     .Setup(_ => _.GetContextsInScene(default))
            //     .Returns(new[] {sceneContextMoq.Object});
            // _sceneRepositoryMoq
            //     .Setup(_ => _.GetComponentsInScene(default))
            //     .Returns(components);

            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            sceneContextService.Boot(default, sceneContextMoq.Object);
            // verify
            Assert.IsTrue(isInjectSceneContainer);
        }
#endif
    }

    [IgnoreInjection]
    public class IgnoreComponent : MonoBehaviour
    {
    }

    public class MockComponent : MonoBehaviour
    {
    }
}