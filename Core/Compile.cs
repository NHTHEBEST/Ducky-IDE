using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;


namespace Core
{
    static class Compiler
    {
        static private readonly string dir = GetTemporaryDirectory();


        public static byte[] cpp(string code)
        {
            ENV.Install();
            string file = Path.Combine(dir, "main.cpp");
            File.WriteAllText(file, code);
            build(file);
            System.Windows.Forms.MessageBox.Show("Test");
            return File.ReadAllBytes(Path.Combine(dir, "out.bin"));
        }

        public static byte[] ducky(string code, string keyboard)
        {
            ENV.Install();
            string file = Path.Combine(dir, "main.txt");
            File.WriteAllText(file, code);
            encode(file, keyboard);
            return File.ReadAllBytes(Path.Combine(dir, "inject.bin"));
        }

        static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            //System.Windows.Forms.MessageBox.Show(tempDirectory);
            return tempDirectory;
        }


        static void encode(string main, string kb)
        {
            string binpath = Path.Combine(ENV.ENVPath, "bin", "java.exe");
            string enc = '"' + Path.Combine(ENV.ENVPath, "encoder.jar") + '"';
            string outp = '"' + Path.Combine(dir, "inject.bin") + '"';
            string inp = '"' + Path.Combine(dir, main) + '"';
            string args = "-jar " + enc + " -l "+kb+" -o "+outp+" -i "+inp;

            run(binpath, args);
        }

        static void build(string main)
        {
            string binpath = Path.Combine(ENV.ENVPath, "bin");
            string libPath = Path.Combine(ENV.ENVPath, "libs");

            string gpp = Path.Combine(binpath, "avr-g++.exe");
            string gcc = Path.Combine(binpath, "avr-gcc.exe");
            string objcopy = Path.Combine(binpath, "avr-objcopy.exe");

            string buildarg = "-c -g -Os -w -fno-exceptions -ffunction-sections -fdata-sections -MMD -mmcu=attiny85 -DF_CPU=16500000L -DARDUINO=10809 -DARDUINO_AVR_DIGISPARK -DARDUINO_ARCH_AVR \"-I"
                +Path.Combine(ENV.ENVPath, "make")+"\" -o \"" + Path.Combine(dir,"out.o")+"\" \""+main+"\"";
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
            bool ret = false;
        retry:
            try
            {
                cmd.Start();
            }
            catch (System.ComponentModel.Win32Exception exception)
            {
                if (ret)
                {
                    throw new Exception("ENV ERROR");
                }
                ENV.ReInstall();
                goto retry;
            }
            //System.ComponentModel.Win32Exception: 'The system cannot find the file specified'
            cmd.BeginErrorReadLine();
            cmd.BeginOutputReadLine();
            cmd.WaitForExit();
        }

        private static void Async_Data_Received(object sender, DataReceivedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show(e.Data); 
        }

        
    }
}
