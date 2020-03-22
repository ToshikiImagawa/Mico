// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using System.Linq;
using System.Reflection;
using Mico.Internal;

namespace Mico
{
    public partial class DiContainer
    {
        public IRegister RegisterNew<T, TInstance>() where TInstance : T, new()
        {
            return RegisterNewBase<TInstance>(new[] {typeof(T)});
        }

        public IRegister RegisterNew<T>() where T : new()
        {
            return RegisterNewBase<T>();
        }

        public IRegister RegisterNew<T>(Type[] injectedTypes) where T : new()
        {
            var instanceType = typeof(T);
            MicoAssert.ImplementedAll(injectedTypes, instanceType);
            return RegisterNewBase<T>(injectedTypes);
        }

        public IRegister RegisterNew(Type instanceType)
        {
            var hitConstructorInfo = GetConstructor(instanceType, Array.Empty<Type>());
            return RegisterNewBase(instanceType, hitConstructorInfo, null);
        }

        public IRegister RegisterNew(Type[] injectedTypes, Type instanceType)
        {
            MicoAssert.ImplementedAll(injectedTypes, instanceType);
            var hitConstructorInfo = GetConstructor(instanceType, Array.Empty<Type>());
            return RegisterNewBase(injectedTypes, instanceType, hitConstructorInfo, null);
        }

        public IRegister RegisterNew<T, TInstance>(params object[] parameters) where TInstance : T
        {
            return RegisterNew(new[] {typeof(T)}, typeof(TInstance), parameters);
        }

        public IRegister RegisterNew<T>(params object[] parameters)
        {
            return RegisterNew(typeof(T), parameters);
        }

        public IRegister RegisterNew<T>(Type[] injectedTypes, params object[] parameters)
        {
            return RegisterNew(injectedTypes, typeof(T), parameters);
        }

        public IRegister RegisterNew(Type instanceType, params object[] parameters)
        {
            var parameterTypes = parameters.Select(param => param.GetType()).ToArray();
            var hitConstructorInfo = GetConstructor(instanceType, parameterTypes);
            return RegisterNewBase(instanceType, hitConstructorInfo, parameters);
        }

        public IRegister RegisterNew(Type[] injectedTypes, Type instanceType, params object[] parameters)
        {
            MicoAssert.ImplementedAll(injectedTypes, instanceType);

            var parameterTypes = parameters.Select(param => param.GetType()).ToArray();
            var hitConstructorInfo = GetConstructor(instanceType, parameterTypes);
            return RegisterNewBase(injectedTypes, instanceType, hitConstructorInfo, parameters);
        }

        public IRegister RegisterInstance<T>(object instance)
        {
            return RegisterInstanceBase(new[] {typeof(T)}, instance);
        }

        public IRegister RegisterInstance(object instance)
        {
            return RegisterInstanceBase(instance);
        }

        public IRegister RegisterInstance(Type[] injectedTypes, object instance)
        {
            var instanceType = instance.GetType();
            MicoAssert.ImplementedAll(injectedTypes, instanceType);

            return RegisterInstanceBase(injectedTypes, instance);
        }

        public IRegister RegisterFactory<T, TInstance>(Func<TInstance> factory) where TInstance : T
        {
            return RegisterFactoryBase(new[] {typeof(T)}, factory);
        }

        public IRegister RegisterFactory<T>(Func<T> factory)
        {
            return RegisterFactoryBase(factory);
        }

        public IRegister RegisterFactory<T>(Type[] injectedTypes, Func<T> factory)
        {
            var instanceType = typeof(T);
            MicoAssert.ImplementedAll(injectedTypes, instanceType);

            return RegisterFactoryBase(injectedTypes, factory);
        }

        public IRegister RegisterFactory(Type instanceType, Func<object> factory)
        {
            return RegisterFactoryBase(instanceType, () =>
            {
                var instance = factory.Invoke();
                MicoAssert.IsInstanceType(instance, instanceType);
                return instance;
            });
        }

        public IRegister RegisterFactory(Type[] injectedTypes, Type instanceType, Func<object> factory)
        {
            MicoAssert.ImplementedAll(injectedTypes, instanceType);

            return RegisterFactoryBase(injectedTypes, instanceType, () =>
            {
                var instance = factory.Invoke();
                MicoAssert.IsInstanceType(instance, instanceType);
                return instance;
            });
        }

        private RegisterInfo RegisterNewBase<T>()
            where T : new()
        {
            return RegisterNewBase<T>(new[] {typeof(T)});
        }

        private RegisterInfo RegisterNewBase<T>(Type[] injectedTypes)
            where T : new()
        {
            var registerInfo = GetRegisterInfo();
            registerInfo.Id = typeof(DefaultId);
            registerInfo.InjectedTypes = injectedTypes;
            registerInfo.InstanceType = typeof(T);
            registerInfo.Lifetime = LifetimeType.Transient;
            registerInfo.Factory = () => new T();
            registerInfo.NonLazy = false;
            return registerInfo;
        }

        private RegisterInfo RegisterNewBase(Type instanceType, ConstructorInfo constructorInfo,
            object[] parameters)
        {
            return RegisterNewBase(new[] {instanceType}, instanceType, constructorInfo, parameters);
        }

        private RegisterInfo RegisterNewBase(Type[] injectedTypes, Type instanceType, ConstructorInfo constructorInfo,
            object[] parameters)
        {
            var registerInfo = GetRegisterInfo();
            registerInfo.Id = typeof(DefaultId);
            registerInfo.InjectedTypes = injectedTypes;
            registerInfo.InstanceType = instanceType;
            registerInfo.Lifetime = LifetimeType.Transient;
            registerInfo.Factory = () => constructorInfo.Invoke(parameters);
            registerInfo.NonLazy = false;
            return registerInfo;
        }

        private RegisterInfo RegisterInstanceBase(object instance)
        {
            return RegisterInstanceBase(new[] {instance.GetType()}, instance);
        }

        private RegisterInfo RegisterInstanceBase(Type[] injectedTypes, object instance)
        {
            var registerInfo = GetRegisterInfo();
            registerInfo.Id = typeof(DefaultId);
            registerInfo.InjectedTypes = injectedTypes;
            registerInfo.InstanceType = instance.GetType();
            registerInfo.Lifetime = LifetimeType.Singleton;
            registerInfo.Factory = () => instance;
            registerInfo.NonLazy = false;
            return registerInfo;
        }

        private RegisterInfo RegisterFactoryBase<T>(Func<T> factory)
        {
            return RegisterFactoryBase(new[] {typeof(T)}, factory);
        }

        private RegisterInfo RegisterFactoryBase<T>(Type[] injectedTypes, Func<T> factory)
        {
            var registerInfo = GetRegisterInfo();
            registerInfo.Id = typeof(DefaultId);
            registerInfo.InjectedTypes = injectedTypes;
            registerInfo.InstanceType = typeof(T);
            registerInfo.Lifetime = LifetimeType.Singleton;
            registerInfo.Factory = () => factory.Invoke();
            registerInfo.NonLazy = false;
            return registerInfo;
        }

        private RegisterInfo RegisterFactoryBase(Type instanceType, Func<object> factory)
        {
            return RegisterFactoryBase(new[] {instanceType}, instanceType, factory);
        }

        private RegisterInfo RegisterFactoryBase(Type[] injectedTypes, Type instanceType, Func<object> factory)
        {
            var registerInfo = GetRegisterInfo();
            registerInfo.Id = typeof(DefaultId);
            registerInfo.InjectedTypes = injectedTypes;
            registerInfo.InstanceType = instanceType;
            registerInfo.Lifetime = LifetimeType.Singleton;
            registerInfo.Factory = factory;
            registerInfo.NonLazy = false;
            return registerInfo;
        }

        private static ConstructorInfo GetConstructor(Type instanceType, Type[] parameters)
        {
            var hitConstructorInfo = Util.Reflection.GetConstructor(instanceType, parameters);
            if (hitConstructorInfo != null) return hitConstructorInfo;
            MicoAssert.Throw($"Constructor {instanceType.FullName}() not found!");
            return null;
        }
    }
}