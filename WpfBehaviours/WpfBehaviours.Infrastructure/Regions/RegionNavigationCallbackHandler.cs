using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Regions;
using WpfBehaviours.Infrastructure.Services;

namespace WpfBehaviours.Infrastructure.Regions
{
    public class RegionNavigationCallbackHandler : IRegionNavigationCallbackHandler
    {
        private IMessageBoxService messageBoxService;

        public RegionNavigationCallbackHandler(IMessageBoxService messageBoxService)
        {
            this.messageBoxService = messageBoxService;
        }

        public void HandleNavigationCallback(NavigationResult navigationResult)
        {
            if (navigationResult.Error != null)
            {
                messageBoxService.ShowError(("There was an error trying to display the view you requested"));
            }
            else
            {
                if (!navigationResult.Result.HasValue || !navigationResult.Result.Value)
                {
                    messageBoxService.ShowError(("There was an error trying to display the view you requested"));
                }
            }
        }
    }
}

