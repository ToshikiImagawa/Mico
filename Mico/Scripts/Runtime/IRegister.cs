// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

namespace Mico
{
    public interface IRegister
    {
        IRegister WithId(object id);
        IRegister AsSingle();
        IRegister AsTransient();
        IRegister NonLazy();
        IRegister Lazy();
    }
}