namespace De.TorstenMandelkow.MetroChart
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows;

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
    using System.Windows.Data;
#endif

    public abstract class ChartBase : Control, INotifyPropertyChanged
    {
        #region Fields

        private bool onApplyTemplateFinished = false;
        private ObservableCollection<string> gridLines = new ObservableCollection<string>();

        public static readonly DependencyProperty PlotterAreaStyleProperty =
            DependencyProperty.Register("PlotterAreaStyle",
            typeof(Style), typeof(ChartBase), new PropertyMetadata(null));

        public static readonly DependencyProperty ChartAreaStyleProperty =
            DependencyProperty.Register("ChartAreaStyle",
            typeof(Style),  typeof(ChartBase), new PropertyMetadata(null));

        public static readonly DependencyProperty ChartLegendItemStyleProperty =
            DependencyProperty.Register("ChartLegendItemStyle",
            typeof(Style), typeof(ChartBase), new PropertyMetadata(null));

        public static readonly DependencyProperty ChartTitleProperty =
            DependencyProperty.Register("ChartTitle",
            typeof(string),  typeof(ChartBase), new PropertyMetadata("ChartTitle"));

        public static readonly DependencyProperty ChartSubTitleProperty =
            DependencyProperty.Register("ChartSubTitle",
            typeof(string), typeof(ChartBase), new PropertyMetadata("ChartSubTitle"));

        public static readonly DependencyProperty ChartTitleVisibilityProperty =
            DependencyProperty.Register("ChartTitleVisibility",
            typeof(Visibility), typeof(ChartBase), new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty ChartLegendVisibilityProperty =
            DependencyProperty.Register("ChartLegendVisibility",
            typeof(Visibility), typeof(ChartBase), new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty PaletteProperty =
            DependencyProperty.Register("Palette",
            typeof(ResourceDictionaryCollection), typeof(ChartBase), new PropertyMetadata(null, OnPaletteChanged));

        public static readonly DependencyProperty DefaultPaletteProperty =
            DependencyProperty.Register("DefaultPalette",
            typeof(ResourceDictionaryCollection), typeof(ChartBase), new PropertyMetadata(null));
        
        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush",
            typeof(Brush), typeof(ChartBase), new PropertyMetadata(new SolidColorBrush(Colors.Orange)));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem",
            typeof(object), typeof(ChartBase), new PropertyMetadata(null));
        
        public static readonly DependencyProperty IsRowColumnSwitchedProperty =
            DependencyProperty.Register("IsRowColumnSwitched",
            typeof(bool), typeof(ChartBase), new PropertyMetadata(false, new PropertyChangedCallback(OnIsRowColumnSwitchedChanged)));

        public static readonly DependencyProperty ChartTitleStyleProperty =
            DependencyProperty.Register("ChartTitleStyle",
            typeof(Style), typeof(ChartBase), new PropertyMetadata(null));

        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register("Series",
            typeof(ObservableCollection<ChartSeries>), typeof(ChartBase), new PropertyMetadata(null, new PropertyChangedCallback(OnSeriesChanged)));
        
        public static readonly DependencyProperty InternalDataContextProperty =
            DependencyProperty.Register("InternalDataContext",
            typeof(object), typeof(ChartBase), new PropertyMetadata(null, new PropertyChangedCallback(InternalDataContextChanged)));

        public static readonly DependencyProperty SeriesSourceProperty =
           DependencyProperty.Register("SeriesSource",
           typeof(IEnumerable), typeof(ChartBase), new PropertyMetadata(null, OnSeriesSourceChanged));

        public static readonly DependencyProperty ToolTipFormatProperty =
            DependencyProperty.Register("ToolTipFormat",
            typeof(string), typeof(ChartBase), new PropertyMetadata("{0} ({1})"));

        public static readonly DependencyProperty ExceptionsProperty =
           DependencyProperty.Register("Exceptions",
           typeof(ObservableCollection<string>), typeof(ChartBase), new PropertyMetadata(new ObservableCollection<string>()));

        public static readonly DependencyProperty SeriesTemplateProperty =
           DependencyProperty.Register("SeriesTemplate",
           typeof(DataTemplate), typeof(ChartBase), new PropertyMetadata(null, OnSeriesTemplateChanged));

        public static readonly DependencyProperty MaxDataPointValueProperty =
            DependencyProperty.Register("MaxDataPointValue",
            typeof(double), typeof(ChartBase), new PropertyMetadata(0.0, OnMaxDataPointValueChanged));

        public static readonly DependencyProperty SumOfDataPointGroupProperty =
             DependencyProperty.Register("SumOfDataPointGroup",
             typeof(double), typeof(ChartBase), new PropertyMetadata(0.0));

        public static readonly DependencyProperty MaxDataPointGroupSumProperty =
             DependencyProperty.Register("MaxDataPointGroupSum",
             typeof(double), typeof(ChartBase), new PropertyMetadata(0.0, OnMaxDataPointGroupSumChanged));

        #endregion Fields

        
        #region DataContext stuff

        public static DependencyProperty DataContextWatcherProperty = DependencyProperty.Register(
            "DataContextWatcher",
            typeof(object),
            typeof(ChartBase),
            new PropertyMetadata(null, DataContextWatcher_Changed));

        public static void DataContextWatcher_Changed(
               DependencyObject sender,
               DependencyPropertyChangedEventArgs args)
        {
            ChartBase senderControl = sender as ChartBase;
            if (senderControl != null)
            {
                (senderControl as ChartBase).InternalDataContextChanged();
            }
        }

        private static void InternalDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartBase).InternalDataContextChanged();
        }

        private void InternalDataContextChanged()
        {
            UpdateDataContextOfSeries();
        }

        public object InternalDataContext
        {
            get { return GetValue(InternalDataContextProperty); }
            set { SetValue(InternalDataContextProperty, value); }
        }

        private void UpdateDataContextOfSeries()
        {
            onApplyTemplateFinished = false;
            foreach (var newItem in this.Series)
            {
                if (newItem is FrameworkElement)
                {
                    (newItem as FrameworkElement).DataContext = this.DataContext;
                }
            }
            onApplyTemplateFinished = true;
            UpdateSeries();
        }

        #endregion

        #region INotifiy stuff

        private static void OnSeriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartBase).AttachEventHandler(e.NewValue);
        }

        private void AttachEventHandler(object collection)
        {
            if (collection is INotifyCollectionChanged)
            {
                (collection as INotifyCollectionChanged).CollectionChanged += ChartBase_CollectionChanged;
            }
        }

        void ChartBase_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var newSeries in e.NewItems)
                {
                    if (newSeries is ItemsControl)
                    {
#if NETFX_CORE
                        (newSeries as ItemsControl).Items.VectorChanged += Items_VectorChanged;

#else
                        if ((newSeries as ItemsControl).Items is INotifyCollectionChanged)
                        {
                            ((INotifyCollectionChanged)(newSeries as ItemsControl).Items).CollectionChanged += new NotifyCollectionChangedEventHandler(Window1_CollectionChanged);
                        }
#endif
                    }
                }
            }
        }

#if NETFX_CORE
        void Items_VectorChanged(Windows.Foundation.Collections.IObservableVector<object> sender, Windows.Foundation.Collections.IVectorChangedEventArgs @event)
        {
            UpdateSeries();
        }
#else
        private void Window1_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //new items added to a series, we may update them
            UpdateSeries();
        }
#endif
        #endregion

        #region Constructors

        public ChartBase()
        {
            Series = new ObservableCollection<ChartSeries>();
            SetBinding(DataContextWatcherProperty, new Binding());

            UpdateGridLines();
        }

        #endregion Constructors

        #region Properties

        public Style PlotterAreaStyle
        {
            get { return (Style)GetValue(PlotterAreaStyleProperty); }
            set { SetValue(PlotterAreaStyleProperty, value); }
        }

        public Style ChartAreaStyle
        {
            get { return (Style)GetValue(ChartAreaStyleProperty); }
            set { SetValue(ChartAreaStyleProperty, value); }
        }

        public Style ChartLegendItemStyle
        {
            get { return (Style)GetValue(ChartLegendItemStyleProperty); }
            set { SetValue(ChartLegendItemStyleProperty, value); }
        }

        public string ChartTitle
        {
            get { return (string)GetValue(ChartTitleProperty); }
            set { SetValue(ChartTitleProperty, value); }
        }

        public string ChartSubTitle
        {
            get { return (string)GetValue(ChartSubTitleProperty); }
            set { SetValue(ChartSubTitleProperty, value); }
        }

        public Visibility ChartTitleVisibility
        {
            get { return (Visibility)GetValue(ChartTitleVisibilityProperty); }
            set { SetValue(ChartTitleVisibilityProperty, value); }
        }
        
        public Style ChartTitleStyle
        {
            get { return (Style)GetValue(ChartTitleStyleProperty); }
            set { SetValue(ChartTitleStyleProperty, value); }
        }

        public double MaxDataPointValue
        {
            get { return (double)GetValue(MaxDataPointValueProperty); }
            set { SetValue(MaxDataPointValueProperty, value); }
        }

        public double MaxDataPointGroupSum
        {
            get { return (double)GetValue(MaxDataPointGroupSumProperty); }
            set { SetValue(MaxDataPointGroupSumProperty, value); }
        }

        public ObservableCollection<ChartSeries> Series
        {
            get { return (ObservableCollection<ChartSeries>)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public bool IsRowColumnSwitched
        {
            get { return (bool)GetValue(IsRowColumnSwitchedProperty); }
            set { SetValue(IsRowColumnSwitchedProperty, value); }
        }

        public ResourceDictionaryCollection Palette
        {
            get { return (ResourceDictionaryCollection)GetValue(PaletteProperty); }
            set { SetValue(PaletteProperty, value); }
        }

        public ResourceDictionaryCollection DefaultPalette
        {
            get { return (ResourceDictionaryCollection)GetValue(DefaultPaletteProperty); }
            set { SetValue(DefaultPaletteProperty, value); }
        }
        
        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }        

        public string ToolTipFormat
        {
            get { return (string)GetValue(ToolTipFormatProperty); }
            set { SetValue(ToolTipFormatProperty, value); }
        }

        public Visibility ChartLegendVisibility
        {
            get { return (Visibility)GetValue(ChartLegendVisibilityProperty); }
            set { SetValue(ChartLegendVisibilityProperty, value); }
        }
                
        public IEnumerable SeriesSource
        {
            get { return (IEnumerable)GetValue(SeriesSourceProperty); }
            set { SetValue(SeriesSourceProperty, value); }
        }

        public ObservableCollection<string> Exceptions
        {
            get { return (ObservableCollection<string>)GetValue(ExceptionsProperty); }
            set { SetValue(ExceptionsProperty, value); }
        }

        public DataTemplate SeriesTemplate
        {
            get { return (DataTemplate)GetValue(SeriesTemplateProperty); }
            set { SetValue(SeriesTemplateProperty, value); }
        }

        public double SumOfDataPointGroup
        {
            get { return (double)GetValue(SumOfDataPointGroupProperty); }
            set { SetValue(SumOfDataPointGroupProperty, value); }
        }

        public ObservableCollection<string> GridLines
        {
            get
            {
                return gridLines;
            }
        }

        /// <summary>
        /// In ColumnGrid we need some space above the column to show the number above the column,
        /// this is not needed in StackedChart
        /// </summary>
        public virtual bool IsUseNextBiggestMaxValue
        {
            get
            {
                return false;
            }
        }

        #endregion Properties

        #region Methods

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

        public void InternalOnApplyTemplate()
        {
            base.OnApplyTemplate();

            onApplyTemplateFinished = true;
            UpdateSeries();
        }

        private ObservableCollection<ChartLegendItemViewModel> chartLegendItems = new ObservableCollection<ChartLegendItemViewModel>();
        public ObservableCollection<ChartLegendItemViewModel> ChartLegendItems
        {
            get
            {
                return chartLegendItems;
            }
        }


        private static void OnSeriesSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IEnumerable oldValue = (IEnumerable)e.OldValue;
            IEnumerable newValue = (IEnumerable)e.NewValue;
            ChartBase source = (ChartBase)d;
            source.OnSeriesSourceChanged(oldValue, newValue);
        }

        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartBase).UpdateColorsOfDataPoints();
        }

        private static void OnIsRowColumnSwitchedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartBase).UpdateData();
        }

        private string GetPropertyValue(object item, string propertyName)
        {
            foreach (PropertyInfo info in item.GetType().GetAllProperties())
            {
                if (info.Name == propertyName)
                {
                    object v = info.GetValue(item, null);
                    return v.ToString();
                }
            }
            throw new Exception("Value not found");
        }

        private Brush GetItemBrush(int index)
        {
            ResourceDictionaryCollection usedPalette = this.DefaultPalette;
            if (this.Palette != null)
            {
                usedPalette = this.Palette;
            }

            if (usedPalette != null)
            {
                // returns the color from palette with the given index
                // for indexes large than the number of color in the palette we will start at the beginning 
                int paletteIndex = index % usedPalette.Count;
                var resDictionary = usedPalette[paletteIndex];
                try
                {
                    foreach (var entry in resDictionary.Values)
                    {
                        if (entry is Brush)
                        {
                            return entry as Brush;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return new SolidColorBrush(Colors.Red);
        }

        /// <summary>
        /// take a number, e.g.
        /// 43456 -> 50000
        /// 1324 -> 1400
        /// 123 -> 130
        /// 8 -> 10
        /// 23 -> 30
        /// 82 -> 90
        /// 92 -> 100
        /// 1.5 -> 2
        /// 33 -> 40
        /// </summary>
        /// <param name="newMaxValue"></param>
        /// <returns></returns>
        private double CalculateMaxValue(double newMaxValue)
        {
            double bestMaxValue = 0.0;
            int bestDivisor = 1;

            GetBestValues(newMaxValue, ref bestMaxValue, ref bestDivisor);

            return bestMaxValue;
        }

        private double CalculateDistance(double givenBestMaxValue)
        {
            double bestMaxValue = 0.0;
            int bestDivisor = 1;
            double distance = 0.0;

            GetBestValues(givenBestMaxValue, ref bestMaxValue, ref bestDivisor);
            distance = bestMaxValue / bestDivisor;

            return distance;
        }


        private void GetBestValues(double wert, ref double bestMaxValue, ref int bestDivisor)
        {
            if (wert == 60.0)
            {
            }

            string wertString = wert.ToString(System.Globalization.CultureInfo.InvariantCulture);
            double tensBelowNull = 1;

            if (wert <= 1)
            {
                //0.72  -> 0.8
                //0.00145
                //0.0007453 0> 7453

                //count digits after comma
                int digitsAfterComma = wertString.Replace("0.", "").Length;
                tensBelowNull = Math.Pow(10, digitsAfterComma);
                wert = wert * tensBelowNull;
                wertString = wert.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            if (wertString.Contains("."))
            {
                wertString = wertString.Substring(0, wertString.IndexOf("."));
            }
            int digitsBeforeComma = wertString.Length;
            int roundedValue = (int)Math.Ceiling(wert);
            double tens = 0;
            if (digitsBeforeComma > 2)
            {
                tens = Math.Pow(10, digitsBeforeComma - 2);
                double wertWith2Digits = wert / tens;
                roundedValue = (int)Math.Ceiling(wertWith2Digits);
            }
            else if (digitsBeforeComma == 1)
            {
                tens = 0.1;
                double wertWith2Digits = wert / tens;
                roundedValue = (int)Math.Ceiling(wertWith2Digits);
            }

            int finaldivisor = FindBestDivisor(ref roundedValue);

            double roundedValueDouble = roundedValue / tensBelowNull;

            if (tens > 0)
            {
                roundedValueDouble = roundedValueDouble * tens;
            }

            bestMaxValue = roundedValueDouble;
            bestDivisor = finaldivisor;

        }

        private int FindBestDivisor(ref int roundedValue)
        {
            if (IsUseNextBiggestMaxValue)
            {
                //roundedValue += 1;
            }
            while (true)
            {
                int[] divisors = new int[] { 2, 5, 10, 25 };
                foreach (int divisor in divisors)
                {
                    int div = roundedValue % divisor;
                    int mod = roundedValue / divisor;

                    if ((roundedValue < 10) && (mod == 1))
                    {
                        return roundedValue;
                    }

                    if ((div == 0) && (mod <= 10))
                    {
                        return mod;
                    }
                }
                roundedValue = roundedValue + 1;
            }
        }

        protected abstract double GridLinesMaxValue
        {
            get;
        }

        protected void UpdateGridLines()
        {
            double distance = CalculateDistance(GridLinesMaxValue);
            gridLines.Clear();
            for (var i = distance; i <= GridLinesMaxValue; i += distance)
            {
                gridLines.Add(i.ToString());
            }
        }

       public bool HasExceptions
        {
            get
            {
                return Exceptions.Any();
            }
        }

        ObservableCollection<DataPointGroup> groupedSeries = new ObservableCollection<DataPointGroup>();
        private void UpdateGroupedSeries()
        {
            // data validation
            this.Exceptions.Clear();

            // ensure that caption of series is not null, otherwise all other series would be ignored (data from the same series are collection to the same datapointgroup) 
            if (this.Series.Any(series => string.IsNullOrEmpty(series.SeriesTitle)))
            {
                Exceptions.Add("Series with empty caption cannot be used.");
            }

            //ensure that each series has a different name
            if (this.Series.GroupBy(series => series.SeriesTitle).Any(group => group.Count() > 1))
            {
                Exceptions.Add("Series with duplicate name cannot be used.");
            }

            if (!HasExceptions)
            {

                List<DataPointGroup> result = new List<DataPointGroup>();
                try
                {
                    if (GetIsRowColumnSwitched())
                    {
                        ///sammle erst alle Gruppen zusammen
                        foreach (ChartSeries initialSeries in this.Series)
                        {
                            int itemIndex = 0;
                            foreach (var seriesItem in initialSeries.Items)
                            {                                
                                string seriesItemCaption = GetPropertyValue(seriesItem, initialSeries.DisplayMember); //Security
                                DataPointGroup dataPointGroup = result.Where(group => group.Caption == seriesItemCaption).FirstOrDefault();
                                if (dataPointGroup == null)
                                {
                                    dataPointGroup = new DataPointGroup(this, seriesItemCaption, this.Series.Count > 1 ? true : false);
                                    dataPointGroup.PropertyChanged += dataPointGroup_PropertyChanged;
                                    result.Add(dataPointGroup);

                                    CreateDataPointGroupBindings(dataPointGroup);

                                    int seriesIndex = 0;
                                    foreach (ChartSeries allSeries in this.Series)
                                    {
                                        DataPoint datapoint = new DataPoint(this);
                                        datapoint.SeriesCaption = allSeries.SeriesTitle;
                                        datapoint.ValueMember = allSeries.ValueMember;
                                        datapoint.DisplayMember = allSeries.DisplayMember;
                                        datapoint.ItemBrush = this.Series.Count == 1 ? GetItemBrush(itemIndex) : GetItemBrush(seriesIndex); //if only one series, use different color for each datapoint, if multiple series we use different color for each series
                                        datapoint.PropertyChanged += groupdItem_PropertyChanged;

                                        CreateDataPointBindings(datapoint, dataPointGroup);

                                        dataPointGroup.DataPoints.Add(datapoint);
                                        seriesIndex++;
                                    }
                                    itemIndex++;
                                }                                
                            }
                        }

                        ///gehe alle Series durch (Security, Naming etc.)
                        foreach (ChartSeries series in this.Series)
                        {
                            foreach (var seriesItem in series.Items)
                            {
                                string seriesItemCaption = GetPropertyValue(seriesItem, series.DisplayMember); //Security

                                //finde die gruppe mit dem Namen
                                DataPointGroup addToGroup = result.Where(group => group.Caption == seriesItemCaption).FirstOrDefault();

                                //finde in der Gruppe 
                                DataPoint groupdItem = addToGroup.DataPoints.Where(item => item.SeriesCaption == series.SeriesTitle).FirstOrDefault();
                                groupdItem.ReferencedObject = seriesItem;
                            }
                        }
                    }
                    else
                    {
                        foreach (ChartSeries initialSeries in this.Series)
                        {
                            //erstelle für jede Series einen DataPointGroup, darin wird dann für jedes Item in jeder Serie ein DataPoint angelegt
                            DataPointGroup dataPointGroup = new DataPointGroup(this, initialSeries.SeriesTitle, this.Series.Count > 1 ? true : false);
                            dataPointGroup.PropertyChanged += dataPointGroup_PropertyChanged;
                            result.Add(dataPointGroup);

                            CreateDataPointGroupBindings(dataPointGroup);

                            //stelle nun sicher, dass alle DataPointGruppen die gleichen Datapoints hat
                            foreach (ChartSeries allSeries in this.Series)
                            {
                                int seriesIndex = 0;
                                foreach (var seriesItem in allSeries.Items)
                                {
                                    string seriesItemCaption = GetPropertyValue(seriesItem, initialSeries.DisplayMember); //Security
                                    DataPoint existingDataPoint = dataPointGroup.DataPoints.Where(datapoint => datapoint.SeriesCaption == seriesItemCaption).FirstOrDefault();
                                    if (existingDataPoint == null)
                                    {
                                        DataPoint datapoint = new DataPoint(this);
                                        datapoint.SeriesCaption = seriesItemCaption;
                                        datapoint.ValueMember = allSeries.ValueMember;
                                        datapoint.DisplayMember = allSeries.DisplayMember;
                                        datapoint.ItemBrush = GetItemBrush(seriesIndex);
                                        datapoint.PropertyChanged += groupdItem_PropertyChanged;

                                        CreateDataPointBindings(datapoint, dataPointGroup);

                                        dataPointGroup.DataPoints.Add(datapoint);
                                    }
                                    seriesIndex++;
                                }
                            }
                        }

                        ///gehe alle Series durch (Security, Naming etc.)
                        foreach (ChartSeries series in this.Series)
                        {
                            foreach (var seriesItem in series.Items)
                            {
                                //finde die gruppe mit dem Namen
                                DataPointGroup addToGroup = result.Where(group => group.Caption == series.SeriesTitle).FirstOrDefault();

                                //finde in der Gruppe das richtige Element
                                string seriesItemCaption = GetPropertyValue(seriesItem, series.DisplayMember); //Security

                                DataPoint groupdItem = addToGroup.DataPoints.Where(item => item.SeriesCaption == seriesItemCaption).FirstOrDefault();
                                groupdItem.ReferencedObject = seriesItem;
                            }
                        }
                    }
                }
                catch
                {
                }

                //finished, copy all to the array
                groupedSeries.Clear();
                foreach (var item in result)
                {
                    groupedSeries.Add(item);
                }

                UpdateColorsOfDataPoints();

                chartLegendItems.Clear();
                DataPointGroup firstgroup = groupedSeries.FirstOrDefault();
                if (firstgroup != null)
                {
                    foreach (DataPoint dataPoint in firstgroup.DataPoints)
                    {
                        ChartLegendItemViewModel legendItem = new ChartLegendItemViewModel();

                        var captionBinding = new Binding();
                        captionBinding.Source = dataPoint;
                        captionBinding.Mode = BindingMode.OneWay;
                        captionBinding.Path = new PropertyPath("SeriesCaption");
                        BindingOperations.SetBinding(legendItem, ChartLegendItemViewModel.CaptionProperty, captionBinding);

                        var brushBinding = new Binding();
                        brushBinding.Source = dataPoint;
                        brushBinding.Mode = BindingMode.OneWay;
                        brushBinding.Path = new PropertyPath("ItemBrush");
                        BindingOperations.SetBinding(legendItem, ChartLegendItemViewModel.ItemBrushProperty, brushBinding);
                        
                        chartLegendItems.Add(legendItem); 
                    }
                }
                RecalcSumOfDataPointGroup();
            }
        }

        private bool GetIsRowColumnSwitched()
        {
            if (IsRowColumnSwitched)
            {                
                //special case for piechart with 1 series, it does not make sense to switch
                if ((this as PieChart) != null)
                {
                    if (Series.Count <= 1)
                    {
                        return false;
                    }
                }

                return true;
            }
            return false;
        }

        private void UpdateColorsOfDataPoints()
        {
            foreach(var dataPointGroup in groupedSeries)
            {
                int index = 0;
                foreach (DataPoint dataPoint in dataPointGroup.DataPoints)
                {
                    dataPoint.SetValue(DataPoint.ItemBrushProperty, GetItemBrush(index));
                    index++;
                }
            }
            /*
            int legendindex = 0;
            foreach (var legendItem in chartLegendItems)
            {
                legendItem.ItemBrush = GetItemBrush(legendindex);
            }
            */
        }

        private void CreateDataPointBindings(DataPoint datapoint, DataPointGroup dataPointGroup)
        {
            //Sende an Datapoints the maximalvalue des Charts mit (wichtig in clustered Column chart)
            var maxDataPointValueBinding = new Binding();
            maxDataPointValueBinding.Source = this;
            maxDataPointValueBinding.Mode = BindingMode.OneWay;
            maxDataPointValueBinding.Path = new PropertyPath("MaxDataPointValue");
            BindingOperations.SetBinding(datapoint, DataPoint.MaxDataPointValueProperty, maxDataPointValueBinding);

            //Sende den Datapoints the höchste Summe einer DataPointGroup mit (wichtig für stacked chart)
            var maxDataPointGroupSumBinding = new Binding();
            maxDataPointGroupSumBinding.Source = this;
            maxDataPointGroupSumBinding.Mode = BindingMode.OneWay;
            maxDataPointGroupSumBinding.Path = new PropertyPath("MaxDataPointGroupSum");
            BindingOperations.SetBinding(datapoint, DataPoint.MaxDataPointGroupSumProperty, maxDataPointGroupSumBinding);

            //Sende den Datapoint die Summe seiner Datagroup
            var sumBinding = new Binding();
            sumBinding.Source = dataPointGroup;
            sumBinding.Mode = BindingMode.OneWay;
            sumBinding.Path = new PropertyPath("SumOfDataPointGroup");
            BindingOperations.SetBinding(datapoint, DataPoint.SumOfDataPointGroupProperty, sumBinding);

            var selectionBinding = new Binding();
            selectionBinding.Source = dataPointGroup;
            selectionBinding.Mode = BindingMode.TwoWay;
            selectionBinding.Path = new PropertyPath("SelectedItem");
            BindingOperations.SetBinding(datapoint, DataPoint.SelectedItemProperty, selectionBinding);

            var selectedBrushBinding = new Binding();
            selectedBrushBinding.Source = this;
            selectedBrushBinding.Mode = BindingMode.OneWay;
            selectedBrushBinding.Path = new PropertyPath("SelectedBrush");
            BindingOperations.SetBinding(datapoint, DataPoint.SelectedBrushProperty, selectedBrushBinding);

            //tooltip format (may change sometimes)
            var tooltipFormatBinding = new Binding();
            tooltipFormatBinding.Source = this;
            tooltipFormatBinding.Mode = BindingMode.OneWay;
            tooltipFormatBinding.Path = new PropertyPath("ToolTipFormat");
            BindingOperations.SetBinding(datapoint, DataPoint.ToolTipFormatProperty, tooltipFormatBinding);

        }

        private void CreateDataPointGroupBindings(DataPointGroup dataPointGroup)
        {
            var groupBinding = new Binding();
            groupBinding.Source = this;
            groupBinding.Mode = BindingMode.TwoWay;
            groupBinding.Path = new PropertyPath("SelectedItem");
            BindingOperations.SetBinding(dataPointGroup, DataPointGroup.SelectedItemProperty, groupBinding);
        }

        void dataPointGroup_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SumOfDataPointGroup")
            {
                RecalcSumOfDataPointGroup();
            }
        }

        private void RecalcSumOfDataPointGroup()
        {
            double maxValue = 0.0;
            foreach (var dataPointGroup in DataPointGroups)
            {
                if (dataPointGroup.SumOfDataPointGroup > maxValue)
                {
                    maxValue = dataPointGroup.SumOfDataPointGroup;
                }
            }
            MaxDataPointGroupSum = CalculateMaxValue(maxValue);
        }

        void groupdItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RecalcMaxDataPointValue();
        }

        private void RecalcMaxDataPointValue()
        {
            double maxValue = 0.0;
            foreach (var dataPointGroup in DataPointGroups)
            {
                foreach (var dataPoint in dataPointGroup.DataPoints)
                {
                    if (dataPoint.Value > maxValue)
                    {
                        maxValue = dataPoint.Value;
                    }
                }
            }
            MaxDataPointValue = CalculateMaxValue(maxValue);
        }

        public ObservableCollection<DataPointGroup> DataPointGroups
        {
            get
            {
                return groupedSeries;
            }
        }

        private void UpdateData()
        {
            UpdateGroupedSeries();
        }

        private void OnSeriesSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            UpdateSeries();
        }

        private T LoadDataTemplate<T>(DataTemplate template, object dataContext) where T : FrameworkElement
        {
            DependencyObject element = template.LoadContent();
            T view = element as T;
            view.DataContext = dataContext;

#if NETFX_CORE

#elif SILVERLIGHT

#else
            // update the bindings for wpf
            var enumerator = element.GetLocalValueEnumerator();
            while (enumerator.MoveNext())
            {
                var bind = enumerator.Current;

                if (bind.Value is BindingExpression)
                {
                    view.SetBinding(bind.Property, ((BindingExpression)bind.Value).ParentBinding);
                }
            }
#endif
            return view;
        }

        private void UpdateSeries()
        {
            if (!onApplyTemplateFinished)
            {
                // avoid updating the chart during initialize phase, wait for final OnApplyTemplate call
                return;
            }

            if (this.Series != null)
            {
                if (SeriesTemplate != null)
                {
                    if (this.SeriesSource != null)
                    {
                        if (this.SeriesSource is INotifyCollectionChanged)
                        {
                            (this.SeriesSource as INotifyCollectionChanged).CollectionChanged += SeriesSourceCollectionChanged;
                        }

                        this.Series.Clear();
                        foreach (object item in this.SeriesSource)
                        {
                            ChartSeries series = LoadDataTemplate<ChartSeries>(SeriesTemplate, item); //.LoadContent() as ChartSeries;

                            if (series != null)
                            {
                                // set data context
                                series.DataContext = item;
                                this.Series.Add(series);
                            }
                        }
                    }
                }

                UpdateGroupedSeries();
            }
        }

        private void SeriesSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // new or remove series, easiest way: recreate all
            UpdateSeries();
        }

        private static void OnMaxDataPointValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartBase).OnMaxDataPointValueChanged((double)e.NewValue);
        }

        protected virtual void OnMaxDataPointValueChanged(double p)
        {

        }

        private static void OnSeriesTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // mandelkow (d as ChartBase).UpdateSeries();
        }

        private static void OnMaxDataPointGroupSumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartBase).OnMaxDataPointGroupSumChanged((double)e.NewValue);
        }

        protected virtual void OnMaxDataPointGroupSumChanged(double p)
        {

        }

        private void UpdateControls()
        {
            if (this.DataContext != null)
            {
                if (this.DataContext is IEnumerable)
                {
                    foreach (object item in (this.DataContext as IEnumerable))
                    {
                        if (item is INotifyPropertyChanged)
                        {
                            INotifyPropertyChanged observable = (INotifyPropertyChanged)item;
                            observable.PropertyChanged += new PropertyChangedEventHandler(observable_PropertyChanged);
                        }
                    }
                }
            }
        }

        private void RaisePropertyChangeEvent(String propertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        /// <summary>
        /// Handles the PropertyChanged event of the observable control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        void observable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            /*
            CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(this.DataContext);
            if (myCollectionView != null)
            {
                myCollectionView.Refresh();
            }
            */
        }

        #endregion Methods
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}