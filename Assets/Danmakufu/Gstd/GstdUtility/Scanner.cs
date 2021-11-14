using System;
using System.Collections.Generic;

namespace Gstd
{
    namespace GstdUtility
    {
        sealed class Scanner
        {
            private char[] buffer;
            private int pointer;//���̈ʒu
            private Token token = new Token();
            private bool bPermitSignNumber;
            private List<Token> listDebugToken = new List<Token>();

            private char _CurrentChar()
            {
                char res = '\0';
                if (pointer < buffer.Length)
                {
                    char ch = buffer[pointer];
                    res = ch;
                }
                return res;
            }
            private char _NextChar()
            {
                if (HasNext() == false)
                {
                    //Logger::WriteTop(L"�I�[�ُ픭��->"); TODO

                    int size = buffer.Length;
                    string source = GetString(0, size);
                    string target = " -> \r\n" + source + "...";//StringUtility::Format(L"�����͑Ώ� -> \r\n%s...", source.c_str());
                    //Logger::WriteTop(target); TODO

                    int index = 1;
                    foreach (Token token in listDebugToken)
                    {
                        // TODO
                        //std::wstring log = StringUtility::Format(L"  %2d token -> type=%2d, element=%s, start=%d, end=%d",
                        //    index, token.GetType(), token.GetElement().c_str(), token.GetStartPointer(), token.GetEndPointer());
                        //Logger::WriteTop(log);
                        index++;
                    }

                    _RaiseError("_NextChar");//:���łɕ�����I�[�ł�");
                }

                pointer++;

                char res = _CurrentChar();
                return res;
            }
            
            private void _SkipComment()
            {
                while (true)
                {
                    int posStart = pointer;
                    _SkipSpace();

                    char ch = _CurrentChar();

                    if (ch == '/')
                    {//�R�����g�A�E�g����
                        int tPos = pointer;
                        ch = _NextChar();
                        if (ch == '/')
                        {// "//"
                            while (ch != '\r' && ch != '\n' && HasNext())
                            {
                                ch = _NextChar();
                            }
                        }
                        else if (ch == '*')
                        {// "/*"-"*/"
                            while (true)
                            {
                                ch = _NextChar();
                                if (ch == '*')
                                {
                                    ch = _NextChar();
                                    if (ch == '/')
                                    {
                                        break;
                                    }
                                }
                            }
                            ch = _NextChar();
                        }
                        else
                        {
                            pointer = tPos;
                            ch = '/';
                        }
                    }

                    //�X�L�b�v���󔒔�΂��������ꍇ�A�I��
                    if (posStart == pointer)
                    {
                        break;
                    }
                }
            }
            private void _SkipSpace()
            {
                char ch = _CurrentChar();
                while (true)
                {
                    if (ch != ' ' && ch != '\t')
                    {
                        break;
                    }
                    ch = _NextChar();
                }
            }
            private void _RaiseError(string str)
            {
                throw new Exception(str);
            }
            public Scanner(char[] buf)
            {
                bPermitSignNumber = true;
                buffer = buf;
                pointer = 0;

                SetPointerBegin();
            }
            public void SetPermitSignNumber(bool bEnable)
            {
                bPermitSignNumber = bEnable;
            }
            public Token GetToken()
            {
                return token;
            }
            public Token Next()
            {
                if (!HasNext())
                {
                    _RaiseError("Next");//:���łɏI�[�ł�");
                }

                _SkipComment();//�R�����g���Ƃ΂��܂�

                char ch = _CurrentChar();

                TokenType type = TokenType.TK_UNKNOWN;
                int posStart = pointer;//�擪��ۑ�

                switch(ch)
                {
                    case '\0': type = TokenType.TK_EOF; break;//�I�[
                    case ',': _NextChar(); type = TokenType.TK_COMMA;  break;
                    case '.': _NextChar(); type = TokenType.TK_PERIOD;  break;
                    case '=': _NextChar(); type = TokenType.TK_EQUAL;  break;
                    case '(': _NextChar(); type = TokenType.TK_OPENP; break;
                    case ')': _NextChar(); type = TokenType.TK_CLOSEP; break;
                    case '[': _NextChar(); type = TokenType.TK_OPENB; break;
                    case ']': _NextChar(); type = TokenType.TK_CLOSEB; break;
                    case '{': _NextChar(); type = TokenType.TK_OPENC; break;
                    case '}': _NextChar(); type = TokenType.TK_CLOSEC; break;
                    case '*': _NextChar(); type = TokenType.TK_ASTERISK; break;
                    case '/': _NextChar(); type = TokenType.TK_SLASH; break;
                    case ':': _NextChar(); type = TokenType.TK_COLON; break;
                    case ';': _NextChar(); type = TokenType.TK_SEMICOLON; break;
                    case '~': _NextChar(); type = TokenType.TK_TILDE; break;
                    case '!': _NextChar(); type = TokenType.TK_EXCLAMATION; break;
                    case '#': _NextChar(); type = TokenType.TK_SHARP; break;
                    case '|': _NextChar(); type = TokenType.TK_PIPE; break;
                    case '&': _NextChar(); type = TokenType.TK_AMPERSAND; break;
                    case '<': _NextChar(); type = TokenType.TK_LESS; break;
                    case '>': _NextChar(); type = TokenType.TK_GREATER; break;
                    
                    case '"':
                    {
                        ch = _NextChar();//1�i�߂�
                        //while ( ch != '"' )ch = _NextChar();//���̃_�u���N�I�[�e�[�V�����܂Ői�߂�
                        char pre = ch;
                        while (true)
                        {
                            if (ch == '"' && pre != '\\')
                            {
                                break;
                            }
                            pre = ch;
                            ch = _NextChar();//���̃_�u���N�I�[�e�[�V�����܂Ői�߂�
                        }
                        if (ch == '"')
                        {
                            _NextChar();//�_�u���N�I�[�e�[�V������������1�i�߂�
                        }
                        else 
                        {
                            string error = GetString(posStart, pointer);
                            string log = /*StringUtility::Format(L*/"Next:";//���łɕ�����I�[�ł�(String������) -> %s", error.c_str());
                            _RaiseError(log);
                        }
                        type = TokenType.TK_STRING;
                        break;
                    }

                    case '\r':case '\n'://���s
                        //���s�����܂ł������悤�Ȃ̂�1�̉��s�Ƃ��Ĉ���
                        while (ch == '\r' || ch == '\n')
                        {
                            ch = _NextChar();
                        }
                        type = TokenType.TK_NEWLINE;
                        break;

                    case '+':case '-':
                    {
                        if (ch == '+')
                        {
                            ch = _NextChar();
                            type = TokenType.TK_PLUS;
                        }
                        else if (ch == '-')
                        {
                            ch = _NextChar();
                            type = TokenType.TK_MINUS;
                        }

                        if (!bPermitSignNumber || !char.IsDigit(ch))
                        {
                            break;//���������łȂ��Ȃ甲����
                        }
                        goto default;
                    }

                    default:
                    {
                        if (char.IsDigit(ch))
                        {
                            //����������
                            while (char.IsDigit(ch))
                            {
                                ch = _NextChar();//���������̊ԃ|�C���^��i�߂�
                            }
                            type = TokenType.TK_INT;
                            if ( ch == '.' )
                            {
                                //�������������𒲂ׂ�B�����_�������������
                                ch = _NextChar();
                                while (char.IsDigit(ch))
                                {
                                    ch = _NextChar();//���������̊ԃ|�C���^��i�߂�
                                }
                                type = TokenType.TK_REAL;					
                            }
                            
                            if ( ch == 'E' || ch == 'e')
                            {
                                //1E-5�݂����ȃP�[�X
                                ch = _NextChar();
                                while (char.IsDigit(ch) || ch == '-')
                                {
                                    ch = _NextChar();//���������̊ԃ|�C���^��i�߂�
                                }
                                type = TokenType.TK_REAL;	
                            }
                        
                        }
                        else if (char.IsLetter(ch) || ch == '_')
                        {
                            //���Ԃ񎯕ʎq
                            while (char.IsLetter(ch) || char.IsDigit(ch) || ch == '_')
                            {
                                ch = _NextChar();//���Ԃ񎯕ʎq�Ȋԃ|�C���^��i�߂�
                            }
                            type = TokenType.TK_ID;
                        }
                        else
                        {
                            _NextChar();
                            type = TokenType.TK_UNKNOWN;
                        }

                        break;
                    }
                }

                if ( type == TokenType.TK_STRING)
                {
                    int pPosStart = posStart;
                    int pPosEnd = pointer;
                    string str = new string(buffer, pPosStart, pPosEnd);
                    str = str.Replace(@"\", @"/");

                    token = new Token(type, str, posStart, pointer);
                }
                else
                {
                    int pPosStart = posStart;
                    int pPosEnd = pointer;
                    string str = new string(buffer, pPosStart, pPosEnd);
                    token = new Token(type, str, posStart, pointer);
                }

                listDebugToken.Add(token);

                return token;
            }
            public bool HasNext()
            {
                return pointer < buffer.Length && _CurrentChar() != '\0' && token.GetTokenType() != TokenType.TK_EOF;
            }
            public void CheckType(Token tok, TokenType type)
            {
                if (tok.GetTokenType() != type)
                {
                    string str = /*StringUtility::Format(L*/"CheckType error[%s]:";//,tok.element_.c_str());
                    _RaiseError(str);
                }
            }
            public void CheckIdentifer(Token tok, string id)
            {
                if (tok.GetTokenType() != TokenType.TK_ID || tok.GetIdentifier() != id)
                {
                    string str = /*StringUtility::Format(L*/"CheckID error[%s]:";//,tok.element_.c_str());
                    _RaiseError(str);
                }	
            }
            public int GetCurrentLine()
            {
                if (buffer.Length == 0)
                {
                    return 0;
                }

                int line = 1;
                int pbuf = 0;
                int ebuf = pointer;
                while (true)
                {
                    if (pbuf >= ebuf)
                    {
                        break;
                    }
                    if (buffer[pbuf] == '\n')
                    {
                        line++;
                    }
                    pbuf++;
                }
                return line;
            }
            public int GetCurrentPointer()
            {
                return pointer;
            }
            public void SetCurrentPointer(int pos)
            {
                pointer = pos;
            }
            public void SetPointerBegin()
            {
                pointer = 0;
            }
            public string GetString(int start, int end)
            {
                string res = new string(buffer, start, end);
                return res;
            }
            //bool CompareMemory(int start, int end, const char* data); TODO remove
        }
    }
}
