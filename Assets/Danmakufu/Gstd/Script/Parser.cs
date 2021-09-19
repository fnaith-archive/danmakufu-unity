using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Gstd
{
    
namespace Script
{

class Parser
{
    private List<Scope> frame;
    private Scanner lex;
    private ScriptEngine engine;
    private bool error;
    private string errorMessage;
    private int errorLine;
    private ScriptTypeManager typeManager;
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
    public Dictionary<string, Block> Events
    {
        get => events;
    }
    public List<Scope> Frame
    {
        get => frame;
    }
    public Parser(ScriptEngine engine, Scanner scanner, Function[] funcv)
    {
        this.engine = engine;
        this.lex = scanner;
        frame = new List<Scope>();
        error = false;
        events = new Dictionary<string, Block>();
        frame.Add(new Scope(BlockKind.BK_normal));
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
            // TODO enable
            ScanCurrentScope(0, null, false);
            ParseStatements(engine.MainBlock);
            if (lex.Next != TokenKind.TK_end)
            {
                string error = "Unable to be interpreted (Don't forget \";\"s).\r\n";
                //error += L"(���߂ł��Ȃ����̂�����܂�(�u;�v��Y��Ă��܂���))"; TODO enable
                throw new ParserException(error);
            }
        }
        catch (ParserException e)
        {
            error = true;
            errorMessage = e.Message;
            errorLine = lex.Line;
        }
    }
    public void RegisterFunction(Function func)
    {
        Symbol s = new Symbol();
        s.Level = 0;
        s.Sub = engine.NewBlock(0, BlockKind.BK_function);
        s.Sub.Arguments = func.Arguments;
        s.Sub.Name = func.Name;
        s.Sub.Func = func.Callback;
        s.Variable = -1;
        frame[0][func.Name] = s;
    }

    private void ScanCurrentScope(int level, List<string> args, bool addingResult)
    {
        Scanner lex2 = new Scanner(lex);
        try
        {
            Scope currentFrame = frame[frame.Count - 1];
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

            while (cur >= 0 && lex2.Next != TokenKind.TK_end && lex2.Next != TokenKind.TK_invalid)
            {
                switch (lex2.Next)
                {
                    case TokenKind.TK_open_cur:
                        ++cur;
                        lex2.Advance();
                        break;
                    case TokenKind.TK_close_cur:
                        --cur;
                        lex2.Advance();
                        break;
                    case TokenKind.TK_at:
                    case TokenKind.TK_SUB:
                    case TokenKind.TK_FUNCTION:
                    case TokenKind.TK_TASK:
                        {
                            TokenKind type = lex2.Next;
                            lex2.Advance();
                            if (cur == 0)
                            {
                                if (currentFrame.ContainsKey(lex2.Word))
                                {
                                    string error = "Functions and variables of the same name are declared in the same scope.\r\n";
                                    //error += L"(�����X�R�[�v�œ����̃��[�`���������錾����Ă��܂�)"; TODO enable
                                    throw new ParserException(error);
                                }
                                BlockKind kind = (type == TokenKind.TK_SUB || type == TokenKind.TK_at) ? BlockKind.BK_sub :
                                ((type == TokenKind.TK_FUNCTION) ? BlockKind.BK_function : BlockKind.BK_microthread);

                                Symbol s = new Symbol();
                                s.Level = level;
                                s.Sub = engine.NewBlock(level + 1, kind);
                                s.Sub.Name = lex2.Word;
                                s.Sub.Func = null;
                                s.Variable = -1;
                                currentFrame[lex2.Word] = s;
                                lex2.Advance();
                                if (kind != BlockKind.BK_sub && lex2.Next == TokenKind.TK_open_par)
                                {
                                    lex2.Advance();
                                    while (lex2.Next == TokenKind.TK_word || lex2.Next == TokenKind.TK_LET || lex2.Next == TokenKind.TK_REAL)
                                    {
                                        ++(s.Sub.Arguments);
                                        if (lex2.Next == TokenKind.TK_LET || lex2.Next == TokenKind.TK_REAL)
                                        {
                                            lex2.Advance();
                                        }
                                        if (lex2.Next == TokenKind.TK_word)
                                        {
                                            lex2.Advance();
                                        }
                                        if (lex2.Next != TokenKind.TK_comma)
                                        {
                                            break;
                                        }
                                        lex2.Advance();
                                    }
                                }
                            }
                        }
                        break;
                    case TokenKind.TK_REAL:
                    case TokenKind.TK_LET:
                        lex2.Advance();
                        if (cur == 0)
                        {
                            if (currentFrame.ContainsKey(lex2.Word))
                            {
                                string error= "Variables of the same name are declared in the same scope.\r\n";
                                //error += L"(�����X�R�[�v�œ����̕ϐ��������錾����Ă��܂�)"; TODO enable
                                throw new ParserException(error);
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
    private void ParseParentheses(Block block)
    {
        if (lex.Next != TokenKind.TK_open_par)
        {
            string error = "\"(\" is nessasary.\r\n";
            //error += L"(\"(\"���K�v�ł�)"; TODO enable
            throw new ParserException(error);
        }
        lex.Advance();

        ParseExpression(block);

        if (lex.Next != TokenKind.TK_close_par)
        {
            string error = "\")\" is nessasary.\r\n";
            //error += L"(\")\"���K�v�ł�)"; TODO enable
            throw new ParserException(error);
        }
        lex.Advance();
    }
    private void ParseClause(Block block)
    {
        if (lex.Next == TokenKind.TK_real)
        {
            block.Codes.Add(new Code(lex.Line, CommandKind.PC_push_value, new Value(engine.GetRealType(), lex.RealValue)));
            lex.Advance();
        }
        else if (lex.Next == TokenKind.TK_char)
        {
            block.Codes.Add(new Code(lex.Line, CommandKind.PC_push_value, new Value(engine.GetCharType(), lex.CharValue)));
            lex.Advance();
        }
        else if (lex.Next == TokenKind.TK_string)
        {
            string str = lex.StringValue;
            lex.Advance();
            while (lex.Next == TokenKind.TK_string || lex.Next == TokenKind.TK_char)
            {
                str += (lex.Next == TokenKind.TK_string) ? lex.StringValue : lex.CharValue.ToString();
                lex.Advance();
            }

            block.Codes.Add(new Code(lex.Line, CommandKind.PC_push_value, new Value(engine.GetStringType(), str)));
        }
        else if (lex.Next == TokenKind.TK_word)
        {
            Symbol s = Search(lex.Word);
            if (s == null)
            {
                string error = String.Format("{0} is not defined.\r\n", lex.Word);
                //error += StringUtility::FormatToWide("(%s�͖���`�̎��ʎq�ł�)", lex.Word); TODO enable
                throw new ParserException(error);
            }

            lex.Advance();

            if (s.Sub != null)
            {
                if (s.Sub.Kind != BlockKind.BK_function)
                {
                    string error = "sub and task cannot call in the statement.\r\n";
                    //error += L"(sub��task�͎����ŌĂׂ܂���)"; TODO enable
                    throw new ParserException(error);
                }

                int argc = ParseArguments(block);

                if (argc != s.Sub.Arguments)
                {
                    string error = String.Format(
                        "{0} incorrect number of parameters. Check to make sure you have the correct number of parameters.\r\n", 
                        s.Sub.Name);
                    //error += StringUtility::FormatToWide("(%s�̈����̐����Ⴂ�܂�)", s->sub->name); TODO enable
                    throw new ParserException(error);
                }

                block.Codes.Add(new Code(lex.Line, CommandKind.PC_call_and_push_result, s.Sub, argc));
            }
            else
            {
                //�ϐ�
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_push_variable, s.Level, s.Variable));
            }
        }
        else if (lex.Next == TokenKind.TK_open_bra)
        {
            lex.Advance();
            block.Codes.Add(new Code(lex.Line, CommandKind.PC_push_value, new Value(engine.GetStringType(), "")));
            while(lex.Next != TokenKind.TK_close_bra)
            {
                ParseExpression(block);
                WriteOperation(block, "append", 2);
                if (lex.Next != TokenKind.TK_comma)
                {
                    break;
                }
                lex.Advance();
            }
            if (lex.Next != TokenKind.TK_close_bra)
            {
                string error = "\"]\" is nessasary.\r\n";
                //error += L"(\"]\"���K�v�ł�)"; TODO enable
                throw new ParserException(error);
            }
            lex.Advance();
        }
        else if (lex.Next == TokenKind.TK_open_abs)
        {
            lex.Advance();
            ParseExpression(block);
            WriteOperation(block, "absolute", 1);
            if (lex.Next != TokenKind.TK_close_abs)
            {
                string error = "\"|\" is nessasary.\r\n";
                //error += L"(\"|)\"���K�v�ł�)"; TODO enable
                throw new ParserException(error);
            }
            lex.Advance();
        }
        else if (lex.Next == TokenKind.TK_open_par)
        {
            ParseParentheses(block);
        }
        else
        {
            string error = "Invalid expression.\r\n";
            //error += L"(���Ƃ��Ė����Ȏ�������܂�)"; TODO enable
            throw new ParserException(error);
        }
    }
    private void ParseSuffix(Block block)
    {
        ParseClause(block);
        if (lex.Next == TokenKind.TK_caret)
        {
            lex.Advance();
            ParseSuffix(block); //�ċA
            WriteOperation(block, "power", 2);
        }
        else
        {
            while (lex.Next == TokenKind.TK_open_bra)
            {
                lex.Advance();
                ParseExpression(block);

                if (lex.Next == TokenKind.TK_range)
                {
                    lex.Advance();
                    ParseExpression(block);
                    WriteOperation(block, "slice", 3);
                }
                else
                {
                    WriteOperation(block, "index_", 2);
                }

                if (lex.Next != TokenKind.TK_close_bra)
                {
                    string error = "\"]\" is nessasary.\r\n";
                    //error += L"(\"]\"���K�v�ł�)"; TODO enable
                    throw new ParserException(error);
                }
                lex.Advance();
            }
        }
    }
    private void ParsePrefix(Block block)
    {
        if (lex.Next == TokenKind.TK_plus)
        {
            lex.Advance();
            ParsePrefix(block);	//�ċA
        }
        else if (lex.Next == TokenKind.TK_minus)
        {
            lex.Advance();
            ParsePrefix(block);	//�ċA
            WriteOperation(block, "negative", 1);
        }
        else if (lex.Next == TokenKind.TK_exclamation)
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
        while (lex.Next == TokenKind.TK_asterisk || lex.Next == TokenKind.TK_slash || lex.Next == TokenKind.TK_percent)
        {
            string name = (lex.Next == TokenKind.TK_asterisk) ? "multiply" : ((lex.Next == TokenKind.TK_slash) ? "divide" : "remainder");
            lex.Advance();
            ParsePrefix(block);
            WriteOperation(block, name, 2);
        }
    }
    private void ParseSum(Block block)
    {
        ParsePoduct(block);
        while (lex.Next == TokenKind.TK_tilde || lex.Next == TokenKind.TK_plus || lex.Next == TokenKind.TK_minus)
        {
            string name = (lex.Next == TokenKind.TK_tilde) ? "concatenate" : ((lex.Next == TokenKind.TK_plus) ? "add" : "subtract");
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
            case TokenKind.TK_assign:
            {
                string error = "Do you not mistake it for \"==\"?\r\n";
                //error += L"(\"==\"�ƊԈႦ�Ă܂��񂩁H)"; TODO enable
                throw new ParserException(error);
            }

            case TokenKind.TK_e:
            case TokenKind.TK_g:
            case TokenKind.TK_ge:
            case TokenKind.TK_l:
            case TokenKind.TK_le:
            case TokenKind.TK_ne:
                TokenKind op = lex.Next;
                lex.Advance();
                ParseSum(block);
                WriteOperation(block, "compare", 2);
                switch (op)
                {
                    case TokenKind.TK_e:
                        block.Codes.Add(new Code(lex.Line, CommandKind.PC_compare_e));
                        break;
                    case TokenKind.TK_g:
                        block.Codes.Add(new Code(lex.Line, CommandKind.PC_compare_g));
                        break;
                    case TokenKind.TK_ge:
                        block.Codes.Add(new Code(lex.Line, CommandKind.PC_compare_ge));
                        break;
                    case TokenKind.TK_l:
                        block.Codes.Add(new Code(lex.Line, CommandKind.PC_compare_l));
                        break;
                    case TokenKind.TK_le:
                        block.Codes.Add(new Code(lex.Line, CommandKind.PC_compare_le));
                        break;
                    case TokenKind.TK_ne:
                        block.Codes.Add(new Code(lex.Line, CommandKind.PC_compare_ne));
                        break;
                }
                break;
        }
    }
    private void ParseLogic(Block block)
    {
        ParseComparison(block);
        while (lex.Next == TokenKind.TK_and_then || lex.Next == TokenKind.TK_or_else)
        {
            CommandKind cmd = (lex.Next == TokenKind.TK_and_then) ? CommandKind.PC_case_if_not : CommandKind.PC_case_if;
            lex.Advance();

            block.Codes.Add(new Code(lex.Line, CommandKind.PC_dup));
            block.Codes.Add(new Code(lex.Line, CommandKind.PC_case_begin));
            block.Codes.Add(new Code(lex.Line, cmd));
            block.Codes.Add(new Code(lex.Line, CommandKind.PC_pop));

            ParseComparison(block);

            block.Codes.Add(new Code(lex.Line, CommandKind.PC_case_end));
        }
    }
    private void ParseExpression(Block block)
    {
        ParseLogic(block);
    }
    private int ParseArguments(Block block)
    {
        int result = 0;
        if (lex.Next == TokenKind.TK_open_par)
        {
            lex.Advance();
            while (lex.Next != TokenKind.TK_close_par)
            {
                ++result;
                ParseExpression(block);
                if (lex.Next != TokenKind.TK_comma)
                {
                    break;
                }
                lex.Advance();
            }
            if (lex.Next != TokenKind.TK_close_par)
            {
                string error = "\")\" is nessasary.\r\n";
                //error += L"(\")\"���K�v�ł�)"; TODO enable
                throw new ParserException(error);
            }
            lex.Advance();
        }
        return result;
    }
    private void ParseStatements(Block block)
    {
        for( ; ; )
        {
            bool needSemicolon = true;

            if (lex.Next == TokenKind.TK_word)
            {
                Symbol s = Search(lex.Word);
                if (s == null)
                { 
                    string error = String.Format("{0} is not defined.\r\n", lex.Word);
                    //eror += StringUtility::FormatToWide("(%s�͖���`�̎��ʎq�ł�)", lex.Word); TODO enable
                    throw new ParserException(error);
                }
                lex.Advance();
                switch (lex.Next)
                {
                    case TokenKind.TK_assign:
                        lex.Advance();
                        ParseExpression(block);
                        block.Codes.Add(new Code(lex.Line, CommandKind.PC_assign, s.Level, s.Variable));
                        break;

                    case TokenKind.TK_open_bra:
                        block.Codes.Add(new Code(lex.Line, CommandKind.PC_push_variable_writable, s.Level, s.Variable));
                        lex.Advance();
                        ParseExpression(block);
                        if (lex.Next != TokenKind.TK_close_bra)
                        {
                            string error = "\"]\" is nessasary.\r\n";
                            //error += L"(\"]\"���K�v�ł�)"; TODO enable
                            throw new ParserException(error);
                        }
                        lex.Advance();
                        WriteOperation(block, "index!", 2);
                        if (lex.Next != TokenKind.TK_assign)
                        {
                            string error = "\"=\" is nessasary.\r\n";
                            //error += L"(\"=\"���K�v�ł�)"; TODO enable
                            throw new ParserException(error);
                        }
                        lex.Advance();
                        ParseExpression(block);
                        block.Codes.Add(new Code(lex.Line, CommandKind.PC_assign_writable));
                        break;

                    case TokenKind.TK_add_assign:
                    case TokenKind.TK_subtract_assign:
                    case TokenKind.TK_multiply_assign:
                    case TokenKind.TK_divide_assign:
                    case TokenKind.TK_remainder_assign:
                    case TokenKind.TK_power_assign:
                        {
                            string f = "";
                            switch(lex.Next)
                            {
                                case TokenKind.TK_add_assign:
                                    f = "add";
                                    break;
                                case TokenKind.TK_subtract_assign:
                                    f = "subtract";
                                    break;
                                case TokenKind.TK_multiply_assign:
                                    f = "multiply";
                                    break;
                                case TokenKind.TK_divide_assign:
                                    f = "divide";
                                    break;
                                case TokenKind.TK_remainder_assign:
                                    f = "remainder";
                                    break;
                                case TokenKind.TK_power_assign:
                                    f = "power";
                                    break;
                            }
                            lex.Advance();

                            block.Codes.Add(new Code(lex.Line, CommandKind.PC_push_variable, s.Level, s.Variable));

                            ParseExpression(block);
                            WriteOperation(block, f, 2);

                            block.Codes.Add(new Code(lex.Line, CommandKind.PC_assign, s.Level, s.Variable));
                        }
                        break;

                    case TokenKind.TK_inc:
                    case TokenKind.TK_dec:
                        {
                            string f = (lex.Next == TokenKind.TK_inc) ? "successor" : "predecessor";
                            lex.Advance();

                            block.Codes.Add(new Code(lex.Line, CommandKind.PC_push_variable, s.Level, s.Variable));
                            WriteOperation(block, f, 1);
                            block.Codes.Add(new Code(lex.Line, CommandKind.PC_assign, s.Level, s.Variable));
                        }
                        break;
                    default:
                        if (s.Sub == null)
                        {
                            string error = "You cannot call a variable as if it were a function or a subroutine.\r\n";
                            //error += L"(�ϐ��͊֐���sub�̂悤�ɂ͌Ăׂ܂���)"; TODO enable
                            throw new ParserException(error);
                        }

                        int argc = ParseArguments(block);

                        if (argc != s.Sub.Arguments)
                        {
                            string error = String.Format("{0} incorrect number of parameters. Check to make sure you have the correct number of parameters.\r\n", 
                                s.Sub.Name);
                            //error += StringUtility::FormatToWide("(%s�̈����̐����Ⴂ�܂�)", s->sub->name); TODO enable
                            throw new ParserException(error);
                        }

                        block.Codes.Add(new Code(lex.Line, CommandKind.PC_call, s.Sub, argc));
                        break;
                }
            }
            else if (lex.Next == TokenKind.TK_LET || lex.Next == TokenKind.TK_REAL)
            {
                lex.Advance();

                if (lex.Next != TokenKind.TK_word)
                {
                    string error = "Symbol name is nessasary.\r\n";
                    //error += L"(���ʎq���K�v�ł�)"; TODO enable
                    throw new ParserException(error);
                }

                Symbol s = Search(lex.Word);

                lex.Advance();
                if (lex.Next == TokenKind.TK_assign)
                {
                    lex.Advance();
                    ParseExpression(block);
                    block.Codes.Add(new Code(lex.Line, CommandKind.PC_assign, s.Level, s.Variable));
                }
            }
            else if (lex.Next == TokenKind.TK_LOCAL)
            {
                lex.Advance();
                ParseInlineBlock(block, BlockKind.BK_normal);
                needSemicolon = false;
            }
            else if (lex.Next == TokenKind.TK_LOOP)
            {
                lex.Advance();
                if (lex.Next == TokenKind.TK_open_par)
                {
                    ParseParentheses(block);
                    int ip = block.Codes.Count;
                    block.Codes.Add(new Code(lex.Line, CommandKind.PC_loop_count));
                    ParseInlineBlock(block, BlockKind.BK_loop);
                    block.Codes.Add(new Code(lex.Line, CommandKind.PC_loop_back, ip));
                    block.Codes.Add(new Code(lex.Line, CommandKind.PC_pop));
                }
                else
                {
                    int ip = block.Codes.Count;
                    ParseInlineBlock(block, BlockKind.BK_loop);
                    block.Codes.Add(new Code(lex.Line, CommandKind.PC_loop_back, ip));
                }
                needSemicolon = false;
            }
            else if (lex.Next == TokenKind.TK_TIMES)
            {
                lex.Advance();
                ParseParentheses(block);
                int ip = block.Codes.Count;
                if (lex.Next == TokenKind.TK_LOOP)
                {
                    lex.Advance();
                }
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_loop_count));
                ParseInlineBlock(block, BlockKind.BK_loop);
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_loop_back, ip));
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_pop));
                needSemicolon = false;
            }
            else if (lex.Next == TokenKind.TK_WHILE)
            {
                lex.Advance();
                int ip = block.Codes.Count;
                ParseParentheses(block);
                if (lex.Next == TokenKind.TK_LOOP)
                {
                    lex.Advance();
                }
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_loop_if));
                ParseInlineBlock(block, BlockKind.BK_loop);
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_loop_back, ip));
                needSemicolon = false;
            }
            else if (lex.Next == TokenKind.TK_ASCENT || lex.Next == TokenKind.TK_DESCENT)
            {
                bool back = lex.Next == TokenKind.TK_DESCENT;
                lex.Advance();

                if (lex.Next != TokenKind.TK_open_par)
                {
                    string error = "\"(\" is nessasary.\r\n";
                    //error += L"(\"(\"���K�v�ł�)"; TODO enable
                    throw new ParserException(error);
                }
                lex.Advance();

                if (lex.Next == TokenKind.TK_LET || lex.Next == TokenKind.TK_REAL)
                {
                    lex.Advance();
                }

                if (lex.Next != TokenKind.TK_word)
                {
                    string error = "The symbol name is nessasary.\r\n";
                    //error += L"(���ʎq���K�v�ł�)"; TODO enable
                    throw new ParserException(error);
                }

                string s = lex.Word;

                lex.Advance();

                if (lex.Next != TokenKind.TK_IN)
                {
                    string error = "\"in\" is nessasary.\r\n";
                    //error += L"(in���K�v�ł�)"; TODO enable
                    throw new ParserException(error);
                }
                lex.Advance();

                ParseExpression(block);

                if (lex.Next != TokenKind.TK_range)
                {
                    string error = "\"..\" is nessasary.\r\n";
                    //error += L"(\"..\"���K�v�ł�)"; TODO enable
                    throw new ParserException(error);
                }
                lex.Advance();

                ParseExpression(block);

                if (lex.Next != TokenKind.TK_close_par)
                {
                    string error = "\")\" is nessasary.\r\n";
                    //error += L"(\")\"���K�v�ł�)"; TODO enable
                    throw new ParserException(error);
                }
                lex.Advance();

                if (lex.Next == TokenKind.TK_LOOP)
                {
                    lex.Advance();
                }

                if (!back)
                {
                    block.Codes.Add(new Code(lex.Line, CommandKind.PC_swap));
                }

                int ip = block.Codes.Count;

                block.Codes.Add(new Code(lex.Line, CommandKind.PC_dup2));
                WriteOperation(block, "compare", 2);

                block.Codes.Add(new Code(lex.Line, back ? CommandKind.PC_loop_descent : CommandKind.PC_loop_ascent));

                if (back)
                {
                    WriteOperation(block, "predecessor", 1);
                }

                Block b = engine.NewBlock(block.Level + 1, BlockKind.BK_loop);
                List<string> counter = new List<string>();
                counter.Add(s);
                ParseBlock(b, counter, false);
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_dup));
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_call, b, 1));

                if (!back)
                {
                    WriteOperation(block, "successor", 1);
                }

                block.Codes.Add(new Code(lex.Line, CommandKind.PC_loop_back, ip));
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_pop));
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_pop));

                needSemicolon = false;
            }
            else if (lex.Next == TokenKind.TK_IF)
            {
                lex.Advance();
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_case_begin));

                ParseParentheses(block);
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_case_if_not));
                ParseInlineBlock(block, BlockKind.BK_normal);
                while(lex.Next == TokenKind.TK_ELSE)
                {
                    lex.Advance();
                    block.Codes.Add(new Code(lex.Line, CommandKind.PC_case_next));
                    if (lex.Next == TokenKind.TK_IF)
                    {
                        lex.Advance();
                        ParseParentheses(block);
                        block.Codes.Add(new Code(lex.Line, CommandKind.PC_case_if_not));
                        ParseInlineBlock(block, BlockKind.BK_normal);
                    }
                    else
                    {
                        ParseInlineBlock(block, BlockKind.BK_normal);
                        break;
                    }
                }

                block.Codes.Add(new Code(lex.Line, CommandKind.PC_case_end));
                needSemicolon = false;
            }
            else if (lex.Next == TokenKind.TK_ALTERNATIVE)
            {
                lex.Advance();
                ParseParentheses(block);
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_case_begin));
                while (lex.Next == TokenKind.TK_CASE)
                {
                    lex.Advance();

                    if (lex.Next != TokenKind.TK_open_par)
                    {
                        string error = "\"(\" is nessasary.\r\n";
                        //error += L"(\"(\"���K�v�ł�)"; TODO enable
                        throw new ParserException(error);
                    }
                    block.Codes.Add(new Code(lex.Line, CommandKind.PC_case_begin));
                    do
                    {
                        lex.Advance();

                        block.Codes.Add(new Code(lex.Line, CommandKind.PC_dup));
                        ParseExpression(block);
                        WriteOperation(block, "compare", 2);
                        block.Codes.Add(new Code(lex.Line, CommandKind.PC_compare_e));
                        block.Codes.Add(new Code(lex.Line, CommandKind.PC_dup));
                        block.Codes.Add(new Code(lex.Line, CommandKind.PC_case_if));
                        block.Codes.Add(new Code(lex.Line, CommandKind.PC_pop));

                    }
                    while (lex.Next == TokenKind.TK_comma);
                    block.Codes.Add(new Code(lex.Line, CommandKind.PC_push_value, new Value(engine.GetBooleanType(), false)));
                    block.Codes.Add(new Code(lex.Line, CommandKind.PC_case_end));
                    if (lex.Next != TokenKind.TK_close_par)
                    {
                        string error = "\")\" is nessasary.\r\n";
                        //error += L"(\")\"���K�v�ł�)"; TODO enable
                        throw new ParserException(error);
                    }
                    lex.Advance();

                    block.Codes.Add(new Code(lex.Line, CommandKind.PC_case_if_not));
                    block.Codes.Add(new Code(lex.Line, CommandKind.PC_pop));
                    ParseInlineBlock(block, BlockKind.BK_normal);
                    block.Codes.Add(new Code(lex.Line, CommandKind.PC_case_next));
                }
                if (lex.Next == TokenKind.TK_OTHERS)
                {
                    lex.Advance();
                    block.Codes.Add(new Code(lex.Line, CommandKind.PC_pop));
                    ParseInlineBlock(block, BlockKind.BK_normal);
                }
                else
                {
                    block.Codes.Add(new Code(lex.Line, CommandKind.PC_pop));
                }
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_case_end));
                needSemicolon = false;
            }
            else if (lex.Next == TokenKind.TK_BREAK)
            {
                lex.Advance();
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_break_loop));
            }
            else if (lex.Next == TokenKind.TK_RETURN)
            {
                lex.Advance();
                switch (lex.Next)
                {
                    case TokenKind.TK_end:
                    case TokenKind.TK_invalid:
                    case TokenKind.TK_semicolon:
                    case TokenKind.TK_close_cur:
                        break;
                    default:
                        ParseExpression(block);
                        Symbol s = SearchResult();
                        if (s == null)
                        {
                            string error = "\"return\" can call in function only.\r\n";
                            //error += L"(������function�̒��ł͂���܂���)"; TODO enable
                            throw new ParserException(error);
                        }

                        block.Codes.Add(new Code(lex.Line, CommandKind.PC_assign, s.Level, s.Variable));
                        break;
                }
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_break_routine));
            }
            else if (lex.Next == TokenKind.TK_YIELD)
            {
                lex.Advance();
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_yield));
            }
            else if (lex.Next == TokenKind.TK_at || lex.Next == TokenKind.TK_SUB || lex.Next == TokenKind.TK_FUNCTION || lex.Next == TokenKind.TK_TASK)
            {
                bool is_event = lex.Next == TokenKind.TK_at;

                lex.Advance();
                if (lex.Next != TokenKind.TK_word)
                {
                    string error = "Symbol name is nessasary.\r\n";
                    //error += L"(���ʎq���K�v�ł�)"; TODO enable
                    throw new ParserException(error);
                }

                Symbol s = Search(lex.Word);

                if (is_event)
                {
                    if (s.Sub.Level > 1)
                    {
                        string error = "\"@\" cannot use in inner function and task.\r\n";
                        //error += L"(�C�x���g��[���K�w�ɋL�q���邱�Ƃ͂ł��܂���)"; TODO enable
                        throw new ParserException(error);
                    }
                    events[s.Sub.Name] = s.Sub;
                }

                lex.Advance();

                List<string> args = new List<string>();
                if (s.Sub.Kind != BlockKind.BK_sub)
                {
                    if (lex.Next == TokenKind.TK_open_par)
                    {
                        lex.Advance();
                        while (lex.Next == TokenKind.TK_word || lex.Next == TokenKind.TK_LET || lex.Next == TokenKind.TK_REAL)
                        {
                            if (lex.Next == TokenKind.TK_LET || lex.Next == TokenKind.TK_REAL)
                            {
                                lex.Advance();
                                if (lex.Next != TokenKind.TK_word)
                                {
                                    string error = "Function parameter is nessasary.\r\n";
                                    //error += L"(���������K�v�ł�)"; TODO enable
                                    throw new ParserException(error);
                                }
                            }
                            args.Add(lex.Word);
                            lex.Advance();
                            if (lex.Next != TokenKind.TK_comma)
                            {
                                break;
                            }
                            lex.Advance();
                        }
                        if (lex.Next != TokenKind.TK_close_par)
                        {
                            string error = "\")\" is nessasary.\r\n";
                            //error += L"(\")\"���K�v�ł�)"; TODO enable
                            throw new ParserException(error);
                        }
                        lex.Advance();
                    }
                }
                else
                {
                    //�݊����̂��ߋ�̊��ʂ�������
                    if (lex.Next == TokenKind.TK_open_par)
                    {
                        lex.Advance();
                        if (lex.Next != TokenKind.TK_close_par)
                        {
                            string error = "\")\" is nessasary.\r\n";
                            //error += L"(\")\"���K�v�c�Ƃ�����\"(\"�v���ł�)"; TODO enable
                            throw new ParserException(error);
                        }
                        lex.Advance();
                    }
                }
                ParseBlock(s.Sub, args, s.Sub.Kind == BlockKind.BK_function);
                needSemicolon = false;
            }

            //�Z�~�R�����������ƌp�����Ȃ�
            if (needSemicolon && lex.Next != TokenKind.TK_semicolon)
            {
                break;
            }

            if (lex.Next == TokenKind.TK_semicolon)
            {
                lex.Advance();
            }
        }
    }
    private Symbol Search(string name)
    {
        for (int i = frame.Count - 1; i >= 0; --i)
        {
            if (frame[i].ContainsKey(name))
            {
                return frame[i][name];
            }
        }
        return null;
    }
    private Symbol SearchResult()
    {
        for (int i = frame.Count - 1; i >= 0; --i)
        {
            if (frame[i].ContainsKey("result"))
            {
                return frame[i]["result"];
            }
            if (frame[i].Kind == BlockKind.BK_sub || frame[i].Kind == BlockKind.BK_microthread)
            {
                return null;
            }
        }
        return null;
    }

    private void WriteOperation(Block block, string name, int clauses)
    {
        Symbol s = Search(name);
        Debug.Assert(s != null);
        if (s.Sub.Arguments != clauses)
        {
            string error = "Overwriting function does not allow to different argument count.\r\n";
            //error += L"(���Z�q�ɑΉ�����֐����㏑����`����܂����������̐����Ⴂ�܂�)"; TODO enable
            throw new ParserException(error);
        }

        block.Codes.Add(new Code(lex.Line, CommandKind.PC_call_and_push_result, s.Sub, clauses));
    }
    private void ParseInlineBlock(Block block, BlockKind kind)
    {
        Block b = engine.NewBlock(block.Level + 1, kind);
        ParseBlock(b, null, false);
        block.Codes.Add(new Code(lex.Line, CommandKind.PC_call, b, 0));
    }
    private void ParseBlock(Block block, List<string> args, bool addingResult)
    {
        if (lex.Next != TokenKind.TK_open_cur)
        {
            string error = "\"{\" is nessasary.\r\n";
            //error += L"(\"{\"���K�v�ł�)"; TODO enable
            throw new ParserException(error);
        }
        lex.Advance();

        frame.Add(new Scope(block.Kind));

        ScanCurrentScope(block.Level, args, addingResult);

        if (args != null)
        {
            foreach (string arg in args)
            {
                Symbol s = Search(arg);
                block.Codes.Add(new Code(lex.Line, CommandKind.PC_assign, s.Level, s.Variable));
            }
        }
        ParseStatements(block);

        frame.RemoveAt(frame.Count - 1);

        if (lex.Next != TokenKind.TK_close_cur)
        {
            string error = "\"}\" is nessasary.\r\n";
            //error += L"(\"}\"���K�v�ł�)"; TODO enable
            throw new ParserException(error);
        }
        lex.Advance();
    }
}

}

}
