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
    public class ContextTests
    {
        [Test]
        public void test_SetContainerでセットしたDiContainerをContainerで取得できること()
        {
            // setup
            IContext context = new GameObject().AddComponent<GameObjectContext>();
#if MICO_TEST_ADD_NSUBSTITUTE
            var container = Substitute.For<DiContainer>();
            // exercise
            context.SetContainer(container);
            var actual = context.Container;
            // verify
            Assert.AreEqual(actual, container);
#endif
        }

#if MICO_TEST_ADD_NSUBSTITUTE
        [Test]
        public void test_DestroyでセットしたDiContainerが削除されること()
        {
            // setup
            var gameObjectContext = new GameObject().AddComponent<GameObjectContext>();
            IContext context = gameObjectContext;
            var container = Substitute.For<DiContainer>();
            context.SetContainer(container);
            // exercise
            Object.DestroyImmediate(gameObjectContext);
            var actual = context.Container;
            // verify
            Assert.AreEqual(actual, container);
        }
#endif
    }
}