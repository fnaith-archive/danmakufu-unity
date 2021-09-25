namespace Gstd
{
    namespace Script
    {
        enum CommandKind // TODO rename enums
        {
            PC_assign, PC_assign_writable, PC_break_loop, PC_break_routine, PC_call, PC_call_and_push_result, PC_case_begin,
            PC_case_end, PC_case_if, PC_case_if_not, PC_case_next, PC_compare_e, PC_compare_g, PC_compare_ge, PC_compare_l,
            PC_compare_le, PC_compare_ne, PC_dup, PC_dup2, PC_loop_ascent, PC_loop_back, PC_loop_count, PC_loop_descent, PC_loop_if,
            PC_pop, PC_push_value, PC_push_variable, PC_push_variable_writable, PC_swap, PC_yield
        }
    }
}
