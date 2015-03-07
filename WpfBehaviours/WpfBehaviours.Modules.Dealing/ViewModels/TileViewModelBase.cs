using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Regions;
using WpfBehaviours.Infrastructure.Services;
using WpfBehaviours.Infrastructure.ViewModel;

namespace WpfBehaviours.Modules.Dealing.ViewModels
{
    public abstract class TileViewModelBase : DisposableViewModel
    {
        private readonly IRegionManager regionManager;
        private readonly IMessageBoxService messageBoxService;
        private Random rand = new Random();
        private double left;
        private double top;
        private static int zIndexCounter = 0;
        private int zIndex;


        public TileViewModelBase(
            IRegionManager regionManager,
            IMessageBoxService messageBoxService)
        {
            this.regionManager = regionManager;
            this.messageBoxService = messageBoxService;
            CloseViewCommand = new SimpleCommand<object, object>(ExecuteCloseViewCommand);
        }


        public void AdjustZIndex()
        {
            zIndexCounter++;
            ZIndex = zIndexCounter;
        }

        public void PlaceItem()
        {
            rand = new Random();
            Left = rand.Next(0, 1024/2);
            Top = rand.Next(0, 768 / 2);
            zIndexCounter++;
            ZIndex = zIndexCounter;
        }

        public double Left
        {
            get
            {
                return this.left;
            }
            protected set
            {
                RaiseAndSetIfChanged(ref this.left, value, () => Left);
            }
        }


        public double Top
        {
            get
            {
                return this.top;
            }
            protected set
            {
                RaiseAndSetIfChanged(ref this.top, value, () => Top);
            }
        }

        public int ZIndex
        {
            get
            {
                return this.zIndex;
            }
            protected set
            {
                RaiseAndSetIfChanged(ref this.zIndex, value, () => ZIndex);
            }
        }

        

        public ICommand CloseViewCommand { get; set; }

        private void ExecuteCloseViewCommand(Object arg)
        {
            var result = messageBoxService.ShowYesNo(
                "You are about to close this Option, you will loose any edits you have. Are you sure?",
                "Confirm close",
                CustomDialogIcons.Warning);

            if (result == CustomDialogResults.Yes)
            {
                IRegion region = regionManager.Regions["MainRegion"];
                region.Remove(this);
                this.Dispose();
            }
        }
    }
}
