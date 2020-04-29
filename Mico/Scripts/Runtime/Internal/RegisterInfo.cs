// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;

namespace Mico.Internal
{
    internal enum LifetimeType
    {
        Transient,
        Singleton
    }

    [IgnoreInjection]
    internal class RegisterInfo : IDisposable, IRegister
    {
        private readonly RegisterInfoPool _pool;
        public Type[] InjectedTypes;
        public Type InstanceType;
        public LifetimeType Lifetime;
        public Func<object> Factory;
        public object Id;
        public bool NonLazy;

        IRegister IRegister.WithId(object id)
        {
            Id = id;
            return this;
        }

        IRegister IRegister.AsSingle()
        {
            Lifetime = LifetimeType.Singleton;
            return this;
        }

        IRegister IRegister.AsTransient()
        {
            Lifetime = LifetimeType.Transient;
            return this;
        }

        IRegister IRegister.NonLazy()
        {
            NonLazy = true;
            return this;
        }

        IRegister IRegister.Lazy()
        {
            NonLazy = false;
            return this;
        }

        private RegisterInfo()
        {
            Reset();
        }

        public RegisterInfo(RegisterInfoPool pool) : this()
        {
            _pool = pool;
        }

        void IDisposable.Dispose()
        {
            _pool.Despawn(this);
        }

        public void Reset()
        {
            Lifetime = LifetimeType.Transient;
            InjectedTypes = null;
            InstanceType = null;
            NonLazy = false;
            Id = null;
            Factory = null;
        }
    }
}