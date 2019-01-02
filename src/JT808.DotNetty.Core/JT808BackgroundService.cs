using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.Core
{
    /// <summary>
    /// 
    /// <see cref="https://blogs.msdn.microsoft.com/cesardelatorre/2017/11/18/implementing-background-tasks-in-microservices-with-ihostedservice-and-the-backgroundservice-class-net-core-2-x/"/>
    /// </summary>
    public abstract class JT808BackgroundService : IHostedService, IDisposable
    {
        /// <summary>
        /// 默认次日过期
        /// </summary>
        public virtual TimeSpan DelayTimeSpan
        {
            get
            {
                DateTime current = DateTime.Now;
#if DEBUG
                DateTime tmp = current.AddSeconds(10);
#else
                DateTime tmp = current.Date.AddDays(1).AddMilliseconds(-1);
#endif
                return tmp.Subtract(current);
            }
            set { }
        }

        private Task _executingTask;

        public abstract string ServiceName { get; }

        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();

        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);

        public void Dispose()
        {
            _stoppingCts.Cancel();
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            // Store the task we're executing
            _executingTask = ExecuteAsync(_stoppingCts.Token);
            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }
            // Otherwise it's running
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }
            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite,cancellationToken));
            }
        }
    }
}
