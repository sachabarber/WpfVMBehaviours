using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using WpfBehaviours.Infrastructure.ViewModel;
using WpfBehaviours.Modules.Dealing.ViewModels.Controllers.Behaviours;

namespace WpfBehaviours.Modules.Dealing.ViewModels.Controllers
{
    public class SpotTileViewModelController : IViewModelController
    {
        private CompositeDisposable disposables = new CompositeDisposable();
        private readonly SpotTileViewModel spotTileViewModel;
        private readonly ISpotTileViewModelBehaviour[] behaviors;

        public SpotTileViewModelController(
            SpotTileViewModel spotTileViewModel,
            ISpotTileViewModelBehaviour[] behaviors)
        {
            this.spotTileViewModel = spotTileViewModel;
            this.behaviors = behaviors;
        }

        public void Start()
        {
            //start behaviors
            foreach (var behavior in behaviors)
            {
                disposables.Add(behavior);
                behavior.Start(spotTileViewModel);
            }
        }

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}
