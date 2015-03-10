using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfBehaviours.Infrastructure.ExtensionMethods;
using WpfBehaviours.Infrastructure.Services;
using WpfBehaviours.Infrastructure.Utils;
using WpfBehaviours.Modules.Dealing.Models;
using WpfBehaviours.Modules.Dealing.Services;

namespace WpfBehaviours.Modules.Dealing.ViewModels.Controllers.Behaviours
{
    public class OkCommandBehaviour : ISpotTileViewModelBehaviour
    {
        private readonly IEventMessager eventMessager;
        private SpotTileViewModel spotTileViewModel;
        private CompositeDisposable disposables = new CompositeDisposable();

        public OkCommandBehaviour(IEventMessager eventMessager)
        {
            this.eventMessager = eventMessager;
        }

        public void Dispose()
        {
            disposables.Dispose();
        }

        public void Start(SpotTileViewModel spotTileViewModel)
        {
            this.spotTileViewModel = spotTileViewModel;
            SetupTopLevelSubscription();
        }

        private void SetupTopLevelSubscription()
        {
           

            disposables.Add(spotTileViewModel.OkCommand.CommandExecutedStream.Subscribe(
                x =>
                {
                    spotTileViewModel.IsEnabled = false;
                    eventMessager.Publish(new SpotTrade(
                        spotTileViewModel.SelectedDate,
                        spotTileViewModel.FakeSpotPair,
                        spotTileViewModel.RateViewModel.WholeRate,
                        DateTime.Now));
                }));
        }
    }
}
