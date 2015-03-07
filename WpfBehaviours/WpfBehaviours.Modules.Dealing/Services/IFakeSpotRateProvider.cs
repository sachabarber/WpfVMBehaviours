using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBehaviours.Modules.Dealing.Services
{
    public interface IFakeSpotRateProvider : IDisposable
    {
        List<string> FakeSpotPairs { get; }
        IObservable<decimal> MonitorFakePair(string pair);
    }
}
