using System;
using System.Collections.Generic;
using System.Text;

namespace EasyValidation
{
    internal static class Check
    {
        public static void IfNullThrow(object value, string message = "value cannot be null")
        {
            if (value == null)
                throw new ArgumentNullException(message);
        }
    }
}
