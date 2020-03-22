// MicoContextTest C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using Mico;
using Mico.Context;
using Mico.Context.Internal;
using NSubstitute;
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
            var container = Substitute.For<DiContainer>();
            // exercise
            context.SetContainer(container);
            var actual = context.Container;
            // verify
            Assert.AreEqual(actual, container);
        }
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
    }
}