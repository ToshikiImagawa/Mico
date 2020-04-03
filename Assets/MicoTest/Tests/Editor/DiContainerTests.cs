// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using Mico;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace MicoTest
{
    public class DiContainerTests
    {
        private DiContainer _container;
        private DiContainer _childContainer;
        private DiContainer _grandChildContainer;

        [SetUp]
        public void Setup()
        {
            // setup
            _container = new DiContainer();
            _childContainer = new DiContainer(_container);
            _grandChildContainer = new DiContainer(_childContainer);
        }

        #region RegisterNew

        [Test]
        public void test_ClassがRegisterNewで登録出来_Resolveで取得出来ること()
        {
            // setup
            _container.RegisterNew(typeof(Mock)).AsTransient().Lazy();
            // exercise
            var mock = _container.Resolve(typeof(Mock)) as Mock;
            // verify
            Assert.IsNotNull(mock);
            Assert.AreEqual(mock.Id, 100);
        }

        [Test]
        public void test_ClassがRegisterNewをパラメーター付きで登録出来_Resolveで取得出来ること()
        {
            // setup
            _container.RegisterNew(typeof(Mock), 50);
            // exercise
            var mock = _container.Resolve(typeof(Mock)) as Mock;
            // verify
            Assert.IsNotNull(mock);
            Assert.AreEqual(mock.Id, 50);
        }

        [Test]
        public void test_ClassがRegisterNew_Genericで登録出来_Resolveで取得出来ること()
        {
            // setup
            _container.RegisterNew<Mock>();
            // exercise
            var mock = _container.Resolve<Mock>();
            // verify
            Assert.IsNotNull(mock);
            Assert.AreEqual(mock.Id, 100);
        }

        [Test]
        public void test_ClassがRegisterNew_Genericをパラメーター付きで登録出来_Resolveで取得出来ること()
        {
            // setup
            _container.RegisterNew<Mock>(50);
            // exercise
            var mock = _container.Resolve<Mock>();
            // verify
            Assert.IsNotNull(mock);
            Assert.AreEqual(mock.Id, 50);
        }

        [Test]
        public void test_複数のInterfaceがRegisterNewで登録出来_Resolveで取得出来ること()
        {
            // setup
            _container.RegisterNew(new[] {typeof(IMockId), typeof(IMockName)}, typeof(Mock));
            // exercise
            var mockId = _container.Resolve(typeof(IMockId)) as IMockId;
            var mockName = _container.Resolve(typeof(IMockName)) as IMockName;
            // verify
            Assert.IsNotNull(mockId);
            Assert.IsNotNull(mockName);
            Assert.AreEqual(mockId.Id, 100);
            Assert.AreEqual(mockName.Name, "Taro");
        }

        [Test]
        public void test_複数のInterfaceがRegisterNewをパラメーター付きで登録出来_Resolveで取得出来ること()
        {
            // setup
            _container.RegisterNew(new[] {typeof(IMockId), typeof(IMockName)}, typeof(Mock), 50);
            // exercise
            var mockId = _container.Resolve(typeof(IMockId)) as IMockId;
            var mockName = _container.Resolve(typeof(IMockName)) as IMockName;
            // verify
            Assert.IsNotNull(mockId);
            Assert.IsNotNull(mockName);
            Assert.AreEqual(mockId.Id, 50);
            Assert.AreEqual(mockName.Name, "Ken");
        }

        [Test]
        public void test_BaseClassがRegisterNew_Genericで登録出来_Resolveで取得出来ること()
        {
            // setup
            _container.RegisterNew(new[] {typeof(Mock)}, typeof(DerivedMock));
            // exercise
            var mock = _container.Resolve(typeof(Mock)) as Mock;
            // verify
            Assert.IsNotNull(mock);
            Assert.AreEqual(mock.Id, 600);
            Assert.AreEqual(mock.Name, "DerivedMock");
        }

        [Test]
        public void test_InterfaceがRegisterNew_Genericで登録出来_Resolveで取得出来ること()
        {
            // setup
            _container.RegisterNew<IMockId, Mock>();
            // exercise
            var mockId = _container.Resolve<IMockId>();
            // verify
            Assert.IsNotNull(mockId);
            Assert.AreEqual(mockId.Id, 100);
        }

        [Test]
        public void test_InterfaceがRegisterNew_Genericをパラメーター付きで登録出来_Resolveで取得出来ること()
        {
            // setup
            _container.RegisterNew<IMockId, Mock>(50);
            // exercise
            var mockId = _container.Resolve<IMockId>();
            // verify
            Assert.IsNotNull(mockId);
            Assert.AreEqual(mockId.Id, 50);
        }

        [Test]
        public void test_複数のInterfaceがRegisterNew_Genericで登録出来_Resolveで取得出来ること()
        {
            // setup
            _container.RegisterNew<Mock>(new[] {typeof(IMockId), typeof(IMockName)});
            // exercise
            var mockId = _container.Resolve<IMockId>();
            var mockName = _container.Resolve(typeof(IMockName)) as IMockName;
            // verify
            Assert.IsNotNull(mockId);
            Assert.IsNotNull(mockName);
            Assert.AreEqual(mockId.Id, 100);
            Assert.AreEqual(mockName.Name, "Taro");
        }

        [Test]
        public void test_複数のInterfaceがRegisterNew_Genericをパラメーター付きで登録出来_Resolveで取得出来ること()
        {
            // setup
            _container.RegisterNew<Mock>(new[] {typeof(IMockId), typeof(IMockName)}, 50);
            // exercise
            var mockId = _container.Resolve<IMockId>();
            var mockName = _container.Resolve(typeof(IMockName)) as IMockName;
            // verify
            Assert.IsNotNull(mockId);
            Assert.IsNotNull(mockName);
            Assert.AreEqual(mockId.Id, 50);
            Assert.AreEqual(mockName.Name, "Ken");
        }

        #endregion

        #region RegisterInstance

        [Test]
        public void test_ClassがRegisterInstanceで登録出来_Resolveで取得出来ること()
        {
            // setup
            var mock = new Mock();
            _container.RegisterInstance(mock);
            // exercise
            var resolveMock = _container.Resolve(typeof(Mock)) as Mock;
            // verify
            Assert.AreEqual(mock, resolveMock);
        }

        [Test]
        public void test_複数のInterfaceがRegisterInstanceで登録出来_Resolveで取得出来ること()
        {
            // setup
            var mock = new Mock();
            _container.RegisterInstance(new[] {typeof(IMockId), typeof(IMockName)}, mock);
            // exercise
            var mockId = _container.Resolve(typeof(IMockId)) as IMockId;
            var mockName = _container.Resolve(typeof(IMockName)) as IMockName;
            // verify
            Assert.AreEqual(mock, mockId);
            Assert.AreEqual(mock, mockName);
        }

        [Test]
        public void test_InterfaceがRegisterInstance_Genericで登録出来_Resolveで取得出来ること()
        {
            // setup
            var mock = new Mock();
            _container.RegisterInstance<IMockId>(mock);
            // exercise
            var mockId = _container.Resolve<IMockId>();
            // verify
            Assert.AreEqual(mockId, mock);
        }

        #endregion

        #region RegisterFactory

        [Test]
        public void test_ClassがRegisterFactoryで登録出来_Resolveで取得出来ること()
        {
            // setup
            object MockFunc() => new Mock(25)
            {
                Name = "Mico"
            };

            _container.RegisterFactory(typeof(Mock), MockFunc);
            // exercise
            var resolveMock = _container.Resolve(typeof(Mock)) as Mock;
            // verify
            Assert.IsNotNull(resolveMock);
            Assert.AreEqual(resolveMock.Id, 25);
            Assert.AreEqual(resolveMock.Name, "Mico");
        }

        [Test]
        public void test_複数のInterfaceがRegisterFactoryで登録出来_Resolveで取得出来ること()
        {
            // setup
            object MockFunc() => new Mock(25)
            {
                Name = "Mico"
            };

            _container.RegisterFactory(new[] {typeof(IMockId), typeof(IMockName)}, typeof(Mock), MockFunc);
            // exercise
            var mockId = _container.Resolve(typeof(IMockId)) as IMockId;
            var mockName = _container.Resolve(typeof(IMockName)) as IMockName;
            // verify
            Assert.IsNotNull(mockId);
            Assert.IsNotNull(mockName);
            Assert.AreEqual(mockId.Id, 25);
            Assert.AreEqual(mockName.Name, "Mico");
        }

        [Test]
        public void test_ClassがRegisterFactory_Genericで登録出来_Resolveで取得出来ること()
        {
            // setup
            Mock MockFunc() => new Mock(25)
            {
                Name = "Mico"
            };

            _container.RegisterFactory(MockFunc);
            // exercise
            var resolveMock = _container.Resolve<Mock>();
            // verify
            Assert.IsNotNull(resolveMock);
            Assert.AreEqual(resolveMock.Id, 25);
            Assert.AreEqual(resolveMock.Name, "Mico");
        }

        [Test]
        public void test_InterfaceがRegisterFactory_Genericで登録出来_Resolveで取得出来ること()
        {
            // setup
            Mock MockFunc() => new Mock(25)
            {
                Name = "Mico"
            };

            _container.RegisterFactory<IMockId, Mock>(MockFunc);
            // exercise
            var resolveMock = _container.Resolve<IMockId>();
            // verify
            Assert.IsNotNull(resolveMock);
            Assert.AreEqual(resolveMock.Id, 25);
        }

        [Test]
        public void test_複数のInterfaceがRegisterFactory_Genericで登録出来_Resolveで取得出来ること()
        {
            // setup
            Mock MockFunc() => new Mock(25)
            {
                Name = "Mico"
            };

            _container.RegisterFactory(new[] {typeof(IMockId), typeof(IMockName)}, MockFunc);

            // exercise
            var mockId = _container.Resolve<IMockId>();
            var mockName = _container.Resolve<IMockName>();
            // verify
            Assert.IsNotNull(mockId);
            Assert.IsNotNull(mockName);
            Assert.AreEqual(mockId.Id, 25);
            Assert.AreEqual(mockName.Name, "Mico");
        }

        #endregion

        [Test]
        public void test_親でRegisterしたインスタンスを子_孫のコンテナでResolve出来ること()
        {
            // setup
            _container.RegisterNew(typeof(Mock));
            // exercise
            var mock = _container.Resolve(typeof(Mock)) as Mock;
            var childMock = _childContainer.Resolve(typeof(Mock)) as Mock;
            var grandChildMock = _grandChildContainer.Resolve(typeof(Mock)) as Mock;
            // verify
            Assert.IsNotNull(mock);
            Assert.AreEqual(mock.Id, 100);
            Assert.IsNotNull(childMock);
            Assert.AreEqual(childMock.Id, 100);
            Assert.IsNotNull(grandChildMock);
            Assert.AreEqual(grandChildMock.Id, 100);
        }

        [Test]
        public void test_WithIdでRegisterしたインスタンスをResolve出来ること()
        {
            // setup
            _container.RegisterNew<Mock>().WithId("id_1");
            _container.RegisterNew<Mock>(30).WithId("id_2");
            // exercise
            var mock1 = _container.Resolve<Mock>("id_1");
            var mock2 = _container.Resolve<Mock>("id_2");
            // verify
            Assert.IsNotNull(mock1);
            Assert.AreEqual(mock1.Id, 100);
            Assert.IsNotNull(mock2);
            Assert.AreEqual(mock2.Id, 30);
        }

        [Test]
        public void test_AsSingleでRegisterしたものがはじめにResolveしたものと次にResolveしたものが同じであること()
        {
            // setup
            _container.RegisterNew<Mock>().AsSingle();
            // exercise
            var mockFirst = _container.Resolve<Mock>();
            var mockNext = _container.Resolve<Mock>();
            // verify
            Assert.IsNotNull(mockFirst);
            Assert.AreEqual(mockFirst.Id, 100);
            Assert.AreEqual(mockFirst, mockNext);
        }

        [Test]
        public void test_NonLazyでRegisterしたインスタンスがCompile直後に生成されること()
        {
            var isRunning = false;

            // setup
            object MockFunc()
            {
                isRunning = true;
                return new Mock(25)
                {
                    Name = "Mico"
                };
            }

            _container.RegisterFactory(MockFunc).NonLazy();
            // exercise
            _container.Compile();
            // verify
            Assert.IsTrue(isRunning);
        }

        [Test]
        public void test_RegisterしたインスタンスがInjectで注入されること()
        {
            // setup
            _container.RegisterNew<IMockId, Mock>();
            var injectMock = new InjectMock();
            // exercise
            _container.Compile();
            _container.Inject(injectMock);
            // verify
            Assert.AreEqual(injectMock.Id, 100);
        }

        [Test]
        public void test_RegisterしたインスタンスがIgnoreInjectionの場合Injectで注入されないこと()
        {
            // setup
            _container.RegisterNew<IMockId, Mock>();
            var ignoreInjectionMock = new IgnoreInjectionMock();
            // exercise
            _container.Compile();
            _container.Inject(ignoreInjectionMock);
            // verify
            Assert.AreEqual(ignoreInjectionMock.Id, -1);
        }

        [Test]
        public void test_Injectでnullを渡してもエラーにならないこと()
        {
            // setup
            _container.Compile();
            // exercise
            _container.Inject(null);
        }

        [Test]
        public void test_同じTypeで複数のインスタンスを複数Inject出来ること()
        {
            // setup
            _container.RegisterNew<IMockId, Mock>().WithId("id_1");
            _container.RegisterNew<IMockId, Mock>(30).WithId("id_2");
            var injectIdMock = new InjectIdMock();
            // exercise
            _container.Compile();
            _container.Inject(injectIdMock);
            // verify
            var id1 = injectIdMock.Id1;
            var id2 = injectIdMock.Id2;
            Assert.AreEqual(id1, 100);
            Assert.AreEqual(id2, 30);
        }

        [Test]
        public void test_Dispose実行後DiContainerのIsCompiledがfalseになること()
        {
            // setup
            _container.RegisterNew<Mock>();
            _container.Compile();
            _container.RegisterNew<Mock>();
            // exercise
            _container.Dispose();
            // verify
            Assert.IsFalse(_container.IsCompiled);
        }

        [Test]
        public void test_Inject後にIInitializable_Initializedが呼ばれること()
        {
            // setup
            _container.RegisterNew<IMockId, Mock>(30);
            var injectMock = new InitializableInjectMock();
            // exercise
            _container.Compile();
            _container.Inject(injectMock);
            // verify
            Assert.IsTrue(injectMock.Initialized);
            var initializedId = injectMock.InitializedId;
            Assert.AreEqual(initializedId, 30);
        }

        public class InjectMock
        {
            [InjectField] private IMockId _mockId;

            public InjectMock() : this(null)
            {
            }

            public InjectMock(IMockId mockId)
            {
                _mockId = mockId;
            }

            public int Id => _mockId?.Id ?? -1;
        }

        public class InjectIdMock
        {
            [InjectField(Id = "id_1")] private IMockId _mockId1;
            [InjectField(Id = "id_2")] private IMockId MockId2 { get; }

            public InjectIdMock() : this(null, null)
            {
            }

            public InjectIdMock(IMockId mockId1, IMockId mockId2)
            {
                _mockId1 = mockId1;
                MockId2 = mockId2;
            }

            public int Id1 => _mockId1?.Id ?? -1;
            public int Id2 => MockId2?.Id ?? -1;
        }

        [IgnoreInjection]
        public class IgnoreInjectionMock
        {
            [InjectField] private IMockId _mockId;

            public IgnoreInjectionMock() : this(null)
            {
            }

            public IgnoreInjectionMock(IMockId mockId)
            {
                _mockId = mockId;
            }

            public int Id => _mockId?.Id ?? -1;
        }

        public class InitializableInjectMock : IInitializable
        {
            [InjectField] private IMockId _mockId;
            public bool Initialized { get; private set; }
            public int InitializedId { get; private set; }

            public int Id => _mockId?.Id ?? -1;

            public InitializableInjectMock() : this(null)
            {
            }

            public InitializableInjectMock(IMockId mockId)
            {
                _mockId = mockId;
            }

            public void Initialize()
            {
                Initialized = true;
                InitializedId = Id;
            }
        }
    }
}