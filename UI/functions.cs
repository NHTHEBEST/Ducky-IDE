﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Micronucleus;

namespace UI
{
    static class Functions
    {
        public delegate void UpdateProgressHandler(int value);
        public static event UpdateProgressHandler UpdateProgress;

        public delegate void UpdateProgressTextHandler(string value);
        public static event UpdateProgressTextHandler UpdateProgressText;

        static readonly BuildProsses buildProsses = new BuildProsses("");

        static string code = "";
        static byte[] inject = { 0, 0 };
        static byte[] prog = { 0, 0 };
        static bool build;

        public static void Build(string duckycode, string keyboard = "us")
        {
            frun();
            if (code == duckycode)
            {
                build = false;
                return;
            }
            code = duckycode;
            buildProsses.DuckyCode = code;
            inject = buildProsses.InjectBin;
            prog = buildProsses.RawBinary;
            build = true;
        }
        public static void Flash()
        {
            frun();
            if (build)
            {
                Flasher.Flash(prog);
            }
        }

        static bool frunb = true;
        static void frun()
        {
            if (frunb)
            {
                Flasher.FlashUpdate += Flasher_FlashUpdate;
                Flasher.Text += Flasher_Text;
                
            }
            frunb = false;
        }
        public static void Clean()
        {
            buildProsses.clean();
        }
        private static void Flasher_Text(string text)
        {
            string Outp = "";
            if (text.StartsWith("> Device fo"))
            {
                Outp = "Device Found";
            }
            else if (text.StartsWith("> Erasing th"))
            {
                Outp = "Erasing";
            }
            else if (text.StartsWith("> Starting to uplo"))
            {
                Outp = "Flashing";
            }
            else if (text.StartsWith("Don"))
            {
                Outp = "Done";
            }
            // end
            UpdateProgressText(Outp);
        }

        private static void Flasher_FlashUpdate(float value, int stage)
        {
            float x = value * 100.0f;
            int y = (int)x;
            UpdateProgress(y);
        }
    }
}
