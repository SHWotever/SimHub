namespace De.TorstenMandelkow.MetroChart
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Windows;  
    using System.Reflection;
    using System.Collections.Specialized;
    using System.Windows.Input;

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
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    
#endif

    public class PiePiece : PieceBase
    {
        #region Fields

        private Path slice;
        private Border label = null;
                
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(PiePiece),
            new PropertyMetadata(0.0));
        
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(PiePiece),
            new PropertyMetadata(0.0, new PropertyChangedCallback(UpdatePie)));
        
        public static readonly DependencyProperty SumOfDataSeriesProperty =
            DependencyProperty.Register("SumOfDataSeries", typeof(double), typeof(PiePiece),
            new PropertyMetadata(0.0, new PropertyChangedCallback(UpdatePie)));
        
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(PiePiece),
            new PropertyMetadata(0.0, new PropertyChangedCallback(UpdatePie)));
        
        public static readonly DependencyProperty StartValueProperty =
            DependencyProperty.Register("StartValue", typeof(double), typeof(PiePiece),
            new PropertyMetadata(0.0, new PropertyChangedCallback(UpdatePie)));
        
        public static readonly DependencyProperty DoughnutInnerRadiusRatioProperty =
            DependencyProperty.Register("DoughnutInnerRadiusRatio", typeof(double), typeof(PiePiece),
            new PropertyMetadata(0.0, OnDoughnutInnerRadiusRatioChanged));
        
        public static readonly DependencyProperty GeometryProperty =
            DependencyProperty.Register("Geometry", typeof(Geometry), typeof(PiePiece),
            new PropertyMetadata(null));
        
        public static readonly DependencyProperty SelectionGeometryProperty =
            DependencyProperty.Register("SelectionGeometry", typeof(Geometry), typeof(PiePiece),
            new PropertyMetadata(null));
        
        public static readonly DependencyProperty MouseOverGeometryProperty =
            DependencyProperty.Register("MouseOverGeometry", typeof(Geometry), typeof(PiePiece),
            new PropertyMetadata(null));
        
        public static readonly DependencyProperty LineGeometryProperty =
            DependencyProperty.Register("LineGeometry", typeof(Geometry), typeof(PiePiece),
            new PropertyMetadata(null));
        
        public static readonly DependencyProperty LabelXPosProperty =
            DependencyProperty.Register("LabelXPos", typeof(double), typeof(PiePiece),
            new PropertyMetadata(10.0));
        
        public static readonly DependencyProperty LabelYPosProperty =
            DependencyProperty.Register("LabelYPos", typeof(double), typeof(PiePiece),
            new PropertyMetadata(10.0));

        #endregion Fields

        #region Constructors

        static PiePiece()        
        {
#if NETFX_CORE
                        
#elif SILVERLIGHT
    
#else
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PiePiece), new FrameworkPropertyMetadata(typeof(PiePiece)));
#endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PiePiece"/> class.
        /// </summary>
        public PiePiece()
        {
#if NETFX_CORE
            this.DefaultStyleKey = typeof(PiePiece);
#endif
#if SILVERLIGHT
            this.DefaultStyleKey = typeof(PiePiece);
#endif
            Loaded += PiePiece_Loaded;
        }

        #endregion Constructors
 
        #region Properties

        public double DoughnutInnerRadiusRatio
        {
            get { return (double)GetValue(DoughnutInnerRadiusRatioProperty); }
            set { SetValue(DoughnutInnerRadiusRatioProperty, value); }
        }        

        public double LabelXPos
        {
            get { return (double)GetValue(LabelXPosProperty); }
            set { SetValue(LabelXPosProperty, value); }
        }
        public double LabelYPos
        {
            get { return (double)GetValue(LabelYPosProperty); }
            set { SetValue(LabelYPosProperty, value); }
        }

        public Geometry LineGeometry
        {
            get { return (Geometry)GetValue(LineGeometryProperty); }
            set { SetValue(LineGeometryProperty, value); }
        }
        
        public Geometry Geometry
        {
            get { return (Geometry)GetValue(GeometryProperty); }
            set { SetValue(GeometryProperty, value); }
        }

        public Geometry SelectionGeometry
        {
            get { return (Geometry)GetValue(SelectionGeometryProperty); }
            set { SetValue(SelectionGeometryProperty, value); }
        }

        public Geometry MouseOverGeometry
        {
            get { return (Geometry)GetValue(MouseOverGeometryProperty); }
            set { SetValue(MouseOverGeometryProperty, value); }
        }

        /// <summary>
        /// The value that this pie piece represents.
        /// </summary>
        public double SumOfDataSeries
        {
            get { return (double)GetValue(SumOfDataSeriesProperty); }
            set { SetValue(SumOfDataSeriesProperty, value); }
        }

        /// <summary>
        /// The value that this pie piece represents.
        /// </summary>
        public double StartValue
        {
            get { return (double)GetValue(StartValueProperty); }
            set { SetValue(StartValueProperty, value); }
        }

        /// <summary>
        /// The value that this pie piece represents.
        /// </summary>
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        /// <summary>
        /// The value that this pie piece represents.
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the percent.
        /// </summary>
        /// <value>The percent.</value>
        public double Percent
        {
            get
            {
                if (SumOfDataSeries > 0.0)
                {
                    return (Value / SumOfDataSeries) * 100;
                }
                return 0.0;
            }
        }

        public bool IsDoughnut
        {
            get
            {
                if (this.ParentChart is DoughnutChart)
                {
                    return true;
                }
                return false;
            }
        }
        
        #endregion Properties

        #region Methods

        private static void UpdatePie(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PiePiece).DrawGeometry();
        }
        
        private static void OnDoughnutInnerRadiusRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PiePiece).DrawGeometry();
        }

        void PiePiece_Loaded(object sender, RoutedEventArgs e)
        {
            DrawGeometry();
        }

        protected override void InternalOnApplyTemplate()
        {
            label = this.GetTemplateChild("PART_Label") as Border;
            slice = this.GetTemplateChild("Slice") as Path;
            RegisterMouseEvents(slice);
        }

        protected override void DrawGeometry(bool withAnimation = true)
        {    
            try
            {
                if (this.ClientWidth <= 0.0)
                {
                    return;
                }
                if (this.ClientHeight <= 0.0)
                {
                    return;
                }
                if (SumOfDataSeries > 0)
                {
                    double sss = this.ActualWidth;

                    double x = this.ClientWidth;
                    double m_startpercent = StartValue / SumOfDataSeries * 100;
                    double m_endpercent = (StartValue + Value) / SumOfDataSeries * 100;

                    Point center = GetCenter();

                    double startAngle = (360 / 100.0) * m_startpercent;
                    double endAngle = (360 / 100.0) * m_endpercent;
                    double radius = GetRadius();
                    bool isLarge = (endAngle - startAngle) > 180.0;

                    LayoutSegment(startAngle, endAngle, radius, this.DoughnutInnerRadiusRatio, center, this.IsDoughnut);
                }
            }
            catch (Exception ex)
            {
            }
        }

         private Point GetCircumferencePoint(double angle, double radius, double centerx, double centery)
         {
             angle = angle - 90;

             double angleRad = (Math.PI / 180.0) * angle;

             double x = centerx + radius * Math.Cos(angleRad);
             double y = centery + radius * Math.Sin(angleRad);

             return new Point(x, y);
         }

        internal void LayoutSegment(double startAngle, double endAngle, double radius, double gapScale, Point center, bool isDoughnut)
        {
            try
            {                
                if (startAngle > 360)
                {
                    return;
                }
                if (endAngle > 360)
                {
                    return;
                }
                if ((startAngle == 0.0) && (endAngle == 0.0))
                {
                    return;
                }
                if (endAngle > 359.5)
                {
                    endAngle = 359.5;    //pie disappears if endAngle is 360                 
                }

                // Segment Geometry
                double pieRadius = radius;
                double gapRadius = pieRadius * ((gapScale == 0.0) ? 0.25 : gapScale);

                Point A = GetCircumferencePoint(startAngle, pieRadius, center.X, center.Y);
                Point B = isDoughnut ? GetCircumferencePoint(startAngle, gapRadius, center.X, center.Y) : center;
                Point C = GetCircumferencePoint(endAngle, gapRadius, center.X, center.Y);
                Point D = GetCircumferencePoint(endAngle, pieRadius, center.X, center.Y);

                bool isReflexAngle = Math.Abs(endAngle - startAngle) > 180.0;
                
                PathSegmentCollection segments = new PathSegmentCollection();
                segments.Add(new LineSegment() { Point = B });

                if (isDoughnut)
                {
                    segments.Add(new ArcSegment()
                    {
                        Size = new Size(gapRadius, gapRadius),
                        Point = C,
                        SweepDirection = SweepDirection.Clockwise,
                        IsLargeArc = isReflexAngle
                    });
                }

                segments.Add(new LineSegment() { Point = D });
                segments.Add(new ArcSegment()
                {
                    Size = new Size(pieRadius, pieRadius),
                    Point = A,
                    SweepDirection = SweepDirection.Counterclockwise,
                    IsLargeArc = isReflexAngle
                });
                
                Path segmentPath = new Path()
                {
                    StrokeLineJoin = PenLineJoin.Round,
                    Stroke = new SolidColorBrush() { Color = Colors.Black },
                    StrokeThickness = 0.0d,
                    Data = new PathGeometry()
                    {
                        Figures = new PathFigureCollection()
                        {
                            new PathFigure()
                            {
                                IsClosed = true,
                                StartPoint = A,
                                Segments = segments
                            }
                        }
                    }
                };
                SetValue(PiePiece.GeometryProperty, CloneDeep(segmentPath.Data as PathGeometry));
                SetValue(PiePiece.SelectionGeometryProperty, CloneDeep(segmentPath.Data as PathGeometry));


                double inRadius = radius * 0.65;
                double outRadius = radius * 1.25;

                double midAngle = startAngle + ((endAngle - startAngle) / 2.0);
                Point pointOnCircle = GetCircumferencePoint(midAngle, pieRadius, center.X, center.Y);

                //recalculate midangle if point is to close to top or lower border
                double distanceToCenter = Math.Abs(pointOnCircle.Y - center.Y);
                double factor = distanceToCenter / center.Y;

                double midAngleBefore = midAngle;
                if ((GetQuadrant(pointOnCircle, center) == 1) || (GetQuadrant(pointOnCircle, center) == 3))
                {   //point is in quadrant 1 center, we go further the end angle
                    midAngle = startAngle + ((endAngle - startAngle) / 2.0) + (((endAngle - startAngle) / 2.0) * factor);
                }
                else
                {
                    //point 
                    midAngle = startAngle + ((endAngle - startAngle) / 2.0) - (((endAngle - startAngle) / 2.0) * factor);
                }
                
                pointOnCircle = GetCircumferencePoint(midAngle, pieRadius, center.X, center.Y);

                Point pointOuterCircle = GetCircumferencePoint(midAngle, pieRadius + 10, center.X, center.Y);
                Point pointerMoreOuter = new Point(pointOuterCircle.X - 10, pointOuterCircle.Y);
                if (pointOnCircle.X > center.X)
                {
                    pointerMoreOuter = new Point(pointOuterCircle.X + 10, pointOuterCircle.Y);
                }

                PathSegmentCollection linesegments = new PathSegmentCollection();
                linesegments.Add(new LineSegment() { Point = pointOuterCircle });
                linesegments.Add(new LineSegment() { Point = pointerMoreOuter });

                Path linesegmentPath = new Path()
                {
                    StrokeLineJoin = PenLineJoin.Round,
                    Stroke = new SolidColorBrush() { Color = Colors.Black },
                    StrokeThickness = 2.0d,
                    Data = new PathGeometry()
                    {
                        Figures = new PathFigureCollection()
                        {
                            new PathFigure()
                            {
                                IsClosed = false,
                                StartPoint = pointOnCircle,
                                Segments = linesegments
                            }
                        }
                    }
                };
                SetValue(PiePiece.LineGeometryProperty, CloneDeep(linesegmentPath.Data as PathGeometry));
                /*
                label.Measure(new Size(420, 420));
                double labelwidth = label.DesiredSize.Width;
                double labelwidth = label.DesiredSize.Width;
                
                Size s = label.DesiredSize;
                double x = this.Value;
                */
                label.SetValue(Canvas.TopProperty, pointerMoreOuter.Y - (label.ActualHeight / 2.0));
                if (pointerMoreOuter.X > center.X)
                {
                    label.SetValue(Canvas.LeftProperty, pointerMoreOuter.X);
                }
                else
                {
                    label.SetValue(Canvas.LeftProperty, pointerMoreOuter.X - (label.ActualWidth));
                }                
            }
            catch (Exception ex)
            {
            }
        }

        private int GetQuadrant(Point pointOnCircle, Point center)
        {
            if (pointOnCircle.X > center.X)
            {
                if (pointOnCircle.Y > center.Y)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (pointOnCircle.Y > center.Y)
                {
                    return 3;
                }
                else
                {
                    return 4;
                }
            }
        }
        
        /// <summary>
        /// Gets the center.
        /// </summary>
        /// <returns></returns>
        private Point GetCenter()
        {
            return new Point(ClientWidth / 2, ClientHeight/2);
        }

        public PathGeometry CloneDeep(PathGeometry pathGeometry)
        {
            var newPathGeometry = new PathGeometry();
            foreach (var figure in pathGeometry.Figures)
            {
                var newFigure = new PathFigure();
                newFigure.StartPoint = figure.StartPoint;
                // Even figures have to be deep cloned. Assigning them directly will result in
                //  an InvalidOperationException being thrown with the message "Element is already the child of another element."
                foreach (var segment in figure.Segments)
                {
                    // I only impemented cloning the abstract PathSegments to one implementation, 
                    //  the PolyLineSegment class. If your paths use other kinds of segments, you'll need
                    //  to implement that kind of coding yourself.
                    var segmentAsPolyLineSegment = segment as PolyLineSegment;
                    if (segmentAsPolyLineSegment != null)
                    {
                        var newSegment = new PolyLineSegment();
                        foreach (var point in segmentAsPolyLineSegment.Points)
                        {
                            newSegment.Points.Add(point);
                        }
                        newFigure.Segments.Add(newSegment);
                    }

                    var segmentAsLineSegment = segment as LineSegment;
                    if (segmentAsLineSegment != null)
                    {
                        var newSegment = new LineSegment();
                        newSegment.Point = segmentAsLineSegment.Point;
                        newFigure.Segments.Add(newSegment);
                    }

                    var segmentAsArcSegment = segment as ArcSegment;
                    if (segmentAsArcSegment != null)
                    {
                        var newSegment = new ArcSegment();
                        newSegment.Point = segmentAsArcSegment.Point;
                        newSegment.SweepDirection = segmentAsArcSegment.SweepDirection;
                        newSegment.RotationAngle = segmentAsArcSegment.RotationAngle;
                        newSegment.IsLargeArc = segmentAsArcSegment.IsLargeArc;
                        newSegment.Size = segmentAsArcSegment.Size;
                        newFigure.Segments.Add(newSegment);
                    }
                }
                newPathGeometry.Figures.Add(newFigure);
            }
            return newPathGeometry;
        }

        /// <summary>
        /// Gets the radius.
        /// </summary>
        /// <returns></returns>
        private double GetRadius()
        {
            double result;
            if (ClientHeight < (ClientWidth - 50))
            {
                result = ClientHeight / 2;
            }
            else
            {
                result = (ClientWidth - 50) / 2;
            }

            return result - 10;
        }

        #endregion Methods
    }
}