using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace Microsoft.CodeAnalysis.CSharp
{
    class MyExpressionVariableBinder : ExpressionVariableBinder
    {
        internal MyExpressionVariableBinder(SyntaxNode scopeDesignator, Binder next) : base(scopeDesignator, next) { }

        protected override BoundExpression BindSimpleBinaryOperator(BinaryExpressionSyntax node, DiagnosticBag diagnostics)
        {
            BoundExpression result = base.BindSimpleBinaryOperator(node, diagnostics);

            if (node.Left is IdentifierNameSyntax)
            {
                var identifierText = (node.Left as IdentifierNameSyntax).Identifier.Text;
                if (identifierText.StartsWith("#"))
                {
                    var bindRight = BindExpression(node.Right, new DiagnosticBag());
                    var correctTypes = new string[] { "int", "short", "byte", "long", "uint", "ulong", "ushort" };
                    if (!bindRight.HasErrors && correctTypes.All(type => type != bindRight.Type.ToString()))
                        diagnostics.Add(ErrorCode.ERR_LoopCounterNoInteger, node.Location);
                }
            }

            return result;
        }
    }
}
