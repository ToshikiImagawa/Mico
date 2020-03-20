// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using UnityEngine;

namespace Mico.Context.Internal
{
    public class GameObjectContextHelper : IGameObjectContextHelper
    {
        public int GetInstanceId(Component component)
        {
            return component == null ? 0 : component.GetInstanceID();
        }

        public T GetComponentInParentOnly<T>(Component component)
        {
            return component == null ? default : component.GetComponentInParentOnly<T>();
        }
    }
}