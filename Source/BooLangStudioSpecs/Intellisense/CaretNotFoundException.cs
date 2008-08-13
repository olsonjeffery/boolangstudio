using System;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    internal class CaretNotFoundException : Exception
    {
        public CaretNotFoundException(string message) : base(message)
        {}
    }
}