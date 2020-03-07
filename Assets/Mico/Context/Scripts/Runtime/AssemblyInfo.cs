// Mico.Context C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

#if UNITY_EDITOR
using System.Runtime.CompilerServices;

#if MICO_TEST_ADD_MOQ
using Castle.Core.Internal;

[assembly: InternalsVisibleTo(InternalsVisible.ToDynamicProxyGenAssembly2)]
#endif

[assembly: InternalsVisibleTo("Mico.Context.Tests")]
[assembly: InternalsVisibleTo("Mico.Context.Tests-Editor")]
#endif