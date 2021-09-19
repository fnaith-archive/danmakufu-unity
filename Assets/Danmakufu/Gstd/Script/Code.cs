using System;

namespace Gstd
{
    
namespace Script
{

class Code
{
    private CommandKind command;
    private int line;
    private Value data;
    private Callback func;
    // TODO union
    private int level;
    private int variable;
    private Block sub;
    private int arguments;
    private int ip;
    public CommandKind Command
    {
        get => command;
        set => command = value;
    }
    public int Line
    {
        get => line;
        set => line = value;
    }
    public Value Data
    {
        get => data;
        set => data = value;
    }
    public Callback Func
    {
        get => func;
        set => func = value;
    }
    public int Level
    {
        get => level;
        set => level = value;
    }
    public int Variable
    {
        get => variable;
        set => variable = value;
    }
    public Block Sub
    {
        get => sub;
        set => sub = value;
    }
    public int Arguments
    {
        get => arguments;
        set => arguments = value;
    }
    public int Ip
    {
        get => ip;
        set => ip = value;
    }
    public Code()
    {
    }
    public Code(int line, CommandKind command)
    {
        Line = line;
        Command = command;
    }
    public Code(int line, CommandKind command, int level, int variable)
    {
        Line = line;
        Command = command;
        Level = level;
        Variable = variable;
    }
    public Code(int line, CommandKind command, Block sub, int arguments)
    {
        Line = line;
        Command = command;
        Sub = sub;
        Arguments = arguments;
    }
    public Code(int line, CommandKind command, int ip)
    {
        Line = line;
        Command = command;
        Ip = ip;
    }
    public Code(int line, CommandKind command, Value data)
    {
        Line = line;
        Command = command;
        Data = data;
    }
}

}

}
