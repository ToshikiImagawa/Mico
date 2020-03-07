// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

namespace Mico.Context.Internal
{
    internal interface ISceneContextRepository
    {
        bool HasSceneContext(int handle);
        IContext GetSceneContext(int handle);
        void RemoveSceneContext(int handle);
        bool SetSceneContext(int handle, IContext context);
    }
}