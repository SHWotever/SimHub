// Copyright (C) 2011 - 2012, Jacob Johnston 
//
// Permission is hereby granted, free of charge, to any person obtaining a 
// copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions: 
//
// The above copyright notice and this permission notice shall be included in 
// all copies or substantial portions of the Software. 
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE. 

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;

namespace WPFSoundVisualizationLib
{
    /// <summary>
    /// A control that allows the user to
    /// see and edit TimeSpans.
    /// </summary>
    [DisplayName("Time Editor")]
    [Description("Displays and edits time spans.")]
    [ToolboxItem(true)]    
    [TemplatePart(Name = "PART_HoursTextBox", Type = typeof(TextBox)),
    TemplatePart(Name = "PART_MinutesTextBox", Type = typeof(TextBox)),
    TemplatePart(Name = "PART_SecondsTextBox", Type = typeof(TextBox)),
    TemplatePart(Name = "PART_SpinUpButton", Type = typeof(Button)),
    TemplatePart(Name = "PART_SpinDownButton", Type = typeof(Button))]
    public class TimeEditor : Control
    {
        #region  Fields
        private bool updatingValue;
        private TextBox hoursTextBox;
        private TextBox minutesTextBox;
        private TextBox secondsTextBox;
        private ButtonBase spinUpButton;
        private ButtonBase spinDownButton;
        private string hoursMask;
        private string minutesMask;
        private string secondsMask;
        private bool hoursLoaded;
        private bool minutesLoaded;
        private bool secondsLoaded;
        private TimeEditFields activeField = TimeEditFields.Seconds;
        #endregion

        #region Enums
        private enum TimeEditFields
        {
            Hours,
            Minutes,
            Seconds
        }
        #endregion

        #region Constructors
        static TimeEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeEditor), new FrameworkPropertyMetadata(typeof(TimeEditor)));
        }

        /// <summary>
        /// Creates an instance of TimeEditor
        /// </summary>
        public TimeEditor()
        {
            UpdateMasks();            
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

            hoursTextBox = GetTemplateChild("PART_HoursTextBox") as TextBox;
            minutesTextBox = GetTemplateChild("PART_MinutesTextBox") as TextBox;
            secondsTextBox = GetTemplateChild("PART_SecondsTextBox") as TextBox;
            spinUpButton = GetTemplateChild("PART_SpinUpButton") as Button;
            spinDownButton = GetTemplateChild("PART_SpinDownButton") as Button;

            if (hoursTextBox != null)
            {
                hoursTextBox.PreviewTextInput += HoursPreviewTextInput;
                hoursTextBox.LostFocus += HoursLostFocus;
                hoursTextBox.PreviewKeyDown += HoursKeyDown;
                hoursTextBox.TextChanged += HoursTextChanged;
                hoursTextBox.GotFocus += HoursGotFocus;
            }
            if (minutesTextBox != null)
            {
                minutesTextBox.PreviewTextInput += MinutesPreviewTextInput;
                minutesTextBox.LostFocus += MinutesLostFocus;
                minutesTextBox.PreviewKeyDown += MinutesKeyDown;
                minutesTextBox.TextChanged += MinutesTextChanged;
                minutesTextBox.GotFocus += MinutesGotFocus;
            }
            if (secondsTextBox != null)
            {
                secondsTextBox.PreviewTextInput += SecondsPreviewTextInput;
                secondsTextBox.LostFocus += SecondsLostFocus;
                secondsTextBox.PreviewKeyDown += SecondsKeyDown;
                secondsTextBox.TextChanged += SecondsTextChanged;
                secondsTextBox.GotFocus += SecondsGotFocus;
            }

            if (spinUpButton != null)
            {
                spinUpButton.Focusable = false;
                spinUpButton.Click += SpinUpClick;
            }
            if (spinDownButton != null)
            {
                spinDownButton.Focusable = false;
                spinDownButton.Click += SpinDownClick;
            }

            CopyValuesToTextBoxes();
            UpdateReadOnly();
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

        #region Dependency Properties
        #region IsReadOnly
        /// <summary>
        /// Identifies the <see cref="IsReadOnly" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(TimeEditor), new UIPropertyMetadata(false, new PropertyChangedCallback(OnIsReadOnlyChanged), new CoerceValueCallback(OnCoerceIsReadOnly)));

        private static object OnCoerceIsReadOnly(DependencyObject o, object value)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                return timeEditor.OnCoerceIsReadOnly((bool)value);
            else
                return value;
        }

        private static void OnIsReadOnlyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                timeEditor.OnIsReadOnlyChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        /// Coerces the value of <see cref="IsReadOnly"/> when a new value is applied.
        /// </summary>
        /// <param name="value">The value that was set on <see cref="IsReadOnly"/></param>
        /// <returns>The adjusted value of <see cref="IsReadOnly"/></returns>
        protected virtual bool OnCoerceIsReadOnly(bool value)
        {            
            return value;
        }

        /// <summary>
        /// Called after the <see cref="IsReadOnly"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="IsReadOnly"/></param>
        /// <param name="newValue">The new value of <see cref="IsReadOnly"/></param>
        protected virtual void OnIsReadOnlyChanged(bool oldValue, bool newValue)
        {
            UpdateReadOnly();
        }

        /// <summary>
        /// Gets or sets whether the TimeEditor is readonly
        /// </summary>
        public bool IsReadOnly
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (bool)GetValue(IsReadOnlyProperty);
            }
            set
            {
                SetValue(IsReadOnlyProperty, value);
            }
        }
        
        #endregion

        #region Minimum
        /// <summary>
        /// Identifies the <see cref="Minimum" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(TimeSpan), typeof(TimeEditor), new UIPropertyMetadata(TimeSpan.Zero, OnMinimumChanged, OnCoerceMinimum));

        private static object OnCoerceMinimum(DependencyObject o, object value)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                return timeEditor.OnCoerceMinimum((TimeSpan)value);
            else
                return value;
        }

        private static void OnMinimumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                timeEditor.OnMinimumChanged((TimeSpan)e.OldValue, (TimeSpan)e.NewValue);
        }

        /// <summary>
        /// Coerces the value of <see cref="Minimum"/> when a new value is applied.
        /// </summary>
        /// <param name="value">The value that was set on <see cref="Minimum"/></param>
        /// <returns>The adjusted value of <see cref="Minimum"/></returns>
        protected virtual TimeSpan OnCoerceMinimum(TimeSpan value)
        {
            return value;
        }

        /// <summary>
        /// Called after the <see cref="Minimum"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="Minimum"/></param>
        /// <param name="newValue">The new value of <see cref="Minimum"/></param>
        protected virtual void OnMinimumChanged(TimeSpan oldValue, TimeSpan newValue)
        {
            CoerceValue(MaximumProperty);
            CoerceValue(ValueProperty);
            UpdateMasks();
        }

        /// <summary>
        /// Gets or sets the minimum value that is allowed by the TimeEditor
        /// </summary>
        public TimeSpan Minimum
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (TimeSpan)GetValue(MinimumProperty);
            }
            set
            {
                SetValue(MinimumProperty, value);
            }
        }
        #endregion

        #region Maximum
        /// <summary>
        /// Identifies the <see cref="Maximum" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(TimeSpan), typeof(TimeEditor), new UIPropertyMetadata(TimeSpan.MaxValue, OnMaximumChanged, OnCoerceMaximum));

        private static object OnCoerceMaximum(DependencyObject o, object value)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                return timeEditor.OnCoerceMaximum((TimeSpan)value);
            else
                return value;
        }

        private static void OnMaximumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                timeEditor.OnMaximumChanged((TimeSpan)e.OldValue, (TimeSpan)e.NewValue);
        }

        /// <summary>
        /// Coerces the value of <see cref="Maximum"/> when a new value is applied.
        /// </summary>
        /// <param name="value">The value that was set on <see cref="Maximum"/></param>
        /// <returns>The adjusted value of <see cref="Maximum"/></returns>
        protected virtual TimeSpan OnCoerceMaximum(TimeSpan value)
        {
            if (value < Minimum)
                return Minimum;
            return value;
        }

        /// <summary>
        /// Called after the <see cref="Maximum"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="Maximum"/></param>
        /// <param name="newValue">The new value of <see cref="Maximum"/></param>
        protected virtual void OnMaximumChanged(TimeSpan oldValue, TimeSpan newValue)
        {
            UpdateMasks();
            NeedsHour = (newValue >= TimeSpan.FromHours(1.0d));
        }

        /// <summary>
        /// Gets or sets the maximum value that is allowed by the TimeEditor
        /// </summary>
        public TimeSpan Maximum
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (TimeSpan)GetValue(MaximumProperty);
            }
            set
            {
                SetValue(MaximumProperty, value);
            }
        }
        #endregion

        #region Value
        /// <summary>
        /// Identifies the <see cref="Value" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(TimeSpan), typeof(TimeEditor), new UIPropertyMetadata(TimeSpan.Zero, OnValueChanged, OnCoerceValue));

        private static object OnCoerceValue(DependencyObject o, object value)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                return timeEditor.OnCoerceValue((TimeSpan)value);
            else
                return value;
        }

        private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                timeEditor.OnValueChanged((TimeSpan)e.OldValue, (TimeSpan)e.NewValue);
        }

        /// <summary>
        /// Coerces the value of <see cref="Value"/> when a new value is applied.
        /// </summary>
        /// <param name="value">The value that was set on <see cref="Value"/></param>
        /// <returns>The adjusted value of <see cref="Value"/></returns>
        protected virtual TimeSpan OnCoerceValue(TimeSpan value)
        {
            if (value < Minimum)
                return Minimum;
            if (value > Maximum)
                return Maximum;
            return value;
        }

        /// <summary>
        /// Called after the <see cref="Value"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="Value"/></param>
        /// <param name="newValue">The new value of <see cref="Value"/></param>
        protected virtual void OnValueChanged(TimeSpan oldValue, TimeSpan newValue)
        {
            if (!updatingValue) // Avoid reentrance
            {
                updatingValue = true;
                Hours = newValue.Hours;
                Minutes = newValue.Minutes;
                Seconds = newValue.Seconds + (newValue.Milliseconds / 1000.0d);
                CopyValuesToTextBoxes();
                updatingValue = false;
            }
        }

        /// <summary>
        /// Gets or sets the value of the TimeEditor
        /// </summary>
        public TimeSpan Value
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (TimeSpan)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Hours
        /// <summary>
        /// Identifies the <see cref="Hours" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty HoursProperty = DependencyProperty.Register("Hours", typeof(int), typeof(TimeEditor), new UIPropertyMetadata(0, OnHoursChanged, OnCoerceHours));

        private static object OnCoerceHours(DependencyObject o, object value)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                return timeEditor.OnCoerceHours((int)value);
            else
                return value;
        }

        private static void OnHoursChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                timeEditor.OnHoursChanged((int)e.OldValue, (int)e.NewValue);
        }

        /// <summary>
        /// Coerces the value of <see cref="Hours"/> when a new value is applied.
        /// </summary>
        /// <param name="value">The value that was set on <see cref="Hours"/></param>
        /// <returns>The adjusted value of <see cref="Hours"/></returns>
        protected virtual int OnCoerceHours(int value)
        {
            return value;
        }

        /// <summary>
        /// Called after the <see cref="Hours"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="Hours"/></param>
        /// <param name="newValue">The new value of <see cref="Hours"/></param>
        protected virtual void OnHoursChanged(int oldValue, int newValue)
        {
            const int maxHours = 23;
            if (newValue > maxHours)
                newValue = maxHours;

            TimeSpan value = new TimeSpan(0, newValue, Minutes, (int)Seconds, ((int)(Seconds * 1000)) % 1000);
            Value = value;
        }

        /// <summary>
        /// Gets or sets just the hours portion of the <see cref="Value"/>
        /// </summary>
        public int Hours
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (int)GetValue(HoursProperty);
            }
            set
            {
                SetValue(HoursProperty, value);
            }
        }
        #endregion

        #region Minutes
        /// <summary>
        /// Identifies the <see cref="Minutes" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty MinutesProperty = DependencyProperty.Register("Minutes", typeof(int), typeof(TimeEditor), new UIPropertyMetadata(0, OnMinutesChanged, OnCoerceMinutes));

        private static object OnCoerceMinutes(DependencyObject o, object value)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                return timeEditor.OnCoerceMinutes((int)value);
            else
                return value;
        }

        private static void OnMinutesChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                timeEditor.OnMinutesChanged((int)e.OldValue, (int)e.NewValue);
        }

        /// <summary>
        /// Coerces the value of <see cref="Minutes"/> when a new value is applied.
        /// </summary>
        /// <param name="value">The value that was set on <see cref="Minutes"/></param>
        /// <returns>The adjusted value of <see cref="Minutes"/></returns>
        protected virtual int OnCoerceMinutes(int value)
        {
            return value;
        }

        /// <summary>
        /// Called after the <see cref="Minutes"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="Minutes"/></param>
        /// <param name="newValue">The new value of <see cref="Minutes"/></param>
        protected virtual void OnMinutesChanged(int oldValue, int newValue)
        {
            const int maxMinutes = 59;
            if (newValue > maxMinutes)
                newValue = maxMinutes;

            TimeSpan value = new TimeSpan(0, Hours, newValue, (int)Seconds, ((int)(Seconds * 1000)) % 1000);
            Value = value;
        }

        /// <summary>
        /// Gets or sets just the hours portion of the <see cref="Minutes"/>
        /// </summary>
        public int Minutes
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (int)GetValue(MinutesProperty);
            }
            set
            {
                SetValue(MinutesProperty, value);
            }
        }
        #endregion

        #region Seconds
        /// <summary>
        /// Identifies the <see cref="Seconds" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty SecondsProperty = DependencyProperty.Register("Seconds", typeof(double), typeof(TimeEditor), new UIPropertyMetadata(0.0d, OnSecondsChanged, OnCoerceSeconds));

        private static object OnCoerceSeconds(DependencyObject o, object value)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                return timeEditor.OnCoerceSeconds((double)value);
            else
                return value;
        }

        private static void OnSecondsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                timeEditor.OnSecondsChanged((double)e.OldValue, (double)e.NewValue);
        }

        /// <summary>
        /// Coerces the value of <see cref="Seconds"/> when a new value is applied.
        /// </summary>
        /// <param name="value">The value that was set on <see cref="Seconds"/></param>
        /// <returns>The adjusted value of <see cref="Seconds"/></returns>
        protected virtual double OnCoerceSeconds(double value)
        {
            return value;
        }

        /// <summary>
        /// Called after the <see cref="Seconds"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="Seconds"/></param>
        /// <param name="newValue">The new value of <see cref="Seconds"/></param>
        protected virtual void OnSecondsChanged(double oldValue, double newValue)
        {
            const double maxSeconds = 60.0d;
            if (newValue > maxSeconds)
                newValue = maxSeconds - 0.01;

            TimeSpan value = new TimeSpan(0, Hours, Minutes, (int)newValue, ((int)(newValue * 1000)) % 1000);
            Value = value;
        }

        /// <summary>
        /// Gets or sets just the seconds portion of the <see cref="Value"/>
        /// </summary>
        public double Seconds
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (double)GetValue(SecondsProperty);
            }
            set
            {
                SetValue(SecondsProperty, value);
            }
        }
        #endregion

        #region CaretBrush
        /// <summary>
        /// Identifies the <see cref="CaretBrush" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register("CaretBrush", typeof(Brush), typeof(TimeEditor), new UIPropertyMetadata(null, OnCaretBrushChanged, OnCoerceCaretBrush));

        private static object OnCoerceCaretBrush(DependencyObject o, object value)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                return timeEditor.OnCoerceCaretBrush((Brush)value);
            else
                return value;
        }

        private static void OnCaretBrushChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                timeEditor.OnCaretBrushChanged((Brush)e.OldValue, (Brush)e.NewValue);
        }

        /// <summary>
        /// Coerces the value of <see cref="CaretBrush"/> when a new value is applied.
        /// </summary>
        /// <param name="value">The value that was set on <see cref="CaretBrush"/></param>
        /// <returns>The adjusted value of <see cref="CaretBrush"/></returns>
        protected virtual Brush OnCoerceCaretBrush(Brush value)
        {
            return value;
        }

        /// <summary>
        /// Called after the <see cref="CaretBrush"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="CaretBrush"/></param>
        /// <param name="newValue">The new value of <see cref="CaretBrush"/></param>
        protected virtual void OnCaretBrushChanged(Brush oldValue, Brush newValue)
        {
            
        }

        /// <summary>
        /// Gets or sets the brush used by the caret in text fields.
        /// </summary>
        public Brush CaretBrush
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (Brush)GetValue(CaretBrushProperty);
            }
            set
            {
                SetValue(CaretBrushProperty, value);
            }
        }
        #endregion

        #region NeedsHour
        /// <summary>
        /// Identifies the <see cref="NeedsHour" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty NeedsHourProperty = DependencyProperty.Register("NeedsHour", typeof(bool), typeof(TimeEditor), new UIPropertyMetadata(false, OnNeedsHourChanged, OnCoerceNeedsHour));

        private static object OnCoerceNeedsHour(DependencyObject o, object value)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                return timeEditor.OnCoerceNeedsHour((bool)value);
            else
                return value;
        }
        
        private static void OnNeedsHourChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TimeEditor timeEditor = o as TimeEditor;
            if (timeEditor != null)
                timeEditor.OnNeedsHourChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        /// Coerces the value of <see cref="NeedsHour"/> when a new value is applied.
        /// </summary>
        /// <param name="value">The value that was set on <see cref="NeedsHour"/></param>
        /// <returns>The adjusted value of <see cref="NeedsHour"/></returns>
        protected virtual bool OnCoerceNeedsHour(bool value)
        {
            return value;
        }

        /// <summary>
        /// Called after the <see cref="NeedsHour"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="NeedsHour"/></param>
        /// <param name="newValue">The new value of <see cref="NeedsHour"/></param>
        protected virtual void OnNeedsHourChanged(bool oldValue, bool newValue)
        {
            
        }

        /// <summary>
        /// Gets whether the <see cref="Maximum"/> value is greater than the 1 hour
        /// to indicate whether the hour field needs to be displayed.
        /// </summary>
        public bool NeedsHour
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (bool)GetValue(NeedsHourProperty);
            }
            protected set
            {
                SetValue(NeedsHourProperty, value);
            }
        }

        #endregion
        #endregion

        #region Event Handlers
        #region Buttons
        private void SpinUpClick(object sender, RoutedEventArgs e)
        {
            switch (activeField)
            {
                case TimeEditFields.Hours:
                    IncrementHours();
                    break;
                case TimeEditFields.Minutes:
                    IncrementMinutes();
                    break;
                case TimeEditFields.Seconds:
                    IncrementSeconds();
                    break;
            }
        }

        private void SpinDownClick(object sender, RoutedEventArgs e)
        {
            switch (activeField)
            {
                case TimeEditFields.Hours:
                    DecrementHours();
                    break;
                case TimeEditFields.Minutes:
                    DecrementMinutes();
                    break;
                case TimeEditFields.Seconds:
                    DecrementSeconds();
                    break;
            }
        }
        #endregion

        #region Key Presses
        private void HoursKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                case Key.Tab:
                    ValidateHours();
                    break;
                case Key.Up:
                    IncrementHours();
                    break;
                case Key.Down:
                    DecrementHours();
                    break;
                case Key.Right:
                    if (hoursTextBox.CaretIndex >= hoursTextBox.Text.Length && minutesTextBox != null)
                    {
                        minutesTextBox.Focus();
                        minutesTextBox.CaretIndex = 0;
                        e.Handled = true;
                    }
                    break;
                case Key.Escape:
                    ValidateHours();
                    RemoveFocusToTop();
                    break;
                default:
                    // Do nothing
                    break;
            }
        }

        private void MinutesKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                case Key.Tab:
                    ValidateMinutes();
                    break;
                case Key.Up:
                    IncrementMinutes();
                    break;
                case Key.Down:
                    DecrementMinutes();
                    break;
                case Key.Right:
                    if (minutesTextBox.CaretIndex >= minutesTextBox.Text.Length && secondsTextBox != null)
                    {
                        secondsTextBox.Focus();
                        secondsTextBox.CaretIndex = 0;
                        e.Handled = true;
                    }
                    break;
                case Key.Left:
                    if (minutesTextBox.CaretIndex <= 0 && hoursTextBox != null && hoursTextBox.IsVisible)
                    {
                        hoursTextBox.Focus();
                        hoursTextBox.CaretIndex = hoursTextBox.Text.Length;
                        e.Handled = true;
                    }
                    break;
                case Key.Escape:
                    ValidateMinutes();
                    RemoveFocusToTop();
                    break;
                default:
                    // Do nothing
                    break;
            }

        }

        private void SecondsKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                case Key.Tab:
                    ValidateSeconds();
                    break;
                case Key.Up:
                    IncrementSeconds();
                    break;
                case Key.Down:
                    DecrementSeconds();
                    break;
                case Key.Left:
                    if (secondsTextBox.CaretIndex <= 0 && minutesTextBox != null)
                    {
                        minutesTextBox.Focus();
                        minutesTextBox.CaretIndex = minutesTextBox.Text.Length;
                        e.Handled = true;
                    }
                    break;
                case Key.Escape:
                    ValidateSeconds();
                    RemoveFocusToTop();
                    break;
                default:
                    // Do nothing
                    break;
            }

        }
        #endregion

        #region Text Input
        private void HoursPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string text = hoursTextBox.Text.Remove(hoursTextBox.SelectionStart, hoursTextBox.SelectionLength);
            text = text.Insert(hoursTextBox.SelectionStart, e.Text);
            e.Handled = !(Regex.IsMatch(text, hoursMask));
            base.OnPreviewTextInput(e);
        }

        private void MinutesPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string text = minutesTextBox.Text.Remove(minutesTextBox.SelectionStart, minutesTextBox.SelectionLength);
            text = text.Insert(minutesTextBox.SelectionStart, e.Text);
            e.Handled = !(Regex.IsMatch(text, minutesMask));
            base.OnPreviewTextInput(e);
        }

        private void SecondsPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string text = secondsTextBox.Text.Remove(secondsTextBox.SelectionStart, secondsTextBox.SelectionLength);
            text = text.Insert(secondsTextBox.SelectionStart, e.Text);
            e.Handled = !(Regex.IsMatch(text, secondsMask));
            base.OnPreviewTextInput(e);
        }

        private void SecondsTextChanged(object sender, TextChangedEventArgs e)
        {
            // Workaround for text clearing during initialization in OnTextContainerChanged()
            if (string.IsNullOrEmpty(secondsTextBox.Text) && !secondsLoaded)
            {
                CopyValuesToTextBoxes();
                secondsLoaded = true;
            }
        }

        private void MinutesTextChanged(object sender, TextChangedEventArgs e)
        {
            // Workaround for text clearing during initialization in OnTextContainerChanged()
            if (string.IsNullOrEmpty(minutesTextBox.Text) && !minutesLoaded)
            {
                CopyValuesToTextBoxes();
                minutesLoaded = true;
            }
        }

        private void HoursTextChanged(object sender, TextChangedEventArgs e)
        {
            // Workaround for text clearing during initialization in OnTextContainerChanged()
            if (string.IsNullOrEmpty(hoursTextBox.Text) && !hoursLoaded)
            {
                CopyValuesToTextBoxes();
                hoursLoaded = true;
            }
        }
        #endregion

        #region Focus
        private void HoursLostFocus(object sender, RoutedEventArgs e)
        {
            ValidateHours();
        }

        private void MinutesLostFocus(object sender, RoutedEventArgs e)
        {
            ValidateMinutes();
        }

        private void SecondsLostFocus(object sender, RoutedEventArgs e)
        {
            ValidateSeconds();
        }

        private void HoursGotFocus(object sender, RoutedEventArgs e)
        {
            activeField = TimeEditFields.Hours;
        }

        private void MinutesGotFocus(object sender, RoutedEventArgs e)
        {
            activeField = TimeEditFields.Minutes;
        }

        private void SecondsGotFocus(object sender, RoutedEventArgs e)
        {
            activeField = TimeEditFields.Seconds;
        }

        /// <summary>
        /// Invoked whenever an unhandled System.Windows.UIElement.GotFocus event reaches
        /// this element in its route.
        /// </summary>
        /// <param name="e">The System.Windows.RoutedEventArgs that contains the event data.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            switch (activeField)
            {
                case TimeEditFields.Hours:
                    if (hoursTextBox != null)
                        hoursTextBox.Focus();
                    break;
                case TimeEditFields.Minutes:
                    if (minutesTextBox != null)
                        minutesTextBox.Focus();
                    break;
                case TimeEditFields.Seconds:
                    if (secondsTextBox != null)
                        secondsTextBox.Focus();
                    break;
            }

            base.OnGotFocus(e);
        }
        #endregion
        #endregion

        #region Private Utility Methods
        private void IncrementHours()
        {
            Hours += 1;
            ValidateHours();
        }

        private void DecrementHours()
        {
            Hours = Math.Max(0, Hours - 1);
            ValidateHours();
        }

        private void IncrementMinutes()
        {
            if (Minutes >= 59)
            {
                IncrementHours();
                Minutes = 0;
            }
            else
            {
                Minutes = Math.Min(60, Minutes + 1);
            }
            ValidateMinutes();
        }

        private void DecrementMinutes()
        {
            if (Minutes == 0 && Hours > 0)
            {
                DecrementHours();
                Minutes = 59;
            }
            else
            {
                Minutes = Math.Max(0, Minutes - 1);
            }
            ValidateMinutes();
        }

        private void IncrementSeconds()
        {
            if (Seconds >= 59)
            {
                IncrementMinutes();
                Seconds = 0;
            }
            else
            {
                Seconds = Math.Min(60.0d, Seconds + 1.0d);
            }
            ValidateSeconds();
        }

        private void DecrementSeconds()
        {
            if (Seconds == 0 && Minutes > 0)
            {
                DecrementMinutes();
                Seconds = 59;
            }
            else
            {
                Seconds = Math.Max(0, Seconds - 1.0d);
            }
            ValidateSeconds();
        }

        private void ValidateHours()
        {
            if (hoursTextBox == null)
                return;

            int hours = 0;
            int.TryParse(hoursTextBox.Text, out hours);
            Hours = hours;
            CopyValuesToTextBoxes();
        }

        private void ValidateMinutes()
        {
            if (minutesTextBox == null)
                return;

            int minutes = 0;
            int.TryParse(minutesTextBox.Text, out minutes);
            Minutes = minutes;
            CopyValuesToTextBoxes();
        }

        private void ValidateSeconds()
        {
            if (secondsTextBox == null)
                return;

            double seconds = 0;
            double.TryParse(secondsTextBox.Text, out seconds);
            Seconds = seconds;
            CopyValuesToTextBoxes();
        }

        private void UpdateMasks()
        {
            hoursMask = CreateMask(0, Minimum.TotalHours, false);
            minutesMask = CreateMask(0, Math.Min(60, Maximum.TotalMinutes), false);
            secondsMask = CreateMask(0, Math.Min(60, Maximum.TotalSeconds), true);
            CoerceValue(ValueProperty);
        }

        private void UpdateReadOnly()
        {
            if (hoursTextBox != null)
                hoursTextBox.IsReadOnly = IsReadOnly;
            if (minutesTextBox != null)
                minutesTextBox.IsReadOnly = IsReadOnly;
            if (secondsTextBox != null)
                secondsTextBox.IsReadOnly = IsReadOnly;
            if (spinUpButton != null)
                spinUpButton.IsEnabled = !IsReadOnly;
            if (spinDownButton != null)
                spinDownButton.IsEnabled = !IsReadOnly;
        }

        private static string CreateMask(double minValue, double maxValue, bool allowDecimals)
        {
            string maskString = string.Empty;
            bool isNegative = minValue < 0;
            string allowNegativeString = isNegative ? @"-?" : string.Empty;
            string allowDecimalString = allowDecimals ? @"(\.\d{0,2})?" : string.Empty;
            if (maxValue == 0 && minValue == 0)
            {
                maskString = string.Format("^{0}{1}{2}$", allowNegativeString, @"\d{0,2}?", allowDecimalString);
            }
            else
            {
                int minValueLength = isNegative ? ((int)minValue).ToString().Length : ((int)minValue).ToString().Length + 1;
                int numericLength = Math.Max(minValueLength, ((int)maxValue).ToString().Length);
                maskString = string.Format("^{0}{1}{2}{3}{4}$", allowNegativeString, @"\d{0,", numericLength, @"}?", allowDecimalString);
            }
            return maskString;
        }

        private void CopyValuesToTextBoxes()
        {
            if (hoursTextBox != null)
                hoursTextBox.Text = Hours.ToString("00");
            if (minutesTextBox != null)
                minutesTextBox.Text = Minutes.ToString("00");
            if (secondsTextBox != null)
                secondsTextBox.Text = Seconds.ToString("00.##");
        }

        private void RemoveFocusToTop()
        {
            FrameworkElement parent = Parent as FrameworkElement;
            while (parent != null && parent is IInputElement && !((IInputElement)parent).Focusable)
            {
                parent = (FrameworkElement)parent.Parent;
            }

            DependencyObject scope = FocusManager.GetFocusScope(this);
            FocusManager.SetFocusedElement(scope, parent as IInputElement);
        }
        #endregion
    }
}
