using System.Runtime.InteropServices;
using System.Text;

namespace ACToolsUtilities
{
    public class FileOP
    {
        [DllImport("Shlwapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private extern static bool PathFileExists(StringBuilder path);

        public static bool Exists(string file)
        {
            // A StringBuilder is required for interops calls that use strings
            StringBuilder builder = new StringBuilder();
            builder.Append(file);
            return PathFileExists(builder);
        }
    }
}