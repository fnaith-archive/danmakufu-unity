using System;

namespace Gstd
{
    
namespace Script
{
class Scanner
{
    private char[] source;
    private int current;
    private TokenKind next;
    private string word;
    private int line;
    private double realValue;
    private char charValue;
    private string stringValue;
    public TokenKind Next
    {
        get => next;
    }
    public string Word
    {
        get => word;
    }
    public int Line
    {
        get => line;
        set => line = value;
    }
    public double RealValue
    {
        get => realValue;
    }
    public char CharValue
    {
        get => charValue;
    }
    public string StringValue
    {
        get => stringValue;
    }
    public Scanner(char[] source)
    {
        this.source = source;
        this.current = 0;
        this.line = 1;
        Advance();
    }
    public Scanner(Scanner source)
    {
        this.source = source.source;
        this.current = source.current;
        this.next = source.next;
        this.word = source.word;
        this.line = source.line;
    }
    public char CurrentChar()
    {
        if (current >= source.Length)
        {
            return '\0';
        }
        return source[current];// TODO fix comment on last line, will cause current become negative
    }
    public char IndexFromCurrentChar(int index)
    {
        int pos = current + index;
        if (pos >= source.Length)
        {
            return '\0';
        }
        return source[pos];
    }
    public char NextChar()
    {
        ++current; // TODO check
        return CurrentChar();
    }
    public void Skip()
    {
        char ch1 = CurrentChar();
        char ch2 = IndexFromCurrentChar(1);
        while (ch1 == '\r' || ch1 == '\n' || ch1 == '\t' || ch1 == ' ' || ch1 == '#' || (ch1 == '/' && (ch2 == '/' || ch2 == '*')))
        {
            if (ch1 == '#' || (ch1 == '/' && (ch2 == '/' || ch2 == '*')))
            {
                if (ch1 == '#' || ch2 == '/')
                {
                    do
                    {
                        ch1 = NextChar();
                    }
                    while (ch1 != '\r' && ch1 != '\n');
                }
                else
                {
                    NextChar();
                    ch1 = NextChar();
                    ch2 = IndexFromCurrentChar(1);
                    while (ch1 != '*' || ch2 != '/')
                    {
                        if (ch1 == '\n')
                        {
                            ++line;
                        }
                        ch1 = NextChar();
                        ch2 = IndexFromCurrentChar(1);
                    }
                    ch1 = NextChar();
                    ch1 = NextChar();
                }
            }
            else if (ch1 == '\n')
            {
                ++line;
                ch1 = NextChar();
            }
            else
            {
                ch1 = NextChar();
            }
            ch2 = IndexFromCurrentChar(1);
        }
    }
    public void Advance()
    {
        Skip();

        char ch = CurrentChar();
        if (ch == '\0' || current >= source.Length)
        {
            next = TokenKind.TK_end;
            return;
        }

        switch (ch)
        {
            case '[':
                next = TokenKind.TK_open_bra;
                ch = NextChar();
                break;
            case ']':
                next = TokenKind.TK_close_bra;
                ch = NextChar();
                break;
            case '(':
                next = TokenKind.TK_open_par;
                ch = NextChar();
                if (ch == '|')
                {
                    next = TokenKind.TK_open_abs;
                    ch = NextChar();
                }
                break;
            case ')':
                next = TokenKind.TK_close_par;
                ch = NextChar();
                break;
            case '{':
                next = TokenKind.TK_open_cur;
                ch = NextChar();
                break;
            case '}':
                next = TokenKind.TK_close_cur;
                ch = NextChar();
                break;
            case '@':
                next = TokenKind.TK_at;
                ch = NextChar();
                break;
            case ',':
                next = TokenKind.TK_comma;
                ch = NextChar();
                break;
            case ';':
                next = TokenKind.TK_semicolon;
                ch = NextChar();
                break;
            case '~':
                next = TokenKind.TK_tilde;
                ch = NextChar();
                break;
            case '*':
                next = TokenKind.TK_asterisk;
                ch = NextChar();
                if (ch == '=')
                {
                    next = TokenKind.TK_multiply_assign;
                    ch = NextChar();
                }
                break;
            case '/':
                next = TokenKind.TK_slash;
                ch = NextChar();
                if (ch == '=')
                {
                    next = TokenKind.TK_divide_assign;
                    ch = NextChar();
                }
                break;
            case '%':
                next = TokenKind.TK_percent;
                ch = NextChar();
                if (ch == '=')
                {
                    next = TokenKind.TK_remainder_assign;
                    ch = NextChar();
                }
                break;
            case '^':
                next = TokenKind.TK_caret;
                ch = NextChar();
                if (ch == '=')
                {
                    next = TokenKind.TK_power_assign;
                    ch = NextChar();
                }
                break;
            case '=':
                next = TokenKind.TK_assign;
                ch = NextChar();
                if (ch == '=')
                {
                    next = TokenKind.TK_e;
                    ch = NextChar();
                }
                break;
            case '>':
                next = TokenKind.TK_g;
                ch = NextChar();
                if (ch == '=')
                {
                    next = TokenKind.TK_ge;
                    ch = NextChar();
                }
                break;
            case '<':
                next = TokenKind.TK_l;
                ch = NextChar();
                if (ch == '=')
                {
                    next = TokenKind.TK_le;
                    ch = NextChar();
                }
                break;
            case '!':
                next = TokenKind.TK_exclamation;
                ch = NextChar();
                if (ch == '=')
                {
                    next = TokenKind.TK_ne;
                    ch = NextChar();
                }
                break;
            case '+':
                next = TokenKind.TK_plus;
                ch = NextChar();
                if (ch == '+')
                {
                    next = TokenKind.TK_inc;
                    ch = NextChar();
                }
                else if (ch == '=')
                {
                    next = TokenKind.TK_add_assign;
                    ch = NextChar();
                }
                break;
            case '-':
                next = TokenKind.TK_minus;
                ch = NextChar();
                if (ch == '-')
                {
                    next = TokenKind.TK_dec;
                    ch = NextChar();
                }
                else if (ch == '=')
                {
                    next = TokenKind.TK_subtract_assign;
                    ch = NextChar();
                }
                break;
            case '&':
                next = TokenKind.TK_ampersand;
                ch = NextChar();
                if (ch == '&')
                {
                    next = TokenKind.TK_and_then;
                    ch = NextChar();
                }
                break;
            case '|':
                next = TokenKind.TK_vertical;
                ch = NextChar();
                if (ch == '|')
                {
                    next = TokenKind.TK_or_else;
                    ch = NextChar();
                }
                else if (ch == ')')
                {
                    next = TokenKind.TK_close_abs;
                    ch = NextChar();
                }
                break;
            case '.':
                ch = NextChar();
                if (ch == '.')
                {
                    next = TokenKind.TK_range;
                    ch = NextChar();
                }
                else
                {
                    string error = "It's script does not allow to alone period\r\n";
                    //error += L"(�P�Ƃ̃s���I�h�͂��̃X�N���v�g�ł͎g���܂���)"; TODO fix
                    throw new ParserError(error);
                }
                break;
            case '\'':
            case '\"':
                {
                    char q = CurrentChar();
                    next = (q == '\"') ? TokenKind.TK_string : TokenKind.TK_char;
                    ch = NextChar();
                    char pre = ch;
                    string s = "";
                    while(true)
                    {
                        if (ch == q && pre != '\\')
                        {
                            break;
                        }

                        if (ch == '\\')
                        {
                            if (pre == '\\')
                            {
                                s += ch;
                            }
                        }
                        else
                        {
                            s += ch;
                        }
        
                        pre = ch;
                        ch = NextChar();
                    }
                    ch = NextChar();
                    stringValue = s;

                    if (q == '\'')
                    {
                        if (stringValue.Length == 1)
                        {
                            charValue = stringValue[0];
                        }
                        else
                        {
                            throw new ParserError("???"); //TODO fix L"�����^�̒l�̒�����1�����ł�");
                        }
                    }
                }
                break;
            case '\\':
                {
                    ch = NextChar();
                    next = TokenKind.TK_char;
                    char c = ch;
                    ch = NextChar();
                    switch (c)
                    {
                        case '0':
                            charValue = '\0';
                            break;
                        case 'n':
                            charValue = '\n';
                            break;
                        case 'r':
                            charValue = '\r';
                            break;
                        case 't':
                            charValue = '\t';
                            break;
                        case 'x':
                            charValue = '\0';
                            while (IsXDigit(ch))
                            {
                                charValue = (char)(charValue * 16 + ((ch >= 'a') ? (ch - 'a' + 10) : ((ch >= 'A') ? ch - 'A' + 10 : ch - '0')));
                                ch = NextChar();
                            }
                            break;
                        default:
                            {
                                string error = "There is a strange character.\r\n";
                                //error += L"���ꕶ�����ςł�(�u\"...\"�v��Y��Ă��܂���)"; TODO fix
                                throw new ParserError(error);
                            }
                    }
                }
                break;
            default:
                if (char.IsDigit(ch))
                {
                    next = TokenKind.TK_real;
                    realValue = 0.0;
                    do
                    {
                        realValue = realValue * 10 + (ch - '0');
                        ch = NextChar();
                    }
                    while (char.IsDigit(ch));

                    char ch2 = IndexFromCurrentChar(1);
                    if (ch == '.' && Char.IsDigit(ch2))
                    {
                        ch = NextChar();
                        double d = 1;
                        while (char.IsDigit(ch))
                        {
                            d = d / 10;
                            realValue = realValue + d * (ch - '0');
                            ch = NextChar();
                        }
                    }
                }
                else if (Char.IsLetter(ch) || ch == '_')
                {
                    next = TokenKind.TK_word;
                    word = "";
                    do
                    {
                        word += ch;
                        ch = NextChar();
                    }
                    while (Char.IsLetter(ch) || ch == '_' || char.IsDigit(ch));
                    
                    if (word == "alternative")
                        next = TokenKind.TK_ALTERNATIVE;
                    else if (word == "ascent")
                        next = TokenKind.TK_ASCENT;
                    else if (word == "break")
                        next = TokenKind.TK_BREAK;
                    else if (word == "case")
                        next = TokenKind.TK_CASE;
                    else if (word == "descent")
                        next = TokenKind.TK_DESCENT;
                    else if (word == "else")
                        next = TokenKind.TK_ELSE;
                    else if (word == "function")
                        next = TokenKind.TK_FUNCTION;
                    else if (word == "if")
                        next = TokenKind.TK_IF;
                    else if (word == "in")
                        next = TokenKind.TK_IN;
                    else if (word == "let" || word == "var")
                        next = TokenKind.TK_LET;
                    else if (word == "local")
                        next = TokenKind.TK_LOCAL;
                    else if (word == "loop")
                        next = TokenKind.TK_LOOP;
                    else if (word == "others")
                        next = TokenKind.TK_OTHERS;
                    else if (word == "real")
                        next = TokenKind.TK_REAL;
                    else if (word == "return")
                        next = TokenKind.TK_RETURN;
                    else if (word == "sub")
                        next = TokenKind.TK_SUB;
                    else if (word == "task")
                        next = TokenKind.TK_TASK;
                    else if (word == "times")
                        next = TokenKind.TK_TIMES;
                    else if (word == "while")
                        next = TokenKind.TK_WHILE;
                    else if (word == "yield")
                        next = TokenKind.TK_YIELD;
                }
                else
                {
                    next = TokenKind.TK_invalid;
                }
                break;
        }
    }
    private static bool IsXDigit(char c)
    {
        if ('0' <= c && c <= '9') return true;
        if ('a' <= c && c <= 'f') return true;
        if ('A' <= c && c <= 'F') return true;
        return false;
    }
}

}

}
