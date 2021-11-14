namespace Gstd
{
    namespace Script
    {
        enum TokenKind
        {
            tk_end, tk_invalid, tk_word, tk_real, tk_char, tk_string, tk_open_par, tk_close_par, tk_open_bra, tk_close_bra, tk_open_cur,
            tk_close_cur, tk_open_abs, tk_close_abs, tk_comma, tk_semicolon, tk_tilde, tk_assign, tk_plus, tk_minus, tk_inc, tk_dec,
            tk_asterisk, tk_slash, tk_percent, tk_caret, tk_e, tk_g, tk_ge, tk_l, tk_le, tk_ne, tk_exclamation, tk_ampersand,
            tk_and_then, tk_vertical, tk_or_else, tk_at, tk_add_assign, tk_subtract_assign, tk_multiply_assign, tk_divide_assign,
            tk_remainder_assign, tk_power_assign, tk_range, tk_ALTERNATIVE, tk_ASCENT, tk_BREAK, tk_CASE, tk_DESCENT, tk_ELSE,
            tk_FUNCTION, tk_IF, tk_IN, tk_LET, tk_LOCAL, tk_LOOP, tk_OTHERS, tk_REAL, tk_RETURN, tk_SUB, tk_TASK,
            tk_TIMES, tk_WHILE, tk_YIELD
        }
    }
}
