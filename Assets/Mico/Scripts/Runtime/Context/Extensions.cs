using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mico.Context
{
    internal static class Extensions
    {
        public static T[] GetComponentsInScene<T>(this Scene self)
        {
            return self.GetRootGameObjects()
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