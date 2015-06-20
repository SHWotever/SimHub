namespace De.TorstenMandelkow.MetroChart
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

#if NETFX_CORE
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Shapes;
    using Windows.UI.Xaml.Markup;
    using Windows.UI.Xaml;
    using Windows.Foundation;
    using Windows.UI;
    using Windows.UI.Core;
#else
    using System.Windows.Controls;
    using System.Windows;
    using System.Windows.Data;
#endif

    public class ResourceDictionaryCollection : ObservableCollection<ResourceDictionary>
    {
        public ResourceDictionaryCollection()
        {
            
        }
    }
}
