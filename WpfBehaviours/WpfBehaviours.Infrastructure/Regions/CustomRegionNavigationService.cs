//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace WpfBehaviours.Infrastructure.Regions
{
    /// <summary>
    /// Provides navigation for regions.
    /// </summary>
    public class CustomRegionNavigationService : IRegionNavigationService
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IRegionNavigationContentLoader regionNavigationContentLoader;
        private IRegionNavigationJournal journal;
        private NavigationContext currentNavigationContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionNavigationService"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="regionNavigationContentLoader">The navigation target handler.</param>
        /// <param name="journal">The journal.</param>
        public CustomRegionNavigationService(IServiceLocator serviceLocator, IRegionNavigationContentLoader regionNavigationContentLoader, IRegionNavigationJournal journal)
        {
            if (serviceLocator == null)
            {
                throw new ArgumentNullException("serviceLocator");
            }

            if (regionNavigationContentLoader == null)
            {
                throw new ArgumentNullException("regionNavigationContentLoader");
            }

            if (journal == null)
            {
                throw new ArgumentNullException("journal");
            }

            this.serviceLocator = serviceLocator;
            this.regionNavigationContentLoader = regionNavigationContentLoader;
            this.journal = journal;
            this.journal.NavigationTarget = this;
        }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>The region.</value>
        public IRegion Region { get; set; }

        /// <summary>
        /// Gets the journal.
        /// </summary>
        /// <value>The journal.</value>
        public IRegionNavigationJournal Journal
        {
            get
            {
                return this.journal;
            }
        }

        /// <summary>
        /// Raised when the region is about to be navigated to content.
        /// </summary>
        public event EventHandler<RegionNavigationEventArgs> Navigating;

        private void RaiseNavigating(NavigationContext navigationContext)
        {
            if (this.Navigating != null)
            {
                this.Navigating(this, new RegionNavigationEventArgs(navigationContext));
            }
        }

        /// <summary>
        /// Raised when the region is navigated to content.
        /// </summary>
        public event EventHandler<RegionNavigationEventArgs> Navigated;

        private void RaiseNavigated(NavigationContext navigationContext)
        {
            if (this.Navigated != null)
            {
                this.Navigated(this, new RegionNavigationEventArgs(navigationContext));
            }
        }

        /// <summary>
        /// Raised when a navigation request fails.
        /// </summary>
        public event EventHandler<RegionNavigationFailedEventArgs> NavigationFailed;

        private void RaiseNavigationFailed(NavigationContext navigationContext, Exception error)
        {
            if (this.NavigationFailed != null)
            {
                this.NavigationFailed(this, new RegionNavigationFailedEventArgs(navigationContext, error));
            }
        }

        /// <summary>
        /// Initiates navigation to the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="navigationCallback">A callback to execute when the navigation request is completed.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception is marshalled to callback")]
        public void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback)
        {
            if (navigationCallback == null) throw new ArgumentNullException("navigationCallback");

            try
            {
                this.DoNavigate(target, navigationCallback, null);
            }
            catch (Exception e)
            {
                this.NotifyNavigationFailed(new NavigationContext(this, target), navigationCallback, e);
            }
        }


        public void RequestNavigate(Uri target, Action<NavigationResult> navigationCallback, IUnityContainer containerToUse)
        {
            if (navigationCallback == null) throw new ArgumentNullException("navigationCallback");
            if (containerToUse == null) throw new ArgumentNullException("containerToUse");

            try
            {
                this.DoNavigate(target, navigationCallback, containerToUse);
            }
            catch (Exception e)
            {
                this.NotifyNavigationFailed(new NavigationContext(this, target), navigationCallback, e);
            }
        }


        private void DoNavigate(Uri source, Action<NavigationResult> navigationCallback, IUnityContainer containerToUse)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (this.Region == null)
            {
                throw new InvalidOperationException("Navigation cannot proceed until a region is set for the RegionNavigationService.");
            }

            this.currentNavigationContext = new NavigationContext(this, source);

            // starts querying the active views
            RequestCanNavigateFromOnCurrentlyActiveView(
                this.currentNavigationContext,
                navigationCallback,
                this.Region.ActiveViews.ToArray(),
                0,
                containerToUse);
        }

        private void RequestCanNavigateFromOnCurrentlyActiveView(
            NavigationContext navigationContext,
            Action<NavigationResult> navigationCallback,
            object[] activeViews,
            int currentViewIndex,
            IUnityContainer containerToUse)
        {
            if (currentViewIndex < activeViews.Length)
            {
                var vetoingView = activeViews[currentViewIndex] as IConfirmNavigationRequest;
                if (vetoingView != null)
                {
                    // the current active view implements IConfirmNavigationRequest, request confirmation
                    // providing a callback to resume the navigation request
                    vetoingView.ConfirmNavigationRequest(
                        navigationContext,
                        canNavigate =>
                        {
                            if (this.currentNavigationContext == navigationContext && canNavigate)
                            {
                                RequestCanNavigateFromOnCurrentlyActiveViewModel(
                                    navigationContext,
                                    navigationCallback,
                                    activeViews,
                                    currentViewIndex,
                                    containerToUse);
                            }
                            else
                            {
                                this.NotifyNavigationFailed(navigationContext, navigationCallback, null);
                            }
                        });
                }
                else
                {
                    RequestCanNavigateFromOnCurrentlyActiveViewModel(
                        navigationContext,
                        navigationCallback,
                        activeViews,
                        currentViewIndex,
                        containerToUse);
                }
            }
            else
            {
                ExecuteNavigation(navigationContext, activeViews, navigationCallback, containerToUse);
            }
        }

        private void RequestCanNavigateFromOnCurrentlyActiveViewModel(
            NavigationContext navigationContext,
            Action<NavigationResult> navigationCallback,
            object[] activeViews,
            int currentViewIndex,
            IUnityContainer containerToUse)
        {
            var frameworkElement = activeViews[currentViewIndex] as FrameworkElement;

            if (frameworkElement != null)
            {
                var vetoingViewModel = frameworkElement.DataContext as IConfirmNavigationRequest;

                if (vetoingViewModel != null)
                {
                    // the data model for the current active view implements IConfirmNavigationRequest, request confirmation
                    // providing a callback to resume the navigation request
                    vetoingViewModel.ConfirmNavigationRequest(
                        navigationContext,
                        canNavigate =>
                        {
                            if (this.currentNavigationContext == navigationContext && canNavigate)
                            {
                                RequestCanNavigateFromOnCurrentlyActiveView(
                                    navigationContext,
                                    navigationCallback,
                                    activeViews,
                                    currentViewIndex + 1,
                                    containerToUse);
                            }
                            else
                            {
                                this.NotifyNavigationFailed(navigationContext, navigationCallback, null);
                            }
                        });

                    return;
                }
            }

            RequestCanNavigateFromOnCurrentlyActiveView(
                navigationContext,
                navigationCallback,
                activeViews,
                currentViewIndex + 1,
                containerToUse);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception is marshalled to callback")]
        private void ExecuteNavigation(NavigationContext navigationContext, object[] activeViews, Action<NavigationResult> navigationCallback, IUnityContainer containerToUse)
        {
            try
            {
                NotifyActiveViewsNavigatingFrom(navigationContext, activeViews);
                object view = null;
                if (containerToUse == null)
                {
                    view = this.regionNavigationContentLoader.LoadContent(this.Region, navigationContext);
                }
                else
                {
                    CustomRegionNavigationContentLoader customRegionNavigationContentLoader = this.regionNavigationContentLoader as CustomRegionNavigationContentLoader;
                    if (customRegionNavigationContentLoader == null)
                        throw new InvalidOperationException("CustomRegionNavigationService.ExecuteNavigation method that takes a container may only be used with a CustomRegionNavigationContentLoader");

                    view = ((CustomRegionNavigationContentLoader)this.regionNavigationContentLoader).LoadContent(this.Region, navigationContext, containerToUse);

                }


                // Raise the navigating event just before activing the view.
                this.RaiseNavigating(navigationContext);

                this.Region.Activate(view);

                // Update the navigation journal before notifying others of navigaton
                IRegionNavigationJournalEntry journalEntry = this.serviceLocator.GetInstance<IRegionNavigationJournalEntry>();
                journalEntry.Uri = navigationContext.Uri;
                this.journal.RecordNavigation(journalEntry);

                // The view can be informed of navigation
                InvokeOnNavigationAwareElement(view, (n) => n.OnNavigatedTo(navigationContext));

                navigationCallback(new NavigationResult(navigationContext, true));

                // Raise the navigated event when navigation is completed.
                this.RaiseNavigated(navigationContext);
            }
            catch (Exception e)
            {
                this.NotifyNavigationFailed(navigationContext, navigationCallback, e);
            }
        }

        private void NotifyNavigationFailed(NavigationContext navigationContext, Action<NavigationResult> navigationCallback, Exception e)
        {
            var navigationResult =
                e != null ? new NavigationResult(navigationContext, e) : new NavigationResult(navigationContext, false);

            navigationCallback(navigationResult);
            this.RaiseNavigationFailed(navigationContext, e);
        }

        private static void NotifyActiveViewsNavigatingFrom(NavigationContext navigationContext, object[] activeViews)
        {
            InvokeOnNavigationAwareElements(activeViews, (n) => n.OnNavigatedFrom(navigationContext));
        }

        private static void InvokeOnNavigationAwareElements(IEnumerable<object> items, Action<INavigationAware> invocation)
        {
            foreach (var item in items)
            {
                InvokeOnNavigationAwareElement(item, invocation);
            }
        }

        private static void InvokeOnNavigationAwareElement(object item, Action<INavigationAware> invocation)
        {
            var navigationAwareItem = item as INavigationAware;
            if (navigationAwareItem != null)
            {
                invocation(navigationAwareItem);
            }

            FrameworkElement frameworkElement = item as FrameworkElement;
            if (frameworkElement != null)
            {
                INavigationAware navigationAwareDataContext = frameworkElement.DataContext as INavigationAware;
                if (navigationAwareDataContext != null)
                {
                    invocation(navigationAwareDataContext);
                }
            }
        }
    }
}

