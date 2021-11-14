#if _TRACE_COMMAND
using System;
#endif

namespace Gstd
{
    namespace Script
    {
        sealed class Code
        {
            public int Line { get; }
            public CommandKind Command { get; }
            public Value Data { get; } = new Value();
            // TODO use union
            public int Level { get; }
            public int Variable { get; }
            public Block Sub { get; }
            public int Arguments { get; }
            public int Ip { get; }
            public Code(int line, CommandKind command)
            {
                Line = line;
                Command = command;
#if _TRACE_COMMAND
                Console.WriteLine("C1:" + command);
#endif
            }
            public Code(int line, CommandKind command, int level, int variable)
            {
                Line = line;
                Command = command;
                Level = level;
                Variable = variable;
#if _TRACE_COMMAND
                Console.WriteLine("C2:" + command);
#endif
            }
            public Code(int line, CommandKind command, Block sub, int arguments)
            {
                Line = line;
                Command = command;
                Sub = sub;
                Arguments = arguments;
#if _TRACE_COMMAND
                Console.WriteLine("C3:" + command);
#endif
            }
            public Code(int line, CommandKind command, int ip)
            {
                Line = line;
                Command = command;
                Ip = ip;
#if _TRACE_COMMAND
                Console.WriteLine("C4:" + command);
#endif
            }
            public Code(int line, CommandKind command, Value data)
            {
                Line = line;
                Command = command;
                Data = new Value(data);
#if _TRACE_COMMAND
                Console.WriteLine("C5:" + command);
#endif
            }
        }
    }
}
