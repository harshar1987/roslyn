﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Threading;

namespace Roslyn.Utilities
{
    internal static class ReaderWriterLockSlimExtensions
    {
        internal static ReadLockExiter DisposableRead(this ReaderWriterLockSlim @lock)
        {
            return new ReadLockExiter(@lock);
        }

        internal struct ReadLockExiter : IDisposable
        {
            private readonly ReaderWriterLockSlim @lock;

            internal ReadLockExiter(ReaderWriterLockSlim @lock)
            {
                this.@lock = @lock;
                @lock.EnterReadLock();
            }

            public void Dispose()
            {
                this.@lock.ExitReadLock();
            }
        }

        internal static WriteLockExiter DisposableWrite(this ReaderWriterLockSlim @lock)
        {
            return new WriteLockExiter(@lock);
        }

        internal struct WriteLockExiter : IDisposable
        {
            private readonly ReaderWriterLockSlim @lock;

            internal WriteLockExiter(ReaderWriterLockSlim @lock)
            {
                this.@lock = @lock;
                @lock.EnterWriteLock();
            }

            public void Dispose()
            {
                this.@lock.ExitWriteLock();
            }
        }

        internal static void AssertCanRead(this ReaderWriterLockSlim @lock)
        {
            if (!@lock.IsReadLockHeld && !@lock.IsUpgradeableReadLockHeld && !@lock.IsWriteLockHeld)
            {
                throw new InvalidOperationException();
            }
        }

        internal static void AssertCanWrite(this ReaderWriterLockSlim @lock)
        {
            if (!@lock.IsWriteLockHeld)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
