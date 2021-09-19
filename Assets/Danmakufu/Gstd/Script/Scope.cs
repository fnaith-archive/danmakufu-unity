using System.Collections.Generic;

namespace Gstd
{
    
namespace Script
{

class Scope : Dictionary<string, Symbol>
{
    private BlockKind kind;
    public BlockKind Kind
    {
        get => kind;
        set => kind = value;
    }
    public Scope(BlockKind kind)
    {
        Kind = kind;
    }
}

}

}
