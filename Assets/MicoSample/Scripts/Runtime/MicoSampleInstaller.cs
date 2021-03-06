// MicoSample C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using Mico;
using Mico.Context;

namespace MicoSample
{
    public class MicoSampleInstaller : MonoInstaller
    {
        public override void InstallRegisters(DiContainer container)
        {
            container.RegisterNew<IMicoSampleLogger, MicoSampleDebugLogger>();
            container.RegisterNew<MicoSceneLoader>("SubScene").NonLazy();
        }
    }
}