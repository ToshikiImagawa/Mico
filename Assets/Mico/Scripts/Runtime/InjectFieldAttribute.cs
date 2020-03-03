// Mico C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using Mico.Internal;

namespace Mico
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class InjectFieldAttribute : Attribute
    {
        public object Id { get; set; } = typeof(DefaultId);
    }
}