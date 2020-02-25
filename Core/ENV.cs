using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
namespace Core
{
    static class ENV
    {
        public static readonly string ENVPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Ducky-ENV");
        public static void Install()
        {
            if (!Installed)
            {
                Directory.CreateDirectory(ENVPath);
                string zip = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".zip");
                File.WriteAllBytes(zip, Properties.Resources.complier);
                UnzipFile(zip, ENVPath);
                zip = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".zip");
                File.WriteAllBytes(zip, Properties.Resources.jvm);
                UnzipFile(zip, ENVPath);
            }
        }
        public static void ReInstall()
        {
            Directory.Delete(ENVPath, true);
            Install();
        }
        public static bool Installed
        {
            get
            {
                return Directory.Exists(ENVPath);
            }
        }

        static void UnzipFile(string zipPath, string folderPath)
        {
            ZipFile.ExtractToDirectory(zipPath, folderPath);
        }
    }
}
