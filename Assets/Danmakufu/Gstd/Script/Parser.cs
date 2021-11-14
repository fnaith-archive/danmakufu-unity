using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Gstd
{
    namespace Script
    {
        sealed class Parser
        {
            private Scanner lex;
            private ScriptEngine engine;
            private bool error;
            private string errorMessage = "";
            private int errorLine;
            public List<Scope> Frame { get; } = new List<Scope>();
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
            public Dictionary<string, Block> Events { get; } = new Dictionary<string, Block>();
            public Parser(ScriptEngine engine, Scanner scanner, Function[] funcv)
            {
                this.engine = engine;
                this.lex = scanner;
                error = false;
                Frame.Add(new Scope(BlockKind.bk_normal));
                foreach (Function func in BuildInOperation.Operations)
                {
                    RegisterFunction(func);
                }
                foreach (Function func in funcv)
                {
                    RegisterFunction(func);
                }
                try
                {
                    ScanCurrentScope(0, null, false);
                    ParseStatements(engine.MainBlock);
                    if (lex.Next != TokenKind.tk_end)
                    {
                        string error = "Unable to be interpreted (Don't forget \";\"s).\r\n";
                        throw new ParserError(error);
                    }
                }
                catch (ParserError e)
                {
                    error = true;
                    errorMessage = e.Message;
                    errorLine = lex.Line;
                }
            }
            private void ParseParentheses(Block block)
            {
                if (lex.Next != TokenKind.tk_open_par)
                {
                    string error = "\"(\" is nessasary.\r\n";
                    throw new ParserError(error);
                }
                lex.Advance();

                ParseExpression(block);

                if (lex.Next != TokenKind.tk_close_par)
                {
                    string error = "\")\" is nessasary.\r\n";
                    throw new ParserError(error);
                }
                lex.Advance();
            }
            private void ParseClause(Block block)
            {
                if (lex.Next == TokenKind.tk_real)
                {
                    block.Codes.Add(new Code(lex.Line, CommandKind.pc_push_value, new Value(engine.GetRealType(), lex.RealValue)));
                    lex.Advance();
                }
                else if (lex.Next == TokenKind.tk_char)
                {
                    block.Codes.Add(new Code(lex.Line, CommandKind.pc_push_value, new Value(engine.GetCharType(), lex.CharValue)));
                    lex.Advance();
                }
                else if (lex.Next == TokenKind.tk_string)
                {
                    string str = lex.StringValue;
                    lex.Advance();
                    while (lex.Next == TokenKind.tk_string || lex.Next == TokenKind.tk_char)
                    {
                        str += (lex.Next == TokenKind.tk_string) ? lex.StringValue : lex.CharValue.ToString();
                        lex.Advance();
                    }

                    block.Codes.Add(new Code(lex.Line, CommandKind.pc_push_value, new Value(engine.GetStringType(), str)));
                }
                else if (lex.Next == TokenKind.tk_word)
                {
                    Symbol s = Search(lex.Word);
                    if (s == null)
                    {
                        string error = String.Format("{0} is not defined.\r\n", lex.Word);
                        throw new ParserError(error);
                    }

                    lex.Advance();

                    if (s.Sub != null)
                    {
                        if (s.Sub.Kind != BlockKind.bk_function)
                        {
                            string error = "sub and task cannot call in the statement.\r\n";
                            throw new ParserError(error);
                        }

                        int argc = ParseArguments(block);

                        if (argc != s.Sub.Arguments)
                        {
                            string error = String.Format(
                                "{0} incorrect number of parameters. Check to make sure you have the correct number of parameters.\r\n", 
                                s.Sub.Name);
                            throw new ParserError(error);
                        }

                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_call_and_push_result, s.Sub, argc));
                    }
                    else
                    {
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_push_variable, s.Level, s.Variable));
                    }
                }
                else if (lex.Next == TokenKind.tk_open_bra)
                {
                    lex.Advance();
                    block.Codes.Add(new Code(lex.Line, CommandKind.pc_push_value, new Value(engine.GetStringType(), "")));
                    while(lex.Next != TokenKind.tk_close_bra)
                    {
                        ParseExpression(block);
                        WriteOperation(block, "append", 2);
                        if (lex.Next != TokenKind.tk_comma)
                        {
                            break;
                        }
                        lex.Advance();
                    }
                    if (lex.Next != TokenKind.tk_close_bra)
                    {
                        string error = "\"]\" is nessasary.\r\n";
                        throw new ParserError(error);
                    }
                    lex.Advance();
                }
                else if (lex.Next == TokenKind.tk_open_abs)
                {
                    lex.Advance();
                    ParseExpression(block);
                    WriteOperation(block, "absolute", 1);
                    if (lex.Next != TokenKind.tk_close_abs)
                    {
                        string error = "\"|\" is nessasary.\r\n";
                        throw new ParserError(error);
                    }
                    lex.Advance();
                }
                else if (lex.Next == TokenKind.tk_open_par)
                {
                    ParseParentheses(block);
                }
                else
                {
                    string error = "Invalid expression.\r\n";
                    throw new ParserError(error);
                }
            }
            private void ParseSuffix(Block block)
            {
                ParseClause(block);
                if (lex.Next == TokenKind.tk_caret)
                {
                    lex.Advance();
                    ParseSuffix(block); //�ċA
                    WriteOperation(block, "power", 2);
                }
                else
                {
                    while (lex.Next == TokenKind.tk_open_bra)
                    {
                        lex.Advance();
                        ParseExpression(block);

                        if (lex.Next == TokenKind.tk_range)
                        {
                            lex.Advance();
                            ParseExpression(block);
                            WriteOperation(block, "slice", 3);
                        }
                        else
                        {
                            WriteOperation(block, "index_", 2);
                        }

                        if (lex.Next != TokenKind.tk_close_bra)
                        {
                            string error = "\"]\" is nessasary.\r\n";
                            throw new ParserError(error);
                        }
                        lex.Advance();
                    }
                }
            }
            private void ParsePrefix(Block block)
            {
                if (lex.Next == TokenKind.tk_plus)
                {
                    lex.Advance();
                    ParsePrefix(block);	//�ċA
                }
                else if (lex.Next == TokenKind.tk_minus)
                {
                    lex.Advance();
                    ParsePrefix(block);	//�ċA
                    WriteOperation(block, "negative", 1);
                }
                else if (lex.Next == TokenKind.tk_exclamation)
                {
                    lex.Advance();
                    ParsePrefix(block);	//�ċA
                    WriteOperation(block, "not", 1);
                }
                else
                {
                    ParseSuffix(block);
                }
            }
            private void ParsePoduct(Block block)
            {
                ParsePrefix(block);
                while (lex.Next == TokenKind.tk_asterisk || lex.Next == TokenKind.tk_slash || lex.Next == TokenKind.tk_percent)
                {
                    string name = (lex.Next == TokenKind.tk_asterisk) ? "multiply" : ((lex.Next == TokenKind.tk_slash) ? "divide" : "remainder");
                    lex.Advance();
                    ParsePrefix(block);
                    WriteOperation(block, name, 2);
                }
            }
            private void ParseSum(Block block)
            {
                ParsePoduct(block);
                while (lex.Next == TokenKind.tk_tilde || lex.Next == TokenKind.tk_plus || lex.Next == TokenKind.tk_minus)
                {
                    string name = (lex.Next == TokenKind.tk_tilde) ? "concatenate" : ((lex.Next == TokenKind.tk_plus) ? "add" : "subtract");
                    lex.Advance();
                    ParsePoduct(block);
                    WriteOperation(block, name, 2);
                }
            }
            private void ParseComparison(Block block)
            {
                ParseSum(block);
                switch (lex.Next)
                {
                    case TokenKind.tk_assign:
                    {
                        string error = "Do you not mistake it for \"==\"?\r\n";
                        throw new ParserError(error);
                    }

                    case TokenKind.tk_e:
                    case TokenKind.tk_g:
                    case TokenKind.tk_ge:
                    case TokenKind.tk_l:
                    case TokenKind.tk_le:
                    case TokenKind.tk_ne:
                        TokenKind op = lex.Next;
                        lex.Advance();
                        ParseSum(block);
                        WriteOperation(block, "compare", 2);
                        switch (op)
                        {
                            case TokenKind.tk_e:
                                block.Codes.Add(new Code(lex.Line, CommandKind.pc_compare_e));
                                break;
                            case TokenKind.tk_g:
                                block.Codes.Add(new Code(lex.Line, CommandKind.pc_compare_g));
                                break;
                            case TokenKind.tk_ge:
                                block.Codes.Add(new Code(lex.Line, CommandKind.pc_compare_ge));
                                break;
                            case TokenKind.tk_l:
                                block.Codes.Add(new Code(lex.Line, CommandKind.pc_compare_l));
                                break;
                            case TokenKind.tk_le:
                                block.Codes.Add(new Code(lex.Line, CommandKind.pc_compare_le));
                                break;
                            case TokenKind.tk_ne:
                                block.Codes.Add(new Code(lex.Line, CommandKind.pc_compare_ne));
                                break;
                        }
                        break;
                }
            }
            private void ParseLogic(Block block)
            {
                ParseComparison(block);
                while (lex.Next == TokenKind.tk_and_then || lex.Next == TokenKind.tk_or_else)
                {
                    CommandKind cmd = (lex.Next == TokenKind.tk_and_then) ? CommandKind.pc_case_if_not : CommandKind.pc_case_if;
                    lex.Advance();

                    block.Codes.Add(new Code(lex.Line, CommandKind.pc_dup));
                    block.Codes.Add(new Code(lex.Line, CommandKind.pc_case_begin));
                    block.Codes.Add(new Code(lex.Line, cmd));
                    block.Codes.Add(new Code(lex.Line, CommandKind.pc_pop));

                    ParseComparison(block);

                    block.Codes.Add(new Code(lex.Line, CommandKind.pc_case_end));
                }
            }
            private void ParseExpression(Block block)
            {
                ParseLogic(block);
            }
            private int ParseArguments(Block block)
            {
                int result = 0;
                if (lex.Next == TokenKind.tk_open_par)
                {
                    lex.Advance();
                    while (lex.Next != TokenKind.tk_close_par)
                    {
                        ++result;
                        ParseExpression(block);
                        if (lex.Next != TokenKind.tk_comma)
                        {
                            break;
                        }
                        lex.Advance();
                    }
                    if (lex.Next != TokenKind.tk_close_par)
                    {
                        string error = "\")\" is nessasary.\r\n";
                        throw new ParserError(error);
                    }
                    lex.Advance();
                }
                return result;
            }
            private void ParseStatements(Block block)
            {
                for ( ; ; )
                {
                    bool needSemicolon = true;
                    //Console.WriteLine(lex.Next);

#if _TRACE_TOKEN
                    Console.WriteLine("P:" + lex.Next);
#endif
                    if (lex.Next == TokenKind.tk_word)
                    {
                        Symbol s = Search(lex.Word);
                        if (s == null)
                        { 
                            string error = String.Format("{0} is not defined.\r\n", lex.Word);
                            throw new ParserError(error);
                        }
                        lex.Advance();
                        switch (lex.Next)
                        {
                            case TokenKind.tk_assign:
                                lex.Advance();
                                ParseExpression(block);
                                block.Codes.Add(new Code(lex.Line, CommandKind.pc_assign, s.Level, s.Variable));
                                break;

                            case TokenKind.tk_open_bra:
                                block.Codes.Add(new Code(lex.Line, CommandKind.pc_push_variable_writable, s.Level, s.Variable));
                                lex.Advance();
                                ParseExpression(block);
                                if (lex.Next != TokenKind.tk_close_bra)
                                {
                                    string error = "\"]\" is nessasary.\r\n";
                                    throw new ParserError(error);
                                }
                                lex.Advance();
                                WriteOperation(block, "index!", 2);
                                if (lex.Next != TokenKind.tk_assign)
                                {
                                    string error = "\"=\" is nessasary.\r\n";
                                    throw new ParserError(error);
                                }
                                lex.Advance();
                                ParseExpression(block);
                                block.Codes.Add(new Code(lex.Line, CommandKind.pc_assign_writable));
                                break;

                            case TokenKind.tk_add_assign:
                            case TokenKind.tk_subtract_assign:
                            case TokenKind.tk_multiply_assign:
                            case TokenKind.tk_divide_assign:
                            case TokenKind.tk_remainder_assign:
                            case TokenKind.tk_power_assign:
                                {
                                    string f = "";
                                    switch (lex.Next)
                                    {
                                        case TokenKind.tk_add_assign:
                                            f = "add";
                                            break;
                                        case TokenKind.tk_subtract_assign:
                                            f = "subtract";
                                            break;
                                        case TokenKind.tk_multiply_assign:
                                            f = "multiply";
                                            break;
                                        case TokenKind.tk_divide_assign:
                                            f = "divide";
                                            break;
                                        case TokenKind.tk_remainder_assign:
                                            f = "remainder";
                                            break;
                                        case TokenKind.tk_power_assign:
                                            f = "power";
                                            break;
                                    }
                                    lex.Advance();

                                    block.Codes.Add(new Code(lex.Line, CommandKind.pc_push_variable, s.Level, s.Variable));

                                    ParseExpression(block);
                                    WriteOperation(block, f, 2);

                                    block.Codes.Add(new Code(lex.Line, CommandKind.pc_assign, s.Level, s.Variable));
                                }
                                break;

                            case TokenKind.tk_inc:
                            case TokenKind.tk_dec:
                                {
                                    string f = (lex.Next == TokenKind.tk_inc) ? "successor" : "predecessor";
                                    lex.Advance();

                                    block.Codes.Add(new Code(lex.Line, CommandKind.pc_push_variable, s.Level, s.Variable));
                                    WriteOperation(block, f, 1);
                                    block.Codes.Add(new Code(lex.Line, CommandKind.pc_assign, s.Level, s.Variable));
                                }
                                break;
                            default:
                                if (s.Sub == null)
                                {
                                    string error = "You cannot call a variable as if it were a function or a subroutine.\r\n";
                                    throw new ParserError(error);
                                }

                                int argc = ParseArguments(block);

                                if (argc != s.Sub.Arguments)
                                {
                                    string error = String.Format("{0} incorrect number of parameters. Check to make sure you have the correct number of parameters.\r\n", 
                                        s.Sub.Name);
                                    throw new ParserError(error);
                                }

                                block.Codes.Add(new Code(lex.Line, CommandKind.pc_call, s.Sub, argc));
                                break;
                        }
                    }
                    else if (lex.Next == TokenKind.tk_LET || lex.Next == TokenKind.tk_REAL)
                    {
                        lex.Advance();

                        if (lex.Next != TokenKind.tk_word)
                        {
                            string error = "Symbol name is nessasary.\r\n";
                            throw new ParserError(error);
                        }

                        Symbol s = Search(lex.Word);

                        lex.Advance();
                        if (lex.Next == TokenKind.tk_assign)
                        {
                            lex.Advance();
                            ParseExpression(block);
                            block.Codes.Add(new Code(lex.Line, CommandKind.pc_assign, s.Level, s.Variable));
                        }
                    }
                    else if (lex.Next == TokenKind.tk_LOCAL)
                    {
                        lex.Advance();
                        ParseInlineBlock(block, BlockKind.bk_normal);
                        needSemicolon = false;
                    }
                    else if (lex.Next == TokenKind.tk_LOOP)
                    {
                        lex.Advance();
                        if (lex.Next == TokenKind.tk_open_par)
                        {
                            ParseParentheses(block);
                            int ip = block.Codes.Count;
                            block.Codes.Add(new Code(lex.Line, CommandKind.pc_loop_count));
                            ParseInlineBlock(block, BlockKind.bk_loop);
                            block.Codes.Add(new Code(lex.Line, CommandKind.pc_loop_back, ip));
                            block.Codes.Add(new Code(lex.Line, CommandKind.pc_pop));
                        }
                        else
                        {
                            int ip = block.Codes.Count;
                            ParseInlineBlock(block, BlockKind.bk_loop);
                            block.Codes.Add(new Code(lex.Line, CommandKind.pc_loop_back, ip));
                        }
                        needSemicolon = false;
                    }
                    else if (lex.Next == TokenKind.tk_TIMES)
                    {
                        lex.Advance();
                        ParseParentheses(block);
                        int ip = block.Codes.Count;
                        if (lex.Next == TokenKind.tk_LOOP)
                        {
                            lex.Advance();
                        }
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_loop_count));
                        ParseInlineBlock(block, BlockKind.bk_loop);
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_loop_back, ip));
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_pop));
                        needSemicolon = false;
                    }
                    else if (lex.Next == TokenKind.tk_WHILE)
                    {
                        lex.Advance();
                        int ip = block.Codes.Count;
                        ParseParentheses(block);
                        if (lex.Next == TokenKind.tk_LOOP)
                        {
                            lex.Advance();
                        }
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_loop_if));
                        ParseInlineBlock(block, BlockKind.bk_loop);
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_loop_back, ip));
                        needSemicolon = false;
                    }
                    else if (lex.Next == TokenKind.tk_ASCENT || lex.Next == TokenKind.tk_DESCENT)
                    {
                        bool back = lex.Next == TokenKind.tk_DESCENT;
                        lex.Advance();

                        if (lex.Next != TokenKind.tk_open_par)
                        {
                            string error = "\"(\" is nessasary.\r\n";
                            throw new ParserError(error);
                        }
                        lex.Advance();

                        if (lex.Next == TokenKind.tk_LET || lex.Next == TokenKind.tk_REAL)
                        {
                            lex.Advance();
                        }

                        if (lex.Next != TokenKind.tk_word)
                        {
                            string error = "The symbol name is nessasary.\r\n";
                            throw new ParserError(error);
                        }

                        string s = lex.Word;

                        lex.Advance();

                        if (lex.Next != TokenKind.tk_IN)
                        {
                            string error = "\"in\" is nessasary.\r\n";
                            throw new ParserError(error);
                        }
                        lex.Advance();

                        ParseExpression(block);

                        if (lex.Next != TokenKind.tk_range)
                        {
                            string error = "\"..\" is nessasary.\r\n";
                            throw new ParserError(error);
                        }
                        lex.Advance();

                        ParseExpression(block);

                        if (lex.Next != TokenKind.tk_close_par)
                        {
                            string error = "\")\" is nessasary.\r\n";
                            throw new ParserError(error);
                        }
                        lex.Advance();

                        if (lex.Next == TokenKind.tk_LOOP)
                        {
                            lex.Advance();
                        }

                        if (!back)
                        {
                            block.Codes.Add(new Code(lex.Line, CommandKind.pc_swap));
                        }

                        int ip = block.Codes.Count;

                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_dup2));
                        WriteOperation(block, "compare", 2);

                        block.Codes.Add(new Code(lex.Line, back ? CommandKind.pc_loop_descent : CommandKind.pc_loop_ascent));

                        if (back)
                        {
                            WriteOperation(block, "predecessor", 1);
                        }

                        Block b = engine.NewBlock(block.Level + 1, BlockKind.bk_loop);
                        List<string> counter = new List<string>();
                        counter.Add(s);
                        ParseBlock(b, counter, false);
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_dup));
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_call, b, 1));

                        if (!back)
                        {
                            WriteOperation(block, "successor", 1);
                        }

                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_loop_back, ip));
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_pop));
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_pop));

                        needSemicolon = false;
                    }
                    else if (lex.Next == TokenKind.tk_IF)
                    {
                        lex.Advance();
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_case_begin));

                        ParseParentheses(block);
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_case_if_not));
                        ParseInlineBlock(block, BlockKind.bk_normal);
                        while(lex.Next == TokenKind.tk_ELSE)
                        {
                            lex.Advance();
                            block.Codes.Add(new Code(lex.Line, CommandKind.pc_case_next));
                            if (lex.Next == TokenKind.tk_IF)
                            {
                                lex.Advance();
                                ParseParentheses(block);
                                block.Codes.Add(new Code(lex.Line, CommandKind.pc_case_if_not));
                                ParseInlineBlock(block, BlockKind.bk_normal);
                            }
                            else
                            {
                                ParseInlineBlock(block, BlockKind.bk_normal);
                                break;
                            }
                        }

                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_case_end));
                        needSemicolon = false;
                    }
                    else if (lex.Next == TokenKind.tk_ALTERNATIVE)
                    {
                        lex.Advance();
                        ParseParentheses(block);
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_case_begin));
                        while (lex.Next == TokenKind.tk_CASE)
                        {
                            lex.Advance();

                            if (lex.Next != TokenKind.tk_open_par)
                            {
                                string error = "\"(\" is nessasary.\r\n";
                                throw new ParserError(error);
                            }
                            block.Codes.Add(new Code(lex.Line, CommandKind.pc_case_begin));
                            do
                            {
                                lex.Advance();

                                block.Codes.Add(new Code(lex.Line, CommandKind.pc_dup));
                                ParseExpression(block);
                                WriteOperation(block, "compare", 2);
                                block.Codes.Add(new Code(lex.Line, CommandKind.pc_compare_e));
                                block.Codes.Add(new Code(lex.Line, CommandKind.pc_dup));
                                block.Codes.Add(new Code(lex.Line, CommandKind.pc_case_if));
                                block.Codes.Add(new Code(lex.Line, CommandKind.pc_pop));

                            }
                            while (lex.Next == TokenKind.tk_comma);
                            block.Codes.Add(new Code(lex.Line, CommandKind.pc_push_value, new Value(engine.GetBooleanType(), false)));
                            block.Codes.Add(new Code(lex.Line, CommandKind.pc_case_end));
                            if (lex.Next != TokenKind.tk_close_par)
                            {
                                string error = "\")\" is nessasary.\r\n";
                                throw new ParserError(error);
                            }
                            lex.Advance();

                            block.Codes.Add(new Code(lex.Line, CommandKind.pc_case_if_not));
                            block.Codes.Add(new Code(lex.Line, CommandKind.pc_pop));
                            ParseInlineBlock(block, BlockKind.bk_normal);
                            block.Codes.Add(new Code(lex.Line, CommandKind.pc_case_next));
                        }
                        if (lex.Next == TokenKind.tk_OTHERS)
                        {
                            lex.Advance();
                            block.Codes.Add(new Code(lex.Line, CommandKind.pc_pop));
                            ParseInlineBlock(block, BlockKind.bk_normal);
                        }
                        else
                        {
                            block.Codes.Add(new Code(lex.Line, CommandKind.pc_pop));
                        }
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_case_end));
                        needSemicolon = false;
                    }
                    else if (lex.Next == TokenKind.tk_BREAK)
                    {
                        lex.Advance();
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_break_loop));
                    }
                    else if (lex.Next == TokenKind.tk_RETURN)
                    {
                        lex.Advance();
                        switch (lex.Next)
                        {
                            case TokenKind.tk_end:
                            case TokenKind.tk_invalid:
                            case TokenKind.tk_semicolon:
                            case TokenKind.tk_close_cur:
                                break;
                            default:
                                ParseExpression(block);
                                Symbol s = SearchResult();
                                if (s == null)
                                {
                                    string error = "\"return\" can call in function only.\r\n";
                                    throw new ParserError(error);
                                }

                                block.Codes.Add(new Code(lex.Line, CommandKind.pc_assign, s.Level, s.Variable));
                                break;
                        }
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_break_routine));
                    }
                    else if (lex.Next == TokenKind.tk_YIELD)
                    {
                        lex.Advance();
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_yield));
                    }
                    else if (lex.Next == TokenKind.tk_at || lex.Next == TokenKind.tk_SUB || lex.Next == TokenKind.tk_FUNCTION || lex.Next == TokenKind.tk_TASK)
                    {
                        bool is_event = lex.Next == TokenKind.tk_at;

                        lex.Advance();
                        if (lex.Next != TokenKind.tk_word)
                        {
                            string error = "Symbol name is nessasary.\r\n";
                            throw new ParserError(error);
                        }

                        Symbol s = Search(lex.Word);

                        if (is_event)
                        {
                            if (s.Sub.Level > 1)
                            {
                                string error = "\"@\" cannot use in inner function and task.\r\n";
                                throw new ParserError(error);
                            }
                            Events[s.Sub.Name] = s.Sub;
                        }

                        lex.Advance();

                        List<string> args = new List<string>();
                        if (s.Sub.Kind != BlockKind.bk_sub)
                        {
                            if (lex.Next == TokenKind.tk_open_par)
                            {
                                lex.Advance();
                                while (lex.Next == TokenKind.tk_word || lex.Next == TokenKind.tk_LET || lex.Next == TokenKind.tk_REAL)
                                {
                                    if (lex.Next == TokenKind.tk_LET || lex.Next == TokenKind.tk_REAL)
                                    {
                                        lex.Advance();
                                        if (lex.Next != TokenKind.tk_word)
                                        {
                                            string error = "Function parameter is nessasary.\r\n";
                                            throw new ParserError(error);
                                        }
                                    }
                                    args.Add(lex.Word);
                                    lex.Advance();
                                    if (lex.Next != TokenKind.tk_comma)
                                    {
                                        break;
                                    }
                                    lex.Advance();
                                }
                                if (lex.Next != TokenKind.tk_close_par)
                                {
                                    string error = "\")\" is nessasary.\r\n";
                                    throw new ParserError(error);
                                }
                                lex.Advance();
                            }
                        }
                        else
                        {
                            if (lex.Next == TokenKind.tk_open_par)
                            {
                                lex.Advance();
                                if (lex.Next != TokenKind.tk_close_par)
                                {
                                    string error = "\")\" is nessasary.\r\n";
                                    throw new ParserError(error);
                                }
                                lex.Advance();
                            }
                        }
                        ParseBlock(s.Sub, args, s.Sub.Kind == BlockKind.bk_function);
                        needSemicolon = false;
                    }

                    //�Z�~�R�����������ƌp�����Ȃ�
                    if (needSemicolon && lex.Next != TokenKind.tk_semicolon)
                    {
                        break;
                    }

                    if (lex.Next == TokenKind.tk_semicolon)
                    {
                        lex.Advance();
                    }
                }
            }
            private void RegisterFunction(Function func)
            {
                Symbol s = new Symbol();
                s.Level = 0;
                s.Sub = engine.NewBlock(0, BlockKind.bk_function);
                s.Sub.Arguments = func.Arguments;
                s.Sub.Name = func.Name;
                s.Sub.Func = func.Callback;
                s.Variable = -1;
                Frame[0][func.Name] = s;
            }
            private Symbol Search(string name)
            {
                for (int i = Frame.Count - 1; i >= 0; --i)
                {
                    if (Frame[i].ContainsKey(name))
                    {
                        return Frame[i][name];
                    }
                }
                return null;
            }
            private Symbol SearchResult()
            {
                for (int i = Frame.Count - 1; i >= 0; --i)
                {
                    if (Frame[i].ContainsKey("result"))
                    {
                        return Frame[i]["result"];
                    }
                    if (Frame[i].Kind == BlockKind.bk_sub || Frame[i].Kind == BlockKind.bk_microthread)
                    {
                        return null;
                    }
                }
                return null;
            }
            private void ScanCurrentScope(int level, List<string> args, bool addingResult)
            {
                Scanner lex2 = new Scanner(lex);
                try
                {
                    Scope currentFrame = Frame[Frame.Count - 1];
                    int cur = 0;
                    int var = 0;

                    if (addingResult)
                    {
                        Symbol s = new Symbol();
                        s.Level = level;
                        s.Sub = null;
                        s.Variable = var;
                        ++var;
                        currentFrame["result"] = s;
                    }

                    if (args != null)
                    {
                        foreach (string arg in args)
                        {
                            Symbol s = new Symbol();
                            s.Level = level;
                            s.Sub = null;
                            s.Variable = var;
                            ++var;
                            currentFrame[arg] = s;
                        }
                    }

                    while (cur >= 0 && lex2.Next != TokenKind.tk_end && lex2.Next != TokenKind.tk_invalid)
                    {
                        switch (lex2.Next)
                        {
                            case TokenKind.tk_open_cur:
                                ++cur;
                                lex2.Advance();
                                break;
                            case TokenKind.tk_close_cur:
                                --cur;
                                lex2.Advance();
                                break;
                            case TokenKind.tk_at:
                            case TokenKind.tk_SUB:
                            case TokenKind.tk_FUNCTION:
                            case TokenKind.tk_TASK:
                                {
                                    TokenKind type = lex2.Next;
                                    lex2.Advance();
                                    if (cur == 0)
                                    {
                                        if (currentFrame.ContainsKey(lex2.Word))
                                        {
                                            string error = "Functions and variables of the same name are declared in the same scope.\r\n";
                                            throw new ParserError(error);
                                        }
                                        BlockKind kind = (type == TokenKind.tk_SUB || type == TokenKind.tk_at) ? BlockKind.bk_sub :
                                        ((type == TokenKind.tk_FUNCTION) ? BlockKind.bk_function : BlockKind.bk_microthread);

                                        Symbol s = new Symbol();
                                        s.Level = level;
                                        s.Sub = engine.NewBlock(level + 1, kind);
                                        s.Sub.Name = lex2.Word;
                                        s.Sub.Func = null;
                                        s.Variable = -1;
                                        currentFrame[lex2.Word] = s;
                                        lex2.Advance();
                                        if (kind != BlockKind.bk_sub && lex2.Next == TokenKind.tk_open_par)
                                        {
                                            lex2.Advance();
                                            while (lex2.Next == TokenKind.tk_word || lex2.Next == TokenKind.tk_LET || lex2.Next == TokenKind.tk_REAL)
                                            {
                                                ++(s.Sub.Arguments);
                                                if (lex2.Next == TokenKind.tk_LET || lex2.Next == TokenKind.tk_REAL)
                                                {
                                                    lex2.Advance();
                                                }
                                                if (lex2.Next == TokenKind.tk_word)
                                                {
                                                    lex2.Advance();
                                                }
                                                if (lex2.Next != TokenKind.tk_comma)
                                                {
                                                    break;
                                                }
                                                lex2.Advance();
                                            }
                                        }
                                    }
                                }
                                break;
                            case TokenKind.tk_REAL:
                            case TokenKind.tk_LET:
                                lex2.Advance();
                                if (cur == 0)
                                {
                                    if (currentFrame.ContainsKey(lex2.Word))
                                    {
                                        string error= "Variables of the same name are declared in the same scope.\r\n";
                                        throw new ParserError(error);
                                    }
                                    Symbol s = new Symbol();
                                    s.Level = level;
                                    s.Sub = null;
                                    s.Variable = var;
                                    ++var;
                                    currentFrame[lex2.Word] = s;

                                    lex2.Advance();
                                }
                                break;
                            default:
                                lex2.Advance();
                                break;
                        }
                    }
                }
                catch
                {
                    lex.Line = lex2.Line;
                    throw;
                }
            }
            private void WriteOperation(Block block, string name, int clauses)
            {
                Symbol s = Search(name);
                Debug.Assert(s != null, name);
                if (s.Sub.Arguments != clauses)
                {
                    string error = "Overwriting function does not allow to different argument count.\r\n";
                    throw new ParserError(error);
                }

                block.Codes.Add(new Code(lex.Line, CommandKind.pc_call_and_push_result, s.Sub, clauses));
            }
            private void ParseInlineBlock(Block block, BlockKind kind)
            {
                Block b = engine.NewBlock(block.Level + 1, kind);
                ParseBlock(b, null, false);
                block.Codes.Add(new Code(lex.Line, CommandKind.pc_call, b, 0));
            }
            private void ParseBlock(Block block, List<string> args, bool addingResult)
            {
                if (lex.Next != TokenKind.tk_open_cur)
                {
                    string error = "\"{\" is nessasary.\r\n";
                    throw new ParserError(error);
                }
                lex.Advance();

                Frame.Add(new Scope(block.Kind));

                ScanCurrentScope(block.Level, args, addingResult);

                if (args != null)
                {
                    foreach (string arg in args)
                    {
                        Symbol s = Search(arg);
                        block.Codes.Add(new Code(lex.Line, CommandKind.pc_assign, s.Level, s.Variable));
                    }
                }
                ParseStatements(block);

                Frame.RemoveAt(Frame.Count - 1);

                if (lex.Next != TokenKind.tk_close_cur)
                {
                    string error = "\"}\" is nessasary.\r\n";
                    throw new ParserError(error);
                }
                lex.Advance();
            }
        }
    }
}
