using System;
using System.Collections.Generic;

namespace Gstd
{
    
namespace Script
{

class Block
{
    private int arguments;
    private string name;
    private Callback func;
    private List<Code> codes;
    private BlockKind kind;
    public int Level { get; set; }
    public int Arguments
    {
        get => arguments;
        set => arguments = value;
    }
    public string Name
    {
        get => name;
        set => name = value;
    }
    public Callback Func
    {
        get => func;
        set => func = value;
    }
    public List<Code> Codes
    {
        get => codes;
        set => codes = value;
    }
    public BlockKind Kind
    {
        get => kind;
        set => kind = value;
    }

    public Block(int level, BlockKind kind)
    {
        Level = level;
        Kind = kind;
        Arguments = 0;
        Name = "";
        Func = null;
        Codes = new List<Code>();
    }
}

}

}
