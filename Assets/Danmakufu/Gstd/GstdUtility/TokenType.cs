using System;

namespace Gstd
{
    namespace GstdUtility
    {
        enum TokenType
        {
            TK_UNKNOWN,TK_EOF,TK_NEWLINE,
            TK_ID,
            TK_INT,TK_REAL,TK_STRING,

            TK_OPENP,TK_CLOSEP,TK_OPENB,TK_CLOSEB,TK_OPENC,TK_CLOSEC,
            TK_SHARP,
            TK_PIPE,TK_AMPERSAND,

            TK_COMMA,TK_PERIOD,TK_EQUAL,
            TK_ASTERISK,TK_SLASH,TK_COLON,TK_SEMICOLON,TK_TILDE,TK_EXCLAMATION,
            TK_PLUS,TK_MINUS,
            TK_LESS,TK_GREATER
        }
    }
}
