using System;

namespace Gstd
{
    namespace Script
    {
        sealed class ParserError : Exception
        {
            public ParserError(string message) : base(message)
            {
            }
        }
    }
}
