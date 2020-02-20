// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;

namespace Mico
{
    public delegate object FactoryMethod();

    public delegate object MemberSetterMethod(object target, object value);

    public enum LifetimeType
    {
        Transient,
        Singleton
    }

    public class RegisterInfo : IDisposable
    {
        public LifetimeType Lifetime;
        public Type InjectedType;
        public object Id = null;
        public ConstructorInfo Constructor;
        public bool IsLazy;

        void IDisposable.Dispose()
        {
            DiContainer.Pool.Despawn(this);
        }

        private void Reset()
        {
            Lifetime = LifetimeType.Transient;
            InjectedType = null;
            IsLazy = false;
            Id = null;
            Constructor = new ConstructorInfo(null);
        }

        public class ConstructorInfo
        {
            public readonly FactoryMethod Factory;

            public ConstructorInfo(FactoryMethod factory)
            {
                Factory = factory;
            }
        }
    }
}