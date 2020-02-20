// MicoSample C# reference source
// Copyright (c) 2016-2020 COMCREATE. All rights reserved.

namespace MicoSample
{
    public class MicoSampleUnityLogger : IMicoSampleLogger
    {
        public void Debug(string message)
        {
            UnityEngine.Debug.Log($"[Mico Debug] message\n{message}");
        }

        public void Error(string message)
        {
            UnityEngine.Debug.LogError($"[Mico Debug] message\n{message}");
        }
    }
}