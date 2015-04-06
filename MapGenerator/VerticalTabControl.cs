using System.Drawing;
using System.Windows.Forms;

namespace MapGenerator
{
    public class VerticalTabControl : TabControl
    {
        public VerticalTabControl()
        {
            this.DrawItem += VerticalTabControl_DrawItem;

            this.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.Alignment = TabAlignment.Left;
        }

        private void VerticalTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _textBrush;

            // Get the item from the collection.
            TabPage _tabPage = this.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = this.GetTabRect(e.Index);
            _tabBounds = new Rectangle(_tabBounds.Y, _tabBounds.X, _tabBounds.Height, _tabBounds.Width);

            if (e.State == DrawItemState.Selected)
            {
                // Draw a different background color, and don't paint a focus rectangle.
                _textBrush = new SolidBrush(Color.Red);
                g.FillRectangle(Brushes.Gray, e.Bounds);
            }
            else
            {
                _textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                e.DrawBackground();
            }

            // Use our own font.
            Font _tabFont = new Font("Arial", (float)10.0, FontStyle.Bold, GraphicsUnit.Pixel);

            g.RotateTransform(-90);
            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;

            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }
    }
}