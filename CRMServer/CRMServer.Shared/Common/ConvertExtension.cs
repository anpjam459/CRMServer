using System;
using System.Collections.Generic;
using System.Text;

namespace CRMServer.Shared.Common
{
    public static class ConvertExtension
    {
        public static int ToInt(this object obj, int defaultValue = 0)
        {
            int value = defaultValue;
            int.TryParse(obj + "", out value);
            return value;
        }
    }
}
