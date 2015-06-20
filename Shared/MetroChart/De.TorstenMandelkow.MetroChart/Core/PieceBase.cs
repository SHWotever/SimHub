namespace De.TorstenMandelkow.MetroChart
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Reflection;
    using System.Collections.Specialized;
    using System.Windows.Input;

#if NETFX_CORE
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Shapes;
    using Windows.UI.Xaml.Markup;
    using Windows.UI.Xaml;
    using Windows.Foundation;
    using Windows.UI;
    using Windows.UI.Xaml.Media.Animation;
    using Windows.UI.Core;
#else
    using System.Windows.Media;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

#endif

    [TemplateVisualState(Name = BarPiece.StateSelectionUnselected, GroupName = BarPiece.GroupSelectionStates)]
    [TemplateVisualState(Name = BarPiece.StateSelectionSelected, GroupName = BarPiece.GroupSelectionStates)]
    public abstract class PieceBase : Control
    {
        #region Fields

        internal const string StateSelectionUnselected = "Unselected";
        internal const string StateSelectionSelected = "Selected";
        internal const string GroupSelectionStates = "SelectionStates";

        public static readonly DependencyProperty ClientHeightProperty =
            DependencyProperty.Register("ClientHeight", typeof(double), typeof(PieceBase),
            new PropertyMetadata(0.0, new PropertyChangedCallback(OnSizeChanged)));
        
        public static readonly DependencyProperty ClientWidthProperty =
            DependencyProperty.Register("ClientWidth", typeof(double), typeof(PieceBase),
            new PropertyMetadata(0.0, new PropertyChangedCallback(OnSizeChanged)));

        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(PieceBase),
            new PropertyMetadata(null));
        
        public static readonly DependencyProperty ParentChartProperty =
            DependencyProperty.Register("ParentChart", typeof(ChartBase), typeof(PieceBase),
            new PropertyMetadata(null));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(PieceBase),
            new PropertyMetadata(false, new PropertyChangedCallback(OnIsSelectedChanged)));
        
        public static readonly DependencyProperty IsClickedByUserProperty =
            DependencyProperty.Register("IsClickedByUser", typeof(bool), typeof(PieceBase),
            new PropertyMetadata(false));

        #endregion Fields

        #region Properties

        public double ClientHeight
        {
            get { return (double)GetValue(ClientHeightProperty); }
            set { SetValue(ClientHeightProperty, value); }
        }

        public double ClientWidth
        {
            get { return (double)GetValue(ClientWidthProperty); }
            set { SetValue(ClientWidthProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public bool IsClickedByUser
        {
            get { return (bool)GetValue(IsClickedByUserProperty); }
            set { SetValue(IsClickedByUserProperty, value); }
        }

        public ChartBase ParentChart
        {
            get { return (ChartBase)GetValue(ParentChartProperty); }
            set { SetValue(ParentChartProperty, value); }
        }

        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        public string Caption
        {
            get
            {
                return (this.DataContext as DataPoint).DisplayName;
            }
        }

        #endregion Properties

        #region Methods

        private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PieceBase).DrawGeometry(false);
        }

        protected virtual void DrawGeometry(bool withAnimation = true)
        {
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PieceBase source = (PieceBase)d;
            bool oldValue = (bool)e.OldValue;
            bool newValue = (bool)e.NewValue;
            source.OnIsSelectedPropertyChanged(oldValue, newValue);
        }

        protected virtual void OnIsSelectedPropertyChanged(bool oldValue, bool newValue)
        {
            this.IsClickedByUser = false;
            VisualStateManager.GoToState(this, newValue ? StateSelectionSelected : StateSelectionUnselected, true);
        }

#if NETFX_CORE
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InternalOnApplyTemplate();
        }
#else
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InternalOnApplyTemplate();
        }
#endif

        protected virtual void InternalOnApplyTemplate()
        {
        }

        protected void RegisterMouseEvents(UIElement slice)
        {

            if (slice != null)
            {
#if NETFX_CORE
                slice.PointerPressed += delegate 
#else
                slice.MouseLeftButtonUp += delegate
#endif
                {
                    InternalMousePressed();
                };

#if NETFX_CORE
                slice.PointerMoved += delegate
#else
                slice.MouseMove += delegate
#endif
                {
                    InternalMouseMoved();
                };
            }
        }

        private void InternalMousePressed()
        {
            SetValue(PieceBase.IsClickedByUserProperty, true);
        }

        private void InternalMouseMoved()
        {
            //SetValue(PieceBase.Is, true);
        }

#if NETFX_CORE
        protected override void OnPointerPressed(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            HandleMouseDown();
            e.Handled = true;
        }
#else
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            HandleMouseDown();
            e.Handled = true;
        }
#endif

        private void HandleMouseDown()
        {
            IsClickedByUser = true;
        }

        #endregion Methods
    }
}
