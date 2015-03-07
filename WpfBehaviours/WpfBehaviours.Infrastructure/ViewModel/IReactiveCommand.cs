using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBehaviours.Infrastructure.ViewModel
{
    public interface IReactiveCommand
    {
        IObservable<object> CommandExecutedStream { get; }
    }
}
