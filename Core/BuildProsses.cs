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
        #region Events
        public delegate void UpdateProgressHandler(int value);
        public event UpdateProgressHandler UpdateProgress;

        public delegate void UpdateProgressTextHandler(string value);
        public event UpdateProgressTextHandler UpdateProgressText;


        #endregion
        #region Private
        private string _DuckyCode;
        private byte[] _InjectBin;
        private string _InjectCode = "";
        #endregion
        #region Setup
        public BuildProsses(string Code)
        {
            DuckyCode = Code;
            frun();
        }
        public BuildProsses(byte[] EncodedBin)
        {
            InjectBin = EncodedBin;
            frun();
        }

        void frun()
        {
            Compiler.UpdateProgress += Compiler_UpdateProgress;
            Compiler.UpdateProgressText += Compiler_UpdateProgressText;
        }

        private void Compiler_UpdateProgressText(string value)
        {
            UpdateProgressText(value);
        }

        private void Compiler_UpdateProgress(int value)
        {
            UpdateProgress(value);
        }
        #endregion
        #region Public
        public void clean()
        {
            Compiler.clean();
        }
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
                    IBR = true;
                    return Compiler.ducky(_DuckyCode, "us");
                }
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
                string tmp;
                if (_InjectCode.Length == 0)
                    _InjectCode = codegen.Gen(InjectBin);
                else
                {
                    tmp = _InjectCode;
                    _InjectCode = codegen.Gen(InjectBin);
                    return tmp;
                }
                return _InjectCode;
            }
            set
            {
                _InjectCode = value;
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
