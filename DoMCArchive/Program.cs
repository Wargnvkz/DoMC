using DoMCModuleControl;
using System.Configuration;

namespace DoMCArchive
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var dbPath = ConfigurationManager.AppSettings["DB"];
            var dbArchivePath = ConfigurationManager.AppSettings["DBArchive"];
            if (!String.IsNullOrEmpty(dbPath) && Path.Exists(dbPath))
            {
                if (!String.IsNullOrEmpty(dbArchivePath) && Path.Exists(dbArchivePath))
                {
                    var archive = new DoMC.Forms.DoMCArchiveForm(new EmptyController(), dbPath, dbArchivePath);
                    Application.Run(archive);
                }
                else
                {
                    MessageBox.Show($"Путь не найден или не существует. DBArchive={dbArchivePath}");
                }
            }
            else
            {
                MessageBox.Show($"Путь не найден или не существует. DB={dbPath}");

            }
        }
    }
}