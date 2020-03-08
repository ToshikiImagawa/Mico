// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using UnityEngine;

namespace Mico.Context.Internal
{
    internal interface IGameObjectContextService
    {
        IContext GetGameObjectContextOrDefault(Component component, IContext defaultContext);
    }
}