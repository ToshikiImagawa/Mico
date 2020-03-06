// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

namespace Mico.Context.Internal
{
    internal interface IGameObjectContextRepository
    {
        bool HasGameObjectContext(int instanceId);
        IContext GetGameObjectContext(int instanceId);
        void RemoveGameObjectContext(int instanceId);
        bool SetGameObjectContext(int instanceId, IContext context);
    }
}