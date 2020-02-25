using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Diagnostics;
using System.Reflection;
using System.IO.Compression;

namespace Ducky_IDE
{
    static class Program
    {
        [STAThread]
        static void StartForm()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Ducky_IDE());
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        static void Main(string[] args)
        {
            if (args.Contains("-driverinstall"))
            {
                if (!IsRunAsAdmin())
                {
                    MessageBox.Show("Please Run As Admin for driver install");
                    goadmin(args);

                    return;
                }
                // 
                string zip = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".zip");
                string temp = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() );
                string data = Path.Combine(temp, "Digistump Drivers");
                string installer = Path.Combine(data, "Install Drivers.exe");
                using (var client = new WebClient())
                {
                    client.DownloadFile("http://api.nhthebest.com/duckyide/Digistump.Drivers.zip", zip);
                }
                ZipFile.ExtractToDirectory(zip, temp);
                Process driverinstaller = Process.Start(installer);
                driverinstaller.WaitForExit();
                using (var client = new WebClient())
                {
                    client.DownloadFile("http://api.nhthebest.com/duckyide/libusb0.dll", "libusb0.dll");
                }
                return;
            }
            if (!File.Exists("libusb0.dll"))
            {
                string[] arg = { "-driverinstall" };
                goadmin(arg);
            }
            if (!File.Exists("Core.dll"))
            {
                // get core.dll
                new Thread(() => MessageBox.Show("Downloading Core.dll"));
                using (var client = new WebClient())
                {
                    client.DownloadFile("http://api.nhthebest.com/duckyide/Core.dll", "Core.dll");
                }
            }
            if (!File.Exists("Ducky.xml"))
            {
                // get file
                new Thread(() => MessageBox.Show("Downloading Ducky.xml"));
                using (var client = new WebClient())
                {
                    client.DownloadFile("http://api.nhthebest.com/duckyide/Ducky.xml", "Ducky.xml");
                }
            }
            if (!File.Exists("FastColoredTextBox.dll"))
            {
                // get file
                new Thread(() => MessageBox.Show("Downloading FastColoredTextBox.dll"));
                using (var client = new WebClient())
                {
                    client.DownloadFile("http://api.nhthebest.com/duckyide/FastColoredTextBox.dll", "FastColoredTextBox.dll");
                }
            }
            if (!File.Exists("Flasher.dll"))
            {
                // get file
                new Thread(() => MessageBox.Show("Downloading Flasher.dll"));
                using (var client = new WebClient())
                {
                    client.DownloadFile("http://api.nhthebest.com/duckyide/Flasher.dll", "Flasher.dll");
                }
            }
            if (!File.Exists("UI components.dll"))
            {
                // get file
                new Thread(() => MessageBox.Show("Downloading UI components.dll"));
                using (var client = new WebClient())
                {
                    client.DownloadFile("http://api.nhthebest.com/duckyide/UI_components.dll", "UI components.dll");
                }
            }
            if (!File.Exists("MetroSet UI.dll"))
            {
                // get file
                new Thread(() => MessageBox.Show("Downloading MetroSet UI.dll"));
                using (var client = new WebClient())
                {
                    client.DownloadFile("http://api.nhthebest.com/duckyide/MetroSet_UI.dll", "MetroSet UI.dll");
                }
            }

            Thread thread = new Thread(StartForm);
            thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread.Start();
            
        }
        static void goadmin(string[] args)
        {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.UseShellExecute = true;
            proc.WorkingDirectory = Environment.CurrentDirectory;
            proc.FileName = Assembly.GetEntryAssembly().CodeBase;

            foreach (string arg in args)
            {
                proc.Arguments += String.Format("\"{0}\" ", arg);
            }

            proc.Verb = "runas";

            try
            {
                var x = Process.Start(proc);
                
            }
            catch
            {
                Console.WriteLine("This application requires elevated credentials in order to operate correctly!");
            }
            Application.Exit();
        }
        private static bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
