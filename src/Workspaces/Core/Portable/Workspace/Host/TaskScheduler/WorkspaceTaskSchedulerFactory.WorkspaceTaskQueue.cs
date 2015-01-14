﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.Host
{
    internal partial class WorkspaceTaskSchedulerFactory
    {
        internal sealed class WorkspaceTaskQueue : IWorkspaceTaskScheduler
        {
            private readonly WorkspaceTaskSchedulerFactory factory;
            private readonly SimpleTaskQueue queue;

            public WorkspaceTaskQueue(WorkspaceTaskSchedulerFactory factory, TaskScheduler taskScheduler)
            {
                this.factory = factory;
                this.queue = new SimpleTaskQueue(taskScheduler);
            }

            public T3 ScheduleTask<T1, T2, T3>(Func<T1, T2, T3> taskScheduler, string taskName, T1 arg1, T2 arg2) where T3 : Task
            {
                taskName = taskName ?? GetType().Name + ".Task";
                var asyncToken = factory.BeginAsyncOperation(taskName);

                var task = taskScheduler(arg1, arg2);

                factory.CompleteAsyncOperation(asyncToken, task);
                return task;
            }

            public Task ScheduleTask(Action taskAction, string taskName, CancellationToken cancellationToken)
            {
                return ScheduleTask((t, c) => this.queue.ScheduleTask(t, c), taskName, taskAction, cancellationToken);
            }

            public Task<T> ScheduleTask<T>(Func<T> taskFunc, string taskName, CancellationToken cancellationToken)
            {
                return ScheduleTask((t, c) => this.queue.ScheduleTask(t, c), taskName, taskFunc, cancellationToken);
            }

            public Task ScheduleTask(Func<Task> taskFunc, string taskName, CancellationToken cancellationToken = default(CancellationToken))
            {
                return ScheduleTask((t, c) => this.queue.ScheduleTask(t, c), taskName, taskFunc, cancellationToken);
            }

            public Task<T> ScheduleTask<T>(Func<Task<T>> taskFunc, string taskName, CancellationToken cancellationToken = default(CancellationToken))
            {
                return ScheduleTask((t, c) => this.queue.ScheduleTask(t, c), taskName, taskFunc, cancellationToken);
            }
        }
    }
}
