// MicoTest C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using Mico;
using Mico.Context.Internal;
using Mico.Context;
#if MICO_TEST_ADD_NSUBSTITUTE
using NSubstitute;
#endif
using UnityEngine.SceneManagement;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace MicoContextTest
{
    public class SceneContextServiceTests
    {
        private ISceneRepository _sceneRepositoryMock;
        private ISceneContextRepository _sceneContextRepositoryMock;
        private ISceneContextHelper _sceneContextHelperMock;
        private ISceneContextService _sceneContextServiceMock;
        private IGameObjectContextRepository _gameObjectContextRepositoryMock;
        private IGameObjectContextService _gameObjectContextServiceMock;
        private IContext _contextMock;
        private IContext _parentContextMock;
        private IInstaller[] _installersMock;

        [SetUp]
        public void Setup()
        {
            var container = new DiContainer();
#if MICO_TEST_ADD_NSUBSTITUTE
            _sceneRepositoryMock = Substitute.For<ISceneRepository>();
            _sceneContextRepositoryMock = Substitute.For<ISceneContextRepository>();
            _sceneContextHelperMock = Substitute.For<ISceneContextHelper>();
            _sceneContextServiceMock = Substitute.For<ISceneContextService>();
            _gameObjectContextRepositoryMock = Substitute.For<IGameObjectContextRepository>();
            _gameObjectContextServiceMock = Substitute.For<IGameObjectContextService>();
            _contextMock = Substitute.For<IContext>();
            _parentContextMock = Substitute.For<IContext>();
            _installersMock = new[]
            {
                Substitute.For<IInstaller>(),
                Substitute.For<IInstaller>(),
                Substitute.For<IInstaller>()
            };

            _contextMock.Container.Returns(new DiContainer());
            _parentContextMock.Container.Returns(new DiContainer());
#endif
            container.RegisterInstance<ISceneRepository>(_sceneRepositoryMock).AsSingle();
            container.RegisterInstance<ISceneContextRepository>(_sceneContextRepositoryMock).AsSingle();
            container.RegisterInstance<ISceneContextHelper>(_sceneContextHelperMock);
            container.RegisterInstance<ISceneContextService>(_sceneContextServiceMock);
            container.RegisterInstance<IGameObjectContextRepository>(_gameObjectContextRepositoryMock);
            container.RegisterInstance<IGameObjectContextService>(_gameObjectContextServiceMock);
            container.Compile();
            ContextContainer.Swap(container);
        }

#if MICO_TEST_ADD_NSUBSTITUTE
        [Test]
        public void test_SceneContextServiceのBoot時にSetSceneContextが失敗した時falseが返ること()
        {
            // setup
            _sceneContextRepositoryMock
                .SetSceneContext(Arg.Any<int>(), Arg.Any<IContext>())
                .ReturnsForAnyArgs(false);
            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            var isSuccessful = sceneContextService.Boot(default, _contextMock, "");
            // verify
            Assert.IsFalse(isSuccessful);
        }

        [Test]
        public void test_SceneContextServiceのBoot時にSetSceneContextが成功した時trueが返ること()
        {
            // setup
            _sceneContextRepositoryMock
                .SetSceneContext(Arg.Any<int>(), Arg.Any<IContext>())
                .ReturnsForAnyArgs(true);
            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            var isSuccessful = sceneContextService.Boot(default, _contextMock, "");
            // verify
            Assert.IsTrue(isSuccessful);
        }

        [Test]
        public void test_SceneContextServiceのBoot時にscenePathがnullの時SetContainerが呼ばれること()
        {
            // setup

            _sceneContextRepositoryMock
                .SetSceneContext(Arg.Any<int>(), Arg.Any<IContext>())
                .ReturnsForAnyArgs(true);
            _sceneContextHelperMock
                .GetContextsInScene(Arg.Any<Scene>())
                .ReturnsForAnyArgs(new[] {_contextMock});
            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            sceneContextService.Boot(default, _contextMock);
            // verify
            _contextMock
                .Received()
                .SetContainer(_contextMock.Container);
        }

        [Test]
        public void test_SceneContextServiceのBoot時にscenePathが有効の時SetContainerとGetCacheSceneが呼ばれること()
        {
            // setup

            _sceneContextRepositoryMock
                .SetSceneContext(Arg.Any<int>(), Arg.Any<IContext>())
                .ReturnsForAnyArgs(true);
            _sceneContextHelperMock
                .GetContextsInScene(Arg.Any<Scene>())
                .ReturnsForAnyArgs(new[] {_contextMock});
            _sceneContextRepositoryMock
                .GetSceneContext(Arg.Any<int>())
                .ReturnsForAnyArgs(_parentContextMock);

            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            sceneContextService.Boot(default, _contextMock, "mockScene");
            // verify
            _sceneRepositoryMock
                .Received()
                .GetCacheScene("mockScene");
            _contextMock
                .Received()
                .SetContainer(_contextMock.Container);
        }

        [Test]
        public void test_SceneContextServiceのBoot時HasSceneContextがfalseの時GetSceneContextが呼ばれないこと()
        {
            // setup
            _sceneContextRepositoryMock
                .SetSceneContext(Arg.Any<int>(), Arg.Any<IContext>())
                .ReturnsForAnyArgs(true);
            _sceneContextHelperMock
                .GetContextsInScene(Arg.Any<Scene>())
                .ReturnsForAnyArgs(new[] {_contextMock});
            _sceneContextRepositoryMock
                .HasSceneContext(Arg.Any<int>())
                .ReturnsForAnyArgs(false);
            _sceneContextRepositoryMock
                .GetSceneContext(Arg.Any<int>())
                .ReturnsForAnyArgs(_parentContextMock);

            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            sceneContextService.Boot(default, _contextMock, "mockScene");
            // verify
            _sceneContextRepositoryMock
                .DidNotReceiveWithAnyArgs()
                .GetSceneContext(Arg.Any<int>());
        }


        [Test]
        public void test_SceneContextServiceのBoot時にGetContextsInSceneが複数返す時各ContextのSetContainerが呼ばれること()
        {
            // setup
            var childrenContextMoq = new[]
            {
                Substitute.For<IContext>(),
                Substitute.For<IContext>(),
                Substitute.For<IContext>()
            };
            foreach (var mock in childrenContextMoq)
            {
                mock.Container
                    .ReturnsForAnyArgs(new DiContainer());
            }

            _sceneContextRepositoryMock
                .SetSceneContext(Arg.Any<int>(), Arg.Any<IContext>())
                .ReturnsForAnyArgs(true);
            _sceneContextHelperMock
                .GetContextsInScene(Arg.Any<Scene>())
                .ReturnsForAnyArgs(childrenContextMoq);

            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            sceneContextService.Boot(default, _contextMock);
            // verify
            foreach (var mock in childrenContextMoq)
            {
                mock.ReceivedWithAnyArgs()
                    .SetContainer(Arg.Any<DiContainer>());
            }
        }

        [Test]
        public void test_SceneContextServiceのBoot時にInstallersが複数返す時各InstallerのInstallRegistersが呼ばれること()
        {
            // setup
            _sceneContextRepositoryMock
                .SetSceneContext(Arg.Any<int>(), Arg.Any<IContext>())
                .ReturnsForAnyArgs(true);
            _sceneContextHelperMock
                .GetContextsInScene(Arg.Any<Scene>())
                .ReturnsForAnyArgs(new[] {_contextMock});
            _contextMock.Installers
                .ReturnsForAnyArgs(_installersMock);

            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            sceneContextService.Boot(default, _contextMock);
            // verify
            foreach (var installer in _installersMock)
            {
                installer.Received().InstallRegisters(_contextMock.Container);
            }
        }

        [Test]
        public void test_SceneContextServiceのBoot時にGetComponentsInSceneが複数返す時各Componentに対してInjectが呼ばれること()
        {
            // setup
            var sceneContextMock = Substitute.For<IContext>();
            var sceneContainer = new DiContainer();
            sceneContextMock.Container
                .ReturnsForAnyArgs(sceneContainer);

            _sceneContextHelperMock
                .GetParentContext(Arg.Any<Component>())
                .ReturnsForAnyArgs(_ => null);
            var components = new[]
            {
                (Component) new GameObject().AddComponent<IgnoreComponent>(),
                new GameObject().AddComponent<MockComponent>(),
                new GameObject().AddComponent<MockComponent>(),
            };
            _sceneContextRepositoryMock
                .SetSceneContext(Arg.Any<int>(), Arg.Any<IContext>())
                .ReturnsForAnyArgs(true);
            _sceneContextHelperMock
                .GetContextsInScene(Arg.Any<Scene>())
                .ReturnsForAnyArgs(new[] {sceneContextMock});
            _sceneContextHelperMock
                .GetComponentsInScene(Arg.Any<Scene>())
                .ReturnsForAnyArgs(components);

            var sceneContextService = new SceneContextService();
            ContextContainer.Inject(sceneContextService);
            // exercise
            sceneContextService.Boot(default, sceneContextMock);
            // verify
            _sceneContextHelperMock
                .Received()
                .Inject(sceneContainer, Arg.Any<MockComponent>());
            _sceneContextHelperMock
                .DidNotReceiveWithAnyArgs()
                .Inject(Arg.Any<DiContainer>(), Arg.Any<IgnoreComponent>());
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
            _sceneContextRepositoryMock
                .Received()
                .RemoveSceneContext(scene.handle);
        }

        [Test]
        public void test_GetSceneContextOrDefault実行時scenePathが無効な時nullを返しHasSceneContextが呼ばれないこと()
        {
            // setup
            var sceneContextService = new SceneContextService();
            _sceneRepositoryMock
                .GetCacheScene(Arg.Any<string>())
                .ReturnsForAnyArgs(_ => null);
            ContextContainer.Inject(sceneContextService);
            // exercise
            var context = sceneContextService.GetSceneContextOrDefault("error_mock");
            // verify
            Assert.IsNull(context);
            _sceneContextRepositoryMock
                .DidNotReceiveWithAnyArgs()
                .HasSceneContext(Arg.Any<int>());
        }

        [Test]
        public void test_GetSceneContextOrDefault実行時scenePathが有効な時HasSceneContextが呼ばれること()
        {
            // setup
            var sceneContextService = new SceneContextService();
            _sceneRepositoryMock
                .GetCacheScene(Arg.Any<string>())
                .ReturnsForAnyArgs(SceneManager.GetActiveScene());
            ContextContainer.Inject(sceneContextService);
            // exercise
            var context = sceneContextService.GetSceneContextOrDefault("mock");
            // verify
            Assert.IsNull(context);
            _sceneContextRepositoryMock.Received()
                .HasSceneContext(Arg.Any<int>());
        }

        [Test]
        public void test_GetSceneContextOrDefault実行時HasSceneContextがtrueの時GetSceneContextが呼ばれること()
        {
            // setup
            var sceneContextService = new SceneContextService();
            var activeScene = SceneManager.GetActiveScene();
            _sceneContextRepositoryMock
                .HasSceneContext(Arg.Any<int>())
                .ReturnsForAnyArgs(true);
            _sceneRepositoryMock
                .GetCacheScene(Arg.Any<string>())
                .ReturnsForAnyArgs(activeScene);
            ContextContainer.Inject(sceneContextService);
            // exercise
            var context = sceneContextService.GetSceneContextOrDefault("mock");
            // verify
            Assert.IsNotNull(context);
            _sceneContextRepositoryMock.Received()
                .GetSceneContext(activeScene.handle);
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