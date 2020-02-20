// Mico C# reference source
// Copyright (c) 2016-2020 COMCREATE. All rights reserved.

using UnityEngine;

namespace Mico.Unity
{
    public abstract class MonoInstaller : MonoBehaviour, IInstaller
    {
        public abstract void InstallRegisters(DiContainer container);
    }

    public interface IInstaller
    {
        void InstallRegisters(DiContainer container);
    }
}