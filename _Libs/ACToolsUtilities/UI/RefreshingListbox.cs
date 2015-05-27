using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACToolsUtilities.UI
{
    public class RefreshingListBox : ListBox
    {
        public new void RefreshItem(int index)
        {
            base.RefreshItem(index);
        }

        public new void RefreshItems()
        {
            try { 
            //if (this.Items.Count > 0)
            {
                int oldidx = this.SelectedIndex;
                base.RefreshItems();
                this.SelectedIndex = Math.Min(oldidx, this.Items.Count - 1);
            }
            }
            catch { }
        }
    }
}
