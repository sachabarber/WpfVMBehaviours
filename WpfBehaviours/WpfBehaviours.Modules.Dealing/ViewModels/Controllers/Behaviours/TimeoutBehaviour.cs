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
using WpfBehaviours.Modules.Dealing.Utils;

namespace WpfBehaviours.Modules.Dealing.ViewModels.Controllers.Behaviours
{
    public class TimeoutBehaviour : ISpotTileViewModelBehaviour
    {
        private SpotTileViewModel spotTileViewModel;
        private CompositeDisposable disposables = new CompositeDisposable();
        private CompositeDisposable fakePairDisposables = new CompositeDisposable();


        public TimeoutBehaviour()
        {
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

            SetupTimeOutSubscription();

        }


        private void SetupTimeOutSubscription()
        {
            //this.disposables.Dispose();
            spotTileViewModel.StartedTiming = true;

            int counter = 0;
            UpdateProgress(counter);

          

            var tileDisabledObservable = spotTileViewModel.ObservePropertyChanged(x => x.IsEnabled)
                .Where(x => !x.NewValue);
            

            this.disposables.Add(Observable.Interval(TimeSpan.FromSeconds(Globals.ProgressTimeOut))
                .TakeUntil(tileDisabledObservable)
                .Subscribe(x =>
                {
                    counter++;

                    UpdateProgress(counter);
                    if (counter == Globals.ProgressSegments - 1)
                    {
                        spotTileViewModel.IsEnabled = false;
                        this.disposables.Dispose();
                    }
                }));
        }


        private void UpdateProgress(int counter)
        {
            var timeRemaining = (Globals.TotalTimeoutInSeconds -
                                 (Globals.ProgressTimeOut * (counter + 1)));

            spotTileViewModel.TimeOutRemaining = string.Format("{0}s of {1}s",
                counter == 0 ? Globals.TotalTimeoutInSeconds : timeRemaining, 
                Globals.TotalTimeoutInSeconds);

             //100/60 * 30 = 50% done
            var secPerSegment = Globals.TotalTimeoutInSeconds/Globals.ProgressSegments;
            spotTileViewModel.Progress = (100/Globals.TotalTimeoutInSeconds) * (secPerSegment * counter+1);
        }
    }
}
