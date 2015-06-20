using System;
using System.Net;
using System.Windows;
using System.Reflection;
using System.Linq;

namespace De.TorstenMandelkow.MetroChart
{
    public static class Extensions
    {
        public static PropertyInfo[] GetAllProperties(this Type type)
        {
#if NETFX_CORE
            return type.GetRuntimeProperties().ToArray();
#else            
    #if SILVERLIGHT
            return type.GetProperties();
#else
            return type.GetProperties();
#endif
#endif
        }
    }
}
