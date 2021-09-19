using System;

namespace Gstd
{
    
namespace Script
{

class Function
{
    private string name;
    private Callback callback;
    private int arguments;
    public string Name
    {
        get => name;
        set => name = value;
    }
    public Callback Callback
    {
        get => callback;
        set => callback = value;
    }
    public int Arguments
    {
        get => arguments;
        set => arguments = value;
    }
    public Function(string name, Callback callback, int arguments)
    {
        this.name = name;
        this.callback = callback;
        this.arguments = arguments;
    }
}

}

}
