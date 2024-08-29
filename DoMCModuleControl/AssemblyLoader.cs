using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl
{
    /// <summary>
    /// класс для загрузки сборок в память иначе библиотека не будет нормально работать
    /// </summary>
    public static class AssemblyLoader
    {
        /// <summary>
        /// Загрузить сборки из библиотек в указанном пути
        /// </summary>
        /// <param name="path"></param>
        public static void LoadAssembliesFromPath(string path)
        {
            var assemblies = Directory.GetFiles(path, "*.dll");
            foreach (var assembly in assemblies)
            {
                Assembly.LoadFrom(assembly);
            }
        }
        public static void LoadAssembliesFromEXEPath()
        {
            LoadAssembliesFromPath(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName));
        }

    }
}
