// MicoContextTest C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using Mico.Context;
using Mico.Context.Internal;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace MicoContextTest
{
    public class ContextRepositoryTests
    {
        private ContextRepository _gameObjectContextRepository;

        [SetUp]
        public void Setup()
        {
            _gameObjectContextRepository = new ContextRepository();
        }

        [Test]
        public void test_SetContext実行時に未設定のGameObjectContextをセットできること()
        {
            // setup
            var component = new GameObject().AddComponent<GameObjectContext>();
            // exercise
            var actual = _gameObjectContextRepository.SetContext(component.GetInstanceID(), component);
            // verify
            Assert.IsTrue(actual);
        }

        [Test]
        public void test_SetContext実行時に設定済みのGameObjectContextをセットできないこと()
        {
            // setup
            var component = new GameObject().AddComponent<GameObjectContext>();
            _gameObjectContextRepository.SetContext(component.GetInstanceID(), component);
            // exercise
            var actual = _gameObjectContextRepository.SetContext(component.GetInstanceID(), component);
            // verify
            Assert.IsFalse(actual);
        }

        [Test]
        public void test_HasContext実行時に未設定のGameObjectContextのInstanceIdを渡した時falseがかえること()
        {
            // setup
            const int instanceId = 222;
            // exercise
            var actual = _gameObjectContextRepository.HasContext(instanceId);
            // verify
            Assert.IsFalse(actual);
        }

        [Test]
        public void test_HasContext実行時に設定済みのGameObjectContextのInstanceIdを渡した時trueがかえること()
        {
            // setup
            var component = new GameObject().AddComponent<GameObjectContext>();
            _gameObjectContextRepository.SetContext(component.GetInstanceID(), component);
            // exercise
            var actual = _gameObjectContextRepository.HasContext(component.GetInstanceID());
            // verify
            Assert.IsTrue(actual);
        }

        [Test]
        public void test_GetContext実行時に設定済みのGameObjectContextのInstanceIdを渡した時設定済みのGameObjectContextがかえること()
        {
            // setup
            var component = new GameObject().AddComponent<GameObjectContext>();
            _gameObjectContextRepository.SetContext(component.GetInstanceID(), component);
            // exercise
            var actual = _gameObjectContextRepository.GetContext(component.GetInstanceID());
            // verify
            Assert.AreEqual(actual, component);
        }

        [Test]
        public void
            test_RemoveContext実行時に設定済みのGameObjectContextのInstanceIdを渡した時設定済みのGameObjectContextが削除されてること()
        {
            // setup
            var component = new GameObject().AddComponent<GameObjectContext>();
            _gameObjectContextRepository.SetContext(component.GetInstanceID(), component);
            // exercise
            _gameObjectContextRepository.RemoveContext(component.GetInstanceID());
            var actual = _gameObjectContextRepository.HasContext(component.GetInstanceID());
            // verify
            Assert.IsFalse(actual);
        }
    }
}