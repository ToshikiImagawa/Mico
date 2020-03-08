// MicoTest C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System.Linq;
using Mico;
using Mico.Context.Internal;
using Mico.Context;
#if MICO_TEST_ADD_MOQ
using Moq;
#endif
using UnityEngine.SceneManagement;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace MicoContextTest
{
    public class SceneContextServiceTests
    {
#if MICO_TEST_ADD_MOQ
        private Mock<ISceneRepository> _sceneRepositoryMoq;

        private Mock<ISceneContextRepository> _sceneContextRepositoryMoq;
        private Mock<ISceneContextHelper> _sceneContextHelper;
        private Mock<ISceneContextService> _sceneContextServiceMoq;
        private Mock<IGameObjectContextRepository> _gameObjectContextRepositoryMoq;
        private Mock<IGameObjectContextService> _gameObjectContextServiceMoq;
        private Mock<IContext> _contextMoq;
        private Mock<IContext> _parentContextMoq;
        private Mock<IInstaller>[] _installersMoq;
#endif
        [SetUp]
        public void Setup()
        {
            var mockContainer = new DiContainer();
#if MICO_TEST_ADD_MOQ
            _sceneRepositoryMoq = new Mock<ISceneRepository>();
            _sceneContextRepositoryMoq = new Mock<ISceneContextRepository>();
            _sceneContextHelper = new Mock<ISceneContextHelper>();
            _sceneContextServiceMoq = new Mock<ISceneContextService>();
            _gameObjectContextRepositoryMoq = new Mock<IGameObjectContextRepository>();
            _gameObjectContextServiceMoq = new Mock<IGameObjectContextService>();
            _contextMoq = new Mock<IContext>();
            _parentContextMoq = new Mock<IContext>();
            _installersMoq = new[]
            {
                new Mock<IInstaller>(), new Mock<IInstaller>(), new Mock<IInstaller>()
            };

            _contextMoq.SetupGet(_ => _.Container)
                .Returns(new DiContainer());
            _parentContextMoq.SetupGet(_ => _.Container)
                .Returns(new DiContainer());
            mockContainer.RegisterInstance<ISceneRepository>(_sceneRepositoryMoq.Object).AsSingle();
            mockContainer.RegisterInstance<ISceneContextRepository>(_sceneContextRepositoryMoq.Object).AsSingle();
            mockContainer.RegisterInstance<ISceneContextHelper>(_sceneContextHelper.Object);
            mockContainer.RegisterInstance<ISceneContextService>(_sceneContextServiceMoq.Object);
            mockContainer.RegisterInstance<IGameObjectContextRepository>(_gameObjectContextRepositoryMoq.Object);
            mockContainer.RegisterInstance<IGameObjectContextService>(_gameObjectContextServiceMoq.Object);
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
                .Setup(_ => _.SetSceneContext(It.IsAny<int>(), It.IsAny<IContext>()))
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
                .Setup(_ => _.SetSceneContext(It.IsAny<int>(), It.IsAny<IContext>()))
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
                .Setup(_ => _.SetSceneContext(It.IsAny<int>(), It.IsAny<IContext>()))
                .Returns(true);
            _sceneContextHelper
                .Setup(_ => _.GetContextsInScene(It.IsAny<Scene>()))
                .Returns(new[] {_contextMoq.Object});
            _sceneContextHelper
                .Setup(_ => _.GetComponentsInScene(It.IsAny<Scene>()))
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
                .Setup(_ => _.SetSceneContext(It.IsAny<int>(), It.IsAny<IContext>()))
                .Returns(true);
            _sceneContextHelper
                .Setup(_ => _.GetContextsInScene(It.IsAny<Scene>()))
                .Returns(new[] {_contextMoq.Object});
            _sceneContextHelper
                .Setup(_ => _.GetComponentsInScene(It.IsAny<Scene>()))
                .Returns(new Component[0]);
            _sceneContextRepositoryMoq
                .Setup(_ => _.GetSceneContext(It.IsAny<int>()))
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
        public void test_SceneContextServiceのBoot時HasSceneContextがfalseの時GetSceneContextが呼ばれないこと()
        {
            // setup
            _sceneContextRepositoryMoq
                .Setup(_ => _.SetSceneContext(It.IsAny<int>(), It.IsAny<IContext>()))
                .Returns(true);
            _sceneContextHelper
                .Setup(_ => _.GetContextsInScene(It.IsAny<Scene>()))
                .Returns(new[] {_contextMoq.Object});
            _sceneContextHelper
                .Setup(_ => _.GetComponentsInScene(It.IsAny<Scene>()))
                .Returns(new Component[0]);
            _sceneContextRepositoryMoq
                .Setup(_ => _.HasSceneContext(It.IsAny<int>()))
                .Returns(false);
            _sceneContextRepositoryMoq
                .Setup(_ => _.GetSceneContext(It.IsAny<int>()))
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
                .Setup(_ => _.SetSceneContext(It.IsAny<int>(), It.IsAny<IContext>()))
                .Returns(true);
            _sceneContextHelper
                .Setup(_ => _.GetContextsInScene(It.IsAny<Scene>()))
                .Returns(childrenContextMoq.Select(_ => _.Object).Concat(new[] {_contextMoq.Object}).ToArray);
            _sceneContextHelper
                .Setup(_ => _.GetComponentsInScene(It.IsAny<Scene>()))
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
                .Setup(_ => _.SetSceneContext(It.IsAny<int>(), It.IsAny<IContext>()))
                .Returns(true);
            _sceneContextHelper
                .Setup(_ => _.GetContextsInScene(It.IsAny<Scene>()))
                .Returns(new[] {_contextMoq.Object});
            _sceneContextHelper
                .Setup(_ => _.GetComponentsInScene(It.IsAny<Scene>()))
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
        public void test_SceneContextServiceのBoot時にGetComponentsInSceneが複数返す時各Componentに対してInjectが呼ばれること()
        {
            // setup
            var sceneContextMoq = new Mock<IContext>();
            var sceneContainer = new DiContainer();
            sceneContextMoq
                .SetupGet(_ => _.Container)
                .Returns(sceneContainer)
                .Callback(() => { Debug.Log("return sceneContainerMoq"); });
            _sceneContextHelper
                .Setup(_ => _.GetParentContext(It.IsAny<Component>()))
                .Returns(() => null);
            var components = new[]
            {
                (Component) new GameObject().AddComponent<IgnoreComponent>(),
                new GameObject().AddComponent<MockComponent>(),
                new GameObject().AddComponent<MockComponent>(),
            };
            _sceneContextRepositoryMoq
                .Setup(_ => _.SetSceneContext(It.IsAny<int>(), It.IsAny<IContext>()))
                .Returns(true);
            _sceneContextHelper
                .Setup(_ => _.GetContextsInScene(It.IsAny<Scene>()))
                .Returns(new[] {sceneContextMoq.Object});
            _sceneContextHelper
                .Setup(_ => _.GetComponentsInScene(It.IsAny<Scene>()))
                .Returns(components);

            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            sceneContextService.Boot(default, sceneContextMoq.Object);
            // verify
            _sceneContextHelper
                .Verify(_ => _.Inject(sceneContainer, It.IsAny<MockComponent>()),
                    Times.AtLeastOnce);
            _sceneContextHelper
                .Verify(_ => _.Inject(It.IsAny<DiContainer>(), It.IsAny<IgnoreComponent>()),
                    Times.Never);
        }

        [Test]
        public void test_RemoveSceneContextが呼ばれた時ISceneContextRepositoryのRemoveSceneContextが呼ばれること()
        {
            // setup
            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            var scene = default(Scene);
            // exercise
            sceneContextService.RemoveSceneContext(scene);
            // verify
            _sceneContextRepositoryMoq
                .Verify(_ => _.RemoveSceneContext(scene.handle), Times.AtLeastOnce);
        }

        [Test]
        public void test_GetSceneContextOrDefault実行時scenePathが無効な時nullを返しHasSceneContextが呼ばれないこと()
        {
            // setup
            var sceneContextService = new SceneContextService();
            _sceneRepositoryMoq
                .Setup(_ => _.GetCacheScene(It.IsAny<string>()))
                .Returns(() => null);
            ContextContainer.Inject(sceneContextService);
            // exercise
            var context = sceneContextService.GetSceneContextOrDefault("error_mock");
            // verify
            Assert.IsNull(context);
            _sceneContextRepositoryMoq
                .Verify(_ => _.HasSceneContext(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void test_GetSceneContextOrDefault実行時scenePathが有効な時HasSceneContextが呼ばれること()
        {
            // setup
            var sceneContextService = new SceneContextService();
            _sceneRepositoryMoq
                .Setup(_ => _.GetCacheScene(It.IsAny<string>()))
                .Returns(SceneManager.GetActiveScene());
            ContextContainer.Inject(sceneContextService);
            // exercise
            var context = sceneContextService.GetSceneContextOrDefault("mock");
            // verify
            Assert.IsNull(context);
            _sceneContextRepositoryMoq
                .Verify(_ => _.HasSceneContext(It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Test]
        public void test_GetSceneContextOrDefault実行時HasSceneContextがtrueの時GetSceneContextが呼ばれること()
        {
            // setup
            var sceneContextService = new SceneContextService();
            _sceneContextRepositoryMoq
                .Setup(_ => _.HasSceneContext(It.IsAny<int>()))
                .Returns(true);
            _sceneRepositoryMoq
                .Setup(_ => _.GetCacheScene(It.IsAny<string>()))
                .Returns(SceneManager.GetActiveScene());
            ContextContainer.Inject(sceneContextService);
            // exercise
            var context = sceneContextService.GetSceneContextOrDefault("mock");
            // verify
            Assert.IsNull(context);
            _sceneContextRepositoryMoq
                .Verify(_ => _.GetSceneContext(It.IsAny<int>()), Times.AtLeastOnce);
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