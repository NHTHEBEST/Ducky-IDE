using System;
using System.IO;
using System.Runtime.InteropServices;


namespace Micronucleus
{
    
    public class Flasher
    {
        public delegate void FlashUpdateEventHandler(float value, int stage);
        public static event FlashUpdateEventHandler FlashUpdate;

        public delegate void TextEventHandler(string text);
        public static event TextEventHandler Text;

        unsafe static public bool flash(byte[] program, bool fastmode = false, int timeout = 10, bool run = false)
        {
            Managed_USB musb = new Managed_USB();

            Managed_USB.ProgressChanged += onUpdate;
            Managed_USB.TextUpdate += onUpdateText;

            fixed (byte* x = &program[0])
            {
                return musb.Flash(x, program.Length, fastmode, timeout, run);
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
                default:
                    break;
                    #endregion
            }
        }
    }
}

