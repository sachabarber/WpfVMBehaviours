using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfBehaviours.ControlSet.Behaviors
{
   


    public static class ScrollViewerBehaviors
    {

        #region ShouldAdjustAdorner

        public static readonly DependencyProperty ShouldAdjustAdornerProperty =
            DependencyProperty.RegisterAttached("ShouldAdjustAdorner", typeof(bool), typeof(ScrollViewerBehaviors),
                new FrameworkPropertyMetadata((bool)false));

        public static bool GetShouldAdjustAdorner(DependencyObject d)
        {
            return (bool)d.GetValue(ShouldAdjustAdornerProperty);
        }

        public static void SetShouldAdjustAdorner(DependencyObject d, bool value)
        {
            d.SetValue(ShouldAdjustAdornerProperty, value);
        }

        #endregion

        


        #region AdjustAdornerForScrollViewer

        public static readonly DependencyProperty AdjustAdornerForScrollViewerProperty =
            DependencyProperty.RegisterAttached("AdjustAdornerForScrollViewer", typeof(ScrollViewer), typeof(ScrollViewerBehaviors),
                new FrameworkPropertyMetadata(null,OnAdjustAdornerForScrollViewerChanged));

        public static bool GetAdjustAdornerForScrollViewer(DependencyObject d)
        {
            return (bool)d.GetValue(AdjustAdornerForScrollViewerProperty);
        }

        public static void SetAdjustAdornerForScrollViewer(DependencyObject d, bool value)
        {
            d.SetValue(AdjustAdornerForScrollViewerProperty, value);
        }

        private static void OnAdjustAdornerForScrollViewerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScrollContentPresenter scp = (ScrollContentPresenter)d;
            ScrollViewer oldScrollViewer = (ScrollViewer)e.OldValue;
            ScrollViewer newScrollViewer = (ScrollViewer)d.GetValue(AdjustAdornerForScrollViewerProperty);

            ScrollChangedEventHandler scrollChangedEventHandler = (s, ea) =>
            {
                int children = VisualTreeHelper.GetChildrenCount(scp);
                for (int i = 0; i < children; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(scp, i);
                    if (child is AdornerLayer)
                    {
                        AdornerLayer al = child as AdornerLayer;
                        al.Dispatcher.BeginInvoke(DispatcherPriority.Send, new ThreadStart(al.Update));
                    }
                }
            };

            if (oldScrollViewer != null)
            {
                oldScrollViewer.ScrollChanged -= scrollChangedEventHandler;
            }

            if (newScrollViewer != null)
            {
                if ((bool)newScrollViewer.GetValue(ShouldAdjustAdornerProperty))
                {
                    newScrollViewer.ScrollChanged += scrollChangedEventHandler;
                }
            }
        }

        #endregion

        

        

        
    }
}
