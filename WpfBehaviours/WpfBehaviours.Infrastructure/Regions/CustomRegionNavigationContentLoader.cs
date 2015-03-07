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
using System.Globalization;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Properties;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace WpfBehaviours.Infrastructure.Regions
{
    /// <summary>
    /// Implementation of <see cref="IRegionNavigationContentLoader"/> that relies on a <see cref="IServiceLocator"/>
    /// to create new views when necessary.
    /// </summary>
    public class CustomRegionNavigationContentLoader : IRegionNavigationContentLoader
    {
        private readonly IServiceLocator serviceLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionNavigationContentLoader"/> class with a service locator.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        public CustomRegionNavigationContentLoader(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }


        /// <summary>
        /// Gets the view to which the navigation request represented by <paramref name="navigationContext"/> applies.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <param name="navigationContext">The context representing the navigation request.</param>
        /// <returns>
        /// The view to be the target of the navigation request.
        /// </returns>
        /// <remarks>
        /// If none of the views in the region can be the target of the navigation request, a new view
        /// is created and added to the region.
        /// </remarks>
        /// <exception cref="ArgumentException">when a new view cannot be created for the navigation request.</exception>
        public object LoadContent(IRegion region, NavigationContext navigationContext)
        {
            return LoadContent(region, navigationContext, null);
        }


        public object LoadContent(IRegion region, NavigationContext navigationContext, IUnityContainer containerToUse)
        {
            if (region == null) throw new ArgumentNullException("region");
            if (navigationContext == null) throw new ArgumentNullException("navigationContext");

            string candidateTargetContract = this.GetContractFromNavigationContext(navigationContext);

            var candidates = this.GetCandidatesFromRegion(region, candidateTargetContract);

            var acceptingCandidates =
                candidates.Where(
                    v =>
                    {
                        var navigationAware = v as INavigationAware;
                        if (navigationAware != null && !navigationAware.IsNavigationTarget(navigationContext))
                        {
                            return false;
                        }

                        var frameworkElement = v as FrameworkElement;
                        if (frameworkElement == null)
                        {
                            return true;
                        }

                        navigationAware = frameworkElement.DataContext as INavigationAware;
                        return navigationAware == null || navigationAware.IsNavigationTarget(navigationContext);
                    });


            var view = acceptingCandidates.FirstOrDefault();

            if (view != null)
            {
                return view;
            }

            view = this.CreateNewRegionItem(candidateTargetContract, containerToUse);

            region.Add(view);

            return view;
        }

        /// <summary>
        /// Provides a new item for the region based on the supplied candidate target contract name.
        /// </summary>
        /// <param name="candidateTargetContract">The target contract to build.</param>
        /// <returns>An instance of an item to put into the <see cref="IRegion"/>.</returns>
        protected virtual object CreateNewRegionItem(string candidateTargetContract, IUnityContainer containerToUse)
        {
            object newRegionItem;
            try
            {
                if (containerToUse == null)
                {
                    newRegionItem = this.serviceLocator.GetInstance<object>(candidateTargetContract);
                }
                else
                {
                    newRegionItem = containerToUse.Resolve<object>(candidateTargetContract);
                }
            }
            catch (ActivationException e)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, "Can not create navigation target {0}", candidateTargetContract),
                    e);
            }
            return newRegionItem;
        }

        /// <summary>
        /// Returns the candidate TargetContract based on the <see cref="NavigationContext"/>.
        /// </summary>
        /// <param name="navigationContext">The navigation contract.</param>
        /// <returns>The candidate contract to seek within the <see cref="IRegion"/> and to use, if not found, when resolving from the container.</returns>
        protected virtual string GetContractFromNavigationContext(NavigationContext navigationContext)
        {
            if (navigationContext == null) throw new ArgumentNullException("navigationContext");

            var candidateTargetContract = UriParsingHelper.GetAbsolutePath(navigationContext.Uri);
            candidateTargetContract = candidateTargetContract.TrimStart('/');
            return candidateTargetContract;
        }

        /// <summary>
        /// Returns the set of candidates that may satisfiy this navigation request.
        /// </summary>
        /// <param name="region">The region containing items that may satisfy the navigation request.</param>
        /// <param name="candidateNavigationContract">The candidate navigation target as determined by <see cref="GetContractFromNavigationContext"/></param>
        /// <returns>An enumerable of candidate objects from the <see cref="IRegion"/></returns>
        protected virtual IEnumerable<object> GetCandidatesFromRegion(IRegion region, string candidateNavigationContract)
        {
            if (region == null) throw new ArgumentNullException("region");
            return region.Views.Where(v =>
                string.Equals(v.GetType().Name, candidateNavigationContract, StringComparison.Ordinal) ||
                string.Equals(v.GetType().FullName, candidateNavigationContract, StringComparison.Ordinal));
        }
    }
}

