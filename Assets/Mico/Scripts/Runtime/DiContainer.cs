// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mico.Internal;

namespace Mico
{
    [IgnoreInjection]
    public partial class DiContainer : IDisposable
    {
        private readonly RegisterInfoPool _registerInfoPool;
        private readonly ListPool<FactoryTuple> _factoryTupleListPool;
        private readonly ListPool<Func<object>> _factoryListPool;
        private readonly Dictionary<(Type, object), object> _cache;
        private DiContainer _parentContainer;
        private DiContainer[] _ancestorContainers;

        private readonly Queue<RegisterInfo> _registerInfos = new Queue<RegisterInfo>();
        private FactoryTable _factoryTable;

        public bool IsCompiled => _factoryTable != null;

        public DiContainer() : this(null)
        {
        }

        public DiContainer(DiContainer parentContainer)
        {
            _parentContainer = parentContainer;
            _ancestorContainers = GetAncestorContainers().ToArray();
            var rootParent = _ancestorContainers.LastOrDefault();
            if (rootParent != null)
            {
                _registerInfoPool = rootParent._registerInfoPool;
                _factoryTupleListPool = rootParent._factoryTupleListPool;
                _factoryListPool = rootParent._factoryListPool;
                _cache = rootParent._cache;
            }
            else
            {
                _registerInfoPool = new RegisterInfoPool();
                _factoryTupleListPool = new ListPool<FactoryTuple>();
                _factoryListPool = new ListPool<Func<object>>();
                _cache = new Dictionary<(Type, object), object>();
            }
        }

        public void Inject(object obj)
        {
            if (obj == null) return;
            if (obj.GetType().GetCustomAttribute<IgnoreInjectionAttribute>() != null) return;
            var setters = Util.Reflection.GetAllSetter(obj, GetFactory);
            foreach (var setter in setters)
            {
                setter?.Invoke();
            }
        }

        private Func<object> GetFactory(Type fieldType, object id)
        {
            {
                if (_factoryTable.TryGet(fieldType, id, out var factory))
                {
                    return factory;
                }
            }
            foreach (var ancestorContainer in _ancestorContainers)
            {
                if (ancestorContainer._factoryTable.TryGet(fieldType, id, out var factory))
                {
                    return factory;
                }
            }

            MicoAssert.Throw($"Type or ID was not found. : Type = {fieldType.FullName}, ID = {id}");
            return null;
        }

        private RegisterInfo GetRegisterInfo()
        {
            var registerInfo = _registerInfoPool.Spawn();
            registerInfo.Reset();
            _registerInfos.Enqueue(registerInfo);
            return registerInfo;
        }

        public void Compile()
        {
            if (IsCompiled) MicoAssert.Throw("Already compiled !");
            _parentContainer?.Resolve();

            var factoryTupleList = _factoryTupleListPool.Spawn();
            factoryTupleList.Clear();
            var nonLazyList = _factoryListPool.Spawn();
            nonLazyList.Clear();
            while (_registerInfos.Count > 0)
            {
                using (var registerInfo = _registerInfos.Dequeue())
                {
                    Func<object> factory;
                    var (type, id) = (registerInfo.InstanceType, registerInfo.Id);
                    var innerFactory = registerInfo.Factory;
                    switch (registerInfo.Lifetime)
                    {
                        case LifetimeType.Transient:
                        {
                            factory = () =>
                            {
                                var instance = innerFactory();
                                Inject(instance);
                                return instance;
                            };
                            break;
                        }
                        case LifetimeType.Singleton:
                        {
                            factory = () =>
                            {
                                if (_cache.ContainsKey((type, id)))
                                {
                                    return _cache[(type, id)];
                                }

                                var instance = innerFactory();
                                Inject(instance);
                                _cache[(type, id)] = instance;
                                return instance;
                            };
                            break;
                        }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (registerInfo.NonLazy) nonLazyList.Add(factory);
                    factoryTupleList.AddRange(registerInfo.InjectedTypes.Select(injectedType =>
                        FactoryTuple.Create(injectedType, registerInfo.Id, factory)));
                }
            }

            _factoryTable = FactoryTable.Create(factoryTupleList.ToArray());
            _factoryTupleListPool.Despawn(factoryTupleList);

            foreach (var func in nonLazyList)
            {
                func?.Invoke();
            }

            _factoryListPool.Despawn(nonLazyList);
        }

        public T Resolve<T>()
        {
            return Resolve<T>(typeof(DefaultId));
        }

        public T Resolve<T>(object id)
        {
            return (T) Resolve(typeof(T), id);
        }

        public object Resolve(Type type)
        {
            return Resolve(type, typeof(DefaultId));
        }

        public object Resolve(Type type, object id)
        {
            Resolve();
            return GetFactory(type, id)();
        }

        public void Dispose()
        {
            if (_parentContainer == null)
            {
                _registerInfoPool.Clear();
                _factoryTupleListPool.Clear();
                _factoryListPool.Clear();
                _cache.Clear();
            }

            while (_registerInfos.Count > 0)
            {
                IDisposable registerInfo = _registerInfos.Dequeue();
                registerInfo.Dispose();
            }

            _registerInfos.Clear();
            _parentContainer = null;
            _ancestorContainers = new DiContainer[0];
            _factoryTable = null;
        }

        private void Resolve()
        {
            if (IsCompiled) return;
            Compile();
        }

        private IEnumerable<DiContainer> GetAncestorContainers()
        {
            if (_parentContainer == null) yield break;
            var current = _parentContainer;
            while (current != null)
            {
                yield return current;
                current = current._parentContainer;
            }
        }
    }
}