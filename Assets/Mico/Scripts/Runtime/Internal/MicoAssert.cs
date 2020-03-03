// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using System.Collections.Generic;

namespace Mico.Internal
{
    internal static class MicoAssert
    {
        public static void Throw(string message)
        {
#if UNITY_EDITOR || !MICO_STRIP_ASSERTS
            throw CreateException(message);
#endif
        }

        public static void ImplementedAll(IEnumerable<Type> injectedTypes, Type instanceType)
        {
            if (!Util.Reflection.ImplementedAll(injectedTypes, instanceType)) Throw("Assert!");
        }

        public static void IsInstanceType(object instance, Type instanceType)
        {
            if (instance.GetType() != instanceType)
            {
                Throw("Assert!");
            }
        }

        public static MicoException CreateException(string message)
        {
            return new MicoException(message);
        }
    }
}