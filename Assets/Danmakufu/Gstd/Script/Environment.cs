using System.Collections.Generic;

namespace Gstd
{
    namespace Script
    {
        sealed class Environment
        {
            public Environment Pred { get; set; } // TODO remove field
            public Environment Succ { get; set; } // TODO remove field
            public Environment Parent { get; set; } // TODO remove field
            public int RefCount { get; set; } // TODO remove field
            public Block Sub { get; set; } // TODO remove field
            public int Ip { get; set; } // TODO remove field
            public List<Value> Variables { get; set; } // TODO remove field
            public List<Value> Stack { get; set; } // TODO remove field, use stack
            public bool HasResult { get; set; } // TODO remove field
            public Environment()
            {
                Variables = new List<Value>();
                Stack = new List<Value>();
            }
        }
    }
}
