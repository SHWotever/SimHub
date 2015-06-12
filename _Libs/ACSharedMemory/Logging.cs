using log4net;
using System.Reflection;

namespace ACSharedMemory
{
    public static class Logging
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static ILog Current { get { return Log; } }
    }
}