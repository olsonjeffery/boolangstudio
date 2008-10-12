using System;

namespace Boo.BooLangService
{
    public class ParseRequestException : Exception
    {
        public ParseRequestException(string message)
            : base(message)
        {}
    }
}