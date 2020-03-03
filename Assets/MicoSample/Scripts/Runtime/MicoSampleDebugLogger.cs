// MicoSample C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

namespace MicoSample
{
    public class MicoSampleDebugLogger : IMicoSampleLogger
    {
        public void Debug(string message, UnityEngine.Object context)
        {
            UnityEngine.Debug.Log($"[Mico Debug] message\n{message}", context);
        }

        public void Error(string message, UnityEngine.Object context)
        {
            UnityEngine.Debug.LogError($"[Mico Error] message\n{message}", context);
        }
    }
}