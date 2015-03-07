using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBehaviours.Infrastructure.Regions
{
    public interface IRegionNavigationCallbackHandler
    {
        void HandleNavigationCallback(NavigationResult navigationCallback);
    }
}
