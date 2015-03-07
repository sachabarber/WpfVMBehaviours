using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using WpfBehaviours.Modules.Dealing.Services;

namespace WpfBehaviours.Modules.Dealing.ViewModels.Controllers.Behaviours
{
    public class LoadFakeSpotCCYPairsBehaviour : ISpotTileViewModelBehaviour
    {
        private readonly IFakeSpotRateProvider fakeSpotRateProvider;
        private SpotTileViewModel spotTileViewModel;
        private CompositeDisposable disposables = new CompositeDisposable();

        public LoadFakeSpotCCYPairsBehaviour(IFakeSpotRateProvider fakeSpotRateProvider)
        {
            this.fakeSpotRateProvider = fakeSpotRateProvider;
            disposables.Add(this.fakeSpotRateProvider);
        }

        public void Dispose()
        {
            disposables.Dispose();
        }

        public void Start(SpotTileViewModel spotTileViewModel)
        {
            this.spotTileViewModel = spotTileViewModel;

            this.spotTileViewModel.FakeSpotPairs = fakeSpotRateProvider.FakeSpotPairs;
        }
    }
}
