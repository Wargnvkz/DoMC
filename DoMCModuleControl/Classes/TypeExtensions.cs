using System.ComponentModel;
using System.Reflection;

namespace DoMCModuleControl.Classes
{
    public static class TypeExtensions
    {
        public static string GetDescriptionOrFullName(this Type value)
        {
            if (value == null) return "";
            var attr = value.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? value.FullName ?? value.ToString();
        }
        public static string GetDescriptionOrName(this Type value)
        {
            if (value == null) return "";
            var attr = value.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? value.Name ?? value.ToString();
        }
    }

}
