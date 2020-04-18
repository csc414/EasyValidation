using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EasyValidation
{
    public class Strategy
    {
        public IEnumerable<PropertyInfo> IncludeProperties { get; set; }

        public IEnumerable<PropertyInfo> ExcludeProperties { get; set; }
    }
}
