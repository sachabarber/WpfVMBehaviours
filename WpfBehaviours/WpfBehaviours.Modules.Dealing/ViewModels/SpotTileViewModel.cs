using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBehaviours.Infrastructure.Services;
using WpfBehaviours.Infrastructure.ViewModel;

namespace WpfBehaviours.Modules.Dealing.ViewModels
{
    public class SpotTileViewModel : TileViewModelBase, INavigationAware
    {
        private List<string> fakeSpotPairs;
        private string fakeSpotPair;
        private decimal currentSpotRateForSelectedPair = 0.0m;
        private Guid uniqueId;
        private DateTime selectedDate = DateTime.Now.Date;
        private readonly IViewModelController controller;
        private bool isEnabled = true;
        private string timeOutRemaining;
        private bool startedTiming = false;
        private int progress = 0;

        public SpotTileViewModel(
            Func<SpotTileViewModel, IViewModelController> controllerFactory,
            IRegionManager regionManager,
            IMessageBoxService messageBoxService)
            : base(regionManager, messageBoxService)
        {
            OkCommand = new ReactiveCommand<object, object>(x => IsValid() && IsEnabled);

            controller = controllerFactory(this);
            this.AddDisposable(controller);
            controller.Start();

            RateViewModel = new RateViewModel();

            base.PlaceItem();
        }


        private bool IsValid()
        {
            return !string.IsNullOrEmpty(this.FakeSpotPair) &&
                    RateViewModel.WholeRate > 0;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            Guid uniqueIdParam = Guid.Parse(navigationContext.Parameters["UniqueId"]);
            return uniqueIdParam == uniqueId;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
           
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.uniqueId = Guid.Parse(navigationContext.Parameters["UniqueId"]);
        }


       

        public List<string> FakeSpotPairs
        {
            get { return this.fakeSpotPairs; }
            set
            {
                RaiseAndSetIfChanged(ref this.fakeSpotPairs, value, () => FakeSpotPairs);
            }
        }

        public string FakeSpotPair
        {
            get { return this.fakeSpotPair; }
            set
            {
                RaiseAndSetIfChanged(ref this.fakeSpotPair, value, () => FakeSpotPair);
            }
        }

 
        public DateTime SelectedDate
        {
            get { return this.selectedDate; }
            set
            {
                RaiseAndSetIfChanged(ref this.selectedDate, value, () => SelectedDate);
            }
        }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set
            {
                RaiseAndSetIfChanged(ref this.isEnabled, value, () => IsEnabled);
                this.RateViewModel.IsEnabled = this.isEnabled;
            }
        }

        public string TimeOutRemaining
        {
            get { return this.timeOutRemaining; }
            set
            {
                RaiseAndSetIfChanged(ref this.timeOutRemaining, value, () => TimeOutRemaining);
            }
        }


        public bool StartedTiming
        {
            get { return this.startedTiming; }
            set
            {
                RaiseAndSetIfChanged(ref this.startedTiming, value, () => StartedTiming);
            }
        }

        public int Progress
        {
            get { return this.progress; }
            set
            {
                RaiseAndSetIfChanged(ref this.progress, value, () => Progress);
            }
        }

        
        
        

        public IReactiveCommand OkCommand { get; set; }
        public RateViewModel RateViewModel { get; private set; }

    }
}
