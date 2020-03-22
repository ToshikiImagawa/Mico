// Mico.Context.Internal C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System.Collections.Generic;

namespace Mico.Context.Internal
{
    internal interface IContext
    {
        IContext ParentContext { get; }
        IEnumerable<IInstaller> Installers { get; }
        DiContainer Container { get; }
        void SetContainer(DiContainer container);
    }
}