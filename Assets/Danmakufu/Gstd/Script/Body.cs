using System.Collections.Generic;

namespace Gstd
{
    namespace Script
    {
        sealed class Body
        {
            public int RefCount { get; set; } // TODO remove field
            public TypeData Type { get; set; } // TODO remove set
            public List<Value> ArrayValue { get; set; } // TODO remove set
            // TODO union
            public double RealValue { get; set; } // TODO remove set
            public char CharValue { get; set; } // TODO remove set
            public bool BooleanValue { get; set; } // TODO remove set
            public Body()
            {
                ArrayValue = new List<Value>(); // TODO lazy init
            }
            public Body(Body source)
            {
                RefCount = source.RefCount;
                Type = source.Type;
                ArrayValue = source.ArrayValue;
                RealValue = source.RealValue;
                CharValue = source.CharValue;
                BooleanValue = source.BooleanValue;
            }
        }
    }
}
