using System.Collections.Generic;

namespace Gstd
{
    
namespace Script
{

class ScriptEngine
{
    //void* data TODO check
    private bool error;
    private string errorMessage;
    private int errorLine;
    private ScriptTypeManager typeManager;
    private List<Block> blocks;
    private Block mainBlock;
    private Dictionary<string, Block> events;
    public bool Error
    {
        get => error;
    }
    public string ErrorMessage
    {
        get => errorMessage;
    }
    public int ErrorLine
    {
        get => errorLine;
    }
    public ScriptTypeManager TypeManager
    {
        get => typeManager;
    }
    public Block MainBlock
    {
        get => mainBlock;
    }
    public Dictionary<string, Block> Events
    {
        get => events;
    }
    public TypeData GetRealType()
    {
        return typeManager.RealType;
    }
    public TypeData GetCharType()
    {
        return typeManager.CharType;
    }
    public TypeData GetBooleanType()
    {
        return typeManager.BooleanType;
    }
    public TypeData GetStringType()
    {
        return typeManager.StringType;
    }
    public TypeData GetArrayType(TypeData element)
    {
        return typeManager.GetArrayType(element);
    }
    public ScriptEngine(ScriptTypeManager typeManager, string source, Function[] funcv)
    {
        this.typeManager = typeManager;
        blocks = new List<Block>();
        mainBlock = NewBlock(0, BlockKind.BK_normal);
        Scanner s = new Scanner(source.ToCharArray());
        Parser p = new Parser(this, s, funcv);
        events = p.Events;
        error = p.Error;
        errorMessage = p.ErrorMessage;
        errorLine = p.ErrorLine;
        /*
        System.Console.WriteLine("Parser Frame.Count : {0}", p.Frame.Count);
        for (int i = 0; i < p.Frame.Count; ++i)
        {
            Scope scope = p.Frame[i];
            System.Console.WriteLine("Parser Frame[{0}].Count : {1}", i, scope.Count);
            foreach (KeyValuePair<string, Symbol> entry in scope)
            {
                Symbol symbol = entry.Value;
                System.Console.WriteLine("Parser Frame[{0}] : {1}={2},{3}", i, entry.Key, symbol.Level, symbol.Variable);
                if (symbol.Sub == null)
                {
                    System.Console.WriteLine("Parser Frame[{0}] : {1}.Block=null", i, entry.Key);
                }
                else
                {
                    System.Console.WriteLine("Parser Frame[{0}] : {1}.Block={2}", i, entry.Key, symbol.Sub.Name);
                    System.Console.WriteLine("Parser Frame[{0}] : {1}.Block.Kind={2}", i, entry.Key, symbol.Sub.Kind);
                    System.Console.WriteLine("Parser Frame[{0}] : {1}.Block.Codes.Count={2}", i, entry.Key, symbol.Sub.Codes.Count);
                }
            }
        }
        System.Console.WriteLine("Parser Events.Count : {0}", p.Events.Count);
        */
    }
    public Block NewBlock(int level, BlockKind kind)
    {
        Block x = new Block(level, kind);
        blocks.Add(x);
        return x;
    }
}

}

}
