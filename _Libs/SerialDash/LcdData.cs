using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace SerialDash
{
    public enum FontSize
    {
        Small = 1,
        Normal = 2,
        Large = 3,
    }

    public class LcdData
    {
        private List<ILcdRenderItem> renderData;

        public LcdData()
        {
            this.renderData = new List<ILcdRenderItem>();
        }

        public List<ILcdRenderItem> RenderData
        {
            get { return renderData; }
            set { renderData = value; }
        }

        public void WriteData(SerialPort port)
        {
            // Clear
            port.Write("5");

            foreach (var item in this.renderData)
            {
                item.WriteData(port);
            }

            // Render
            port.Write("6");
        }
    }

    public interface ILcdRenderItem
    {
        void WriteData(SerialPort port);
    }

    public class LcdText : ILcdRenderItem
    {
        private FontSize fontSize;
        private int x;
        private int y;
        private string text;
        private bool rightAlign;

        public LcdText(FontSize FontSize, int x, int y, string text, bool rightAlign = false)
        {
            this.x = x;
            this.y = y;
            this.text = text;
            this.rightAlign = rightAlign;
            this.fontSize = FontSize;
        }

        public void WriteData(SerialPort port)
        {
            port.Write("7");

            port.Write(new char[] { (char)x, (char)y, (char)fontSize, rightAlign ? (char)1 : (char)0, (char)text.Length }, 0, 5);
            if (!rightAlign)
            {
                port.Write(this.text);
            }
            else
            {
                port.Write(string.Join("", this.text.Reverse()));
            }
        }
    }

    public class LcdRectangle : ILcdRenderItem
    {
        private int x;
        private int y;
        private int w;
        private int h;
        private bool fill;

        public LcdRectangle(int x, int y, int w, int h, bool fill = false)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.fill = fill;
        }

        public void WriteData(SerialPort port)
        {
            port.Write("8");
            port.Write(new char[] { (char)x, (char)y, (char)w, (char)h, fill ? (char)1 : (char)0 }, 0, 5);
        }
    }
}