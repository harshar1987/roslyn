﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.
using System.Threading;

namespace Microsoft.CodeAnalysis.CodeActions
{
    /// <summary>
    /// Represents a preview operation for generating a custom user preview for the operation.
    /// </summary>
    public abstract class PreviewOperation : CodeActionOperation
    {
        /// <summary>
        /// Gets a custom preview control for the operation.
        /// If preview is null and <see cref="CodeActionOperation.Title"/> is non-null, then <see cref="CodeActionOperation.Title"/> is used to generate the preview.
        /// </summary>
        public abstract object GetPreview();
    }
}
