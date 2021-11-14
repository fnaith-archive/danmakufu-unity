using System.Collections.Generic;

namespace Gstd
{
    namespace Script
    {
        sealed class Scope : Dictionary<string, Symbol>
        {
            public BlockKind Kind { get; }
            public Scope(BlockKind kind)
            {
                Kind = kind;
            }
        }
    }
}
