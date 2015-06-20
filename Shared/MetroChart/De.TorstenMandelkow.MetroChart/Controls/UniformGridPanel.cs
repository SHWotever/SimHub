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
    public class UniformGridPanel : Panel
#else
    public class UniformGridPanel : Grid
#endif
    {
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        public double MinimalGridWidth
        {
            get { return (double)GetValue(MinimalGridWidthProperty); }
            set { SetValue(MinimalGridWidthProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
          DependencyProperty.Register("Orientation",
          typeof(Orientation), typeof(UniformGridPanel), new PropertyMetadata(Orientation.Horizontal, null));
        public static readonly DependencyProperty MinimalGridWidthProperty =
          DependencyProperty.Register("MinimalGridWidth",
          typeof(double), typeof(UniformGridPanel), new PropertyMetadata(100.0));

        private int cols = 0;
        private int rows = 0;
        Size squaresize = new Size(0.0, 0.0);

        protected override Size MeasureOverride(Size availableSize)
        {
            return RecalcRowsAndCols(availableSize, true);
        }

        private Size RecalcRowsAndCols(Size availableSize, bool withMeasure)
        {
            if (Children.Count == 0)
            {
                rows = 0;
                cols = 0;
                return new Size(0, 0);
            }

            // we need to calc the size of a tile
            double gridMinimalWidth = MinimalGridWidth;
            double smallestSize = 0.0;

            foreach (UIElement child in Children)
            {
                if (withMeasure) // only measure when called from MeasureOverride to prevent layout cycle
                {
                    child.Measure(availableSize);
                }
                if (child.DesiredSize.Width > gridMinimalWidth)
                {
                    gridMinimalWidth = child.DesiredSize.Width;
                }
                if (child.DesiredSize.Height > gridMinimalWidth)
                {
                    gridMinimalWidth = child.DesiredSize.Height;
                }
                if (child.DesiredSize.Width > smallestSize)
                {
                    smallestSize = child.DesiredSize.Width;
                }
                if (child.DesiredSize.Height > smallestSize)
                {
                    smallestSize = child.DesiredSize.Height;
                }
            }

            if (Children.Count == 1)
            {
                cols = 1;
                rows = 1;
                return new Size(smallestSize, smallestSize);
            }
          
            //ok, we try to place the childs elements in a nice grid and try to use the available space

            // 1. do we have all space in all directions, then we create a rectangle with rows equals cols
            if(double.IsInfinity(availableSize.Width) && double.IsInfinity(availableSize.Height) )
            {
                double squareRoot = Math.Sqrt(Children.Count);
                rows = (int)Math.Ceiling(squareRoot);
                cols = (int)Math.Ceiling(squareRoot);
                squaresize.Width = gridMinimalWidth;
                squaresize.Height = gridMinimalWidth;

                //we use a uniform grid with same width and height
                // e.g. 8 children = 3 x 3
                // e.g. 4 = 2 x 2 
            }
            else if (double.IsInfinity(availableSize.Width))
            {
                // ok, width is infinite, height is limited
                // we try to fit as much elements in height

                // is height enough for 1 item
                double assumeMinimalGridSize = gridMinimalWidth;

                // it there enough height to place at least one item?
                if (availableSize.Height < assumeMinimalGridSize)
                {
                    // no, height is not enough, so we use the available height for our square
                    assumeMinimalGridSize = availableSize.Height;
                }

                // try to fit the elements into the space
                rows = (int)Math.Floor(availableSize.Height / gridMinimalWidth);
                if (rows == 0)
                {
                    rows = 1;
                }

                cols = (int)Math.Ceiling((Children.Count * 1.0) / (rows * 1.0));
                                
                squaresize.Width = availableSize.Height / rows;
                squaresize.Height = availableSize.Height / rows;
                
            }
            else if (double.IsInfinity(availableSize.Height))
            {
                // ok, height is infinite, width is limited

                // ok, width is infinite, height is limited
                // we try to fit as much elements in height

                // is height enough for 1 item
                double assumeMinimalGridSize = gridMinimalWidth;

                // it there enough height to place at least one item?
                if (availableSize.Width < assumeMinimalGridSize)
                {
                    // no, height is not enough, so we use the available height for our square
                    assumeMinimalGridSize = availableSize.Width;
                }

                // try to fit the elements into the space
                cols = (int)Math.Floor(availableSize.Width / gridMinimalWidth);
                if (cols == 0)
                {
                    cols = 1;
                } 
                
                rows = (int)Math.Ceiling((Children.Count * 1.0) / (cols * 1.0));
                               
                squaresize.Width = availableSize.Width / cols;
                squaresize.Height = availableSize.Width / cols;

                //is there more space in wi
            }
            else
            {
                // size is strict, we fill the available space
                // critical part
                // find the maximum square size which can be placed inside the available space
                // 1. is there enough space for all Elements??
                double squareFootageAvailable = availableSize.Width * availableSize.Height;
                double squareFootageChildren = Children.Count * (gridMinimalWidth * gridMinimalWidth);
                /*
                if (squareFootageAvailable >= squareFootageChildren)
                {
                    // ok, we start with the current size and find the best fit of rows and cols
                }
                else
                {
                    */
                    // ok, not enough space for all children, we need to scale the items
                    // so we need to find the largest square which can be place inside the area
                    // we start with placing all children in one row and go down to place children in one column
                    // between these combination lies the truth
                    squaresize = new Size(0.0, 0.0); // we start with the smallest size
                    int testcols = 0;
                    for (int testrows = 1; testrows <= Children.Count; testrows++)
                    {
                        testcols = (int)Math.Ceiling((Children.Count * 1.0) / (testrows * 1.0));

                        double _squarewidth = availableSize.Width / testcols;
                        double _squareheight = availableSize.Height / testrows;
                        double _squareSize = Math.Min(_squarewidth, _squareheight);

                        if (_squareSize >= squaresize.Width || _squareSize >= squaresize.Height)
                        {
                            squaresize = new Size(_squareSize, _squareSize);
                            rows = testrows;
                            cols = testcols;
                        }
                    }
                //}
            }

            //stretch it to availableSize            
            Size minimalSize = new Size(cols * squaresize.Width, rows * squaresize.Height);
            return minimalSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            // we need to recalculate the layout because maybe we get finally more space then expected (then we will fill it)
            if (Children.Count == 0)
            {
                return new Size(0.0, 0.0);
            }

            if (Children.Count == 1)
            {
                Rect rect = new Rect(0, 0, finalSize.Width, finalSize.Height);
                Children[0].Arrange(rect);
                return finalSize;
            }

            Size result = RecalcRowsAndCols(finalSize, false);

            //do we have more space???
            if (cols > 0)
            {
                if (finalSize.Width > (squaresize.Width * cols))
                {
                    squaresize.Width = finalSize.Width / cols;
                }
            }
            if (rows > 0)
            {
                if (finalSize.Height > (squaresize.Height * rows))
                {
                    squaresize.Height = finalSize.Height / rows;
                }
            }

            int currentrow = 0;
            int currentcol = 0;
            for (int i = 0; i < Children.Count; i++)
            {
                double left = currentcol * squaresize.Width;
                double top = currentrow * squaresize.Height;
                Rect rect = new Rect(left, top, squaresize.Width, squaresize.Height);
                Children[i].Arrange(rect);

                currentcol++;

                if (currentcol == cols)
                {
                    currentcol = 0;
                    currentrow++;
                }
            }

            return finalSize;
        }
    }
}
