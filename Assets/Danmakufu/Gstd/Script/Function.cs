namespace Gstd
{
    namespace Script
    {
        sealed class Function
        {
            public string Name { get; set; } // TODO remove set
            public Callback Callback { get; set; } // TODO remove set
            public int Arguments { get; set; } // TODO remove set
            public Function(string name, Callback callback, int arguments)
            {
                Name = name;
                Callback = callback;
                Arguments = arguments;
            }
        }
    }
}
