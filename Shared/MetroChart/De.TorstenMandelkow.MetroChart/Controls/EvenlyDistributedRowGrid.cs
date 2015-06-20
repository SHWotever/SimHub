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

    public class EvenlyDistributedRowGrid : Panel
    {
        public EvenlyDistributedRowGrid()
        {
            // this.Background = new SolidColorBrush(Colors.Green);
        }

        public static readonly DependencyProperty IsReverseOrderProperty =
            DependencyProperty.Register("IsReverseOrder",
            typeof(bool),
            typeof(EvenlyDistributedRowGrid),
            new PropertyMetadata(false));      

        public bool IsReverseOrder
        {
            get { return (bool)GetValue(IsReverseOrderProperty); }
            set { SetValue(IsReverseOrderProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size returnedSize = availableSize;

            if (double.IsInfinity(availableSize.Height))
            {
                // ok, we have all the space we need, so we take it
                double maxColumnWidth = 0.0;
                double minColumnHeight = 0.0;
                foreach (UIElement child in Children)
                {
                    child.Measure(availableSize);
                    if (maxColumnWidth < child.DesiredSize.Width)
                    {
                        maxColumnWidth = child.DesiredSize.Width;
                    }
                    if (minColumnHeight < child.DesiredSize.Height)
                    {
                        minColumnHeight = child.DesiredSize.Height;
                    }
                }

                returnedSize.Width = maxColumnWidth;
                returnedSize.Height = minColumnHeight * Children.Count;
                return returnedSize;
            }
            else
            {
                // oh no, the height is limited, so we can only take this height
                double spaceForHeight = availableSize.Height / Children.Count;

                double maxColumnWidth = 0.0;
                double minColumnHeight = 0.0;
                foreach (UIElement child in Children)
                {
                    child.Measure(new Size(availableSize.Width, spaceForHeight));
                    if (maxColumnWidth < child.DesiredSize.Width)
                    {
                        maxColumnWidth = child.DesiredSize.Width;
                    }
                    if (minColumnHeight < child.DesiredSize.Height)
                    {
                        minColumnHeight = child.DesiredSize.Height;
                    }
                }

                returnedSize.Width = maxColumnWidth;
                returnedSize.Height = minColumnHeight * Children.Count;
                return returnedSize;
            }
        }

        private double GetLargestElementWidth(Size availableSize)
        {
            double minimalWidth = 0.0;
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
                if (child.DesiredSize.Width > minimalWidth)
                {
                    minimalWidth = child.DesiredSize.Width;
                }
            }
            return minimalWidth;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Size cellSize = new Size(finalSize.Width, finalSize.Height / Children.Count);
            int row = 0, col = 0;
            double reverseStartPoint = finalSize.Height - cellSize.Height;
            foreach (UIElement child in Children)
            {
                if (IsReverseOrder)
                {
                    child.Arrange(new Rect(new Point(cellSize.Width * col, reverseStartPoint - cellSize.Height * row), cellSize));
                }
                else
                {
                    child.Arrange(new Rect(new Point(cellSize.Width * col, cellSize.Height * row), cellSize));
                }
                row++;
            }

            /*
            if (minimalWidth > 0.0)
            {
                if (this.Width != minimalWidth)
                {
                    this.Width = minimalWidth;
                }
            }
            */

            return new Size(finalSize.Width, finalSize.Height);
        }
    }
}
