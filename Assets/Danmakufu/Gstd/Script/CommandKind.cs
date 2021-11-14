namespace Gstd
{
    namespace Script
    {
        enum CommandKind
        {
            pc_assign, pc_assign_writable, pc_break_loop, pc_break_routine, pc_call, pc_call_and_push_result, pc_case_begin,
            pc_case_end, pc_case_if, pc_case_if_not, pc_case_next, pc_compare_e, pc_compare_g, pc_compare_ge, pc_compare_l,
            pc_compare_le, pc_compare_ne, pc_dup, pc_dup2, pc_loop_ascent, pc_loop_back, pc_loop_count, pc_loop_descent, pc_loop_if,
            pc_pop, pc_push_value, pc_push_variable, pc_push_variable_writable, pc_swap, pc_yield
        }
    }
}
