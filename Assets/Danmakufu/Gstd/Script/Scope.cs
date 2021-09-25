using System.Collections.Generic;

namespace Gstd
{
    namespace Script
    {
        sealed class Scope : Dictionary<string, Symbol>
        {
            public BlockKind Kind { get; set; } // TODO remove field
            public Scope(BlockKind kind)
            {
                Kind = kind;
            }
        }
    }
}
