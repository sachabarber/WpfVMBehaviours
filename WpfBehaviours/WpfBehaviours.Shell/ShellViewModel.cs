using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfBehaviours.Infrastructure.Messages;
using WpfBehaviours.Infrastructure.Services;
using WpfBehaviours.Infrastructure.ViewModel;
using WpfBehaviours.Modules.Dealing.Models;

namespace WpfBehaviours.Shell
{
    public class ShellViewModel : INPCBase
    {
        public ShellViewModel(IEventMessager eventMessager)
        {
            Title = "Big XAML Apps Demo";
            ShowSpotTileCommand = new SimpleCommand<object, object>(x => eventMessager.Publish(new ShowNewSpotTileMessage()));
            SpotTrades = new ObservableCollection<SpotTrade>();

            eventMessager.Observe<SpotTrade>()
                .Subscribe(x =>
                {
                    this.SpotTrades.Add(x);
                    base.RaisePropertyChanged(()=>SpotTrades);
                });
        }

        public string Title { get; set; }

        public ICommand ShowSpotTileCommand { get; private set; }

        public ObservableCollection<SpotTrade> SpotTrades { get; private set; }
    }
}
