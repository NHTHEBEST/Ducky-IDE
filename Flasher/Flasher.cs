using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

namespace Micronucleus
{
    public class Flasher
    {
        public delegate void FlashUpdateEventHandler(float value, int stage);
        public static event FlashUpdateEventHandler FlashUpdate;

        public delegate void TextEventHandler(string text);
        public static event TextEventHandler Text;

        private static bool fm;
        private static bool r;
        private static int to = 10;

        /// <summary>
        /// FastMode Flash
        /// </summary>
        public static bool FastMode { get { return fm; } set { fm = value; } }
        /// <summary>
        /// Run After Flash
        /// </summary>
        public static bool Run { get { return r; } set { r = value; } }
        /// <summary>
        /// Device Search Timeout
        /// </summary>
        public static int TimeOut { get { return to; } set { to = value; } } 

        /// <summary>
        /// Flashes Device With Program
        /// </summary>
        /// <param name="program">Raw bytes to be flashed</param>
        /// <returns>True on Succses & False on Fail</returns>
        public static bool Flash(byte[] program)
        {
            return flash(program);
        }
        /// <summary>
        /// Flashes Device With Program
        /// </summary>
        /// <param name="file">Program file to be flash can be raw intelhex or elf</param>
        /// <returns>True on Succses & False on Fail</returns>
        public static bool Flash(string file)
        {
            byte[] data = File.ReadAllBytes(file);
            
            if (data[1] == 'E' && data[2] == 'L' && data[3] == 'F')
            {
                // elf
                data = getFromElf(data);
            }
            else if (data[0] == ':')
            {
                // hex
                data = getFromHex(data);
            }
            return flash(data);
        }

        private static byte[] getFromElf(byte[] data)
        {
            return data;
        }
        public static byte[] getFromHex(byte[] data)
        {
            List<char> chars = new List<char>();
            foreach (byte x in data)
            {
                chars.Add((char)x);
            }
            List<string> lines = new List<string>();
            string line = "";
            foreach (char x in chars.ToArray())
            {
                if (x == '\n')
                {
                    lines.Add(line);
                    line = "";
                }
                else if (x == '\r');
                else
                {
                    line = line + x;
                }
            }
            line = "";
            StringBuilder sb = new StringBuilder();
            foreach (string l in lines)
            {
                string binvalue = "";
                line = l.Substring(9);
                char[] cha = line.ToCharArray();

                if (cha.Length > 32)
                {
                    binvalue = new string(cha, 0, 32);
                    sb.Append(binvalue);
                }
            }

            return StringToByteArray(sb.ToString());
        }
        static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        unsafe static private bool flash(byte[] program)
        {
            Managed_USB musb = new Managed_USB();

            Managed_USB.ProgressChanged += onUpdate;
            Managed_USB.TextUpdate += onUpdateText;

            fixed (byte* x = &program[0])
            {
                return musb.Flash(x, program.Length, FastMode, TimeOut, Run);
            }
        }

        private static void onUpdate(float data, int stage)
        {
            FlashUpdate(data, stage);
        }
        private static void onUpdateText(int code, int data1, int data2)
        {
            switch (code)
            {
                // go thrue all and chage to ralavant info
                #region cases
                case 1:
                    Text("> waiting for ");
                    Text(data1.ToString());
                    Text(" sec\n> Please plug in the device ... \n");
                    break;
                case 2:
                    Text("> Device search timed out\n");
                    break;
                case 3:
                    Text("> Device found!\n");
                    break;
                case 4:
                    Text("> Device has firmware version ");
                    Text(data1.ToString());
                    Text(".");
                    Text(data2.ToString());
                    Text("\n");
                    break;
                case 5:
                    Text("> Device signature: 0x1e");
                    Text(data1.ToString("x2"));
                    Text(data2.ToString("x2"));
                    Text(" \n");
                    break;
                case 6:
                    Text("> Available space for user applications: ");
                    Text(data1.ToString());
                    Text(" bytes\n");
                    break;
                case 7:
                    Text("> Suggested sleep time between sending pages: ");
                    Text(((uint)data1).ToString());
                    Text("ms\n");
                    break;
                case 8:
                    Text("> Whole page count: ");
                    Text(data1.ToString());
                    Text("  page size: ");
                    Text(data2.ToString());
                    Text("\n");
                    break;
                case 9:
                    Text("> Erase function sleep duration: ");
                    Text(data1.ToString());
                    Text("ms\n");
                    break;
                case 10:
                    Text("> No data in input file, exiting.\n");
                    break;
                case 11:
                    Text("> Program file is ");
                    Text(data1.ToString());
                    Text(" bytes too big for the bootloader!\n");
                    break;
                case 12:
                    Text("> Erasing the memory ...\n");
                    break;
                case 13:
                    Text(">> (!) Connection not working. Unplug and reconnect\n");
                    Text("   device usb connector, or reset it some other way to continue.\n");
                    break;
                case 14:
                    Text(">> Reconnected! Continuing upload sequence...\n");
                    break;
                case 15:
                    Text(">> Flash erase error ");
                    Text(data1.ToString());
                    Text(" has occured ...\n");
                    Text(">> Please unplug the device and restart the program.\n");
                    break;
                case 16:
                    Text("> Starting to upload ...\n");
                    break;
                case 17:
                    Text(">> Flash write error ");
                    Text(data1.ToString());
                    Text(" has occured ...\n");
                    Text(">> Please unplug the device and restart the program.\n");
                    break;
                case 18:
                    Text("> Starting the user app ...\n");
                    Text("> Warning Ducky Script will run in 5 sec\n");
                    break;
                case 19:
                    Text(">> Run error ");
                    Text(data1.ToString());
                    Text(" has occured ...\n");
                    Text(">> Please unplug the device and restart the program. \n");
                    break;
                    #endregion
            }
        }
    }
}

