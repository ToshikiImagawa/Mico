// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using UnityEngine;

namespace Mico.Context.Internal
{
    public interface IGameObjectContextHelper
    {
        int GetInstanceId(Component component);
        T GetComponentInParentOnly<T>(Component component);
    }
}