using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace ACDashboard.LCDDisplay
{
    /// <summary>
    /// Logique d'interaction pour Display.xaml
    /// </summary>
    public partial class Display : UserControl
    {
        public Display()
        {
            try
            {
                foreach (var fontFile in System.IO.Directory.GetFiles(".\\Fonts\\", "*.lcd"))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(FONT));
                    StreamReader reader = new StreamReader(fontFile);
                    var font = (FONT)serializer.Deserialize(reader);
                    AddGLCDFont(font);
                    reader.Close();
                }
            }
            catch { }

            InitializeComponent();
            PixelSize = 3;
            PixelSpacing = 0;
        }

        private int PixelSize { get; set; }

        private int PixelSpacing { get; set; }

        public int PixelWidth { get { return (int)(this.RenderSize.Width / (PixelSize + PixelSpacing)); } }

        public int PixelHeight { get { return (int)(this.RenderSize.Height / (PixelSize + PixelSpacing)); } }

        private DrawingContext dc;
        private DrawingVisual dv;

        private DrawingContext DrawingContext
        {
            get
            {
                if (dv == null)
                {
                    dv = new DrawingVisual();
                }

                if (dc == null)
                {
                    dc = dv.RenderOpen();
                }
                return dc;
            }
        }

        public void Print(int x, int y, string text, string Font = "6x7")
        {
            if (this.Fonts.ContainsKey(Font))
            {
                var currentFont = this.Fonts[Font];
                foreach (char c in text)
                {
                    if (currentFont.Characters.ContainsKey(c))
                    {
                        var cDef = currentFont.Characters[c];
                        for (var line = 0; line < currentFont.Height; line++)
                        {
                            for (var col = 0; col < currentFont.Width; col++)
                            {
                                SetPixel(x + col, y + line, cDef[line * currentFont.Width + col]);
                            }
                        }
                    }
                    x += currentFont.Width;
                }
            }
        }

        public void SetPixel(int x, int y, bool status)
        {
            Color color = status ? Colors.Blue : Color.FromArgb(0, 0, 0, 0);

            //if (x < PixelWidth && y < PixelHeight && x >= 0 && y >= 0)
            {
                DrawingContext.DrawRectangle(new SolidColorBrush(color), null, new Rect(x * (PixelSize + PixelSpacing), y * (PixelSize + PixelSpacing), PixelSize, PixelSize));
            }
        }

        public void Clear()
        {
            lock (this)
            {
                dc.Close();
                dc = null;
                dv = null;
            }
        }

        public void Show()
        {
            lock (this)
            {
                if (this.RenderSize.Height > 0 && this.RenderSize.Width > 0)
                {
                    if (dc != null)
                    {
                        dc.Close();
                        dc = null;
                    }
                    if (dv == null)
                    {
                        return;
                    }

                    RenderTargetBitmap rtb = new RenderTargetBitmap((int)this.RenderSize.Width, (int)this.RenderSize.Height, 96, 96, PixelFormats.Pbgra32);
                    rtb.Render(dv);
                    displayImage.Source = rtb;
                }
            }
        }

        private Dictionary<string, PixelFont> Fonts = new Dictionary<string, PixelFont>(StringComparer.OrdinalIgnoreCase);

        private void AddGLCDFont(FONT font)
        {
            PixelFont result = new PixelFont();
            result.Width = font.FONTSIZE.WIDTH;
            result.Height = font.FONTSIZE.HEIGHT;
            foreach (var charItem in font.CHARS)
            {
                var data = charItem.PIXELS.Split(',');
                List<string> fontData = new List<string>();
                for (var line = 0; line < result.Height; line++)
                {
                    string lineData = "";
                    for (var col = 0; col < result.Width; col++)
                    {
                        lineData += data[col * font.FONTSIZE.HEIGHT + line] == "0" ? '*' : ' ';
                    }
                    fontData.Add(lineData);
                }
                result.AddChar((char)charItem.CODE, fontData.ToArray());
            }

            this.Fonts.Add(font.FONTNAME, result);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            Show();
            //CreatePixels();
            //base.OnRenderSizeChanged(sizeInfo);
        }

        private class PixelFont
        {
            public PixelFont()
            {
                this.Characters = new Dictionary<char, bool[]>();
            }

            public int Width { get; set; }

            public int Height { get; set; }

            public Dictionary<char, bool[]> Characters { get; private set; }

            public void AddChar(char charId, params string[] linesData)
            {
                bool[] charPixels = new bool[this.Width * this.Height];
                for (int lines = 0; lines < this.Height; lines++)
                {
                    for (int cols = 0; cols < this.Width; cols++)
                    {
                        charPixels[lines * this.Width + cols] = linesData[lines][cols] != ' ';
                    }
                }
                this.Characters.Add(charId, charPixels);
            }
        }
    }
}