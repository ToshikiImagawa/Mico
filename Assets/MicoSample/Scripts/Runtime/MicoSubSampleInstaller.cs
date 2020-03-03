// MicoSample C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using Mico;
using Mico.Context;

namespace MicoSample
{
    public class MicoSubSampleInstaller : MonoInstaller
    {
        public override void InstallRegisters(DiContainer container)
        {
#if UNITY_EDITOR
            container.RegisterNew<IMicoSampleLogger, MicoSampleFileLogger>("./log.txt");
#else
            container.RegisterNew<IMicoSampleLogger, MicoSampleDebugLogger>();
#endif
        }
    }
}