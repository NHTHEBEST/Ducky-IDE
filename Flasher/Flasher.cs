using System;
using System.IO;


namespace Micronucleus
{
    
    public class Flasher
    {
        public delegate void FlashUpdateEventHandler(float value);
        public static event FlashUpdateEventHandler FlashUpdate;


        unsafe static public bool flash(byte[] program, bool fastmode = false, int timeout = 10, bool run = false)
        {
            Managed_USB musb = new Managed_USB();
            //musb.CLRProgressDelegate += new musb.CLRProgressDelegate(onUpdate);
            fixed (byte* x = &program[0])
            {
                return musb.Flash(x, program.Length, fastmode, timeout, run);
            }
        }

        private void onUpdate(float data)
        {
            FlashUpdate(data);
        }
    }
}

