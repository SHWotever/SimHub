using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ACDashboard.LcdLabel
{
    /// <summary>
    /// Enumerated type defining the different types of display available
    /// </summary>
    public enum DotMatrix
    {
        /// <summary>
        /// 5 pixels by 7 pixels matrix
        /// </summary>
        mat5x7,

        /// <summary>
        /// 5 pixels by 8 pixels matrix
        /// </summary>
        mat5x8,

        /// <summary>
        /// 7 pixels by 9 pixels matrix
        /// </summary>
        mat7x9,

        /// <summary>
        /// 9 pixels by 12 pixels matrix
        /// </summary>
        mat9x12,

        /// <summary>
        /// Hitachi style
        /// </summary>
        Hitachi,

        /// <summary>
        /// Hitachi 2 style
        /// </summary>
        Hitachi2,

        /// <summary>
        /// DOS style display
        /// </summary>
        dos5x7
    }

    /// <summary>
    /// Enumerated type defining the available LCD pixel sizes
    /// </summary>
    public enum PixelSize
    {
        /// <summary>
        /// 1 screen pixel is used for a LCD pixel
        /// </summary>
        pix1x1,

        /// <summary>
        /// 2 screen pixels are used for a LCD pixel
        /// </summary>
        pix2x2,

        /// <summary>
        /// 3 screen pixels are used for a LCD pixel
        /// </summary>
        pix3x3,

        /// <summary>
        /// 4 screen pixels are used for a LCD pixel
        /// </summary>
        pix4x4,

        /// <summary>
        /// 5 screen pixels are used for a LCD pixel
        /// </summary>
        pix5x5,

        /// <summary>
        /// 6 screen pixels are used for a LCD pixel
        /// </summary>
        pix6x6,

        /// <summary>
        /// 7 screen pixels are used for a LCD pixel
        /// </summary>
        pix7x7,

        /// <summary>
        /// 8 screen pixels are used for a LCD pixel
        /// </summary>
        pix8x8,

        /// <summary>
        /// 9 screen pixels are used for a LCD pixel
        /// </summary>
        pix9x9,

        /// <summary>
        /// 10 screen pixels are used for a LCD pixel
        /// </summary>
        pix10x10,

        /// <summary>
        /// 11 screen pixels are used for a LCD pixel
        /// </summary>
        pix11x11,

        /// <summary>
        /// 12 screen pixels are used for a LCD pixel
        /// </summary>
        pix12x12,

        /// <summary>
        /// 13 screen pixels are used for a LCD pixel
        /// </summary>
        pix13x13,

        /// <summary>
        /// 14 screen pixels are used for a LCD pixel
        /// </summary>
        pix14x14,

        /// <summary>
        /// 15 screen pixels are used for a LCD pixel
        /// </summary>
        pix15x15,

        /// <summary>
        /// 16 screen pixels are used for a LCD pixel
        /// </summary>
        pix16x16,

        /// <summary>
        /// A custom defined number of pixels are used for a LCD pixel
        /// </summary>
        pixCustom
    }

    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfApplication1"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfApplication1;assembly=WpfApplication1"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:LcdLabel/>
    ///
    /// </summary>
    public class LcdLabel : Control
    {
        static LcdLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LcdLabel), new FrameworkPropertyMetadata(typeof(LcdLabel)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LcdLabel"/> class.
        /// </summary>
        public LcdLabel()
            : base()
        {
            FWidth = 0;
            FHeight = 0;
            FCharSpacing = 2;
            FLineSpacing = 2;
            FPixelSpacing = 1;
            FBorderSpace = 3;
            FTextLines = 1;
            FNoOfChars = 10;
            FBorderColor = Colors.Black;
            FPixOnColor = Colors.Black;
            FPixOffColor = Color.FromArgb(0x00, 0xAA, 0xAA, 0xAA);
            FPixelSize = PixelSize.pix2x2;
            CalcHalfColor();             // Halftone On color
            CalcSize();                  // Get correct sizes at start
        }

        private PixelSize FPixelSize;

        /// <summary>
        /// Gets or sets the size of a LCD pixel (in screen pixels)
        /// </summary>
        /// <value>The size of the pixel.</value>
        public PixelSize PixelSize
        {
            get { return FPixelSize; }
            set
            {
                if (value != FPixelSize)
                {
                    FPixelSize = value;
                    InvalidateVisual();
                }
            }
        }

        private DotMatrix FDotMatrix = DotMatrix.mat5x7;

        /// <summary>
        /// Gets or sets the type of character matrix on the LCD
        /// </summary>
        /// <value>The type of character matrix on the LCD.</value>
        public DotMatrix DotMatrix
        {
            get
            {
                return FDotMatrix;
            }
            set
            {
                FDotMatrix = value;
                InvalidateVisual();
            }
        }

        private int FPixelSpacing;

        /// <summary>
        /// Gets or sets the space between each pixel in the matrix.
        /// </summary>
        /// <value>The pixel spacing.</value>
        public int PixelSpacing
        {
            get { return FPixelSpacing; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Pixel spacing can't be less than zero");
                else
                {
                    if (value != FPixelSpacing)
                    {
                        FPixelSpacing = value;
                        InvalidateVisual();
                    }
                }
            }
        }

        private int FCharSpacing;

        /// <summary>
        /// Gets or sets the space between the characters on the LCD
        /// </summary>
        /// <value>The character spacing.</value>
        public int CharSpacing
        {
            get { return FCharSpacing; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Character spacing can't be less than zero");
                else
                {
                    if (value != FCharSpacing)
                    {
                        FCharSpacing = value;
                        InvalidateVisual();
                    }
                }
            }
        }

        private int FLineSpacing;

        /// <summary>
        /// Gets or sets the space between text lines on the display
        /// </summary>
        /// <value>The line spacing.</value>
        public int LineSpacing
        {
            get { return FLineSpacing; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Line spacing can't be less than zero");
                else
                {
                    if (value != FLineSpacing)
                    {
                        FLineSpacing = value;
                        InvalidateVisual();
                    }
                }
            }
        }

        private int FBorderSpace;

        /// <summary>
        /// Gets or sets the distance from the LCD border.
        /// </summary>
        /// <value>The distance from the LCD border.</value>
        public int BorderSpace
        {
            get { return FBorderSpace; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Border spacing can't be less than zero");
                else
                {
                    if (value != FBorderSpace)
                    {
                        FBorderSpace = value;
                        InvalidateVisual();
                    }
                }
            }
        }

        private int FTextLines;

        /// <summary>
        /// Gets or sets the number of text lines on the LCD.
        /// </summary>
        /// <value>The number of text lines on the LCD.</value>
        public int TextLines
        {
            get { return FTextLines; }
            set
            {
                if (value < 1)
                    throw new ArgumentException("Display needs at least one line");
                else
                {
                    if (value != FTextLines)
                    {
                        FTextLines = value;
                        InvalidateVisual();
                    }
                }
            }
        }

        private int FNoOfChars;

        /// <summary>
        /// Gets or sets the number of characters on a single line.
        /// </summary>
        /// <value>The number of characters on a single line.</value>
        public int NumberOfCharacters
        {
            get { return FNoOfChars; }
            set
            {
                if (value < 1)
                    throw new ArgumentException("Display needs at least one character");
                else
                {
                    if (value != FNoOfChars)
                    {
                        FNoOfChars = value;
                        InvalidateVisual();
                    }
                }
            }
        }

        private Color FPixOnColor = Colors.Black;

        /// <summary>
        /// Gets or sets the pixel on colour.
        /// </summary>
        /// <value>The pixel on colour.</value>
        public Color PixelOn
        {
            get { return FPixOnColor; }
            set
            {
                if (value != FPixOnColor)
                {
                    FPixOnColor = value;
                    CalcHalfColor();
                    InvalidateVisual();
                }
            }
        }

        private Color FPixOffColor = Colors.Gray;

        /// <summary>
        /// Gets or sets the pixel off colour.
        /// </summary>
        /// <value>The pixel off colour.</value>
        public Color PixelOff
        {
            get { return FPixOffColor; }
            set
            {
                if (value != FPixOffColor)
                {
                    FPixOffColor = value;
                    InvalidateVisual();
                }
            }
        }

        private Color backgroundColor = Colors.Gray;

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                if (value != backgroundColor)
                {
                    backgroundColor = value;
                    InvalidateVisual();
                }
            }
        }

        private Color FPixHalfColor;          // Half intensity ON color                  }

        private int FPixWidth;

        /// <summary>
        /// Gets or sets the width of a LCD pixel.
        /// </summary>
        /// <value>The width of a LCD pixel.</value>
        public int PixelWidth
        {
            get { return FPixWidth; }
            set
            {
                if (FPixelSize == PixelSize.pixCustom)
                {
                    if (value != FPixWidth)
                    {
                        if (value < 1)
                            throw new ArgumentException("Display pixel width must be 1 or greater");
                        else
                        {
                            FPixWidth = value;
                            InvalidateVisual();
                        }
                    }
                }
            }
        }

        private int FPixHeight;

        /// <summary>
        /// Gets or sets the height of a LCD pixel.
        /// </summary>
        /// <value>The height of a LCD pixel.</value>
        public int PixelHeight
        {
            get { return FPixHeight; }
            set
            {
                if (FPixelSize == PixelSize.pixCustom)
                {
                    if (value != FPixHeight)
                    {
                        if (value < 1)
                            throw new ArgumentException("Display pixel height must be 1 or greater");
                        else
                        {
                            FPixHeight = value;
                            InvalidateVisual();
                        }
                    }
                }
            }
        }

        private Color FBorderColor = Colors.Black;

        /// <summary>
        /// Gets or sets the color of the border.
        /// </summary>
        /// <value>The color of the border.</value>
        public Color BorderColor
        {
            get { return FBorderColor; }
            set
            {
                if (value != FBorderColor)
                {
                    FBorderColor = value;
                    InvalidateVisual();
                }
            }
        }

        private int FWidth;                   // Label width in pixels                    }
        private int FHeight;                  // Label height in pixels                   }
        private int charw, charh;             // Temp. storage of character sizes         }
        private int psx, psy;                 // Internal pixel width variables           }
        private int pix_x;
        private int pix_y;

        private int first_c, last_c;          // First and last character to draw         }

        private string text;

        /// <summary>
        /// Gets or sets the text displayed on the LCD display.
        /// </summary>
        /// <returns>
        /// The text displayed on the LCD display.
        /// </returns>
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (Text != value)
                {
                    text = value;
                    InvalidateVisual();  // Force a direct re-paint of label without any flicker
                }
            }
        }

        private void DrawMatrix(DrawingContext graphics, int xpos, int ypos, int charindex)
        {
            int tx, ty;
            Brush currentBrush;
            tx = xpos;
            ty = ypos;

            charindex = charindex - first_c;

            SolidColorBrush onBrush = new SolidColorBrush(FPixOnColor);
            SolidColorBrush offBrush = new SolidColorBrush(FPixOffColor);
            Pen pen = new Pen();
            for (int i = 0; i < pix_y; i++)
            {
                for (int j = 0; j < pix_x; j++)
                {
                    currentBrush = offBrush;
                    switch (FDotMatrix)
                    {
                        case DotMatrix.mat5x7:
                            if (Matrix.Char5x7[charindex, i, j] == 1)
                                currentBrush = onBrush;
                            else
                                currentBrush = offBrush;
                            break;

                        case DotMatrix.mat5x8:
                            if (Matrix.Char5x8[charindex, i, j] == 1)
                                currentBrush = onBrush;
                            else
                                currentBrush = offBrush;
                            break;

                        case DotMatrix.Hitachi:
                            if (Matrix.CharHitachi[charindex, i, j] == 1)
                                currentBrush = onBrush;
                            else
                                currentBrush = offBrush;
                            break;

                        case DotMatrix.Hitachi2:
                            // Use full height for character 194 - 223
                            if (charindex <= 193)
                            {  // Normal Hitachi
                                if (i < 7)
                                {
                                    if (Matrix.CharHitachi[charindex, i, j] == 1)
                                        currentBrush = onBrush;
                                    else
                                        currentBrush = offBrush;
                                }
                                else
                                    currentBrush = offBrush;
                            }
                            else
                            {
                                // Extended height
                                if (Matrix.CharHitachiExt[charindex, i, j] == 1)
                                    currentBrush = onBrush;
                                else
                                    currentBrush = offBrush;
                            }
                            break;

                        case DotMatrix.mat7x9:
                            if (Matrix.Char7x9[charindex, i, j] == 1)
                                currentBrush = onBrush;
                            else
                                currentBrush = offBrush;
                            break;

                        case DotMatrix.mat9x12:
                            if (Matrix.Char9x12[charindex, i, j] == 1)
                                currentBrush = onBrush;
                            else
                                currentBrush = offBrush;
                            break;

                        case DotMatrix.dos5x7:
                            if (Matrix.CharDOS5x7[charindex, i, j] == 1)
                                currentBrush = onBrush;
                            else
                                currentBrush = offBrush;
                            break;
                    }

                    // Paint pixels in selected shape
                    graphics.DrawRectangle(currentBrush, pen, new Rect(tx, ty, psx, psy));

                    tx = tx + psx + FPixelSpacing;
                }
                tx = xpos;
                ty = ty + psy + FPixelSpacing;
            }
        }

        private void DrawCharacters(DrawingContext graphics)
        {
            if (Text == null)
                return;

            int xpos, ypos;
            int charindex;
            int cc;
            bool textend;

            xpos = FBorderSpace + 1;
            ypos = FBorderSpace + 1;
            cc = 1;
            textend = false;
            for (int row = 1; row <= FTextLines; row++)
            {
                // Line counter             }
                for (int col = 1; col <= FNoOfChars; col++)
                {
                    // Character counter        }
                    if (!textend)              // Check for string end     }
                        if (cc > Text.Length)
                            textend = true;

                    if (textend)
                        charindex = 0;
                    else
                        charindex = Convert.ToInt32(Text[cc - 1]);

                    if (charindex < first_c)        // Limit charactes inside interval }
                        charindex = first_c;

                    if (charindex > last_c)
                        charindex = last_c;

                    DrawMatrix(graphics, xpos, ypos, charindex);
                    xpos = xpos + charw + FCharSpacing;
                    cc++;
                }
                ypos = ypos + charh + FLineSpacing;
                xpos = FBorderSpace + 1;
            }
        }

        //{ Calculate half color intensity }
        private void CalcHalfColor()
        {
            byte red, green, blue, control;
            blue = (byte)(FPixOnColor.B / 2);
            green = (byte)(FPixOnColor.G / 2);
            red = (byte)(FPixOnColor.R / 2);
            control = FPixOnColor.A;
            FPixHalfColor = Color.FromArgb(control, red, green, blue);
        }

        //{ Calculations for width and height }
        private void CalcSize()
        {
            if (PixelSize.pixCustom == FPixelSize)  // Custom size }
            {
                psx = FPixWidth;
                psy = FPixHeight;
            }
            else              // Predefined width selected - make square pixels }
            {
                psx = (int)FPixelSize + 1;
                psy = psx;
                FPixWidth = psx;
                FPixHeight = psy;
            }

            switch (FDotMatrix)         //{ Calculate the space taken by the character matrix }
            {
                case DotMatrix.mat5x7:
                case DotMatrix.Hitachi:
                    pix_x = Matrix.HITACHI_WIDTH;
                    pix_y = Matrix.HITACHI_HEIGHT;
                    break;

                case DotMatrix.Hitachi2:
                    pix_x = Matrix.HITACHI2_WIDTH;
                    pix_y = Matrix.HITACHI2_HEIGHT;
                    break;

                case DotMatrix.mat5x8:
                    pix_x = Matrix.MAT5X8_WIDTH;
                    pix_y = Matrix.MAT5X8_HEIGHT;
                    break;

                case DotMatrix.mat7x9:
                    pix_x = Matrix.MAT7X9_WIDTH;
                    pix_y = Matrix.MAT7X9_HEIGHT;
                    break;

                case DotMatrix.mat9x12:
                    pix_x = Matrix.MAT9X12_WIDTH;
                    pix_y = Matrix.MAT9X12_HEIGHT;
                    break;

                case DotMatrix.dos5x7:
                    pix_x = Matrix.DOS5X7_WIDTH;
                    pix_y = Matrix.DOS5X7_HEIGHT;
                    break;
            }

            charw = (pix_x * psx) + ((pix_x - 1) * FPixelSpacing);
            charh = (pix_y * psy) + ((pix_y - 1) * FPixelSpacing);
            Width = (FBorderSpace * 2) +                //{ Distance to sides (there are two) }
                 (FCharSpacing * (FNoOfChars - 1)) + //{ Spaces between charactes          }
                 (charw * FNoOfChars) +              //{ The characters itself             }
                 2;                                  //{ Border outside character area     }
            Height = (FBorderSpace * 2) +                //{ Distance to top and bottom        }
                 (FLineSpacing * (FTextLines - 1)) + //{ Spacing between lines             }
                 (charh * FTextLines) +              //{ The lines                         }
                 2;                                  //{ The Border                        }
            FWidth = (int)Width;
            FHeight = (int)Height;
        }

        //{ Get interval for ASCII values }
        private void GetAsciiInterval()
        {
            switch (FDotMatrix)
            {
                case DotMatrix.mat5x7:
                case DotMatrix.Hitachi:
                    first_c = Matrix.HITACHI_FIRST;
                    last_c = Matrix.HITACHI_LAST;
                    break;

                case DotMatrix.Hitachi2:
                    first_c = Matrix.HITACHI2_FIRST;
                    last_c = Matrix.HITACHI2_LAST;
                    break;

                case DotMatrix.mat5x8:
                    first_c = Matrix.MAT5X8_FIRST;
                    last_c = Matrix.MAT5X8_LAST;
                    break;

                case DotMatrix.mat7x9:
                    first_c = Matrix.MAT7X9_FIRST;
                    last_c = Matrix.MAT7X9_LAST;
                    break;

                case DotMatrix.mat9x12:
                    first_c = Matrix.MAT9X12_FIRST;
                    last_c = Matrix.MAT9X12_LAST;
                    break;

                case DotMatrix.dos5x7:
                    first_c = Matrix.DOS5X7_FIRST;
                    last_c = Matrix.DOS5X7_LAST;
                    break;
            }
        }

        //{ Calculate no of characters and lines from width and heigth }
        private void CalcCharSize()
        {
            if (PixelSize.pixCustom == FPixelSize) // Custom size
            {
                psx = FPixWidth;
                psy = FPixHeight;
            }
            else              // Predefined width selected - make square pixels
            {
                psx = (int)FPixelSize + 1;
                psy = psx;
                FPixWidth = psx;
                FPixHeight = psy;
            }

            switch (FDotMatrix)         //{ Calculate the space taken by the character matrix }
            {
                case DotMatrix.mat5x7:
                case DotMatrix.Hitachi:
                    pix_x = Matrix.HITACHI_WIDTH;
                    pix_y = Matrix.HITACHI_HEIGHT;
                    break;

                case DotMatrix.Hitachi2:
                    pix_x = Matrix.HITACHI2_WIDTH;
                    pix_y = Matrix.HITACHI2_HEIGHT;
                    break;

                case DotMatrix.mat5x8:
                    pix_x = Matrix.MAT5X8_WIDTH;
                    pix_y = Matrix.MAT5X8_HEIGHT;
                    break;

                case DotMatrix.mat7x9:
                    pix_x = Matrix.MAT7X9_WIDTH;
                    pix_y = Matrix.MAT7X9_HEIGHT;
                    break;

                case DotMatrix.mat9x12:
                    pix_x = Matrix.MAT9X12_WIDTH;
                    pix_y = Matrix.MAT9X12_HEIGHT;
                    break;

                case DotMatrix.dos5x7:
                    pix_x = Matrix.DOS5X7_WIDTH;
                    pix_y = Matrix.DOS5X7_HEIGHT;
                    break;
            }

            charw = (pix_x * psx) + ((pix_x - 1) * FPixelSpacing);
            charh = (pix_y * psy) + ((pix_y - 1) * FPixelSpacing);
            FNoOfChars = (int)(Width - (2 * FBorderSpace) + FCharSpacing) / (charw + FCharSpacing);
            FTextLines = (int)(Height - (2 * FBorderSpace) + FLineSpacing) / (charh + FLineSpacing);
            if (FNoOfChars < 1)
                FNoOfChars = 1;
            if (FTextLines < 1)
                FTextLines = 1;
            Width = (FBorderSpace * 2) +                //{ Distance to sides (there are two) }
                 (FCharSpacing * (FNoOfChars - 1)) + //{ Spaces between charactes          }
                 (charw * FNoOfChars) +              //{ The characters itself             }
                 2;                                  //{ For the border                    }
            Height = (FBorderSpace * 2) +                //{ Distance to top and bottom        }
                 (FLineSpacing * (FTextLines - 1)) + //{ Spacing between lines             }
                 (charh * FTextLines) +              //{ The lines                         }
                 2;                                  //{ For the border                    }
            FWidth = (int)Width;
            FHeight = (int)Height;
        }

        /// <summary>
        /// Paints the controls and raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnRender(DrawingContext e)
        {
            base.OnRender(e);

            bool flag = false;

            if (Width != FWidth)
            {
                flag = true;
                FWidth = (int)Width;
            }

            if (Height != FHeight)
            {
                flag = true;
                FHeight = (int)Height;
            }

            GetAsciiInterval();          // Calculate interval for ASCII values }
            if (flag)
                CalcCharSize();
            else
                CalcSize();                // Get Width and Height correct }

            // draw background
            SolidColorBrush brush = new SolidColorBrush(backgroundColor);
            e.DrawRectangle(brush, new Pen(), new Rect(0, 0, Width, Height));

            // Character drawing
            DrawCharacters(e);
        }
    }
}