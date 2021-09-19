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
        if (result.Pred == null)
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
        //
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
