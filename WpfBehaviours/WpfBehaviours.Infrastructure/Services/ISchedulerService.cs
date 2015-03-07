using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;

namespace WpfBehaviours.Infrastructure.Services
{
    public interface ISchedulerService
    {
        IScheduler Immediate { get; }
        IScheduler CurrentThread { get; }
        IScheduler NewThread { get; }
        IScheduler TaskPool { get; }
        IScheduler ThreadPool { get; }
        IScheduler EventLoop { get; }
    }
}
