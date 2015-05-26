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
                this.SelectedIndex = Math.Min(this.SelectedIndex, this.Items.Count - 1);
                base.RefreshItems();
            }
            }
            catch { }
        }
    }
}
