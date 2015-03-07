using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBehaviours.Infrastructure.ViewModel;

namespace WpfBehaviours.Modules.Dealing.ViewModels
{
    public class RateViewModel : INPCBase
    {
        private bool isEnabled = true;
        private string part1;
        private string part2;
        private string part3;
        private decimal wholeRate;

        public RateViewModel()
        {
        }


        public void AcceptNewPrice(decimal newPrice)
        {
            if (isEnabled)
            {
                WholeRate = newPrice;
                string rateAsString = newPrice.ToString();
                int decimalPoint = rateAsString.IndexOf(".", System.StringComparison.Ordinal) + 1;

                string safeRateString = rateAsString.Substring(0, decimalPoint) +
                                        rateAsString.Substring(decimalPoint).PadRight(4);

                Part1 = safeRateString.Substring(0, decimalPoint);
                Part2 = safeRateString.Substring(decimalPoint, 2);
                Part3 = safeRateString.Substring(decimalPoint + 2, 2);
            }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set
            {
                RaiseAndSetIfChanged(ref this.isEnabled, value, () => IsEnabled);
            }
        }  

        public decimal WholeRate
        {
            get { return this.wholeRate; }
            set
            {
                RaiseAndSetIfChanged(ref this.wholeRate, value, () => WholeRate);
            }
        }  

        public string Part1
        {
            get { return this.part1; }
            set
            {
                RaiseAndSetIfChanged(ref this.part1, value, () => Part1);
            }
        }  
        
        public string Part2
        {
            get { return this.part2; }
            set
            {
                RaiseAndSetIfChanged(ref this.part2, value, () => Part2);
            }
        }

        public string Part3
        {
            get { return this.part3; }
            set
            {
                RaiseAndSetIfChanged(ref this.part3, value, () => Part3);
            }
        }

    }
}
