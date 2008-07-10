using System;
using Boo.BooLangService;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public static class IntellisenseDeclarationsTestExtensionMethods
    {
        public static string GetDescriptionMatchingName(this IntellisenseDeclarations declarations, string name)
        {
            int index;
            bool uniqueMatch;

            declarations.GetBestMatch(name, out index, out uniqueMatch);

            if (index < 0)
                throw new MissingDeclarationException("Couldn't find a declaration matching '" + name + "'.");

            return declarations.GetDescription(index);
        }
    }

    internal class MissingDeclarationException : Exception
    {
        public MissingDeclarationException(string message)
            : base(message)
        {}
    }
}