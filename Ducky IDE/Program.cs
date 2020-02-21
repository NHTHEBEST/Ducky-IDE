using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net;

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
        
        static void Main()
        {
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
    }
}
