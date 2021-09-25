using System.Collections.Generic;

namespace Gstd
{
    namespace Script
    {
        sealed class Block
        {
            public int Level { get; set; } // TODO remove set
            public int Arguments { get; set; } // TODO remove set
            public string Name { get; set; } // TODO remove set
            public Callback Func { get; set; } // TODO remove set
            public List<Code> Codes { get; set; } // TODO remove set
            public BlockKind Kind { get; set; } // TODO remove set
            public Block(int level, BlockKind kind)
            {
                Level = level;
                Arguments = 0;
                Name = "";
                Func = null;
                Codes = new List<Code>();
                Kind = kind;
            }
        }
    }
}
