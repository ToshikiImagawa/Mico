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

        public static void Inject(object obj)
        {
            _container.Inject(obj);
        }

        static ContextContainer()
        {
            _container = new DiContainer();
            _container.RegisterNew<ISceneRepository, SceneRepository>().AsSingle();
            _container.RegisterNew<ISceneContextRepository, SceneContextRepository>().AsSingle();
            _container.RegisterNew<ISceneContextService, SceneContextService>();
            _container.RegisterNew<ISceneContextHelper, SceneContextHelper>();
            _container.RegisterNew<IGameObjectContextRepository, GameObjectContextRepository>();
            _container.RegisterNew<IGameObjectContextService, GameObjectContextService>();
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