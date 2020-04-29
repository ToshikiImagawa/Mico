// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

namespace Mico.Internal
{
    public interface IPool<out TValue> : IPool
    {
        TValue Spawn();
    }

    public interface IPool
    {
        void Despawn(object value);
        void Clear();
    }
}