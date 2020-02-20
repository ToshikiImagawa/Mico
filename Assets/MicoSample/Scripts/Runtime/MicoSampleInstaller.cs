// MicoSample C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using Mico;
using Mico.Unity;

namespace MicoSample
{
    public class MicoSampleInstaller : MonoInstaller
    {
        public override void InstallRegisters(DiContainer container)
        {
            container.RegisterType<IMicoSampleLogger, MicoSampleUnityLogger>();
        }
    }
}