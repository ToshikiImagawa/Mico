// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Mico
{
    public class DiContainer
    {
        private readonly DiContainer _parentContainer;

        private readonly List<ConcreteRegisterBase> _concreteRegisters = new List<ConcreteRegisterBase>();
        private FactoryTable _factoryTable;

        public bool IsCompiled => _factoryTable != null;

        private FactoryTable FactoryTable
        {
            get
            {
                if (_factoryTable != null) return _factoryTable;
                Compile();
                return _factoryTable;
            }
        }

        public DiContainer()
        {
            _parentContainer = null;
        }

        public DiContainer(DiContainer parentContainer)
        {
            _parentContainer = parentContainer;
        }

        public void Inject(object obj)
        {
            if (obj == null) return;
            var setters = Util.Reflection.GetAllSetter(obj, GetFactory);
            foreach (var setter in setters)
            {
                setter?.Invoke();
            }
        }

        private Func<object> GetFactory(Type fieldType, object id)
        {
            if (_parentContainer == null)
            {
                return id == null
                    ? FactoryTable.Get(fieldType)
                    : FactoryTable.Get(fieldType, id);
            }

            Func<object> factory;
            if (id == null
                ? FactoryTable.TryGet(fieldType, out factory)
                : FactoryTable.TryGet(fieldType, id, out factory))
            {
                return factory;
            }

            return _parentContainer.GetFactory(fieldType, id);
        }

        public void Compile()
        {
            if (IsCompiled) MicoAssert.Throw("Already compiled !");
            var items = _concreteRegisters.ToArray();
            _concreteRegisters.Clear();
            var nonLazies = new Queue<FactoryTuple>();
            var factoryTuples = items.Select(item =>
            {
                var factoryTuple = CreateFactoryTuple(item);
                if (!item.Info.IsLazy) nonLazies.Enqueue(factoryTuple);
                return factoryTuple;
            }).ToArray();
            while (nonLazies.Count > 0)
            {
                var factoryTuple = nonLazies.Dequeue();
                factoryTuple.Factory();
            }
            _factoryTable = FactoryTable.Create(factoryTuples);
        }

        public ConcreteWithIdRegister<TInjected> RegisterType<TInjected, TInjectedImpl>()
            where TInjectedImpl : TInjected, new()
        {
            if (IsCompiled)
            {
                MicoAssert.Throw(
                    $"Already compiled, can’t register new type when compile finished. type = {typeof(TInjected).FullName}.");
            }

            var item = new ConcreteWithIdRegister<TInjected>(() => new TInjectedImpl());
            _concreteRegisters.Add(item);
            return item;
        }

        public ConcreteWithIdRegister<TInjected> RegisterType<TInjected>(Func<TInjected> factory)
        {
            if (IsCompiled)
            {
                MicoAssert.Throw(
                    $"Already compiled, can’t register new type when compile finished. type = {typeof(TInjected).FullName}.");
            }

            var item = new ConcreteWithIdRegister<TInjected>(factory);
            _concreteRegisters.Add(item);
            return item;
        }

        public ConcreteWithIdRegister RegisterType(Type type, Func<object> factory)
        {
            if (IsCompiled)
            {
                MicoAssert.Throw(
                    $"Already compiled, can’t register new type when compile finished. type = {type.FullName}.");
            }

            var item = new ConcreteWithIdRegister(type, factory);
            _concreteRegisters.Add(item);
            return item;
        }

        public T ResolveType<T>() where T : new()
        {
            var instance = new T();
            Inject(instance);
            return instance;
        }

        private FactoryTuple CreateFactoryTuple(ConcreteRegisterBase register)
        {
            Func<object> factory;

            switch (register.Info.Lifetime)
            {
                case LifetimeType.Transient:
                {
                    factory = () =>
                    {
                        var item = register.Info.Constructor.Factory();
                        Inject(item);
                        return item;
                    };
                    break;
                }
                case LifetimeType.Singleton:
                {
                    var lazy = new Lazy(() =>
                    {
                        var item = register.Info.Constructor.Factory();
                        Inject(item);
                        return item;
                    });
                    factory = () => lazy;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return register.Info.Id == null
                ? FactoryTuple.Create(register.Info.InjectedType, factory)
                : FactoryTuple.Create(register.Info.InjectedType, register.Info.Id, factory);
        }

        public class Pool
        {
            private readonly Dictionary<Type, Queue<object>> _cache = new Dictionary<Type, Queue<object>>();
            private static readonly Pool Instance;

            static Pool()
            {
                Instance = new Pool();
            }

            public static T Spawn<T>() where T : class, new()
            {
                return Instance.__Spawn<T>();
            }

            public static T Spawn<T>(Func<T> factory) where T : class
            {
                return Instance.__Spawn(factory);
            }

            public static void Despawn<T>(T value) where T : class
            {
                Instance.__Despawn(value);
            }

            public static void ClearAll()
            {
                Instance.__ClearAll();
            }

            private T __Spawn<T>() where T : class, new()
            {
                return __Spawn(() => new T());
            }

            private T __Spawn<T>(Func<T> factory) where T : class
            {
                var type = typeof(T);
                if (!_cache.ContainsKey(type)) _cache[type] = new Queue<object>();
                if (_cache[type].Count > 0) return _cache[type].Dequeue() as T;
                return factory();
            }

            private void __Despawn<T>(T value) where T : class
            {
                var type = typeof(T);
                if (!_cache.ContainsKey(type)) _cache[type] = new Queue<object>();
                if (!_cache[type].Contains(value)) _cache[type].Enqueue(value);
            }

            private void __ClearAll()
            {
                var items = _cache.Values.ToArray();
                _cache.Clear();
                foreach (var queue in items)
                {
                    while (queue.Count > 0)
                    {
                        if (queue.Dequeue() is IDisposable disposable) disposable.Dispose();
                    }
                }
            }
        }
    }
}