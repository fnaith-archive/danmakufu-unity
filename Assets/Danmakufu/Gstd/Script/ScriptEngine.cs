using System;
using System.Collections.Generic;

namespace Gstd
{
    namespace Script
    {
        sealed class ScriptEngine : System.IDisposable
        {
            //object data TODO check
            private List<Block> blocks = new List<Block>();
            public bool Error { get; }
            public string ErrorMessage { get; }
            public int ErrorLine { get; }
            public ScriptTypeManager TypeManager { get; }
            public TypeData GetRealType()
            {
                return TypeManager.RealType;
            }
            public TypeData GetCharType()
            {
                return TypeManager.CharType;
            }
            public TypeData GetBooleanType()
            {
                return TypeManager.BooleanType;
            }
            public TypeData GetStringType()
            {
                return TypeManager.StringType;
            }
            public TypeData GetArrayType(TypeData element)
            {
                return TypeManager.GetArrayType(element);
            }
            public Block MainBlock { get; }
            public Dictionary<string, Block> Events { get; }
            public ScriptEngine(ScriptTypeManager typeManager, string source, Function[] funcv)
            {
                TypeManager = typeManager;
                MainBlock = NewBlock(0, BlockKind.bk_normal);
                Scanner s = new Scanner(source.ToCharArray());
                Parser p = new Parser(this, s, funcv);
                Events = p.Events;
                Error = p.Error;
                ErrorMessage = p.ErrorMessage;
                ErrorLine = p.ErrorLine;
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
            public void Dispose() // TODO remove
            {
                blocks.Clear();
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
