using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.CodeAnalysis.CSharp
{
    class MyBlockBinder : BlockBinder
    {
        internal MyBlockBinder(Binder enclosing, BlockSyntax block) : base(enclosing, block) { }

        internal MyBlockBinder(Binder enclosing, BlockSyntax block, BinderFlags additionalFlags)
            : base(enclosing, block, additionalFlags) { }

        protected override bool ValidateNameConflictsInScope(Symbol symbol, Location location, string name, DiagnosticBag diagnostics)
        {
            if (string.IsNullOrEmpty(name)) return false;

            bool allowShadowing = Compilation.IsFeatureEnabled(MessageID.IDS_FeatureNameShadowingInNestedFunctions);
            bool wasCheckedConflict = false;

            for (Binder? binder = this; binder != null; binder = binder.Next)
            {
                // no local scopes enclose members
                if (binder is InContainerBinder) return false;

                var scope = binder as LocalScopeBinder;
                if (!wasCheckedConflict && scope?.EnsureSingleDefinition(symbol, name, location, diagnostics) == true)
                {
                    return true;
                }

                if (!wasCheckedConflict && symbol.Kind == SymbolKind.Local) wasCheckedConflict = true;


                // If shadowing is enabled, avoid checking for conflicts outside of local functions or lambdas.
                if (allowShadowing && binder.IsNestedFunctionBinder) return false;

                // Declarations within a member do not conflict with declarations outside.
                if (binder.IsLastBinderWithinMember()) return false;
            }

            return false;
        }
    }
}
