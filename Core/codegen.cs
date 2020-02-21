using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    static class codegen
    {
        public static string Gen(byte[] payload)
        {
            string start = Properties.Resources.Start;
            string end = Properties.Resources.End;
            StringBuilder builder = new StringBuilder();
            var l = payload.Length;

            builder.Append(start);
            builder.Append("\n\n#define DUCK_LEN ");
            builder.Append(l.ToString());
            builder.Append("\nconst PROGMEM uint8_t duckraw [DUCK_LEN] = {\n");
            for (int c = 0; c != l - 1; c++)
            {
                builder.Append("0x"+Convert.ToString(payload[c], 16).ToUpper());
                builder.Append(", ");
            }
            builder.Append("0x"+Convert.ToString(payload[l - 1], 16).ToUpper());
            builder.Append("\n};\n\n");
            builder.Append(end);
            return builder.ToString();
        }
    }
}
