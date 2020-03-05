// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using UnityEngine;

namespace Mico.Context
{
    public abstract class MonoInstaller : MonoBehaviour, IInstaller
    {
        public abstract void InstallRegisters(DiContainer container);
    }
}