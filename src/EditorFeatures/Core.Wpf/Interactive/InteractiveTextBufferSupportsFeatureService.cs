﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Editor.Shared;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.InteractiveWindow;

namespace Microsoft.CodeAnalysis.Editor.Implementation.Interactive
{
    [ExportWorkspaceService(typeof(ITextBufferSupportsFeatureService), WorkspaceKind.Interactive), Shared]
    internal sealed class InteractiveTextBufferSupportsFeatureService : ITextBufferSupportsFeatureService
    {
        public bool SupportsCodeFixes(ITextBuffer textBuffer)
        {
            if (textBuffer != null)
            {
                var evaluator = (IInteractiveEvaluator)textBuffer.Properties[typeof(IInteractiveEvaluator)];
                var window = evaluator?.CurrentWindow;
                if (window?.CurrentLanguageBuffer == textBuffer)
                {
                    // These are only correct if we're on the UI thread.
                    // Otherwise, they're guesses and they might change immediately even if they're correct.
                    // If we return true and the buffer later becomes readonly, it appears that the 
                    // the code fix simply has no effect.
                    return !window.IsResetting && !window.IsRunning;
                }
            }

            return false;
        }

        public bool SupportsRefactorings(ITextBuffer textBuffer)
        {
            return false;
        }

        public bool SupportsRename(ITextBuffer textBuffer)
        {
            return false;
        }

        public bool SupportsNavigationToAnyPosition(ITextBuffer textBuffer)
        {
            return true;
        }
    }
}
