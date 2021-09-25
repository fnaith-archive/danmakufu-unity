namespace Gstd
{
    namespace Script
    {
        enum TokenKind // TODO rename enums
        {
            TK_end, TK_invalid, TK_word, TK_real, TK_char, TK_string, TK_open_par, TK_close_par, TK_open_bra, TK_close_bra, TK_open_cur,
            TK_close_cur, TK_open_abs, TK_close_abs, TK_comma, TK_semicolon, TK_tilde, TK_assign, TK_plus, TK_minus, TK_inc, TK_dec,
            TK_asterisk, TK_slash, TK_percent, TK_caret, TK_e, TK_g, TK_ge, TK_l, TK_le, TK_ne, TK_exclamation, TK_ampersand,
            TK_and_then, TK_vertical, TK_or_else, TK_at, TK_add_assign, TK_subtract_assign, TK_multiply_assign, TK_divide_assign,
            TK_remainder_assign, TK_power_assign, TK_range, TK_ALTERNATIVE, TK_ASCENT, TK_BREAK, TK_CASE, TK_DESCENT, TK_ELSE,
            TK_FUNCTION, TK_IF, TK_IN, TK_LET, TK_LOCAL, TK_LOOP, TK_OTHERS, TK_REAL, TK_RETURN, TK_SUB, TK_TASK,
            TK_TIMES, TK_WHILE, TK_YIELD
        }
    }
}
