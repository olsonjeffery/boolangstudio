using System;
using System.Collections.Generic;
using Boo.BooLangService;
using Xunit;

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

        public static void AssertPresenceOf(this IntellisenseDeclarations declarations, params string[] expectedDeclarationNames)
        {
            Dictionary<string, bool> foundDeclarations = declarations.GetExpectedDeclarations(expectedDeclarationNames);

            // assert each entry is true in the dictionary, meaning all declarations were
            // found in the list
            foreach (var name in foundDeclarations.Keys)
            {
                Assert.True(foundDeclarations[name],
                    "Expected to find declaration '" + name + "' in list of intellisense declarations, but didn't.");
            }
        }

        public static void AssertNonPresenceOf(this IntellisenseDeclarations declarations, params string[] expectedDeclarationNames)
        {
            Dictionary<string, bool> foundDeclarations = declarations.GetExpectedDeclarations(expectedDeclarationNames);

            // assert each entry is true in the dictionary, meaning all declarations were
            // found in the list
            foreach (var name in foundDeclarations.Keys)
            {
                Assert.False(foundDeclarations[name],
                    "Expected to NOT find declaration '" + name + "' in list of intellisense declarations, but did.");
            }
        }

        private static Dictionary<string, bool> GetExpectedDeclarations(this IntellisenseDeclarations declarations, string[] expectedDeclarationNames)
        {
            var foundDeclarations = new Dictionary<string, bool>();

            // setup expectations
            foreach (var name in expectedDeclarationNames)
            {
                foundDeclarations[name] = false;
            }

            // iterate declarations, setting each found one in the foundDecs dictionary
            for (int i = 0; i < declarations.GetCount(); i++)
            {
                string name = declarations.GetName(i);

                if (foundDeclarations.ContainsKey(name))
                    foundDeclarations[name] = true;
            }
            return foundDeclarations;
        }

    }

    internal class MissingDeclarationException : Exception
    {
        public MissingDeclarationException(string message)
            : base(message)
        {}
    }
}