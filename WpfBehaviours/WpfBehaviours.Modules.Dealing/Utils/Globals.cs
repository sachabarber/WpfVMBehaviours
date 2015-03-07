using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBehaviours.Modules.Dealing.Utils
{
    public static class Globals
    {
        public static int TotalTimeoutInSeconds;
        public static int ProgressSegments;
        public static int ProgressTimeOut;

        static Globals() 
        {
            TotalTimeoutInSeconds = 20;
            ProgressSegments = 10;
            ProgressTimeOut = TotalTimeoutInSeconds / ProgressSegments;
           
        }
    }
}
