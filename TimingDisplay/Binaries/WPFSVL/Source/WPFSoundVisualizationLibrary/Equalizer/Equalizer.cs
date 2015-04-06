using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace WPFSoundVisualizationLib
{    
    /// <summary>
    /// A control that displays and edits banded frequency amplification.
    /// </summary>
    [DisplayName("Equalizer")]
    [Description("Displays and edits banded frequency amplification.")]
    [ToolboxItem(true)]
    [TemplatePart(Name = "PART_EqualizerGrid", Type = typeof(Grid))]  
    public class Equalizer : Control
    {
        #region Fields
        private readonly List<Slider> sliders = new List<Slider>();
        private Grid equalizerGrid;
        #endregion

        #region DependencyProperties
        #region EqualizerValues
        /// <summary>
        /// Identifies the <see cref="EqualizerValues" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty EqualizerValuesProperty = DependencyProperty.Register("EqualizerValues", typeof(float[]), typeof(Equalizer), new UIPropertyMetadata(new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f }, OnEqualizerValuesChanged, OnCoerceEqualizerValues));

        private static object OnCoerceEqualizerValues(DependencyObject o, object value)
        {
            Equalizer equalizer = o as Equalizer;
            if (equalizer != null)
                return equalizer.OnCoerceEqualizerValues((float[])value);
            else
                return value;
        }

        private static void OnEqualizerValuesChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Equalizer equalizer = o as Equalizer;
            if (equalizer != null)
                equalizer.OnEqualizerValuesChanged((float[])e.OldValue, (float[])e.NewValue);
        }

        /// <summary>
        /// Coerces the value of <see cref="EqualizerValues"/> when a new value is applied.
        /// </summary>
        /// <param name="value">The value that was set on <see cref="EqualizerValues"/></param>
        /// <returns>The adjusted value of <see cref="EqualizerValues"/></returns>
        protected virtual float[] OnCoerceEqualizerValues(float[] value)
        {
            if (value == null || value.Length != NumberOfBands)
                return new float[NumberOfBands];
            return value;
        }

        /// <summary>
        /// Called after the <see cref="EqualizerValues"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="EqualizerValues"/></param>
        /// <param name="newValue">The new value of <see cref="EqualizerValues"/></param>
        protected virtual void OnEqualizerValuesChanged(float[] oldValue, float[] newValue)
        {
            if (newValue == null || newValue.Length != NumberOfBands)
                SetEqualizerValues(new float[NumberOfBands]);
            else
                SetEqualizerValues(newValue);
        }

        /// <summary>
        /// Gets or sets the values of each equalizer band.
        /// </summary>
        /// <remarks>The number of elements in the EqualizerValues array must be equal to the number of bands. If not, all values will be set to zero.</remarks>
        public float[] EqualizerValues
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (float[])GetValue(EqualizerValuesProperty);
            }
            set
            {
                SetValue(EqualizerValuesProperty, value);
            }
        }
        #endregion

        #region NumberOfBands
        /// <summary>
        /// Identifies the <see cref="NumberOfBands" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty NumberOfBandsProperty = DependencyProperty.Register("NumberOfBands", typeof(int), typeof(Equalizer), new UIPropertyMetadata(7, OnNumberOfBandsChanged, OnCoerceNumberOfBands));

        private static object OnCoerceNumberOfBands(DependencyObject o, object value)
        {
            Equalizer equalizer = o as Equalizer;
            if (equalizer != null)
                return equalizer.OnCoerceNumberOfBands((int)value);
            else
                return value;
        }

        private static void OnNumberOfBandsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Equalizer equalizer = o as Equalizer;
            if (equalizer != null)
                equalizer.OnNumberOfBandsChanged((int)e.OldValue, (int)e.NewValue);
        }

        /// <summary>
        /// Coerces the value of <see cref="NumberOfBands"/> when a new value is applied.
        /// </summary>
        /// <param name="value">The value that was set on <see cref="NumberOfBands"/></param>
        /// <returns>The adjusted value of <see cref="NumberOfBands"/></returns>
        protected virtual int OnCoerceNumberOfBands(int value)
        {
            value = Math.Max(1, value);
            return value;
        }

        /// <summary>
        /// Called after the <see cref="NumberOfBands"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="NumberOfBands"/></param>
        /// <param name="newValue">The new value of <see cref="NumberOfBands"/></param>
        protected virtual void OnNumberOfBandsChanged(int oldValue, int newValue)
        {
            CreateSliders();
            SetEqualizerValues(new float[NumberOfBands]);
        }

        /// <summary>
        /// Gets or sets the number of bands that the equalizer will display.
        /// </summary>
        public int NumberOfBands
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (int)GetValue(NumberOfBandsProperty);
            }
            set
            {
                SetValue(NumberOfBandsProperty, value);
            }
        }

        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an instance of Equalizer.
        /// </summary>
        public Equalizer()
        {
            CreateSliders();
        }

        static Equalizer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Equalizer), new FrameworkPropertyMetadata(typeof(Equalizer)));
        }
        #endregion

        #region Template Overrides
        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code
        /// or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (equalizerGrid != null)
                equalizerGrid.Children.Clear();

            equalizerGrid = GetTemplateChild("PART_EqualizerGrid") as Grid;

            CreateSliders();
        }

        /// <summary>
        /// Called whenever the control's template changes. 
        /// </summary>
        /// <param name="oldTemplate">The old template</param>
        /// <param name="newTemplate">The new template</param>
        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            base.OnTemplateChanged(oldTemplate, newTemplate);
        }
        #endregion

        #region Private Utility Methods
        private void CreateSliders()
        {
            if (equalizerGrid == null)
                return;

            ClearSliders();
            equalizerGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < NumberOfBands; i++)
            {
                ColumnDefinition channelColumn = new ColumnDefinition()
                {
                    Width = new GridLength(10.0d, GridUnitType.Star)
                };
                equalizerGrid.ColumnDefinitions.Add(channelColumn);
                Slider slider = new Slider()
                {
                    Style = (Style)FindResource("EqualizerSlider"),
                    Maximum = 1.0,
                    Minimum = -1.0,
                    SmallChange = 0.01,
                    LargeChange = 0.1,
                };

                Grid.SetColumn(slider, i);
                sliders.Add(slider);
                equalizerGrid.Children.Add(slider);
                slider.ValueChanged += slider_ValueChanged;
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            EqualizerValues = GetEqualizerValues();
        }

        private void ClearSliders()
        {
            foreach (Slider slider in sliders)
            {
                slider.ValueChanged -= slider_ValueChanged;
                equalizerGrid.Children.Remove(slider);
            }
            sliders.Clear();
        }

        private float[] GetEqualizerValues()
        {
            float[] sliderValues = new float[NumberOfBands];
            for (int i = 0; i < NumberOfBands; i++)
                sliderValues[i] = (float)sliders[i].Value;
            return sliderValues;
        }

        private void SetEqualizerValues(float[] values)
        {
            for (int i = 0; i < NumberOfBands; i++)
                sliders[i].Value = values[i];
        }
        #endregion
    }
}
