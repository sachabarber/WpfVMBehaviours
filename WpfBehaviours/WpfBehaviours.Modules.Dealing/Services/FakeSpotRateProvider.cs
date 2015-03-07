using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace WpfBehaviours.Modules.Dealing.Services
{
    public class FakeSpotRateProvider : IFakeSpotRateProvider
    {
        private Dictionary<string, ReplaySubject<decimal>> fakeDataProviders = 
            new Dictionary<string, ReplaySubject<decimal>>();

        private Dictionary<string, decimal> fakeDataProviderBaseValues =
            new Dictionary<string, decimal>();

        private Random rand = new Random();
   

        private CompositeDisposable disposables = new CompositeDisposable();

        public FakeSpotRateProvider()
        {
            fakeDataProviders.Add("GBPUSD", new ReplaySubject<decimal>());
            fakeDataProviders.Add("GBPEUR", new ReplaySubject<decimal>());
            fakeDataProviders.Add("USDEUR", new ReplaySubject<decimal>());
            fakeDataProviders.Add("USDGBP", new ReplaySubject<decimal>());
            fakeDataProviders.Add("EURUSD", new ReplaySubject<decimal>());
            fakeDataProviders.Add("EURGBP", new ReplaySubject<decimal>());

            fakeDataProviderBaseValues.Add("GBPUSD", 1.54m);
            fakeDataProviderBaseValues.Add("GBPEUR", 1.37m);
            fakeDataProviderBaseValues.Add("USDEUR", 0.89m);
            fakeDataProviderBaseValues.Add("USDGBP", 0.65m);
            fakeDataProviderBaseValues.Add("EURUSD", 1.12m);
            fakeDataProviderBaseValues.Add("EURGBP", 0.73m);

            fakeDataProviders["GBPUSD"].StartWith(new[] { fakeDataProviderBaseValues["GBPUSD"] });
            fakeDataProviders["GBPEUR"].StartWith(new[] { fakeDataProviderBaseValues["GBPEUR"] });
            fakeDataProviders["USDEUR"].StartWith(new[] { fakeDataProviderBaseValues["USDEUR"] });
            fakeDataProviders["USDGBP"].StartWith(new[] { fakeDataProviderBaseValues["USDGBP"] });
            fakeDataProviders["EURUSD"].StartWith(new[] { fakeDataProviderBaseValues["EURUSD"] });
            fakeDataProviders["EURGBP"].StartWith(new[] { fakeDataProviderBaseValues["EURGBP"] });

            this.disposables.Add(Observable.Interval(TimeSpan.FromSeconds(0.5))
                .Subscribe(x => UpdateFakeRates()));
        }

        private void UpdateFakeRates()
        {
            foreach (var fake in fakeDataProviders.AsParallel())
            {
                var randValue = rand.NextDouble();

                decimal onNextValue = randValue > 0.5
                    ? fakeDataProviderBaseValues[fake.Key] * 1.01m
                    : fakeDataProviderBaseValues[fake.Key] * 0.99m;
                fake.Value.OnNext(onNextValue);
            }
        }
        

        public List<string> FakeSpotPairs
        {
            get { return fakeDataProviders.Keys.ToList(); }
        }

        public IObservable<decimal> MonitorFakePair(string pair)
        {
            return fakeDataProviders[pair].AsObservable();
        }

        public void Dispose()
        {
            this.disposables.Dispose();
        }
    }
}
