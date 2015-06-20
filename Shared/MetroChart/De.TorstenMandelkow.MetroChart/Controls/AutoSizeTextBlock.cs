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
    public class AutoSizeTextBlock : Control
#else
    public class AutoSizeTextBlock : Control
#endif
    {
        public static readonly DependencyProperty IsHeightExceedsSpaceProperty =
            DependencyProperty.Register("IsHeightExceedsSpace", typeof(bool), typeof(AutoSizeTextBlock),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsWidthExceedsSpaceProperty =
            DependencyProperty.Register("IsWidthExceedsSpace", typeof(bool), typeof(AutoSizeTextBlock),
            new PropertyMetadata(false));

        public static readonly DependencyProperty TextBlockStyleProperty =
            DependencyProperty.Register("TextBlockStyle", typeof(Style), typeof(AutoSizeTextBlock),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AutoSizeTextBlock),
            new PropertyMetadata(null));

        static AutoSizeTextBlock()
        {
#if NETFX_CORE
            //do nothing
#elif SILVERLIGHT
            //do nothing
#else
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoSizeTextBlock), new FrameworkPropertyMetadata(typeof(AutoSizeTextBlock))); 
#endif
        }

        public AutoSizeTextBlock()
        {
#if NETFX_CORE
            this.DefaultStyleKey = typeof(AutoSizeTextBlock);
#elif SILVERLIGHT
            this.DefaultStyleKey = typeof(AutoSizeTextBlock);
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
        double initialheight = 0.0;
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

        public bool IsHeightExceedsSpace
        {
            get { return (bool)GetValue(IsHeightExceedsSpaceProperty); }
            set { SetValue(IsHeightExceedsSpaceProperty, value); }
        }

        public bool IsWidthExceedsSpace
        {
            get { return (bool)GetValue(IsWidthExceedsSpaceProperty); }
            set { SetValue(IsWidthExceedsSpaceProperty, value); }
        }
                
        protected override Size MeasureOverride(Size availableSize)
        {
            Size returnedSize = new Size(0,0); //we do not need space
            mainTextBlock.Measure(new Size(double.MaxValue, double.MaxValue));

            if (double.IsInfinity(availableSize.Height) || (availableSize.Height > mainTextBlock.DesiredSize.Height))
            {      
                // there is enough space, we return our minimum space
                returnedSize.Height = mainTextBlock.DesiredSize.Height;
            }

            if (double.IsInfinity(availableSize.Width)  || (availableSize.Width > mainTextBlock.DesiredSize.Width))
            {
                // there is enough space, we return our minimum space
                returnedSize.Width = mainTextBlock.DesiredSize.Width;
            }
                       
            return returnedSize;
           
            /*
            mainTextBlock.Visibility = Visibility.Collapsed;
            if (availableSize.Height < initialheight)
            {
                //if the is not enough height for the text, 
                //return new Size(0, availableSize.Height);
            }
            else
            {
                //return new Size(0, initialheight);
            }

            return new Size(0, 0);
           // Size baseSize = base.MeasureOverride(availableSize);
           // return baseSize;
             * */
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

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (mainTextBlock.ActualHeight > 0.0)
            {
                if ((mainTextBlock.ActualHeight > finalSize.Height) || (mainTextBlock.ActualWidth > finalSize.Width))
                {
                    IsHeightExceedsSpace = true;
                    this.Opacity = 0;
                }
                else
                {
                    IsHeightExceedsSpace = false;
                    this.Opacity = 1;
                }
            }

            if (mainTextBlock.ActualWidth > 0.0)
            {
                if ((mainTextBlock.ActualHeight > finalSize.Height) || (mainTextBlock.ActualWidth > finalSize.Width))
                {
                    IsWidthExceedsSpace = true;
                    this.Opacity = 0;
                }
                else
                {
                    IsWidthExceedsSpace = false;
                    this.Opacity = 1;
                }
            }
            /*
            //mainTextBlock.MaxWidth = finalSize.Width;
            //mainTextBlock.Visibility = Visibility.Visible;
            TextBlock tempBlock = GetCopyOfMainTextBlock();
            tempBlock.Text = mainTextBlock.Text;
            tempBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            double currentWidth = tempBlock.DesiredSize.Width;

            if (tempBlock.DesiredSize.Height > finalSize.Height)
            {
                IsHeightExceedsSpace = true;
                this.Opacity = 0;
            }
            else
            {
                IsHeightExceedsSpace = false;
                this.Opacity = 1;
            }

            //is textblock larger than the available width, then we scale it down
            if (currentWidth > finalSize.Width)
            {
                
            }
            else
            {
                mainTextBlock.Visibility = Visibility.Visible;
            }

            mainBorder.Width = finalSize.Width;
            mainBorder.Height = finalSize.Height;
             * */
            return base.ArrangeOverride(finalSize);
        }
    }
}
