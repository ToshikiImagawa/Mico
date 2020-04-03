// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

namespace Mico.Context.Internal
{
    internal interface IContextRepository
    {
        bool HasContext(int instanceId);
        IContext GetContext(int instanceId);
        void RemoveContext(int instanceId);
        bool SetContext(int instanceId, IContext context);
    }
}