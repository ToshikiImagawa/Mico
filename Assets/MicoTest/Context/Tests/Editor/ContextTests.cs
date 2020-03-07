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
        public void test_()
        {
        }
#endif
    }
}