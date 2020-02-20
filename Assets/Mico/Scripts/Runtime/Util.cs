// Mico C# reference source
// Copyright (c) 2016-2020 COMCREATE. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Mico
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

    internal static class Extensions
    {
        public static T[] GetComponentsInScene<T>(this Component self)
        {
            return self.gameObject.scene.GetRootGameObjects()
                .SelectMany(gameObject => gameObject.GetComponentsInChildren<T>())
                .ToArray();
        }

        public static T[] GetComponentsInChildrenOnly<T>(this Component self)
        {
            return self.GetComponentsInChildren<T>().Where(component => !component.Equals(self)).ToArray();
        }

        public static T GetComponentInParentOnly<T>(this Component self)
        {
            return self.GetComponentsInParent<T>().FirstOrDefault(component => !component.Equals(self));
        }

        public static string FullName(this Component component)
        {
            if (component is Transform transform) return transform.FullName();
            return component.transform.FullName();
        }

        private static string FullName(this Transform transform)
        {
            var name = transform.name;
            var parent = transform.parent;
            while (parent != null)
            {
                name = $"{parent.name}/{name}";
                parent = parent.parent;
            }

            return name;
        }
    }
}