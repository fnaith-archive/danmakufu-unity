namespace Gstd
{
    namespace Script
    {
        sealed class Function
        {
            public string Name { get; }
            public Callback Callback { get; }
            public int Arguments { get; }
            public Function(string name, Callback callback, int arguments)
            {
                Name = name;
                Callback = callback;
                Arguments = arguments;
            }
        }
    }
}
