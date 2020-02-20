// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;

namespace Mico
{
    public class ConcreteWithIdRegister<TInjected> : ConcreteSingleRegister<TInjected>
    {
        internal ConcreteWithIdRegister(Func<TInjected> factory) : base(factory)
        {
        }

        public ConcreteSingleRegister<TInjected> WithId(object id)
        {
            Info.Id = id;
            return this;
        }
    }

    public class ConcreteSingleRegister<TInjected> : ConcreteRegister<TInjected>
    {
        internal ConcreteSingleRegister(Func<TInjected> factory) : base(factory)
        {
        }

        public ConcreteRegister<TInjected> AsSingle()
        {
            Info.Lifetime = LifetimeType.Singleton;
            return this;
        }
    }

    public class ConcreteRegister<TInjected> : ConcreteRegisterBase
    {
        internal ConcreteRegister(Func<TInjected> factory)
        {
            Info = DiContainer.Pool.Spawn<RegisterInfo>();
            Info.InjectedType = typeof(TInjected);
            Info.Constructor = new RegisterInfo.ConstructorInfo(() => factory.Invoke());
        }

        public ConcreteRegisterBase Lazy()
        {
            Info.IsLazy = true;
            return this;
        }
    }

    public class ConcreteWithIdRegister : ConcreteSingleRegister
    {
        internal ConcreteWithIdRegister(Type type, Func<object> factory) : base(type, factory)
        {
        }

        public ConcreteSingleRegister WithId(object id)
        {
            Info.Id = id;
            return this;
        }
    }

    public class ConcreteSingleRegister : ConcreteRegister
    {
        internal ConcreteSingleRegister(Type type, Func<object> factory) : base(type, factory)
        {
        }

        public ConcreteRegister AsSingle()
        {
            Info.Lifetime = LifetimeType.Singleton;
            return this;
        }
    }

    public class ConcreteRegister : ConcreteRegisterBase
    {
        internal ConcreteRegister(Type type, Func<object> factory)
        {
            Info = DiContainer.Pool.Spawn<RegisterInfo>();
            Info.InjectedType = type;
            Info.Constructor = new RegisterInfo.ConstructorInfo(() =>
            {
                var value = factory();
                if (Info.InjectedType.IsInstanceOfType(value))
                    MicoAssert.Throw(
                        $"Type does not match. Value type = {value.GetType().FullName}, expected type = {Info.InjectedType.FullName}");
                return value;
            });
        }

        public ConcreteRegisterBase Lazy()
        {
            Info.IsLazy = true;
            return this;
        }
    }

    public class ConcreteRegisterBase : IDisposable
    {
        public RegisterInfo Info { get; protected set; }

        public void Dispose()
        {
            ((IDisposable) Info)?.Dispose();
        }
    }
}