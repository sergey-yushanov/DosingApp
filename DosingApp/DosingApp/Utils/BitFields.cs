using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DosingApp.Utils
{
    public class BitFields
    {
        struct _union_bit_field_s
        {
            ushort bits;

            public bool this[int i]
            {
                get
                {
                    return (bits & (1 << i)) != 0;
                }
                set
                {
                    if (value)
                    {
                        bits |= (ushort)(1 << i);
                    }

                    else
                    {
                        bits &= (ushort)~(1 << i);
                    }
                }
            }
        }


        [StructLayout(LayoutKind.Explicit)]
        struct union_bit_field_s
        {
            [FieldOffset(0)]
            public _union_bit_field_s s;
            [FieldOffset(0)]
            public ushort w;
        }
    }
}
