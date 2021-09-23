using System.Collections.Generic;

namespace Gstd
{
    
namespace Script
{

class Environment
{
    private Environment pred;
    private Environment succ;
    private Environment parent;
    private int refCount;
    private Block sub;
    private int ip;
    private List<Value> variables;
    private List<Value> stack;
    private bool hasResult;
    public Environment Pred
    {
        get => pred;
        set => pred = value;
    }
    public Environment Succ
    {
        get => succ;
        set => succ = value;
    }
    public Environment Parent
    {
        get => parent;
        set => parent = value;
    }
    public int RefCount
    {
        get => refCount;
        set => refCount = value;
    }
    public Block Sub
    {
        get => sub;
        set => sub = value;
    }
    public int Ip
    {
        get => ip;
        set => ip = value;
    }
    public List<Value> Variables
    {
        get => variables;
        set => variables = value;
    }
    public List<Value> Stack
    {
        get => stack;
        set => stack = value;
    }
    public bool HasResult
    {
        get => hasResult;
        set => hasResult = value;
    }
    public Environment()
    {
        variables = new List<Value>();
        stack = new List<Value>();
    }
}

}

}
