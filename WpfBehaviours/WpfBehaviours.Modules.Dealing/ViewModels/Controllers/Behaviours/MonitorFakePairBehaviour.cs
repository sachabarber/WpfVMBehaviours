using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfBehaviours.Infrastructure.ExtensionMethods;
using WpfBehaviours.Infrastructure.Utils;
using WpfBehaviours.Modules.Dealing.Services;

namespace WpfBehaviours.Modules.Dealing.ViewModels.Controllers.Behaviours
{
    public class MonitorFakePairBehaviour : ISpotTileViewModelBehaviour
    {
        private readonly IFakeSpotRateProvider fakeSpotRateProvider;
        private SpotTileViewModel spotTileViewModel;
        private CompositeDisposable disposables = new CompositeDisposable();
        private CompositeDisposable fakePairDisposables = new CompositeDisposable();

        public MonitorFakePairBehaviour(IFakeSpotRateProvider fakeSpotRateProvider)
        {
            this.fakeSpotRateProvider = fakeSpotRateProvider;
            disposables.Add(this.fakeSpotRateProvider);
        }

        public void Dispose()
        {
            disposables.Dispose();
            fakePairDisposables.Dispose();
        }

        public void Start(SpotTileViewModel spotTileViewModel)
        {
            this.spotTileViewModel = spotTileViewModel;
            SetupTopLevelSubscription();
        }

        private void SetupTopLevelSubscription()
        {
            //listen for changes in the number of legs
            DisposableHelper.CreateNewCompositeDisposable(ref fakePairDisposables);
            fakePairDisposables.Add(spotTileViewModel.ObservePropertyChanged(x => x.FakeSpotPair)
                                    .Where(x => !string.IsNullOrEmpty(x.NewValue))
                                    .Subscribe(x =>
                                    {
                                        this.SetupFakePairSubscription();
                                    }));

            if (!string.IsNullOrEmpty(spotTileViewModel.FakeSpotPair))
            {
                SetupFakePairSubscription();
            }
        }

        private void SetupFakePairSubscription()
        {
            if (spotTileViewModel == null)
                return;


            fakePairDisposables.Add(fakeSpotRateProvider.MonitorFakePair(spotTileViewModel.FakeSpotPair)
                .Subscribe(x =>
                {
                    if (spotTileViewModel.IsEnabled)
                    {
                        spotTileViewModel.RateViewModel.AcceptNewPrice(x);
                    }
                }));
        }


    }
}
