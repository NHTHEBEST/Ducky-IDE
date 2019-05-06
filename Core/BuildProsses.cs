using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    return Encode(_DuckyCode);
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
                return ToCode(InjectBin);
            }
        }
        public byte[] RawBinary
        {
            get
            {
                return Compile(InjectCode);
            }
        }
        #endregion
        #region Funtions
        private byte[] Encode(string duckycode)
        {
            return null;
        }
        private string ToCode(byte[] encodedpayload)
        {
            return "";
        }
        private byte[] Compile(string cppcode)
        {
            return null;
        }
        #endregion
    }
}
