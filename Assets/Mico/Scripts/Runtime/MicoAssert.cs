// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

namespace Mico
{
    internal static class MicoAssert
    {
        public static void Throw(string message)
        {
#if UNITY_EDITOR || !MICO_STRIP_ASSERTS
            throw CreateException(message);
#endif
        }

        public static MicoException CreateException(string message)
        {
            return new MicoException(message);
        }
    }
}