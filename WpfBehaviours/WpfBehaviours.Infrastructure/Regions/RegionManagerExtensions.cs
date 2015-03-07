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
using System.Globalization;
using System.Threading;
using Microsoft.Practices.Prism.Properties;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace WpfBehaviours.Infrastructure.Regions
{
    /// <summary>
    /// Class that creates a fluent interface for the <see cref="IRegionManager"/> class, with respect to 
    /// adding views to regions (View Injection pattern), registering view types to regions (View Discovery pattern)
    /// </summary>
    public static class RegionManagerExtensions
    {
        /// <summary>
        /// Navigates the specified region manager.
        /// </summary>
        /// <param name="regionManager">The regionmanager that this extension method effects.</param>
        /// <param name="regionName">The name of the region to call Navigate on.</param>
        /// <param name="source">The URI of the content to display.</param>
        /// <param name="navigationCallback">The navigation callback.</param>
        /// <param name="containerToUse">The IUnityContainer to use, which could be the main application container or a child container.</param>
        public static void RequestNavigateUsingSpecificContainer(this IRegionManager regionManager, string regionName, Uri source, Action<NavigationResult> navigationCallback, IUnityContainer containerToUse)
        {
            if (regionManager == null) throw new ArgumentNullException("regionManager");
            if (navigationCallback == null) throw new ArgumentNullException("navigationCallback");
            if (containerToUse == null) throw new ArgumentNullException("containerToUse");

            if (regionManager.Regions.ContainsRegionWithName(regionName))
            {
                regionManager.Regions[regionName].RequestNavigateUsingSpecificContainer(source, navigationCallback, containerToUse);
            }
            else
            {
                navigationCallback(new NavigationResult(new NavigationContext(null, source), false));
            }
        }

        /// <summary>
        /// Navigates the specified region manager.
        /// </summary>
        /// <param name="regionManager">The regionmanager that this extension method effects.</param>
        /// <param name="regionName">The name of the region to call Navigate on.</param>
        /// <param name="source">The URI of the content to display.</param>
        /// <param name="containerToUse">The IUnityContainer to use, which could be the main application container or a child container.</param>
        public static void RequestNavigateUsingSpecificContainer(this IRegionManager regionManager, string regionName, Uri source, IUnityContainer containerToUse)
        {
            if (containerToUse == null) throw new ArgumentNullException("containerToUse");
            RequestNavigateUsingSpecificContainer(regionManager, regionName, source, nr => { }, containerToUse);
        }

        /// <summary>
        /// Navigates the specified region manager.
        /// </summary>
        /// <param name="regionManager">The regionmanager that this extension method effects.</param>
        /// <param name="regionName">The name of the region to call Navigate on.</param>
        /// <param name="source">The URI of the content to display.</param>
        /// <param name="navigationCallback">The navigation callback.</param>
        /// <param name="containerToUse">The IUnityContainer to use, which could be the main application container or a child container.</param>
        public static void RequestNavigateUsingSpecificContainer(this IRegionManager regionManager, string regionName, string source, Action<NavigationResult> navigationCallback, IUnityContainer containerToUse)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (containerToUse == null) throw new ArgumentNullException("containerToUse");
            RequestNavigateUsingSpecificContainer(regionManager, regionName, new Uri(source, UriKind.RelativeOrAbsolute), navigationCallback, containerToUse);
        }

        /// <summary>
        /// Navigates the specified region manager.
        /// </summary>
        /// <param name="regionManager">The regionmanager that this extension method effects.</param>
        /// <param name="regionName">The name of the region to call Navigate on.</param>
        /// <param name="source">The URI of the content to display.</param>
        /// <param name="containerToUse">The IUnityContainer to use, which could be the main application container or a child container.</param>
        public static void RequestNavigateUsingSpecificContainer(this IRegionManager regionManager, string regionName, string source, IUnityContainer containerToUse)
        {
            if (containerToUse == null) throw new ArgumentNullException("containerToUse");
            RequestNavigateUsingSpecificContainer(regionManager, regionName, source, nr => { }, containerToUse);
        }
    }
}

