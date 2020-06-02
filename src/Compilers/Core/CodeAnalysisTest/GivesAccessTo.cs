// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.UnitTests
{
    public class GivesAccessTo
    {
        [Fact, WorkItem(26459, "https://github.com/dotnet/roslyn/issues/26459")]
        public void TestGivesAccessTo_CrossLanguageAndCompilation()
        {
            var csharpTree = CSharpSyntaxTree.ParseText(@"
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo(""VB"")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo(""CS2"")]
internal class CS
{
}
");
            var csharpTree2 = CSharpSyntaxTree.ParseText(@"
internal class CS2
{
}
");
            var csc = (Compilation)CSharpCompilation.Create("CS", new[] { csharpTree }, new MetadataReference[] { TestBase.MscorlibRef });
            var CS = csc.GlobalNamespace.GetMembers("CS").First() as INamedTypeSymbol;

            var csc2 = (Compilation)CSharpCompilation.Create("CS2", new[] { csharpTree2 }, new MetadataReference[] { TestBase.MscorlibRef });
            var CS2 = csc2.GlobalNamespace.GetMembers("CS2").First() as INamedTypeSymbol;

            Assert.True(CS.ContainingAssembly.GivesAccessTo(CS2.ContainingAssembly));
        }
    }
}
