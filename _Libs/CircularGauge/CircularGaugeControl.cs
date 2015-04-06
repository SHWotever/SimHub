/*Copyright (c) 2009 T.Evelyn (evescode@gmail.com)

All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1.Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2.Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in
 the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,

THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS

BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE

GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT

LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH

DAMAGE.*/

using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace CircularGauge
{
    /// <summary>
    /// Represents a Circular Gauge control
    /// </summary>
    [TemplatePart(Name = "LayoutRoot", Type = typeof(Grid))]
    [TemplatePart(Name = "Pointer", Type = typeof(Path))]
    [TemplatePart(Name = "RangeIndicatorLight", Type = typeof(Ellipse))]
    [TemplatePart(Name = "PointerCap", Type = typeof(Ellipse))]
    public class CircularGaugeControl : Control
    {
        #region Private variables

        //Private variables
        private Grid rootGrid;

        private Path rangeIndicator;
        private Path pointer;
        private Ellipse pointerCap;
        private Ellipse lightIndicator;
        private bool isInitialValueSet = false;
        private Double arcradius1;
        private Double arcradius2;
        private int animatingSpeedFactor = 6;

        #endregion Private variables

        #region Dependency properties

        /// <summary>
        /// Dependency property to Get/Set the current value
        /// </summary>
        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register("CurrentValue", typeof(double), typeof(CircularGaugeControl),
            new PropertyMetadata(Double.MinValue, new PropertyChangedCallback(CircularGaugeControl.OnCurrentValuePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Minimum Value
        /// </summary>
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Maximum Value
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Radius of the gauge
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Pointer cap Radius
        /// </summary>
        public static readonly DependencyProperty PointerCapRadiusProperty =
            DependencyProperty.Register("PointerCapRadius", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the pointer length
        /// </summary>
        public static readonly DependencyProperty PointerLengthProperty =
            DependencyProperty.Register("PointerLength", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the scale Radius
        /// </summary>
        public static readonly DependencyProperty ScaleRadiusProperty =
            DependencyProperty.Register("ScaleRadius", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the scale Radius
        /// </summary>
        public static readonly DependencyProperty MinorScaleRadiusProperty =
            DependencyProperty.Register("MinorScaleRadius", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the starting angle of scale
        /// </summary>
        public static readonly DependencyProperty ScaleStartAngleProperty =
            DependencyProperty.Register("ScaleStartAngle", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the sweep angle of scale
        /// </summary>
        public static readonly DependencyProperty ScaleSweepAngleProperty =
            DependencyProperty.Register("ScaleSweepAngle", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the number of major divisions on the scale
        /// </summary>
        public static readonly DependencyProperty MajorDivisionsCountProperty =
            DependencyProperty.Register("MajorDivisionsCount", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the number of minor divisions on the scale
        /// </summary>
        public static readonly DependencyProperty MinorDivisionsCountProperty =
            DependencyProperty.Register("MinorDivisionsCount", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set Optimal Range End Value
        /// </summary>
        public static readonly DependencyProperty OptimalRangeEndValueProperty =
           DependencyProperty.Register("OptimalRangeEndValue", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnOptimalRangeEndValuePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set Optimal Range Start Value
        /// </summary>
        public static readonly DependencyProperty OptimalRangeStartValueProperty =
           DependencyProperty.Register("OptimalRangeStartValue", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnOptimalRangeStartValuePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the image source
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty =
          DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the image offset
        /// </summary>
        public static readonly DependencyProperty ImageOffsetProperty =
          DependencyProperty.Register("ImageOffset", typeof(double), typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the range indicator light offset
        /// </summary>
        public static readonly DependencyProperty RangeIndicatorLightOffsetProperty =
          DependencyProperty.Register("RangeIndicatorLightOffset", typeof(double), typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the image Size
        /// </summary>
        public static readonly DependencyProperty ImageSizeProperty =
          DependencyProperty.Register("ImageSize", typeof(Size), typeof(CircularGaugeControl), null);

        /// <summary>
        /// Dependency property to Get/Set the Range Indicator Radius
        /// </summary>
        public static readonly DependencyProperty RangeIndicatorRadiusProperty =
          DependencyProperty.Register("RangeIndicatorRadius", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Range Indicator Thickness
        /// </summary>
        public static readonly DependencyProperty RangeIndicatorThicknessProperty =
         DependencyProperty.Register("RangeIndicatorThickness", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the scale label Radius
        /// </summary>
        public static readonly DependencyProperty ScaleLabelRadiusProperty =
            DependencyProperty.Register("ScaleLabelRadius", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Scale Label Size
        /// </summary>
        public static readonly DependencyProperty ScaleLabelSizeProperty =
         DependencyProperty.Register("ScaleLabelSize", typeof(Size), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Scale Label FontSize
        /// </summary>
        public static readonly DependencyProperty ScaleLabelFontSizeProperty =
            DependencyProperty.Register("ScaleLabelFontSize", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Scale Label FontSize
        /// </summary>
        public static readonly DependencyProperty ScaleLabelFontStyleProperty =
            DependencyProperty.Register("ScaleLabelFontStyle", typeof(FontStyle), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Scale Label FontSize
        /// </summary>
        public static readonly DependencyProperty ScaleLabelFontWeightProperty =
            DependencyProperty.Register("ScaleLabelFontWeight", typeof(FontWeight), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Scale Label Foreground
        /// </summary>

        public static readonly DependencyProperty ScaleLabelForegroundProperty =
            DependencyProperty.Register("ScaleLabelForeground", typeof(Color), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Major Tick Size
        /// </summary>
        public static readonly DependencyProperty MajorTickSizeProperty =
          DependencyProperty.Register("MajorTickRectSize", typeof(Size), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Minor Tick Size
        /// </summary>
        public static readonly DependencyProperty MinorTickSizeProperty =
          DependencyProperty.Register("MinorTickSize", typeof(Size), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Major Tick Color
        /// </summary>
        public static readonly DependencyProperty MajorTickColorProperty =
           DependencyProperty.Register("MajorTickColor", typeof(Color), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Minor Tick Color
        /// </summary>
        public static readonly DependencyProperty MinorTickColorProperty =
          DependencyProperty.Register("MinorTickColor", typeof(Color), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Gauge Background Color
        /// </summary>
        public static readonly DependencyProperty GaugeBackgroundColorProperty =
          DependencyProperty.Register("GaugeBackgroundColor", typeof(Color), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Pointer Thickness
        /// </summary>
        public static readonly DependencyProperty PointerThicknessProperty =
        DependencyProperty.Register("PointerThickness", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the an option to reset the pointer on start up to the minimum value
        /// </summary>
        public static readonly DependencyProperty ResetPointerOnStartUpProperty =
        DependencyProperty.Register("ResetPointerOnStartUp", typeof(bool), typeof(CircularGaugeControl), new PropertyMetadata(false, null));

        /// <summary>
        /// Dependency property to Get/Set the Scale Value Precision
        /// </summary>
        public static readonly DependencyProperty ScaleValuePrecisionProperty =
        DependencyProperty.Register("ScaleValuePrecision", typeof(int), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Below Optimal Range Color
        /// </summary>

        public static readonly DependencyProperty BelowOptimalRangeColorProperty =
            DependencyProperty.Register("BelowOptimalRangeColor", typeof(Color), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Optimal Range Color
        /// </summary>

        public static readonly DependencyProperty OptimalRangeColorProperty =
            DependencyProperty.Register("OptimalRangeColor", typeof(Color), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Above Optimal Range Color
        /// </summary>

        public static readonly DependencyProperty AboveOptimalRangeColorProperty =
            DependencyProperty.Register("AboveOptimalRangeColor", typeof(Color), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Dial Text
        /// </summary>

        public static readonly DependencyProperty DialTextProperty =
            DependencyProperty.Register("DialText", typeof(string), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Dial Text Color
        /// </summary>

        public static readonly DependencyProperty DialTextColorProperty =
            DependencyProperty.Register("DialTextColor", typeof(Color), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Dial Text Font Size
        /// </summary>

        public static readonly DependencyProperty DialTextFontSizeProperty =
            DependencyProperty.Register("DialTextFontSize", typeof(int), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Dial Text Offset
        /// </summary>

        public static readonly DependencyProperty DialTextOffsetProperty =
            DependencyProperty.Register("DialTextOffset", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        /// <summary>
        /// Dependency property to Get/Set the Range Indicator light Radius
        /// </summary>

        public static readonly DependencyProperty RangeIndicatorLightRadiusProperty =
            DependencyProperty.Register("RangeIndicatorLightRadius", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnScalePropertyChanged)));

        #endregion Dependency properties

        #region Wrapper properties

        /// <summary>
        /// Gets/Sets the current value
        /// </summary>
        public double CurrentValue
        {
            get
            {
                return (double)GetValue(CurrentValueProperty);
            }
            set
            {
                SetValue(CurrentValueProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Minimum Value
        /// </summary>
        public double MinValue
        {
            get
            {
                return (double)GetValue(MinValueProperty);
            }
            set
            {
                SetValue(MinValueProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Maximum Value
        /// </summary>
        public double MaxValue
        {
            get
            {
                return (double)GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Minimum Value
        /// </summary>
        public double Radius
        {
            get
            {
                return (double)GetValue(RadiusProperty);
            }
            set
            {
                SetValue(RadiusProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Pointer cap radius
        /// </summary>
        public double PointerCapRadius
        {
            get
            {
                return (double)GetValue(PointerCapRadiusProperty);
            }
            set
            {
                SetValue(PointerCapRadiusProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Pointer Length
        /// </summary>
        public double PointerLength
        {
            get
            {
                return (double)GetValue(PointerLengthProperty);
            }
            set
            {
                SetValue(PointerLengthProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Pointer Thickness
        /// </summary>
        public double PointerThickness
        {
            get
            {
                return (double)GetValue(PointerThicknessProperty);
            }
            set
            {
                SetValue(PointerThicknessProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Scale radius
        /// </summary>
        public double ScaleRadius
        {
            get
            {
                return (double)GetValue(ScaleRadiusProperty);
            }
            set
            {
                SetValue(ScaleRadiusProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Scale radius
        /// </summary>
        public double MinorScaleRadius
        {
            get
            {
                return (double)GetValue(MinorScaleRadiusProperty);
            }
            set
            {
                SetValue(MinorScaleRadiusProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the scale start angle
        /// </summary>
        public double ScaleStartAngle
        {
            get
            {
                return (double)GetValue(ScaleStartAngleProperty);
            }
            set
            {
                SetValue(ScaleStartAngleProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the scale sweep angle
        /// </summary>
        public double ScaleSweepAngle
        {
            get
            {
                return (double)GetValue(ScaleSweepAngleProperty);
            }
            set
            {
                SetValue(ScaleSweepAngleProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the number of major divisions on the scale
        /// </summary>
        public double MajorDivisionsCount
        {
            get
            {
                return (double)GetValue(MajorDivisionsCountProperty);
            }
            set
            {
                SetValue(MajorDivisionsCountProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the number of minor divisions on the scale
        /// </summary>
        public double MinorDivisionsCount
        {
            get
            {
                return (double)GetValue(MinorDivisionsCountProperty);
            }
            set
            {
                SetValue(MinorDivisionsCountProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Optimal range end value
        /// </summary>
        public double OptimalRangeEndValue
        {
            get
            {
                return (double)GetValue(OptimalRangeEndValueProperty);
            }
            set
            {
                SetValue(OptimalRangeEndValueProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Optimal Range Start Value
        /// </summary>
        public double OptimalRangeStartValue
        {
            get
            {
                return (double)GetValue(OptimalRangeStartValueProperty);
            }
            set
            {
                SetValue(OptimalRangeStartValueProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Gauge image source
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return (ImageSource)GetValue(ImageSourceProperty);
            }
            set
            {
                SetValue(ImageSourceProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Image offset
        /// </summary>
        public double ImageOffset
        {
            get
            {
                return (double)GetValue(ImageOffsetProperty);
            }
            set
            {
                SetValue(ImageOffsetProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Range Indicator Light offset
        /// </summary>
        public double RangeIndicatorLightOffset
        {
            get
            {
                return (double)GetValue(RangeIndicatorLightOffsetProperty);
            }
            set
            {
                SetValue(RangeIndicatorLightOffsetProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Image width and height
        /// </summary>
        public Size ImageSize
        {
            get
            {
                return (Size)GetValue(ImageSizeProperty);
            }
            set
            {
                SetValue(ImageSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Range Indicator Radius
        /// </summary>
        public double RangeIndicatorRadius
        {
            get
            {
                return (double)GetValue(RangeIndicatorRadiusProperty);
            }
            set
            {
                SetValue(RangeIndicatorRadiusProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Range Indicator Thickness
        /// </summary>
        public double RangeIndicatorThickness
        {
            get
            {
                return (double)GetValue(RangeIndicatorThicknessProperty);
            }
            set
            {
                SetValue(RangeIndicatorThicknessProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Scale Label Radius
        /// </summary>
        public double ScaleLabelRadius
        {
            get
            {
                return (double)GetValue(ScaleLabelRadiusProperty);
            }
            set
            {
                SetValue(ScaleLabelRadiusProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Scale Label Size
        /// </summary>
        public Size ScaleLabelSize
        {
            get
            {
                return (Size)GetValue(ScaleLabelSizeProperty);
            }
            set
            {
                SetValue(ScaleLabelSizeProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Scale Label Font Size
        /// </summary>
        public double ScaleLabelFontSize
        {
            get
            {
                return (double)GetValue(ScaleLabelFontSizeProperty);
            }
            set
            {
                SetValue(ScaleLabelFontSizeProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Scale Label Font Style
        /// </summary>
        public FontStyle ScaleLabelFontStyle
        {
            get
            {
                return (FontStyle)GetValue(ScaleLabelFontStyleProperty);
            }
            set
            {
                SetValue(ScaleLabelFontStyleProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Scale Label Font Weight
        /// </summary>
        public FontWeight ScaleLabelFontWeight
        {
            get
            {
                return (FontWeight)GetValue(ScaleLabelFontWeightProperty);
            }
            set
            {
                SetValue(ScaleLabelFontWeightProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Scale Label Foreground
        /// </summary>
        public Color ScaleLabelForeground
        {
            get
            {
                return (Color)GetValue(ScaleLabelForegroundProperty);
            }
            set
            {
                SetValue(ScaleLabelForegroundProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Major Tick Size
        /// </summary>
        public Size MajorTickSize
        {
            get
            {
                return (Size)GetValue(MajorTickSizeProperty);
            }
            set
            {
                SetValue(MajorTickSizeProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Minor Tick Size
        /// </summary>
        public Size MinorTickSize
        {
            get
            {
                return (Size)GetValue(MinorTickSizeProperty);
            }
            set
            {
                SetValue(MinorTickSizeProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Major Tick Color
        /// </summary>
        public Color MajorTickColor
        {
            get
            {
                return (Color)GetValue(MajorTickColorProperty);
            }
            set
            {
                SetValue(MajorTickColorProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Minor Tick Color
        /// </summary>
        public Color MinorTickColor
        {
            get
            {
                return (Color)GetValue(MinorTickColorProperty);
            }
            set
            {
                SetValue(MinorTickColorProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets the Gauge Background color
        /// </summary>
        public Color GaugeBackgroundColor
        {
            get
            {
                return (Color)GetValue(GaugeBackgroundColorProperty);
            }
            set
            {
                SetValue(GaugeBackgroundColorProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets option to reset the pointer to minimum on start up, Default is true
        /// </summary>
        public bool ResetPointerOnStartUp
        {
            get
            {
                return (bool)GetValue(ResetPointerOnStartUpProperty);
            }
            set
            {
                SetValue(ResetPointerOnStartUpProperty, value);
            }
        }

        /// <summary>
        /// Gets/Sets scale value precision
        /// </summary>
        public int ScaleValuePrecision
        {
            get
            {
                return (int)GetValue(ScaleValuePrecisionProperty);
            }
            set
            {
                SetValue(ScaleValuePrecisionProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets Below Optimal Range Color
        /// </summary>
        public Color BelowOptimalRangeColor
        {
            get
            {
                return (Color)GetValue(BelowOptimalRangeColorProperty);
            }
            set
            {
                SetValue(BelowOptimalRangeColorProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets Optimal Range Color
        /// </summary>
        public Color OptimalRangeColor
        {
            get
            {
                return (Color)GetValue(OptimalRangeColorProperty);
            }
            set
            {
                SetValue(OptimalRangeColorProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets Above Optimal Range Color
        /// </summary>
        public Color AboveOptimalRangeColor
        {
            get
            {
                return (Color)GetValue(AboveOptimalRangeColorProperty);
            }
            set
            {
                SetValue(AboveOptimalRangeColorProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets Dial Text
        /// </summary>
        public string DialText
        {
            get
            {
                return (string)GetValue(DialTextProperty);
            }
            set
            {
                SetValue(DialTextProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets Dial Text Color
        /// </summary>
        public Color DialTextColor
        {
            get
            {
                return (Color)GetValue(DialTextColorProperty);
            }
            set
            {
                SetValue(DialTextColorProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets Dial Text Font Size
        /// </summary>
        public int DialTextFontSize
        {
            get
            {
                return (int)GetValue(DialTextFontSizeProperty);
            }
            set
            {
                SetValue(DialTextFontSizeProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets Dial Text Offset
        /// </summary>
        public double DialTextOffset
        {
            get
            {
                return (double)GetValue(DialTextOffsetProperty);
            }
            set
            {
                SetValue(DialTextOffsetProperty, value);
                this.RefreshScale();
            }
        }

        /// <summary>
        /// Gets/Sets Range Indicator Light Radius
        /// </summary>
        public double RangeIndicatorLightRadius
        {
            get
            {
                return (double)GetValue(RangeIndicatorLightRadiusProperty);
            }
            set
            {
                SetValue(RangeIndicatorLightRadiusProperty, value);
                this.RefreshScale();
            }
        }

        #endregion Wrapper properties

        #region Constructor

        static CircularGaugeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CircularGaugeControl), new FrameworkPropertyMetadata(typeof(CircularGaugeControl)));
        }

        public CircularGaugeControl()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(2000);
                        try
                        {
                            RefreshScale();
                        }
                        catch { }
                    }
                });
            }
        }

        #endregion Constructor

        #region Methods

        private static void OnCurrentValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Get access to the instance of CircularGaugeConrol whose property value changed
            CircularGaugeControl gauge = d as CircularGaugeControl;
            gauge.OnCurrentValueChanged(e);
        }

        private static void OnOptimalRangeEndValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Get access to the instance of CircularGaugeConrol whose property value changed
            CircularGaugeControl gauge = d as CircularGaugeControl;
            if ((double)e.NewValue > gauge.MaxValue)
            {
                gauge.OptimalRangeEndValue = gauge.MaxValue;
            }
        }

        private static void OnOptimalRangeStartValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Get access to the instance of CircularGaugeConrol whose property value changed
            CircularGaugeControl gauge = d as CircularGaugeControl;
            if ((double)e.NewValue < gauge.MinValue)
            {
                gauge.OptimalRangeStartValue = gauge.MinValue;
            }
        }

        private static void OnScalePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Get access to the instance of CircularGaugeConrol whose property value changed
            CircularGaugeControl gauge = d as CircularGaugeControl;
            gauge.RefreshScale();
        }

        public virtual void OnCurrentValueChanged(DependencyPropertyChangedEventArgs e)
        {
            //Validate and set the new value
            double newValue = (double)e.NewValue;
            double oldValue = (double)e.OldValue;

            if (newValue > this.MaxValue)
            {
                newValue = this.MaxValue;
            }
            else if (newValue < this.MinValue)
            {
                newValue = this.MinValue;
            }

            if (oldValue > this.MaxValue)
            {
                oldValue = this.MaxValue;
            }
            else if (oldValue < this.MinValue)
            {
                oldValue = this.MinValue;
            }

            if (pointer != null)
            {
                double db1 = 0;
                Double oldcurr_realworldunit = 0;
                Double newcurr_realworldunit = 0;
                Double realworldunit = (ScaleSweepAngle / (MaxValue - MinValue));
                //Resetting the old value to min value the very first time.
                if (oldValue == 0 && !isInitialValueSet)
                {
                    oldValue = MinValue;
                    isInitialValueSet = true;
                }
                if (oldValue < 0)
                {
                    db1 = MinValue + Math.Abs(oldValue);
                    oldcurr_realworldunit = ((double)(Math.Abs(db1 * realworldunit)));
                }
                else
                {
                    db1 = Math.Abs(MinValue) + oldValue;
                    oldcurr_realworldunit = ((double)(db1 * realworldunit));
                }
                if (newValue < 0)
                {
                    db1 = MinValue + Math.Abs(newValue);
                    newcurr_realworldunit = ((double)(Math.Abs(db1 * realworldunit)));
                }
                else
                {
                    db1 = Math.Abs(MinValue) + newValue;
                    newcurr_realworldunit = ((double)(db1 * realworldunit));
                }

                Double oldcurrentvalueAngle = (ScaleStartAngle + oldcurr_realworldunit);
                Double newcurrentvalueAngle = (ScaleStartAngle + newcurr_realworldunit);

                //Animate the pointer from the old value to the new value
                AnimatePointer(oldcurrentvalueAngle, newcurrentvalueAngle);
            }
        }

        /// <summary>
        /// Animates the pointer to the current value to the new one
        /// </summary>
        /// <param name="oldcurrentvalueAngle"></param>
        /// <param name="newcurrentvalueAngle"></param>
        private void AnimatePointer(double oldcurrentvalueAngle, double newcurrentvalueAngle)
        {
            if (pointer != null)
            {
                DoubleAnimation da = new DoubleAnimation();
                da.From = oldcurrentvalueAngle;
                da.To = newcurrentvalueAngle;

                double animDuration = Math.Abs(oldcurrentvalueAngle - newcurrentvalueAngle) * animatingSpeedFactor;
                da.Duration = new Duration(TimeSpan.FromMilliseconds(animDuration));

                Storyboard sb = new Storyboard();
                sb.Completed += new EventHandler(sb_Completed);
                sb.Children.Add(da);
                Storyboard.SetTarget(da, pointer);
                Storyboard.SetTargetProperty(da, new PropertyPath("(Path.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));

                if (newcurrentvalueAngle != oldcurrentvalueAngle)
                {
                    sb.Begin();
                }
            }
        }

        /// <summary>
        /// Move pointer without animating
        /// </summary>
        /// <param name="angleValue"></param>
        private void MovePointer(double angleValue)
        {
            if (pointer != null)
            {
                TransformGroup tg = pointer.RenderTransform as TransformGroup;
                RotateTransform rt = tg.Children[0] as RotateTransform;
                rt.Angle = angleValue;
            }
        }

        /// <summary>
        /// Switch on the Range indicator light after the pointer completes animating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sb_Completed(object sender, EventArgs e)
        {
            if (this.CurrentValue > OptimalRangeEndValue)
            {
                lightIndicator.Fill = GetRangeIndicatorGradEffect(AboveOptimalRangeColor);
            }
            else if (this.CurrentValue <= OptimalRangeEndValue && this.CurrentValue >= OptimalRangeStartValue)
            {
                lightIndicator.Fill = GetRangeIndicatorGradEffect(OptimalRangeColor);
            }
            else if (this.CurrentValue < OptimalRangeStartValue)
            {
                lightIndicator.Fill = GetRangeIndicatorGradEffect(BelowOptimalRangeColor);
            }
        }

        /// <summary>
        /// Get gradient brush effect for the range indicator light
        /// </summary>
        /// <param name="gradientColor"></param>
        /// <returns></returns>
        private GradientBrush GetRangeIndicatorGradEffect(Color gradientColor)
        {
            LinearGradientBrush gradient = new LinearGradientBrush();
            gradient.StartPoint = new Point(0, 0);
            gradient.EndPoint = new Point(1, 1);
            GradientStop color1 = new GradientStop();
            if (gradientColor == Colors.Transparent)
            {
                color1.Color = gradientColor;
            }
            else
                color1.Color = Colors.LightGray;

            color1.Offset = 0.2;
            gradient.GradientStops.Add(color1);
            GradientStop color2 = new GradientStop();
            color2.Color = gradientColor; color2.Offset = 0.5;
            gradient.GradientStops.Add(color2);
            GradientStop color3 = new GradientStop();
            color3.Color = gradientColor; color3.Offset = 0.8;
            gradient.GradientStops.Add(color3);
            return gradient;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //Get reference to known elements on the control template
            rootGrid = GetTemplateChild("LayoutRoot") as Grid;
            pointer = GetTemplateChild("Pointer") as Path;
            pointerCap = GetTemplateChild("PointerCap") as Ellipse;
            lightIndicator = GetTemplateChild("RangeIndicatorLight") as Ellipse;

            pointer.Effect = new DropShadowEffect
            {
                Color = new Color { A = 128, R = 0, G = 0, B = 0 },
                Direction = 320,
                ShadowDepth = 0,
                Opacity = 1
            };

            //Draw scale and range indicator
            DrawScale();
            DrawRangeIndicator();

            //Set Zindex of pointer and pointer cap to a really high number so that it stays on top of the
            //scale and the range indicator
            Canvas.SetZIndex(pointer, 100000);
            Canvas.SetZIndex(pointerCap, 100001);

            if (ResetPointerOnStartUp)
            {
                //Reset Pointer
                MovePointer(ScaleStartAngle);
            }
        }

        //Drawing the scale with the Scale Radius
        private void DrawScale()
        {
            //Calculate one major tick angle
            Double majorTickUnitAngle = ScaleSweepAngle / MajorDivisionsCount;

            //Obtaining One minor tick angle
            Double minorTickUnitAngle = ScaleSweepAngle / MinorDivisionsCount;

            //Obtaining One major ticks value
            Double majorTicksUnitValue = (MaxValue - MinValue) / MajorDivisionsCount;
            majorTicksUnitValue = Math.Round(majorTicksUnitValue, ScaleValuePrecision);

            Double minvalue = MinValue; ;

            // Drawing Major scale ticks
            for (Double i = ScaleStartAngle; i <= (ScaleStartAngle + ScaleSweepAngle); i = i + majorTickUnitAngle)
            {
                //Majortick is drawn as a rectangle
                Rectangle majortickrect = new Rectangle();
                majortickrect.Height = MajorTickSize.Height;
                majortickrect.Width = MajorTickSize.Width;
                majortickrect.Fill = new SolidColorBrush(MajorTickColor);
                Point p = new Point(0.5, 0.5);
                majortickrect.RenderTransformOrigin = p;
                majortickrect.HorizontalAlignment = HorizontalAlignment.Center;
                majortickrect.VerticalAlignment = VerticalAlignment.Center;

                TransformGroup majortickgp = new TransformGroup();
                RotateTransform majortickrt = new RotateTransform();

                //Obtaining the angle in radians for calulating the points
                Double i_radian = (i * Math.PI) / 180;
                majortickrt.Angle = i;
                majortickgp.Children.Add(majortickrt);
                TranslateTransform majorticktt = new TranslateTransform();

                //Finding the point on the Scale where the major ticks are drawn
                //here drawing the points with center as (0,0)
                majorticktt.X = (int)((ScaleRadius) * Math.Cos(i_radian));
                majorticktt.Y = (int)((ScaleRadius) * Math.Sin(i_radian));

                //Points for the textblock which hold the scale value
                TranslateTransform majorscalevaluett = new TranslateTransform();
                //here drawing the points with center as (0,0)
                majorscalevaluett.X = (int)((ScaleLabelRadius) * Math.Cos(i_radian));
                majorscalevaluett.Y = (int)((ScaleLabelRadius) * Math.Sin(i_radian));

                //Defining the properties of the scale value textbox
                TextBlock tb = new TextBlock();
                tb.Margin = new Thickness(0, -5, 0, 0);
                tb.Height = ScaleLabelSize.Height;
                tb.Width = ScaleLabelSize.Width;
                tb.FontSize = ScaleLabelFontSize;
                tb.Foreground = new SolidColorBrush(ScaleLabelForeground);
                tb.TextAlignment = TextAlignment.Center;
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.FontStyle = ScaleLabelFontStyle;
                tb.FontWeight = ScaleLabelFontWeight;

                //Writing and appending the scale value

                //checking minvalue < maxvalue w.r.t scale precion value
                if (Math.Round(minvalue, ScaleValuePrecision) <= Math.Round(MaxValue, ScaleValuePrecision))
                {
                    minvalue = Math.Round(minvalue, ScaleValuePrecision);
                    tb.Text = minvalue.ToString();
                    minvalue = minvalue + majorTicksUnitValue;
                }
                else
                {
                    break;
                }
                majortickgp.Children.Add(majorticktt);
                majortickrect.RenderTransform = majortickgp;
                tb.RenderTransform = majorscalevaluett;
                rootGrid.Children.Add(majortickrect);
                rootGrid.Children.Add(tb);

                //Drawing the minor axis ticks
                Double onedegree = ((i + majorTickUnitAngle) - i) / (MinorDivisionsCount);

                if ((i < (ScaleStartAngle + ScaleSweepAngle)) && (Math.Round(minvalue, ScaleValuePrecision) <= Math.Round(MaxValue, ScaleValuePrecision)))
                {
                    //Drawing the minor scale
                    for (Double mi = i + onedegree; mi < (i + majorTickUnitAngle); mi = mi + onedegree)
                    {
                        //here the minortick is drawn as a rectangle
                        Rectangle mr = new Rectangle();
                        mr.Height = MinorTickSize.Height;
                        mr.Width = MinorTickSize.Width;
                        mr.Fill = new SolidColorBrush(MinorTickColor);
                        mr.HorizontalAlignment = HorizontalAlignment.Center;
                        mr.VerticalAlignment = VerticalAlignment.Center;
                        Point p1 = new Point(0.5, 0.5);
                        mr.RenderTransformOrigin = p1;

                        TransformGroup minortickgp = new TransformGroup();
                        RotateTransform minortickrt = new RotateTransform();
                        minortickrt.Angle = mi;
                        minortickgp.Children.Add(minortickrt);
                        TranslateTransform minorticktt = new TranslateTransform();

                        //Obtaining the angle in radians for calulating the points
                        Double mi_radian = (mi * Math.PI) / 180;
                        //Finding the point on the Scale where the minor ticks are drawn
                        minorticktt.X = (int)((MinorScaleRadius == 0 ? ScaleRadius : MinorScaleRadius) * Math.Cos(mi_radian));
                        minorticktt.Y = (int)((MinorScaleRadius == 0 ? ScaleRadius : MinorScaleRadius) * Math.Sin(mi_radian));

                        minortickgp.Children.Add(minorticktt);
                        mr.RenderTransform = minortickgp;
                        rootGrid.Children.Add(mr);
                    }
                }
            }
        }

        /// <summary>
        /// Obtaining the Point (x,y) in the circumference
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private Point GetCircumferencePoint(Double angle, Double radius)
        {
            Double angle_radian = (angle * Math.PI) / 180;
            //Radius-- is the Radius of the gauge
            Double X = (Double)((Radius) + (radius) * Math.Cos(angle_radian));
            Double Y = (Double)((Radius) + (radius) * Math.Sin(angle_radian));
            Point p = new Point(X, Y);
            return p;
        }

        /// <summary>
        /// Draw the range indicator
        /// </summary>
        private void DrawRangeIndicator()
        {
            Double realworldunit = (ScaleSweepAngle / (MaxValue - MinValue));
            Double optimalStartAngle;
            Double optimalEndAngle;
            double db;

            //Checking whether the  OptimalRangeStartvalue is -ve
            if (OptimalRangeStartValue < 0)
            {
                db = MinValue + Math.Abs(OptimalRangeStartValue);
                optimalStartAngle = ((double)(Math.Abs(db * realworldunit)));
            }
            else
            {
                db = Math.Abs(MinValue) + OptimalRangeStartValue;
                optimalStartAngle = ((double)(db * realworldunit));
            }

            //Checking whether the  OptimalRangeEndvalue is -ve
            if (OptimalRangeEndValue < 0)
            {
                db = MinValue + Math.Abs(OptimalRangeEndValue);
                optimalEndAngle = ((double)(Math.Abs(db * realworldunit)));
            }
            else
            {
                db = Math.Abs(MinValue) + OptimalRangeEndValue;
                optimalEndAngle = ((double)(db * realworldunit));
            }
            // calculating the angle for optimal Start value

            Double optimalStartAngleFromStart = (ScaleStartAngle + optimalStartAngle);

            // calculating the angle for optimal Start value

            Double optimalEndAngleFromStart = (ScaleStartAngle + optimalEndAngle);

            //Calculating the Radius of the two arc for segment
            arcradius1 = (RangeIndicatorRadius + RangeIndicatorThickness);
            arcradius2 = RangeIndicatorRadius;

            double endAngle = ScaleStartAngle + ScaleSweepAngle;

            // Calculating the Points for the below Optimal Range segment from the center of the gauge

            Point A = GetCircumferencePoint(ScaleStartAngle, arcradius1);
            Point B = GetCircumferencePoint(ScaleStartAngle, arcradius2);
            Point C = GetCircumferencePoint(optimalStartAngleFromStart, arcradius2);
            Point D = GetCircumferencePoint(optimalStartAngleFromStart, arcradius1);

            bool isReflexAngle = Math.Abs(optimalStartAngleFromStart - ScaleStartAngle) > 180.0;
            DrawSegment(A, B, C, D, isReflexAngle, BelowOptimalRangeColor);

            // Calculating the Points for the Optimal Range segment from the center of the gauge

            Point A1 = GetCircumferencePoint(optimalStartAngleFromStart, arcradius1);
            Point B1 = GetCircumferencePoint(optimalStartAngleFromStart, arcradius2);
            Point C1 = GetCircumferencePoint(optimalEndAngleFromStart, arcradius2);
            Point D1 = GetCircumferencePoint(optimalEndAngleFromStart, arcradius1);
            bool isReflexAngle1 = Math.Abs(optimalEndAngleFromStart - optimalStartAngleFromStart) > 180.0;
            DrawSegment(A1, B1, C1, D1, isReflexAngle1, OptimalRangeColor);

            // Calculating the Points for the Above Optimal Range segment from the center of the gauge

            Point A2 = GetCircumferencePoint(optimalEndAngleFromStart, arcradius1);
            Point B2 = GetCircumferencePoint(optimalEndAngleFromStart, arcradius2);
            Point C2 = GetCircumferencePoint(endAngle, arcradius2);
            Point D2 = GetCircumferencePoint(endAngle, arcradius1);
            bool isReflexAngle2 = Math.Abs(endAngle - optimalEndAngleFromStart) > 180.0;
            DrawSegment(A2, B2, C2, D2, isReflexAngle2, AboveOptimalRangeColor);
        }

        //Drawing the segment with two arc and two line

        private void DrawSegment(Point p1, Point p2, Point p3, Point p4, bool reflexangle, Color clr)
        {
            // Segment Geometry
            PathSegmentCollection segments = new PathSegmentCollection();

            // First line segment from pt p1 - pt p2
            segments.Add(new LineSegment() { Point = p2 });

            //Arc drawn from pt p2 - pt p3 with the RangeIndicatorRadius
            segments.Add(new ArcSegment()
            {
                Size = new Size(arcradius2, arcradius2),
                Point = p3,
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = reflexangle
            });

            // Second line segment from pt p3 - pt p4
            segments.Add(new LineSegment() { Point = p4 });

            //Arc drawn from pt p4 - pt p1 with the Radius of arcradius1
            segments.Add(new ArcSegment()
            {
                Size = new Size(arcradius1, arcradius1),
                Point = p1,
                SweepDirection = SweepDirection.Counterclockwise,
                IsLargeArc = reflexangle
            });

            // Defining the segment path properties
            Color rangestrokecolor;
            if (clr == Colors.Transparent)
            {
                rangestrokecolor = clr;
            }
            else
                rangestrokecolor = Colors.White;

            rangeIndicator = new Path()
            {
                StrokeLineJoin = PenLineJoin.Round,
                Stroke = new SolidColorBrush(rangestrokecolor),
                //Color.FromArgb(0xFF, 0xF5, 0x9A, 0x86)
                Fill = new SolidColorBrush(clr),
                Opacity = 0.65,
                StrokeThickness = 0.25,
                Data = new PathGeometry()
                {
                    Figures = new PathFigureCollection()
                     {
                        new PathFigure()
                        {
                            IsClosed = true,
                            StartPoint = p1,
                            Segments = segments
                        }
                    }
                }
            };

            //Set Z index of range indicator
            rangeIndicator.SetValue(Canvas.ZIndexProperty, 150);
            // Adding the segment to the root grid
            rootGrid.Children.Add(rangeIndicator);
        }

        public void RefreshScale()
        {
            rootGrid = GetTemplateChild("LayoutRoot") as Grid;
            if (rootGrid == null)
            {
                return;
            }
            for (int i = rootGrid.Children.Count - 1; i >= 0; i--)
            {
                TextBlock textblock = rootGrid.Children[i] as TextBlock;
                if (textblock != null)// If the UIElement is a Textbox
                {
                    if (textblock.Text != this.DialText)//don't remove dial text
                        rootGrid.Children.Remove(textblock);
                    continue;
                }

                Rectangle rectangle = rootGrid.Children[i] as Rectangle;
                if (rectangle != null)  // If the UIElement is a Rectangle
                {
                    rootGrid.Children.Remove(rectangle);
                    continue;
                }
            }
            this.OnApplyTemplate();
        }

        #endregion Methods
    }
}