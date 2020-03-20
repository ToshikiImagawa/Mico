// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

namespace Mico.Context.Internal
{
    internal static class ContextContainer
    {
        private static DiContainer _container;

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        static ContextContainer()
        {
            _container = new DiContainer();
            _container.RegisterNew<ISceneRepository, SceneRepository>().AsSingle();
            _container.RegisterNew<IContextRepository, ContextRepository>()
                .WithId(typeof(SceneContextService))
                .AsSingle();
            _container.RegisterNew<ISceneContextService, SceneContextService>();
            _container.RegisterNew<ISceneContextHelper, SceneContextHelper>();
            _container.RegisterNew<IContextRepository, ContextRepository>()
                .WithId(typeof(GameObjectContextService))
                .AsSingle();
            _container.RegisterNew<IGameObjectContextService, GameObjectContextService>();
            _container.RegisterNew<IGameObjectContextHelper, GameObjectContextHelper>();
            _container.Compile();
        }

        public static void Swap(DiContainer container)
        {
            _container = container;
        }

        public static void Dispose()
        {
            _container.Dispose();
            _container = null;
        }
    }
}