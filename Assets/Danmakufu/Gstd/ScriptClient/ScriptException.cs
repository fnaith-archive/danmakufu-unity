using System;

namespace Gstd
{
    namespace Script
    {
        sealed class ScriptException : Exception
        {
            public ScriptException(string message) : base(message)
            {
            }
        }
    }
}
