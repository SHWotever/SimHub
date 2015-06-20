using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

#if NETFX_CORE

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;

#else

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

#endif

namespace TestApplication.Shared
{
    /// <summary>
    /// Layout aware page which can be used in SL, WinRT and WPF
    /// </summary>
    public class PortablePopup : ContentControl
    {
        private double CONSTPOPUPWIDTH = 400;

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
          DependencyProperty.Register("IsOpen",
          typeof(bool), typeof(PortablePopup), new PropertyMetadata(false, OnIsOpenChanged));

        public object HideOnPropertyChange
        {
            get { return (object)GetValue(HideOnPropertyChangeProperty); }
            set { SetValue(HideOnPropertyChangeProperty, value); }
        }

        public static readonly DependencyProperty HideOnPropertyChangeProperty =
          DependencyProperty.Register("HideOnPropertyChange",
          typeof(object), typeof(PortablePopup), new PropertyMetadata(false, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PortablePopup).HidePopup();
        }

        private void HidePopup()
        {
            if (popup != null)
            {
                ClosePopup();
            }
        }

        private void ClosePopup()
        {
            this.SetValue(IsOpenProperty, false);
        }

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((e.NewValue as bool?) == true)
            {
                (d as PortablePopup).OnPopupOpened();
            }
            else
            {
                (d as PortablePopup).OnPopupClosed();
            }
        }

        private void OnPopupClosed()
        {
#if SILVERLIGHT
            popup.ReleaseMouseCapture();
            var popupAncestor = FindHighestAncestor(this.popup);
            if (popupAncestor == null)
            {
                return;
            }
            // popupAncestor.RemoveHandler(MouseLeftButtonUpEvent, (MouseButtonEventHandler)OnMouseLeftButtonDown);
#endif
        }

        private void OnPopupOpened()
        {
#if SILVERLIGHT
            popup.CaptureMouse();
            var popupAncestor = FindHighestAncestor(this.popup);
            if (popupAncestor == null)
            {
                return;
            }
            // popupAncestor.AddHandler(MouseLeftButtonUpEvent, (MouseButtonEventHandler)OnMouseLeftButtonDown, true);
#endif
        }

#if SILVERLIGHT
        private static FrameworkElement FindHighestAncestor(Popup popup)
        {
            var ancestor = (FrameworkElement)popup;
            while (true)
            {
                var parent = VisualTreeHelper.GetParent(ancestor) as FrameworkElement;
                if (parent == null)
                {
                    return ancestor;
                }

                ancestor = parent;
            }
        }
#endif

        public PortablePopup()
        {

#if NETFX_CORE
            this.DefaultStyleKey = typeof(PortablePopup);
#elif SILVERLIGHT
            this.DefaultStyleKey = typeof(PortablePopup);
#else
            
#endif
        }

        static PortablePopup()
        {
#if NETFX_CORE

#elif SILVERLIGHT

#else
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PortablePopup), new FrameworkPropertyMetadata(typeof(PortablePopup)));
#endif
        }

#if NETFX_CORE
        protected override void OnApplyTemplate()
        {
            InternalOnApplyTemplate();
        }
#else
        public override void OnApplyTemplate()
        {
            InternalOnApplyTemplate();
        }
#endif
        private ContentControl contentControl = null;
        private Popup popup = null;
        private Button closeButton = null;
        public void InternalOnApplyTemplate()
        {
            popup = this.GetTemplateChild("PART_Popup") as Popup;
            contentControl = this.GetTemplateChild("PART_Content") as ContentControl;
            closeButton = this.GetTemplateChild("PART_CloseButton") as Button;

            closeButton.Click += closeButton_Click;
#if NETFX_CORE
            //popup.IsLightDismissEnabled = true;
            popup.PointerPressed += popup_PointerPressed;
            popup.Closed += popup_Closed;
            popup.Width = CONSTPOPUPWIDTH;
            popup.HorizontalOffset = -75;
            contentControl.Width = CONSTPOPUPWIDTH;
            contentControl.MaxHeight = Window.Current.Bounds.Height - 150;

#elif SILVERLIGHT
            //do nothing
            popup.Closed += PopupOnClosed;
            //popup.MouseLeftButtonUp += PopupOnMouseLeftButtonUp;

            //contentControl.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(TheStackPanel_MouseDown), true);

            //contentControl.MouseLeftButtonUp += contentControl_MouseLeftButtonUp;
            //contentControl.Loaded += ContentControlOnLoaded;
            //popup.SetWindow();

            popup.Width = CONSTPOPUPWIDTH;
            popup.HorizontalOffset = -75;
            contentControl.Width = CONSTPOPUPWIDTH;
            contentControl.MaxHeight = TestApplicationSL.App.Current.Host.Content.ActualHeight - 150;
#else
            popup.Width = CONSTPOPUPWIDTH;
            popup.AllowsTransparency = true;
            popup.HorizontalOffset = -75;
            popup.Height = Application.Current.MainWindow.Height - 150;
#endif
            base.OnApplyTemplate();
        }

        void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsOpen = false;
        }
        
#if NETFX_CORE
        private void popup_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.SetValue(IsOpenProperty, false);
        }
        void popup_Closed(object sender, object e)
        {
            this.SetValue(IsOpenProperty, false);
        }
#elif SILVERLIGHT
        private void PopupOnClosed(object sender, EventArgs eventArgs)
        {
            this.SetValue(IsOpenProperty, false);
        }
#endif
    }
}
