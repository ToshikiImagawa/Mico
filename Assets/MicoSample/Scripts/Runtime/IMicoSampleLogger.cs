// MicoSample C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

namespace MicoSample
{
    public interface IMicoSampleLogger
    {
        void Debug(string message, UnityEngine.Object context);

        void Error(string message, UnityEngine.Object context);
    }
}