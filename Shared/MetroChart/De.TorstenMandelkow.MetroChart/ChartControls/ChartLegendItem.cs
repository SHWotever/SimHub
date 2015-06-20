using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if NETFX_CORE

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

#else

using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media;

#endif

namespace De.TorstenMandelkow.MetroChart
{
    public class ChartLegendItem : ContentControl
    {        
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(ChartLegendItem),
            new PropertyMetadata(null));
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(ChartLegendItem),
            new PropertyMetadata(0.0));
        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("Percentage", typeof(double), typeof(ChartLegendItem),
            new PropertyMetadata(null));
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.Register("ItemBrush", typeof(Brush), typeof(ChartLegendItem),
            new PropertyMetadata(null));
     
        static ChartLegendItem()
        {
#if NETFX_CORE
            //do nothing
#elif SILVERLIGHT
            //do nothing
#else
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartLegendItem), new FrameworkPropertyMetadata(typeof(ChartLegendItem))); 
#endif
        }

        public ChartLegendItem()
        {
#if NETFX_CORE
            this.DefaultStyleKey = typeof(ChartLegendItem);
#elif SILVERLIGHT
            this.DefaultStyleKey = typeof(ChartLegendItem);
#else
            //do nothing
#endif
        }

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        public Brush ItemBrush
        {
            get { return (Brush)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }
    }
}
