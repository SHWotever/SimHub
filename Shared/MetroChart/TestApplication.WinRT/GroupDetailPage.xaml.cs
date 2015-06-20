using TestApplication.WinRT.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;

// Die Elementvorlage für die Seite "Gruppendetails" ist unter http://go.microsoft.com/fwlink/?LinkId=234229 dokumentiert.

namespace TestApplication.WinRT
{
    /// <summary>
    /// Eine Seite, auf der eine Übersicht über eine einzelne Gruppe einschließlich einer Vorschau der Elemente
    /// in der Gruppe angezeigt wird.
    /// </summary>
    public sealed partial class GroupDetailPage : TestApplication.WinRT.Common.LayoutAwarePage
    {        
        public GroupDetailPage()
        {
            
            this.InitializeComponent();

            this.DataContext = new TestApplication.Shared.TestPageViewModel();
        }
    }
}
