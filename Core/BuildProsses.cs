using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Core
{
    public class BuildProsses
    {
        #region Private
        private string _DuckyCode;
        private byte[] _InjectBin;
        #endregion
        #region Setup
        public BuildProsses(string Code)
        {
            DuckyCode = Code;
        }
        public BuildProsses(byte[] EncodedBin)
        {
            InjectBin = EncodedBin;
        }
        #endregion
        #region Public
        public string DuckyCode
        {
            get
            {
                IBR = false;
                return _DuckyCode;
            }
            set
            {
                _DuckyCode = value;
            }
        }
        private bool IBR;
        public byte[] InjectBin
        {
            get
            {
                if (IBR)
                {
                    return _InjectBin;
                }
                else { 
                    return Compiler.ducky(_DuckyCode, "us");
                }
                IBR = true;
            }
            set
            {
                _InjectBin = value;
                IBR = true;
            }
        }
        public string InjectCode
        {
            get
            {
                return codegen.Gen(InjectBin);
            }
        }
        public byte[] RawBinary
        {
            get
            {
                return Compiler.cpp(InjectCode);
            }
        }
        #endregion
    }
}
