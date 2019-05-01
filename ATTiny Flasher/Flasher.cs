using System;
using System.IO;	

namespace Attiny
{
    public class Flasher
{
   /* public static void Main(string[] args)
    {
		foreach(string arg in args){
			Console.Write(arg+"   ");
			Console.WriteLine(flash(File.ReadAllBytes(arg)));
		}
    }//*/

    unsafe public static bool flash(byte[] program, bool fastmode = false, int timeout = 10, bool run = false)
    {
        Managed_USB foo = new Managed_USB();
        fixed (byte *x = &program[0])
        {
            return foo.Flash(x, program.Length, fastmode, timeout, run);
        }
    }

    //public [System.Serializable]
    
}
}

