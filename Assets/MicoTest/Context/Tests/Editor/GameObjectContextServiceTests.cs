// MicoContextTest C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using Mico;
using Mico.Context.Internal;
using NUnit.Framework;
#if MICO_TEST_ADD_MOQ
using Moq;

#endif
namespace MicoContextTest
{
    public class GameObjectContextServiceTests
    {
#if MICO_TEST_ADD_MOQ
        private Mock<IGameObjectContextRepository> _gameObjectContextRepositoryMoq;
#endif
        [SetUp]
        public void Setup()
        {
            var mockContainer = new DiContainer();
#if MICO_TEST_ADD_MOQ
            _gameObjectContextRepositoryMoq = new Mock<IGameObjectContextRepository>();
#endif
            mockContainer.Compile();
            ContextContainer.Swap(mockContainer);
        }
    }
}