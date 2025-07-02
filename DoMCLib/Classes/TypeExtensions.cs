using System.ComponentModel;
using System.Reflection;

namespace DoMCLib.Classes
{
    public static class TypeExtensions
    {
        public static string GetDescription(this Type value)
        {
            if (value == null) return "";
            var attr = value.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? value.FullName ?? value.ToString();
        }
    }

}
