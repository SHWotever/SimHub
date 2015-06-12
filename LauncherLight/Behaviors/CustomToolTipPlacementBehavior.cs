namespace LauncherLight.Behaviors
{
    using LauncherLight.Enums;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Interactivity;

    public class CustomToolTipPlacementBehavior : Behavior<FrameworkElement>
    {
        #region Public Properties

        public HorizontalPlacement HorizontalPlacement { get; set; }

        public VerticalPlacement VerticalPlacement { get; set; }

        #endregion Public Properties

        #region Overridden Methods

        protected override void OnAttached()
        {
            base.OnAttached();

            var toolTip = this.AssociatedObject.ToolTip as ToolTip;
            if (toolTip != null)
            {
                toolTip.CustomPopupPlacementCallback = CalculatePopupPlacement;
            }
        }

        #endregion Overridden Methods

        #region Private Methods

        protected virtual CustomPopupPlacement[] CalculatePopupPlacement(Size popupSize, Size targetSize, Point offset)
        {
            var verticalOffset = 0.0;
            GetVerticalOffset(VerticalPlacement, popupSize.Height, targetSize.Height, offset.Y);

            var horizontalOffset =
                    GetHorizontalOffset(HorizontalPlacement, popupSize.Width, targetSize.Width, offset.X);

            var placement1 =
                    new CustomPopupPlacement(new Point(horizontalOffset, verticalOffset), PopupPrimaryAxis.Horizontal);

            return new[] { placement1 };
        }

        private static double GetHorizontalOffset(HorizontalPlacement horizontalPlacement, double popupWidth, double targetWidth, double offsetWidth)
        {
            var horizontalOffset = offsetWidth;
            switch (horizontalPlacement)
            {
                case HorizontalPlacement.Left:
                    horizontalOffset += -popupWidth;
                    break;

                case HorizontalPlacement.Center:
                    horizontalOffset += targetWidth / 2 - popupWidth / 2;
                    break;

                case HorizontalPlacement.Right:
                    horizontalOffset += targetWidth;
                    break;

                default:
                    throw new Exception("Invalid Vertical Placement");
            }

            return horizontalOffset;
        }

        private static double GetVerticalOffset(VerticalPlacement verticalPlacement, double popupHeight, double targetHeight, double offsetHeight)
        {
            var verticalOffset = offsetHeight;

            switch (verticalPlacement)
            {
                case VerticalPlacement.Top:
                    verticalOffset += -popupHeight;
                    break;

                case VerticalPlacement.Center:
                    verticalOffset += targetHeight / 2 - popupHeight / 2;
                    break;

                case VerticalPlacement.Bottom:
                    verticalOffset += targetHeight;
                    break;

                default:
                    throw new Exception("Invalid Vertical Placement");
            }

            return verticalOffset;
        }

        #endregion Private Methods
    }
}