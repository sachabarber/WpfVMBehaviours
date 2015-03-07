using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using WpfBehaviours.Infrastructure.Services;
using WpfBehaviours.Infrastructure.Regions;
using WpfBehaviours.Modules.Dealing;
using WpfBehaviours.ControlSet.Windows.Controls;

namespace WpfBehaviours.Shell
{
    public class Bootstrapper : UnityBootstrapper
    {

        protected override IModuleCatalog CreateModuleCatalog()
        {
 
            var moduleCatalog = new ModuleCatalog();
            moduleCatalog.AddModule(typeof(DealingModule));
            return moduleCatalog;
        }

      
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

 
            //windows
            Container.RegisterType<ShellWindow>(new ContainerControlledLifetimeManager());
       
       
            //services
            Container.RegisterType<IEventMessager, EventMessager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IDispatcherService, DispatcherService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ISchedulerService, SchedulerService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IMessageBoxService, MessageBoxService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ShellViewModel>(new ContainerControlledLifetimeManager());


            //custom region stuff to support child container navigation
            Container.RegisterType<IRegionNavigationContentLoader, CustomRegionNavigationContentLoader>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRegionNavigationService, CustomRegionNavigationService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRegionNavigationCapacityChecker, RegionNavigationCapacityChecker>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRegionNavigationCallbackHandler, RegionNavigationCallbackHandler>(new ContainerControlledLifetimeManager());
        }

        protected override DependencyObject CreateShell()
        {
            return this.Container.Resolve<ShellWindow>();
        }


        protected override void InitializeShell()
        {
            Window shellWindow = (Window)Shell;
            App.Current.MainWindow = shellWindow;
            App.Current.MainWindow.Show();
            shellWindow.WindowState = WindowState.Maximized;
            shellWindow.Activate();
        }
    }
}
