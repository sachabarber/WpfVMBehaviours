using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBehaviours.Infrastructure.Regions
{
    public interface IRegionNavigationCapacityChecker
    {
        bool IsNavigationAllowedForRegion(string regionName);
    }
}
