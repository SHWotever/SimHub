using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using LauncherLight.Enums;

namespace LauncherLight.Behaviors
{
    public class CustomToolTipPrioritizedPlacementBehavior : CustomToolTipPlacementBehavior
    {
        protected override CustomPopupPlacement[] CalculatePopupPlacement(Size popupSize, Size targetSize, Point offset)
        {
            var verticalOffsets = GetVerticalOffsets(VerticalPlacement, popupSize.Height, targetSize.Height, offset.Y);
            var horizontalOffsets = GetHorizontalOffsets(HorizontalPlacement, popupSize.Width, targetSize.Width, offset.X);

            var placement1 = new CustomPopupPlacement(new Point(horizontalOffsets[0], verticalOffsets[0]), PopupPrimaryAxis.Vertical);
            var placement2 = new CustomPopupPlacement(new Point(horizontalOffsets[1], verticalOffsets[1]), PopupPrimaryAxis.Horizontal);

            return new[] { placement1, placement2 };
        }

        private static double[] GetHorizontalOffsets(HorizontalPlacement horizontalPlacement, double popupWidth, double targetWidth, double offsetWidth)
        {
            var horizontalOffsets = Enumerable.Repeat<double>(offsetWidth, 2).ToArray();
            switch (horizontalPlacement)
            {
                case HorizontalPlacement.Left:
                    horizontalOffsets[0] += -popupWidth;
                    horizontalOffsets[1] += targetWidth;
                    break;

                case HorizontalPlacement.Center:
                    horizontalOffsets[0] += targetWidth / 2 - popupWidth / 2;
                    horizontalOffsets[1] = horizontalOffsets[0];
                    break;

                case HorizontalPlacement.Right:
                    horizontalOffsets[0] += targetWidth;
                    horizontalOffsets[1] += -popupWidth;
                    break;

                default:
                    throw new Exception("Invalid Vertical Placement");
            }

            return horizontalOffsets;
        }

        private static double[] GetVerticalOffsets(VerticalPlacement verticalPlacement, double popupHeight, double targetHeight, double offsetHeight)
        {
            var verticalOffsets = Enumerable.Repeat<double>(offsetHeight, 2).ToArray();

            switch (verticalPlacement)
            {
                case VerticalPlacement.Top:
                    verticalOffsets[0] += -popupHeight;
                    verticalOffsets[1] += targetHeight;
                    break;

                case VerticalPlacement.Center:
                    verticalOffsets[0] += targetHeight / 2 - popupHeight / 2;
                    verticalOffsets[1] += verticalOffsets[0];
                    break;

                case VerticalPlacement.Bottom:
                    verticalOffsets[0] += targetHeight;
                    verticalOffsets[1] += -popupHeight;
                    break;

                default:
                    throw new Exception("Invalid Vertical Placement");
            }

            return verticalOffsets;
        }
    }
}