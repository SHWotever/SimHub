using System;
using System.Collections;
namespace De.TorstenMandelkow.MetroChart
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Collections.Specialized;

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
    using Windows.UI.Xaml.Data;

#else
    using System.Windows.Media;
    using System.Windows.Controls;
    using System.Windows;
#endif

    public class ChartSeries : ItemsControl
    { 
        public static readonly DependencyProperty DisplayMemberProperty =
            DependencyProperty.Register("DisplayMember",
            typeof(string),
            typeof(ChartSeries),
            new PropertyMetadata(null));
        public static readonly DependencyProperty ValueMemberProperty =
            DependencyProperty.Register("ValueMember",
            typeof(string),
            typeof(ChartSeries),
            new PropertyMetadata(null));
        public static readonly DependencyProperty SeriesTitleProperty =
            DependencyProperty.Register("SeriesTitle",
            typeof(string),
            typeof(ChartSeries),
            new PropertyMetadata(null));
        
        public ChartSeries()
        {   
        }

        public string SeriesTitle
        {
            get
            {
                return (string)GetValue(SeriesTitleProperty);
            }
            set
            {
                SetValue(SeriesTitleProperty, value);
            }
        }

        public string DisplayMember
        {
            get
            {
                return (string)GetValue(DisplayMemberProperty);
            }
            set
            {
                SetValue(DisplayMemberProperty, value);
            }
        }

        public string ValueMember
        {
            get
            {
                return (string)GetValue(ValueMemberProperty);
            }
            set
            {
                SetValue(ValueMemberProperty, value);
            }
        }
    }
}
