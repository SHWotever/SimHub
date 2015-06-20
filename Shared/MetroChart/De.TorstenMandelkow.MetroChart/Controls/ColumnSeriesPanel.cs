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

    public class ColumnSeriesPanel : Panel
    {
        private bool arrangeItems = false;
        private int numberOfRows = 0;
        private double maxElementHeightInRow1 = 0.0;
        private double maxElementHeightInRow2 = 0.0;
        private double minimalColumnWidth = 0.0;

        protected override Size MeasureOverride(Size availableSize)
        {
            if (double.IsInfinity(availableSize.Width))
            {
                // ok we have enough space, we return our minimum required space (2 columns, max. 2 lines per text
                Size optimalMinimumSize = new Size(0, 0);// GetOptimalMinimumSize();
                numberOfRows = 2;

                Size returnedSize = availableSize;
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

                // ok, we place the elements in 2 rows
                double widthOfPanelInOneRow = maxColumnWidth * Children.Count;
                minimalColumnWidth = maxColumnWidth / 2 + maxColumnWidth / 4;

                returnedSize.Width = minimalColumnWidth * Children.Count;
                returnedSize.Height = minColumnHeight * 2; // 2 because we have two rows

                maxElementHeightInRow1 = minColumnHeight;
                maxElementHeightInRow2 = minColumnHeight;

                return returnedSize;                
            }
            else
            {
                // ok, we have fixed size, so we can recalculate all

                // check, if all texts have enough space to be placed in a single row, if not, switch to 2 rows
                // number of rows for the given width
                numberOfRows = GetNumberOfRows(availableSize);

                // space for each column depending on the number of rows
                double columnwidth = GetColumnWidth(availableSize, numberOfRows);

                // get max height in each row, includes measurement for each child
                maxElementHeightInRow1 = GetHighestElementInRow(columnwidth, numberOfRows, 1);

                maxElementHeightInRow2 = GetHighestElementInRow(columnwidth, numberOfRows, 2);
                
                return new Size(availableSize.Width, maxElementHeightInRow1 + maxElementHeightInRow2);
            }
        }
        /*
        private Size GetOptimalMinimumSize()
        {
            // optimum: if needed we use 2 rows
            // get the minimum size of each textbox with maximum 2 lines

        }
        */
        protected override Size ArrangeOverride(Size finalSize)
        {
            double columnwidth = GetColumnWidth(finalSize, numberOfRows);

            // sometimes we get more space that we need, so maybe we can switch back to 1 row
            if (numberOfRows == 2)
            {
                if ((minimalColumnWidth * Children.Count) < finalSize.Width)
                {
                    numberOfRows = 1;
                    columnwidth = GetColumnWidth(finalSize, numberOfRows);
                }
            }

                // now we can place the items
                // to calculate the middle points for reach cell we need the normal width of them
                double normalCellWidth = finalSize.Width / Children.Count;
                
                int row = 0; int col = 0;
                foreach (UIElement child in Children)
                {
                    // now find out the height of each row depending on the width of the columns (some text need 2 lines, some only 1 line
                    double middlePointX = normalCellWidth * col + normalCellWidth / 2;
                    double leftStartPoint = middlePointX - columnwidth / 2.0;

                    if (numberOfRows == 1)
                    {
                        child.Arrange(new Rect(new Point(leftStartPoint, maxElementHeightInRow1 * row), new Size(columnwidth, maxElementHeightInRow1)));
                    }
                    else
                    {
                        // problem with alternating row, y start point is height of first row, but height of element is height of 2nd row
                        if (row == 0)
                        {
                            child.Arrange(new Rect(new Point(leftStartPoint, maxElementHeightInRow1 * row), new Size(columnwidth, maxElementHeightInRow1)));
                        }
                        else
                        {
                            child.Arrange(new Rect(new Point(leftStartPoint, maxElementHeightInRow1 * row), new Size(columnwidth, maxElementHeightInRow2)));
                        }
                    }

                    // alternate the rows
                    if (numberOfRows == 2)
                    {
                        if (row == 0)
                        {
                            row = 1;
                        }
                        else
                        {
                            row = 0;
                        }
                    }
                    col++;
                }
           
            /*
            else
            {
                //ok, now we know the width, setting the width forces a second measure override
                this.Width = finalSize.Width;
            }
            */
            return new Size(finalSize.Width, finalSize.Height);
        }

        private double GetHighestElementInRow(double columnWidth, int numberOfRows, int currentRow)
        {
            if (numberOfRows == 1)
            {
                if (currentRow == 1)
                {
                    // measure a childs depending on the width of each column
                    // here we see if all text have space in one line or 2 lines
                    double highestElementHeight = 0.0;
                    foreach (UIElement child in Children)
                    {
                        child.Measure(new Size(columnWidth, double.MaxValue));
                        if (child.DesiredSize.Height > highestElementHeight)
                        {
                            highestElementHeight = child.DesiredSize.Height;
                        }
                    }
                    return highestElementHeight;
                }
                else
                {
                    return 0.0; // 2nd row is empty 
                }
            }
            else
            {
                // we have 2 rows
                double highestElementHeight = 0.0;
                int row = 1;
                foreach (UIElement child in Children)
                {
                    if (row == currentRow)
                    {
                        child.Measure(new Size(columnWidth, double.MaxValue));
                        if (child.DesiredSize.Height > highestElementHeight)
                        {
                            highestElementHeight = child.DesiredSize.Height;
                        }
                    }

                    if (row == 1)
                    {
                        row = 2;                        
                    }
                    else
                    {
                        row = 1;                        
                    }                    
                }
                return highestElementHeight;
            }
        }

        private double GetColumnWidth(Size finalSize, int numberOfRows)
        {
            // return the width of each column for the given number of rows
            double cellWidth = finalSize.Width / Children.Count;
            if (numberOfRows == 1)
            {
                return cellWidth;
            }

            return cellWidth + cellWidth / 2.0;
        }

        private int GetNumberOfRows(Size finalSize)
        {
            int numberOfRows = 1;
            double cellwidth = finalSize.Width / Children.Count;

            // we try to find out if each text can be displayed in a single line
            // so we compare the height for an infinitee available width and for a limited with
            foreach (UIElement child in Children)
            {
                child.Measure(new Size(double.MaxValue, double.MaxValue));
                Size inOneLine = child.DesiredSize;

                child.Measure(new Size(cellwidth, double.MaxValue));
                Size inInCell = child.DesiredSize;

                if (inInCell.Height != inOneLine.Height)
                {
                    // ok, one text needs more space, we display in two rows
                    numberOfRows = 2;
                }
            }

            return numberOfRows;
        }       
    }
}
