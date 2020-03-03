// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mico.Internal
{
    internal static class Util
    {
        public static class Reflection
        {
            private const BindingFlags AllInstanceBindingFlags =
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

            public static int HashCodeCombine<T1, T2>(T1 item, T2 item2)
            {
                unchecked
                {
                    var hashCode = item.GetHashCode();
                    hashCode = (hashCode * 397) ^ item2.GetHashCode();
                    return hashCode;
                }
            }

            public static IEnumerable<Action> GetAllSetter(object obj, Func<Type, object, Func<object>> factory)
            {
                var type = obj.GetType();
                if (type.GetCustomAttribute<IgnoreInjectionAttribute>() != null) return Enumerable.Empty<Action>();
                var injects = GetAllInjects(type);
                return injects.Select(inject =>
                {
                    var (fieldInfo, id) = inject;
                    return new Action(() =>
                    {
                        var value = factory(fieldInfo.FieldType, id)();
                        fieldInfo.SetValue(obj, value);
                    });
                }).ToArray();
            }

            public static bool IsImplemented(Type instanceType, Type implementedType)
            {
                if (instanceType == implementedType) return true;
                if (implementedType.IsInterface)
                {
                    return IsFindInterface(instanceType, implementedType);
                }

                if (implementedType.IsClass)
                {
                    return IsFindAbstract(instanceType, implementedType);
                }

                return false;
            }

            public static bool ImplementedAll(IEnumerable<Type> injectedTypes, Type instanceType)
            {
                return injectedTypes.All(injectedType => IsImplemented(instanceType, injectedType));
            }

            public static ConstructorInfo GetConstructor(Type self, Type[] parameters)
            {
                return self.GetConstructor(AllInstanceBindingFlags, null, parameters, null);
            }

            private static bool IsFindInterface(Type instanceType, Type findInterfaceType)
            {
                var interfaceTypes = instanceType.GetInterfaces();
                foreach (var interfaceType in interfaceTypes)
                {
                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == findInterfaceType)
                        return true;
                    if (interfaceType == findInterfaceType) return true;
                }

                return false;
            }

            private static bool IsFindAbstract(Type instanceType, Type findAbstractType)
            {
                var baseType = instanceType.BaseType;
                while (baseType != null)
                {
                    if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == findAbstractType) return true;
                    if (baseType == findAbstractType) return true;
                    baseType = baseType.BaseType;
                }

                return false;
            }

            private static IEnumerable<(FieldInfo, object)> GetAllInjects(IReflect type)
            {
                return GetAllInjectFields(type).Union(GetAllInjectProperties(type)).ToArray();
            }

            private static IEnumerable<(FieldInfo, object)> GetAllInjectFields(IReflect type)
            {
                return type.GetFields(AllInstanceBindingFlags)
                    .Where(field => field.GetCustomAttribute<InjectFieldAttribute>() != null)
                    .Select(field => (field, field.GetCustomAttribute<InjectFieldAttribute>().Id)).ToArray();
            }

            private static IEnumerable<(FieldInfo, object)> GetAllInjectProperties(IReflect type)
            {
                var backingFields = type.GetProperties(AllInstanceBindingFlags)
                    .Where(property => property.GetCustomAttribute<InjectFieldAttribute>() != null)
                    .Select(property => (
                        backingFieldName: BackingField(property.Name),
                        id: property.GetCustomAttribute<InjectFieldAttribute>().Id)
                    ).ToArray();

                return (from fieldInfo in type.GetFields(AllInstanceBindingFlags)
                    from item in backingFields
                    where fieldInfo.Name == item.backingFieldName
                    select (fieldInfo, item.id)).ToArray();
            }

            private static string BackingField(string propertyName)
            {
                return $"<{propertyName}>k__BackingField";
            }
        }
    }
}