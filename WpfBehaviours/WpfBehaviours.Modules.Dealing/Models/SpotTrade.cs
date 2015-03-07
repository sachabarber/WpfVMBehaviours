using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBehaviours.Modules.Dealing.Models
{
    public class SpotTrade
    {
        public SpotTrade(DateTime valueDate, string ccyPair, decimal rate, DateTime tradeDate)
        {
            ValueDate = valueDate;
            CCYPair = ccyPair;
            Rate = rate;
            TradeDate = tradeDate;
        }

        public DateTime ValueDate { get; private set; }    
        public string CCYPair { get;  private set; }    
        public decimal Rate { get; private set; }    
        public DateTime TradeDate { get; private set; }    
    }
}
