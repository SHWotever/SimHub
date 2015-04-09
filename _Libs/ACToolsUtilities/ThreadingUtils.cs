using System.ComponentModel;
using System.Windows.Forms;

namespace ACToolsUtilities
{
    public class ThreadingUtils
    {
        public static readonly ISynchronizeInvoke SynchronisingObject = new Control();
    }
}