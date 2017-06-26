﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Shared.Utilities;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.CommentSelection
{
    internal abstract class AbstractCommentSelectionService : ICommentSelectionService
    {
        public abstract string BlockCommentEndString { get; }
        public abstract string BlockCommentStartString { get; }
        public abstract string SingleLineCommentString { get; }
        public abstract bool SupportsBlockComment { get; }

        public Document Format(Document document, ImmutableArray<TextSpan> changes, CancellationToken cancellationToken)
        {
            var root = document.GetSyntaxRootSynchronously(cancellationToken);
            var formattingSpans = changes.Select(s => CommonFormattingHelpers.GetFormattingSpan(root, s));

            return Formatter.FormatAsync(document, formattingSpans, cancellationToken: cancellationToken).WaitAndGetResult(cancellationToken);
        }

        public CommentSelectionInfo GetInfo(SourceText sourceText, TextSpan textSpan)
            => SupportsBlockComment
                ? new CommentSelectionInfo(true, SupportsBlockComment, SingleLineCommentString, BlockCommentStartString, BlockCommentEndString)
                : new CommentSelectionInfo(true, SupportsBlockComment, SingleLineCommentString, "", "");
    }
}