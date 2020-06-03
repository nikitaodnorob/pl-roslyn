// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Roslyn.Test.Utilities;
using Microsoft.CodeAnalysis.Test.Utilities;
using static Microsoft.CodeAnalysis.CodeGen.CompilationTestData;

namespace Microsoft.CodeAnalysis.CSharp.UnitTests
{
    public static class BasicCompilationUtils
    {
        private static BasicTestBase s_instance;

        private static BasicTestBase Instance => s_instance ?? (s_instance = new BasicTestBase());

        private sealed class BasicTestBase : CommonTestBase
        {
            internal override string VisualizeRealIL(IModuleSymbol peModule, MethodData methodData, IReadOnlyDictionary<int, string> markers, bool areLocalsZeroed)
            {
                throw new NotImplementedException();
            }
        }
    }
}
