using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Regions;
using WpfBehaviours.Infrastructure.Services;


namespace WpfBehaviours.Infrastructure.Regions
{
    public class RegionNavigationCapacityChecker : IRegionNavigationCapacityChecker
    {
        private Dictionary<string, int> regionCapacityLimits = new Dictionary<string, int>();
        private IRegionManager regionManager;
        private IMessageBoxService messageBoxService;

        public RegionNavigationCapacityChecker(IRegionManager regionManager, IMessageBoxService messageBoxService)
        {
            this.messageBoxService = messageBoxService;
            this.regionManager = regionManager;
            regionCapacityLimits.Add(RegionNames.MainRegion, 8);
        }


        public bool IsNavigationAllowedForRegion(string regionName)
        {
            bool result = true;
            if(!regionCapacityLimits.ContainsKey(regionName))
            {
                //key not found so no limit
                return true;
            }

          

            int existingViews = regionManager.Regions[regionName].Views.Count();
            result  = existingViews + 1 < regionCapacityLimits[regionName];
            if (!result)
            {
                messageBoxService.ShowWarning("You have exceeded the amount of allowable tabs open please close a tab");
            }
            return result;
        }


    }
}
