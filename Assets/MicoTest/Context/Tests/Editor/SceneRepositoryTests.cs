// MicoContextTest C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using Mico;
using Mico.Context.Internal;
using NSubstitute;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace MicoContextTest
{
    public class SceneRepositoryTests
    {
        private MicoSceneManager _micoSceneManager;

        [SetUp]
        public void Setup()
        {
            var container = new DiContainer();
#if MICO_TEST_ADD_NSUBSTITUTE
            _micoSceneManager = Substitute.For<MicoSceneManager>();
#endif
            container.RegisterNew<ISceneRepository, SceneRepository>().AsSingle();
            container.RegisterInstance<MicoSceneManager>(_micoSceneManager);
            container.Compile();
            ContextContainer.Swap(container);
        }

        [Test]
        public void test_GetCacheScene実行時にGetSceneByPathがtrueの時をSceneが取得できること()
        {
            // setup
            var sceneRepository = ContextContainer.Resolve<ISceneRepository>();
            _micoSceneManager.GetSceneByPath(Arg.Any<string>(), out var scene)
                .ReturnsForAnyArgs(true);
            const string scenePath = "scenePath";
            // exercise
            var actual = sceneRepository.GetCacheScene(scenePath);
            // verify
            Assert.IsTrue(actual.HasValue);
        }

        [Test]
        public void test_GetCacheScene実行時にGetSceneByPathがfalseの時をSceneが取得できないこと()
        {
            // setup
            var sceneRepository = ContextContainer.Resolve<ISceneRepository>();
            _micoSceneManager.GetSceneByPath(Arg.Any<string>(), out var scene)
                .ReturnsForAnyArgs(false);
            const string scenePath = "scenePath";
            Exception exception = null;
            // exercise
            try
            {
                sceneRepository.GetCacheScene(scenePath);
            }
            catch (Exception e)
            {
                exception = e;
            }

            // verify
            Assert.IsNotNull(exception);
        }

        [Test]
        public void test_GetCacheScene実行時にキャッシュがある場合GetSceneByPathがfalseの時にSceneが取得できること()
        {
            // setup
            var sceneRepository = ContextContainer.Resolve<ISceneRepository>();
            _micoSceneManager.GetSceneByPath(Arg.Any<string>(), out var scene1)
                .ReturnsForAnyArgs(true);
            const string scenePath = "scenePath";
            sceneRepository.GetCacheScene(scenePath);
            _micoSceneManager.GetSceneByPath(Arg.Any<string>(), out var scene2)
                .ReturnsForAnyArgs(false);
            // exercise
            var actual = sceneRepository.GetCacheScene(scenePath);
            // verify
            Assert.IsTrue(actual.HasValue);
        }
        
        [Test]
        public void test_Dispose実行時にキャッシュされたシーンが削除せれていること()
        {
            // setup
            var sceneRepository = ContextContainer.Resolve<ISceneRepository>();
            _micoSceneManager.GetSceneByPath(Arg.Any<string>(), out var scene1)
                .ReturnsForAnyArgs(true);
            const string scenePath = "scenePath";
            sceneRepository.GetCacheScene(scenePath);
            _micoSceneManager.GetSceneByPath(Arg.Any<string>(), out var scene2)
                .ReturnsForAnyArgs(false);
            Exception exception = null;
            // exercise
            ContextContainer.Dispose();
            try
            {
                sceneRepository.GetCacheScene(scenePath);
            }
            catch (Exception e)
            {
                exception = e;
            }

            // verify
            Assert.IsNotNull(exception);
        }
    }
}