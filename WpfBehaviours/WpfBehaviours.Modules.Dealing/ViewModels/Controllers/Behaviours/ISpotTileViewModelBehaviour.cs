using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBehaviours.Modules.Dealing.ViewModels.Controllers.Behaviours
{
    public interface ISpotTileViewModelBehaviour : IDisposable
    {
        void Start(SpotTileViewModel spotTileViewModel);
    }
}
