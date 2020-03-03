// MicoTest C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using Mico;
using NUnit.Framework;

namespace MicoTest
{
    public class DiContainerErrorTests
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
        public void test_実装していない複数のInterfaceがRegisterNew_Genericで登録出来ないこと()
        {
            // setup
            MicoException error = null;
            // exercise
            try
            {
                // setup
                _container.RegisterNew<PlainMock>(new[] {typeof(IMockId), typeof(IMockName)});
            }
            catch (MicoException e)
            {
                error = e;
            }

            // verify
            Assert.IsNotNull(error);
        }

        [Test]
        public void test_引数なしのClassをRegisterNew_Genericで登録出来ないこと()
        {
            // setup
            MicoException error = null;
            // exercise
            try
            {
                // setup
                _container.RegisterNew<ArgumentsMock>();
            }
            catch (MicoException e)
            {
                error = e;
            }

            // verify
            Assert.IsNotNull(error);
        }

        [Test]
        public void test_実装していない複数のInterfaceがRegisterNewで登録出来ないこと()
        {
            // setup
            MicoException error = null;
            // exercise
            try
            {
                // setup
                _container.RegisterNew(new[] {typeof(IMockId), typeof(IMockName)}, typeof(PlainMock));
            }
            catch (MicoException e)
            {
                error = e;
            }

            // verify
            Assert.IsNotNull(error);
        }

        [Test]
        public void test_実装していないBaseClassがRegisterNewで登録出来ないこと()
        {
            // setup
            MicoException error = null;
            // exercise
            try
            {
                // setup
                _container.RegisterNew(new[] {typeof(Mock)}, typeof(PlainMock));
            }
            catch (MicoException e)
            {
                error = e;
            }

            // verify
            Assert.IsNotNull(error);
        }

        [Test]
        public void test_引数なしのClassをRegisterNewで登録出来ないこと()
        {
            // setup
            MicoException error = null;
            // exercise
            try
            {
                // setup
                _container.RegisterNew(typeof(ArgumentsMock));
            }
            catch (MicoException e)
            {
                error = e;
            }

            // verify
            Assert.IsNotNull(error);
        }

        #endregion

        #region RegisterFactory

        [Test]
        public void test_RegisterFactoryで返すインスタンスがinstanceTypeと異なる時Resolve時にエラーになること()
        {
            // setup
            MicoException error = null;
            _container.RegisterFactory(new[] {typeof(IMockId), typeof(IMockName)}, typeof(Mock),
                () => new PlainMock());
            // exercise
            try
            {
                // setup
                var mock = _container.Resolve<IMockId>();
            }
            catch (MicoException e)
            {
                error = e;
            }

            // verify
            Assert.IsNotNull(error);
        }

        #endregion

        [Test]
        public void test_Registerで登録していないClassをResolveで取得出来ないこと()
        {
            // setup
            MicoException error = null;
            // exercise
            try
            {
                var mock = _container.Resolve(typeof(Mock)) as Mock;
            }
            catch (MicoException e)
            {
                error = e;
            }

            // verify
            Assert.IsNotNull(error);
        }

        [Test]
        public void test_RegisterでId登録していないClassをResolveで取得出来ないこと()
        {
            // setup
            MicoException error = null;
            // exercise
            try
            {
                var mock = _container.Resolve(typeof(Mock)) as Mock;
            }
            catch (MicoException e)
            {
                error = e;
            }

            // verify
            Assert.IsNotNull(error);
        }

        [Test]
        public void test_WithIdでRegisterで登録していないClassをResolveで取得出来ないこと()
        {
            // setup
            MicoException error = null;
            _container.RegisterNew(typeof(Mock));
            // exercise
            try
            {
                var mock = _container.Resolve(typeof(Mock), "mock_1") as Mock;
            }
            catch (MicoException e)
            {
                error = e;
            }

            // verify
            Assert.IsNotNull(error);
        }

        [Test]
        public void test_Compileを複数回以上した時にエラーになること()
        {
            // setup
            MicoException error = null;
            // exercise
            try
            {
                _container.Compile();
                _container.Compile();
            }
            catch (MicoException e)
            {
                error = e;
            }

            // verify
            Assert.IsNotNull(error);
        }

        public class PlainMock
        {
        }

        public class ArgumentsMock
        {
            public ArgumentsMock(int id, string name)
            {
            }
        }
    }
}