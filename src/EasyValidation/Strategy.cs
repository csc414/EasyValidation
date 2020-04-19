using System.Collections.Generic;
using System.Reflection;

namespace EasyValidation
{
    public class Strategy
    {
        public ICollection<PropertyInfo> IncludeProperties { get; } = new HashSet<PropertyInfo>();

        public ICollection<PropertyInfo> ExcludeProperties { get; } = new HashSet<PropertyInfo>();
    }
}