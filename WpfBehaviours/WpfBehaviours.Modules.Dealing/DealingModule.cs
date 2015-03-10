using System;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using WpfBehaviours.Infrastructure.ExtensionMethods;
using WpfBehaviours.Infrastructure.Messages;
using WpfBehaviours.Infrastructure.Regions;
using WpfBehaviours.Infrastructure.Services;
using WpfBehaviours.Infrastructure.ViewModel;
using WpfBehaviours.Modules.Dealing;
using WpfBehaviours.Modules.Dealing.Services;
using WpfBehaviours.Modules.Dealing.ViewModels;
using WpfBehaviours.Modules.Dealing.ViewModels.Controllers;
using WpfBehaviours.Modules.Dealing.ViewModels.Controllers.Behaviours;


namespace WpfBehaviours.Modules.Dealing
{

    public class DealingModule : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private readonly IRegionNavigationCallbackHandler regionNavigationCallbackHelper;
        private readonly IRegionNavigationCapacityChecker regionNavigationCapacityChecker;


        public DealingModule(
            IUnityContainer container,
            IRegionManager regionManager,
            IEventMessager eventMessager,
            IRegionNavigationCallbackHandler regionNavigationCallbackHelper,
            IRegionNavigationCapacityChecker regionNavigationCapacityChecker)
        {
            this.container = container;
            this.regionManager = regionManager;
            this.regionNavigationCallbackHelper = regionNavigationCallbackHelper;
            this.regionNavigationCapacityChecker = regionNavigationCapacityChecker;

            eventMessager.Observe<ShowNewSpotTileMessage>()
                .Subscribe(NavigateToNewSpotTile);


        }

        public void Initialize()
        {
        }


        #region SpotTileViewModel

        private void NavigateToNewSpotTile(ShowNewSpotTileMessage showNewSpotTileMessage)
        {
            if (regionNavigationCapacityChecker.IsNavigationAllowedForRegion(RegionNames.MainRegion))
            {
                UriQuery parameters = new UriQuery();
                parameters.Add("UniqueId", Guid.NewGuid().ToString());

                IUnityContainer childContainer = ConfigureSpotTileContainer();
                var uri = new Uri(typeof(SpotTileViewModel).FullName + parameters, UriKind.RelativeOrAbsolute);
                regionManager.RequestNavigateUsingSpecificContainer(RegionNames.MainRegion, uri,
                    regionNavigationCallbackHelper.HandleNavigationCallback, childContainer);
            }
        }


        private IUnityContainer ConfigureSpotTileContainer()
        {
            IUnityContainer childContainer = container.CreateChildContainer();

            //navigation windows
            childContainer.RegisterTypeForNavigationWithChildContainer<SpotTileViewModel>(
                new HierarchicalLifetimeManager());

            //viwemodel controllers
            childContainer.RegisterType<IViewModelController, SpotTileViewModelController>(
                "SpotTileViewModelController", new HierarchicalLifetimeManager());

            //viewmodel controller factories
            childContainer.RegisterType<Func<SpotTileViewModel, IViewModelController>>(
                new HierarchicalLifetimeManager(),
                new InjectionFactory(c =>
                    new Func<SpotTileViewModel, IViewModelController>(
                        viewModel =>
                            c.Resolve<IViewModelController>("SpotTileViewModelController",
                                new DependencyOverride<SpotTileViewModel>(viewModel)))));

            //behaviours
            childContainer.RegisterType<ISpotTileViewModelBehaviour, LoadFakeSpotCCYPairsBehaviour>("LoadFakeSpotCCYPairsBehaviour", new HierarchicalLifetimeManager());
            childContainer.RegisterType<ISpotTileViewModelBehaviour, MonitorFakePairBehaviour>("MonitorFakePairBehaviour", new HierarchicalLifetimeManager());
            childContainer.RegisterType<ISpotTileViewModelBehaviour, OkCommandBehaviour>("OkCommandBehaviour", new HierarchicalLifetimeManager());
            childContainer.RegisterType<ISpotTileViewModelBehaviour, TimeoutBehaviour>("TimeoutBehaviour", new HierarchicalLifetimeManager());
            
            
            //services
            childContainer.RegisterType<IFakeSpotRateProvider, FakeSpotRateProvider>(new HierarchicalLifetimeManager());

            return childContainer;
        }
        #endregion

    }

}