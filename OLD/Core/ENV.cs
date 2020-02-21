using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
            try
            {
                if (!File.Exists(zipPath))
                {
                    throw new FileNotFoundException();
                }
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                Shell32.Shell objShell = new Shell32.Shell();
                Shell32.Folder destinationFolder = objShell.NameSpace(folderPath);
                Shell32.Folder sourceFile = objShell.NameSpace(zipPath);
                foreach (var file in sourceFile.Items())
                {
                    destinationFolder.CopyHere(file, 4 | 16);
                }
            }
            catch (Exception e)
            {
                //handle error
            }
        }
    }
}
