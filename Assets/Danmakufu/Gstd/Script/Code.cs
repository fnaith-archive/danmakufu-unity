namespace Gstd
{
    namespace Script
    {
        sealed class Code
        {
            public int Line { get; set; } // TODO remove set
            public CommandKind Command { get; set; } // TODO remove set
            public Value Data { get; set; } // TODO remove set
            // TODO use union
            public int Level { get; set; } // TODO remove set
            public int Variable { get; set; } // TODO remove set
            public Block Sub { get; set; } // TODO remove set
            public int Arguments { get; set; } // TODO remove set
            public int Ip { get; set; } // TODO remove set
            public Code() // TODO remove
            {
                Line = 0;
                Command = CommandKind.PC_assign; // TODO remove
                Data = new Value(); // TODO remove
            }
            public Code(int line, CommandKind command)
            {
                Line = line;
                Command = command;
                Data = new Value(); // TODO remove
            }
            public Code(int line, CommandKind command, int level, int variable)
            {
                Line = line;
                Command = command;
                Data = new Value(); // TODO remove
                Level = level;
                Variable = variable;
            }
            public Code(int line, CommandKind command, Block sub, int arguments)
            {
                Line = line;
                Command = command;
                Data = new Value(); // TODO remove
                Sub = sub;
                Arguments = arguments;
            }
            public Code(int line, CommandKind command, int ip)
            {
                Line = line;
                Command = command;
                Data = new Value(); // TODO remove
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
