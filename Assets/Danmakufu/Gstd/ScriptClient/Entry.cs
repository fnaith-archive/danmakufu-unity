namespace Gstd
{
    namespace ScriptClient
    {
        sealed class Entry
        {
            public int LineStart { get; set; } // TODO remove set
            public int LineEnd { get; set; } // TODO remove set
            public int LineStartOriginal { get; set; } // TODO remove set
            public int LineEndOriginal { get; set; } // TODO remove set
            public string Path { get; set; } // TODO remove set
            public Entry()
            {
            }
            public Entry(Entry source)
            {
                LineStart = source.LineStart;
                LineEnd = source.LineEnd;
                LineStartOriginal = source.LineStartOriginal;
                LineEndOriginal = source.LineEndOriginal;
                Path = source.Path;
            }
        }
    }
}
