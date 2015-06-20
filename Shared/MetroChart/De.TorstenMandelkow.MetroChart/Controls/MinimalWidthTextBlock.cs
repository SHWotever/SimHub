namespace De.TorstenMandelkow.MetroChart
{
    using System;
    using System.Collections.Generic;
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
    using Windows.UI.Xaml.Media.Animation;
    using Windows.UI.Core;
#else
    using System.Windows.Media;
    using System.Windows.Controls;
    using System.Windows;
#endif

#if SILVERLIGHT
    public class MinimalWidthTextBlock : Control
#else
    public class MinimalWidthTextBlock : Control
#endif
    { 
        private const char DEFAULTCHARSEPARATOR = '|';


        public static readonly DependencyProperty TextBlockStyleProperty =
            DependencyProperty.Register("TextBlockStyle", typeof(Style), typeof(MinimalWidthTextBlock),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MinimalWidthTextBlock),
            new PropertyMetadata(null));

        static MinimalWidthTextBlock()
        {
#if NETFX_CORE
            //do nothing
#elif SILVERLIGHT
            //do nothing
#else
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MinimalWidthTextBlock), new FrameworkPropertyMetadata(typeof(MinimalWidthTextBlock))); 
#endif
        }

        public MinimalWidthTextBlock()
        {
#if NETFX_CORE
            this.DefaultStyleKey = typeof(MinimalWidthTextBlock);
#elif SILVERLIGHT
            this.DefaultStyleKey = typeof(MinimalWidthTextBlock);
#else
            //do nothing
#endif
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
        Border mainBorder = null;
        TextBlock mainTextBlock = null;
        private void InternalOnApplyTemplate()
        {
            mainBorder = this.GetTemplateChild("PART_Border") as Border; 
            mainTextBlock = this.GetTemplateChild("PART_TextBlock") as TextBlock;
        }

        public Style TextBlockStyle
        {
            get { return (Style)GetValue(TextBlockStyleProperty); }
            set { SetValue(TextBlockStyleProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size baseSize = base.MeasureOverride(availableSize);

            if (mainTextBlock != null)
            {
                string text = mainTextBlock.Text;
                char separator = DEFAULTCHARSEPARATOR;

                if (text.Contains(" "))
                {
                    separator = ' ';
                }
                if (text.Contains("."))
                {
                    separator = '.';
                }
                if (separator != DEFAULTCHARSEPARATOR)
                {
                    //find all combinations how the sentence could be split into 2 lines
                    List<string[]> allcombinations = GetAllLinesCombinations(text, separator);

                    double bestWidth = double.PositiveInfinity;
                    foreach (string[] combination in allcombinations)
                    {
                        //now find the max width of the combination
                        double maxwidth = GetMaxTextWidth(combination);

                        //but we want the smallest combination
                        if (maxwidth < bestWidth)
                        {
                            bestWidth = maxwidth;
                        }
                    }

                    return new Size(bestWidth, GetLineHeight(mainTextBlock.Text) * 2);
                }                
            }

            return baseSize;
        }

        private double GetLineHeight(string teststring)
        {
            TextBlock b = GetCopyOfMainTextBlock();
            b.Text = "teststring";
            b.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            return b.DesiredSize.Height;
        }

        private TextBlock GetCopyOfMainTextBlock()
        {
            TextBlock b = new TextBlock();
            b.FontFamily = mainTextBlock.FontFamily;
            b.FontSize = mainTextBlock.FontSize;
            b.FontStyle = mainTextBlock.FontStyle;
            b.LineHeight = mainTextBlock.LineHeight;
            b.LineStackingStrategy = mainTextBlock.LineStackingStrategy;
            b.FontWeight = mainTextBlock.FontWeight;
            return b;
        }

        private double GetMaxTextWidth(string[] combination)
        {
            double longestLineWidth = 0.0;
            foreach (string line in combination)
            {
                double lineWidth = GetLineWidth(line);
                if (lineWidth > longestLineWidth)
                {
                    longestLineWidth = lineWidth;
                }
            }
            return longestLineWidth;
        }

        private double GetLineWidth(string line)
        {
            TextBlock b = GetCopyOfMainTextBlock();            
            b.Text = line;
            b.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            return b.DesiredSize.Width;
        }

        private List<string[]> GetAllLinesCombinations(string text, char separator)
        {
            // Number of site features
            List<string[]> combinations = new List<string[]>();
            //string[] allWords = text.Split(new char[] { ' ' });
            int startposition = 0;

            while (true)
            {
                int spacePosition = text.IndexOf(separator, startposition);
                if (spacePosition < 0)
                {
                    return combinations;
                }

                string firstPart = text.Substring(0, spacePosition);
                string secondPart = text.Substring(spacePosition);

                combinations.Add(new string[] { firstPart.Trim(), secondPart.Trim() });
                startposition = spacePosition+1;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            mainBorder.Width = finalSize.Width;
            mainBorder.Height = finalSize.Height;
            return base.ArrangeOverride(finalSize);
        }
    }
}
