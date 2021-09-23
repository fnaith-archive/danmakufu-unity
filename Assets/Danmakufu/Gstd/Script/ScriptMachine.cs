using System.Collections.Generic;
using System.Diagnostics;

namespace Gstd
{
    
namespace Script
{

class ScriptMachine
{
    private ScriptEngine engine;
    private bool bTerminate;
    private List<Environment> callStartParentEnvironmentList;
    private Environment firstUsingEnvironment;
    private Environment lastUsingEnvironment;
    private Environment firstGarbageEnvironment;
    private Environment lastGarbageEnvironment;
    private List<Environment> threads;
    private int currentThreadIndex;
    private bool finished;
    private bool stopped;
    private bool resuming;
    //void* data TODO check
    private bool error;
    private string errorMessage;
    private int errorLine;
    public bool Finished
    {
        get => finished;
    }
    public bool Stopped
    {
        get => stopped;
    }
    public bool Resuming
    {
        get => resuming;
    }
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
    public ScriptEngine Engine
    {
        get => engine;
    }
    public ScriptMachine(ScriptEngine engine)
    {
        Debug.Assert(!engine.Error);
        this.engine = engine;
        threads = new List<Environment>();
        callStartParentEnvironmentList = new List<Environment>();

        firstUsingEnvironment= null;
        lastUsingEnvironment = null;
        firstGarbageEnvironment = null;
        lastGarbageEnvironment = null;

        error = false;
        bTerminate = false;
    }
    ~ScriptMachine()
    {
        while (firstUsingEnvironment != null)
        {
            Environment obj = firstUsingEnvironment;
            firstUsingEnvironment = firstUsingEnvironment.Succ;
            //delete object;
        }

        while (firstGarbageEnvironment != null)
        {
            Environment obj = firstGarbageEnvironment;
            firstGarbageEnvironment = firstGarbageEnvironment.Succ;
            //delete object;
        }
    }
    public Environment NewEnvironment(Environment parent, Block b)
    {
        Environment result = null;

        if (firstGarbageEnvironment != null)
        {
            result = firstGarbageEnvironment;
            firstGarbageEnvironment = result.Succ;
            if (result.Succ != null)
            {
                result.Succ.Pred = result.Pred;
            }
            else
            {
                lastGarbageEnvironment = result.Pred;
            }
        }

        if (result == null)
        {
            result = new Environment();
        }

        result.Parent = parent;
        result.RefCount = 1;
        result.Sub = b;
        result.Ip = 0;
        result.Variables.Clear();
        result.Stack.Clear();
        result.HasResult = false;

        result.Pred = lastUsingEnvironment;
        result.Succ = null;
        if (result.Pred != null)
        {
            result.Pred.Succ = result;
        }
        else
        {
            firstUsingEnvironment = result;
        }
        lastUsingEnvironment = result;

        return result;
    }
    public void DisposeEnvironment(Environment obj)
    {
        Debug.Assert(obj.RefCount == 0);

        if (obj.Pred != null)
        {
            obj.Pred.Succ = obj.Succ;
        }
        else
        {
            firstUsingEnvironment = obj.Succ;
        }
        if (obj.Succ != null)
        {
            obj.Succ.Pred = obj.Pred;
        }
        else
        {
            lastUsingEnvironment = obj.Pred;
        }

        obj.Pred = lastGarbageEnvironment;
        obj.Succ = null;
        if (obj.Pred != null)
        {
            obj.Pred.Succ = obj;
        }
        else
        {
            firstGarbageEnvironment = obj;
        }
        lastGarbageEnvironment = obj;
    }
    public void Run()
    {
        if (bTerminate)
        {
            return;
        }

        Debug.Assert(!error);
        if (firstUsingEnvironment == null)
        {
            errorLine = -1;
            threads.Clear();
            threads.Add(NewEnvironment(null, engine.MainBlock));
            currentThreadIndex = 0;
            finished = false;
            stopped = false;
            resuming = false;

            while (!finished && !bTerminate)
            {
                Advance();
            }
        }
    }
    public void Resume()
    {
        if (bTerminate)
        {
            return;
        }

        Debug.Assert(!error);
        Debug.Assert(stopped);
        stopped = false;
        finished = false;
        resuming = true;
        while (!finished && !bTerminate)
        {
            Advance();
        }
    }
    public void Call(string eventName)
    {
        if (bTerminate)
        {
            return;
        }

        Debug.Assert(!error);
        Debug.Assert(!stopped);
        if (engine.Events.ContainsKey(eventName))
        {
            Run();

            int threadIndex = currentThreadIndex;
            currentThreadIndex = 0;

            Block b = engine.Events[eventName];
            ++(threads[0].RefCount);
            threads[0] = NewEnvironment(threads[0], b);

            finished = false;
            Environment epp = threads[0].Parent.Parent;
            callStartParentEnvironmentList.Add(epp);
            while (!finished && !bTerminate)
            {
                Advance();
            }
            callStartParentEnvironmentList.RemoveAt(callStartParentEnvironmentList.Count - 1);
            finished = false;
            currentThreadIndex = threadIndex;
        }
    }
    public void Stop()
    {
        finished = true;
        stopped = true;
    }
    public void Yield()
    {
        if (currentThreadIndex > 0)
        {
            --currentThreadIndex;
        }
        else
        {
            currentThreadIndex = threads.Count - 1;
        }
    }
    public void Advance()
    {
        Debug.Assert(currentThreadIndex < threads.Count);
        Environment current = threads[currentThreadIndex];

        if (current.Ip >= current.Sub.Codes.Count)
        {
            Environment removing = current;
            current = current.Parent;

            bool bFinish = false;
            if (current == null)
            {
                bFinish = true;
            }
            else
            {
                if (callStartParentEnvironmentList.Count > 1)
                {
                    Environment env = callStartParentEnvironmentList[0];
                    if (current == env)
                    {
                        bFinish = true;
                    }
                }
            }

            if (bFinish)
            {
                finished = true;
            }
            else
            {
                threads[currentThreadIndex] = current;

                if (removing.HasResult)
                {
                    Debug.Assert(current != null && removing.Variables.Count > 0);
                    current.Stack.Add(removing.Variables[0]);
                }
                else if (removing.Sub.Kind == BlockKind.BK_microthread)
                {
                    threads.RemoveAt(currentThreadIndex);
                    Yield();
                }

                Debug.Assert(removing.Stack.Count == 0);

                for ( ; ; )
                {
                    --(removing.RefCount);
                    if (removing.RefCount > 0)
                    {
                        break;
                    }
                    Environment next = removing.Parent;
                    DisposeEnvironment(removing);
                    removing = next;
                }
            }
        }
        else
        {
            Code c = current.Sub.Codes[current.Ip];
            errorLine = c.Line;	//��
            ++(current.Ip);

            switch (c.Command)
            {
                case CommandKind.PC_assign:
                    {
                        List<Value> stack = current.Stack;
                        Debug.Assert(stack.Count > 0);
                        for (Environment i = current; i != null; i = i.Parent)
                        {
                            if (i.Sub.Level == c.Level)
                            {
                                List<Value> vars = i.Variables;
                                if (vars.Count <= c.Variable)
                                {
                                    while (vars.Count <= c.Variable)
                                    {
                                        vars.Add(new Value());
                                    }
                                }
                                Value dest = vars[c.Variable];
                                Value src = stack[stack.Count - 1];
                                if (dest.HasData() && dest.GetDataType() != src.GetDataType()
                                && !(dest.GetDataType().Kind == TypeKind.TK_ARRAY
                                && src.GetDataType().Kind == TypeKind.TK_ARRAY
                                && (dest.LengthAsArray() > 0 || src.LengthAsArray() > 0)))
                                {
                                    string error = "A variable was changing it's value type.\r\n";
                                    //error += L"(����ɂ���Č^���ς����悤�Ƃ��܂���)"; TODO enable
                                    RaiseError(error);
                                }
                                vars[c.Variable] = src;
                                stack.RemoveAt(stack.Count - 1);
                                break;
                            }
                        }
                    }
                    break;

                case CommandKind.PC_assign_writable:
                    {
                        List<Value> stack = current.Stack;
                        Debug.Assert(stack.Count >= 2);
                        Value dest = stack[stack.Count - 2];
                        Value src = stack[stack.Count - 1];
                        if (dest.HasData() && dest.GetDataType() != src.GetDataType()
                        && !(dest.GetDataType().Kind == TypeKind.TK_ARRAY && src.GetDataType().Kind == TypeKind.TK_ARRAY
                        && (dest.LengthAsArray() > 0 || src.LengthAsArray() > 0)))
                        {
                            string error = "A variable was changing it's value type.\r\n";
                            //error += L"(����ɂ���Č^���ς����悤�Ƃ��܂���)"; TODO enable
                            RaiseError(error);
                        }
                        else
                        {
                            dest.Overwrite(src);
                            stack.RemoveAt(stack.Count - 1);
                            stack.RemoveAt(stack.Count - 1);
                        }
                    }
                    break;

                case CommandKind.PC_break_loop:
                case CommandKind.PC_break_routine:
                    for (Environment i = current; i != null; i = i.Parent)
                    {
                        i.Ip = i.Sub.Codes.Count;

                        if (c.Command == CommandKind.PC_break_loop)
                        {
                            if (i.Sub.Kind == BlockKind.BK_loop)
                            {
                                Environment e = i.Parent;
                                Debug.Assert(e != null);
                                do
                                {
                                    ++(e.Ip);
                                }
                                while (e.Sub.Codes[e.Ip - 1].Command != CommandKind.PC_loop_back);
                                break;
                            }
                        }
                        else
                        {
                            if (i.Sub.Kind == BlockKind.BK_sub || i.Sub.Kind == BlockKind.BK_function
                            || i.Sub.Kind == BlockKind.BK_microthread)
                            {
                                break;
                            }
                            else if (i.Sub.Kind == BlockKind.BK_loop)
                            {
                                i.Parent.Stack.Clear(); //���׍H�������Ƃ���
                            }
                        }
                    }
                    break;

                case CommandKind.PC_call:
                case CommandKind.PC_call_and_push_result:
                    {
                        List<Value> currentStack = current.Stack;
                        Debug.Assert(currentStack.Count >= c.Arguments);
                        if (c.Sub.Func != null)
                        {
                            //�l�C�e�B�u�Ăяo��
                            Value[] argv = new Value[c.Arguments];
                            for (int i = 0; i < c.Arguments; ++i)
                            {
                                argv[i] = currentStack[currentStack.Count - c.Arguments + i];
                            }
                            Value ret;
                            ret = c.Sub.Func(this, c.Arguments, argv);
                            if (stopped)
                            {
                                --(current.Ip);
                            }
                            else
                            {
                                resuming = false;
                                //�l�܂ꂽ�������폜
                                for (int i = 0; i < c.Arguments; ++i)
                                {
                                    currentStack.RemoveAt(currentStack.Count - 1);
                                }
                                //currentStack->length -= c->arguments;
                                //�߂�l
                                if (c.Command == CommandKind.PC_call_and_push_result)
                                {
                                    currentStack.Add(ret);
                                }
                            }
                        }
                        else if (c.Sub.Kind == BlockKind.BK_microthread)
                        {
                            //�}�C�N���X���b�h�N��
                            ++(current.RefCount);
                            Environment e = NewEnvironment(current, c.Sub);
                            ++currentThreadIndex;
                            threads.Insert(currentThreadIndex, e);
                            //�����̐ςݑւ�
                            for (int i = 0; i < c.Arguments; ++i)
                            {
                                e.Stack.Add(currentStack[currentStack.Count - 1]);
                                currentStack.RemoveAt(currentStack.Count - 1);
                            }
                        }
                        else
                        {
                            //�X�N���v�g�Ԃ̌Ăяo��
                            ++(current.RefCount);
                            Environment e = NewEnvironment(current, c.Sub);
                            e.HasResult = c.Command == CommandKind.PC_call_and_push_result;
                            threads[currentThreadIndex] = e;
                            //�����̐ςݑւ�
                            for (int i = 0; i < c.Arguments; ++i)
                            {
                                e.Stack.Add(currentStack[currentStack.Count - 1]);
                                currentStack.RemoveAt(currentStack.Count - 1);
                            }
                        }
                    }
                    break;

                case CommandKind.PC_case_begin:
                case CommandKind.PC_case_end:
                    break;

                case CommandKind.PC_case_if:
                case CommandKind.PC_case_if_not:
                case CommandKind.PC_case_next:
                    {
                        bool exit = true;
                        if (c.Command != CommandKind.PC_case_next)
                        {
                            List<Value> currentStack = current.Stack;
                            exit = currentStack[currentStack.Count - 1].AsBoolean();
                            if (c.Command == CommandKind.PC_case_if_not)
                            {
                                exit = !exit;
                            }
                            currentStack.RemoveAt(currentStack.Count - 1);
                        }
                        if (exit)
                        {
                            int nested = 0;
                            for ( ; ; )
                            {
                                switch (current.Sub.Codes[current.Ip].Command)
                                {
                                    case CommandKind.PC_case_begin:
                                        ++nested;
                                        break;
                                    case CommandKind.PC_case_end:
                                        --nested;
                                        if (nested < 0)
                                        {
                                            goto next;
                                        }
                                        break;
                                    case CommandKind.PC_case_next:
                                        if (nested == 0 && c.Command != CommandKind.PC_case_next)
                                        {
                                            ++(current.Ip);
                                            goto next;
                                        }
                                        break;
                                }
                                ++(current.Ip);
                            }
                        next:
                            ;
                        }
                    }
                    break;

                case CommandKind.PC_compare_e:
                case CommandKind.PC_compare_g:
                case CommandKind.PC_compare_ge:
                case CommandKind.PC_compare_l:
                case CommandKind.PC_compare_le:
                case CommandKind.PC_compare_ne:
                    {
                        List<Value> stack = current.Stack;
                        Value t = stack[stack.Count - 1];
                        double r = t.AsReal();
                        bool b = false;
                        switch (c.Command)
                        {
                            case CommandKind.PC_compare_e:
                                b = r == 0;
                                break;
                            case CommandKind.PC_compare_g:
                                b = r > 0;
                                break;
                            case CommandKind.PC_compare_ge:
                                b = r >= 0;
                                break;
                            case CommandKind.PC_compare_l:
                                b = r < 0;
                                break;
                            case CommandKind.PC_compare_le:
                                b = r <= 0;
                                break;
                            case CommandKind.PC_compare_ne:
                                b = r != 0;
                                break;
                        }
                        t.Set(engine.GetBooleanType(), b);
                    }
                    break;

                case CommandKind.PC_dup:
                    {
                        List<Value> stack = current.Stack;
                        Debug.Assert(stack.Count > 0);
                        stack.Add(stack[stack.Count - 1]);
                    }
                    break;

                case CommandKind.PC_dup2:
                    {
                        List<Value> stack = current.Stack;
                        int len = stack.Count;
                        Debug.Assert(len >= 2);
                        stack.Add(stack[len - 2]);
                        stack.Add(stack[len - 1]);
                    }
                    break;

                case CommandKind.PC_loop_back:
                    current.Ip = c.Ip;
                    break;

                case CommandKind.PC_loop_ascent:
                    {
                        List<Value> stack = current.Stack;
                        Value i = stack[stack.Count - 1];
                        if (i.AsReal() <= 0)
                        {
                            do
                            {
                                ++(current.Ip);
                            }
                            while (current.Sub.Codes[current.Ip - 1].Command != CommandKind.PC_loop_back);
                        }
                        current.Stack.RemoveAt(current.Stack.Count - 1);
                    }
                    break;

                case CommandKind.PC_loop_descent:
                    {
                        List<Value> stack = current.Stack;
                        Value i = stack[stack.Count - 1];
                        if (i.AsReal() >= 0)
                        {
                            do
                            {
                                ++(current.Ip);
                            }
                            while (current.Sub.Codes[current.Ip - 1].Command != CommandKind.PC_loop_back);
                        }
                        current.Stack.RemoveAt(current.Stack.Count - 1);
                    }
                    break;

                case CommandKind.PC_loop_count:
                    {
                        List<Value> stack = current.Stack;
                        Value i = stack[stack.Count - 1];
                        Debug.Assert(i.GetDataType().Kind == TypeKind.TK_REAL);
                        double r = i.AsReal();
                        if (r > 0)
                        {
                            i.Set(engine.GetRealType(), r - 1);
                        }
                        else
                        {
                            do
                            {
                                ++(current.Ip);
                            }
                            while (current.Sub.Codes[current.Ip - 1].Command != CommandKind.PC_loop_back);
                        }
                    }
                    break;

                case CommandKind.PC_loop_if:
                    {
                        List<Value> stack = current.Stack;
                        bool b = stack[stack.Count - 1].AsBoolean();
                        current.Stack.RemoveAt(current.Stack.Count - 1);
                        if (!b)
                        {
                            do
                            {
                                ++(current.Ip);
                            }
                            while (current.Sub.Codes[current.Ip - 1].Command != CommandKind.PC_loop_back);
                        }
                    }
                    break;

                case CommandKind.PC_pop:
                    Debug.Assert(current.Stack.Count > 0);
                    current.Stack.RemoveAt(current.Stack.Count - 1);
                    break;

                case CommandKind.PC_push_value:
                    current.Stack.Add(c.Data);
                    break;

                case CommandKind.PC_push_variable:
                case CommandKind.PC_push_variable_writable:
                    for (Environment i = current; i != null; i = i.Parent)
                    {
                        if (i.Sub.Level == c.Level)
                        {
                            List<Value> vars = i.Variables;
                            if (vars.Count <= c.Variable || !vars[c.Variable].HasData())
                            {
                                string error = "you are using a variable that has not been set yet.\r\n";
                                //error += L"(����������Ă��Ȃ��ϐ����g�����Ƃ��܂���)"; TODO enable
                                RaiseError(error);
                            }
                            else
                            {
                                Value var = vars[c.Variable];
                                if (c.Command == CommandKind.PC_push_variable_writable)
                                {
                                    var.Unique();
                                }
                                current.Stack.Add(var);
                            }
                            break;
                        }
                    }
                    break;

                case CommandKind.PC_swap:
                    {
                        int len = current.Stack.Count;
                        Debug.Assert(len >= 2);
                        Value t = current.Stack[len - 1];
                        current.Stack[len - 1] = current.Stack[len - 2];
                        current.Stack[len - 2] = t;
                    }
                    break;

                case CommandKind.PC_yield:
                    Yield();
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }
    public bool HasEvent(string eventName)
    {
        Debug.Assert(!error);
        return engine.Events.ContainsKey(eventName);
    }
    public int GetCurrentLine()
    {
        Environment current = threads[currentThreadIndex];
        Code c = current.Sub.Codes[current.Ip];
        return c.Line;
    }
    public void RaiseError(string message)
    {
        error = true;
        errorMessage = message;
        finished = true;
    }

    public int GetThreadCount()
    {
        return threads.Count;
    }
}

}

}
