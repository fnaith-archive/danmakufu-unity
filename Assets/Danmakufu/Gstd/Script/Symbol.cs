namespace Gstd
{
    
namespace Script
{

class Symbol
{
    private int level;
    private Block sub;
    private int variable;
    public int Level
    {
        get => level;
        set => level = value;
    }
    public Block Sub
    {
        get => sub;
        set => sub = value;
    }
    public int Variable
    {
        get => variable;
        set => variable = value;
    }
}

}

}
