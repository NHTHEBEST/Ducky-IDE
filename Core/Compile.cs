using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;


namespace Core
{
    public class Compile
    {
        static private string dir = GetTemporaryDirectory();

        static public bool GO()
        {
            GetEnv();
            build(@"C:\Users\NHTHEBEST\Downloads\arduino_build_821058\Wifi_pass.ino.cpp");
            return false;
        }
        static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        static void GetEnv()
        {
            string zip = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()+ ".zip");
            File.WriteAllBytes(zip, Properties.Resources.complier);
            UnzipFile(zip, dir);
        }
        
        static void build(string main)
        {
            string binpath = Path.Combine(dir, "bin");
            string libPath = Path.Combine(dir, "libs");

            string gpp = Path.Combine(binpath, "avr-g++.exe");
            string gcc = Path.Combine(binpath, "avr-gcc.exe");
            string objcopy = Path.Combine(binpath, "avr-objcopy.exe");

            string buildarg = "-c -g -Os -w -fno-exceptions -ffunction-sections -fdata-sections -MMD -mmcu=attiny85 -DF_CPU=16500000L -DARDUINO=10809 -DARDUINO_AVR_DIGISPARK -DARDUINO_ARCH_AVR \"-I"
                +Path.Combine(dir,"make")+"\" -o \"" + Path.Combine(dir,"out.o")+"\" \""+main+"\"";
            string linkarg = "-Os -Wl,--gc-sections -mmcu=attiny85 -o \""+Path.Combine(dir,"out.elf") + "\" \"" + Path.Combine(dir, "out.o") + "\" \"" + Path.Combine(libPath, "DigisparkKeyboard.o") + "\" \"" + 
                Path.Combine(libPath, "pins_arduino.c.o") + "\" \"" + Path.Combine(libPath, "core.a") +"\" \"-L"+dir+"\" -lm";
            string objcopyarg = "-O binary -R .eeprom \""+ Path.Combine(dir, "out.elf")+"\" \""+ Path.Combine(dir, "out.bin")+"\"";

            run(gpp, buildarg);
            Thread.Sleep(100);
            run(gcc, linkarg);
            Thread.Sleep(100);
            run(objcopy, objcopyarg);
        }

        static void run(string exePath, string Args)
        {
            ProcessStartInfo psi = new ProcessStartInfo(exePath, Args);

            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;

            Process cmd = new Process() { StartInfo = psi, EnableRaisingEvents = true };

            cmd.ErrorDataReceived += Async_Data_Received;
            cmd.OutputDataReceived += Async_Data_Received;
            cmd.Start();
            cmd.BeginErrorReadLine();
            cmd.BeginOutputReadLine();
            cmd.WaitForExit();
        }

        private static void Async_Data_Received(object sender, DataReceivedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show(e.Data); 
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
