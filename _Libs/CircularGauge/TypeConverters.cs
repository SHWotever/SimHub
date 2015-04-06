/*Copyright (c) 2009 T.Evelyn (evescode@gmail.com)

All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1.Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2.Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in  the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS 
BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH 
DAMAGE.*/

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CircularGauge
{
    /// <summary>
    /// Converts the given color to a SolidColorBrush
    /// </summary>
    public class ColorToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            Color c = (Color)value;

            return new SolidColorBrush(c);
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// A type converter for converting image offset into render transform
    /// </summary>
    public class ImageOffsetConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            double dblVal = (double)value;
            TranslateTransform tt = new TranslateTransform();
            tt.Y = dblVal;
            return tt;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts radius to diameter
    /// </summary>
    public class RadiusToDiameterConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            double dblVal = (double)value;

            return (dblVal * 2);
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Calculates the pointer position
    /// </summary>
    public class PointerCenterConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            double dblVal = (double)value;
            TransformGroup tg = new TransformGroup();
            RotateTransform rt = new RotateTransform();
            TranslateTransform tt = new TranslateTransform();

            tt.X = dblVal / 2;
            tg.Children.Add(rt);
            tg.Children.Add(tt);

            return tg;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Calculates the range indicator light position
    /// </summary>
    public class RangeIndicatorLightPositionConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            double dblVal = (double)value;
            TransformGroup tg = new TransformGroup();
            RotateTransform rt = new RotateTransform();
            TranslateTransform tt = new TranslateTransform();

            tt.Y = dblVal;
            tg.Children.Add(rt);
            tg.Children.Add(tt);

            return tg;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts the given Size to height and width
    /// </summary>
    public class SizeConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            double i = 0;
            Size s = (Size)value;
            if (parameter.ToString() == "Height")
            {
                i = s.Height;
            }
            else if (parameter.ToString() == "Width")
            {
                i = s.Width;
            }

            return i;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Scaling factor for drawing the glass effect.
    /// </summary>
    public class GlassEffectWidthConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            double dbl = (double)value;
            return (dbl * 2) * 0.94;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts background color to Gradient effect
    /// </summary>
    public class BackgroundColorConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            Color c = (Color)value;
            RadialGradientBrush radBrush = new RadialGradientBrush();
            GradientStop g1 = new GradientStop();
            g1.Offset = 0.982;
            g1.Color = c;
            GradientStop g2 = new GradientStop();
            g2.Color = Color.FromArgb(0xFF, 0xAF, 0xB2, 0xB0);
            radBrush.GradientStops.Add(g1);
            radBrush.GradientStops.Add(g2);
            return radBrush;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}