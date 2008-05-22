using System;
using System.Collections.Generic;
using System.Reflection;
using Boo.BooLangService.VSInterop;
using BooLangService;
using Microsoft.VisualStudio.Package;

namespace Boo.BooLangService.Document
{
    /// <summary>
    /// Badly named.
    /// Will parse the document for MemberSelect, but also handles querying the available namespaces
    /// for intellisense. Probably needs splitting and renaming.
    /// </summary>
    [Obsolete("Being phased out in favor of the tree parser.")]
    public class DocumentParser
    {
        [Obsolete("Being phased out in favor of the tree parser.")]
        public AuthoringScope GetNamespaceSelect(IList<ProjectReference> references, string search)
        {
            List<IMemberDeclaration> namespaces = new List<IMemberDeclaration>();

            foreach (ProjectReference reference in references)
            {
                if (reference.IsAssembly)
                {
                    Assembly asm = Assembly.LoadFrom(reference.Target);

                    foreach (Type type in asm.GetExportedTypes())
                    {
                        string ns = type.Namespace;

                        if (!ns.StartsWith(search)) continue;

                        ns = ns.Remove(0, search.Length);
                        ns = ns.Contains(".") ? ns.Substring(0, ns.IndexOf(".")) : ns;

                        if (!namespaces.Exists(delegate(IMemberDeclaration d) { return d.Name == ns; }))
                            namespaces.Add(new NamespaceMemberDeclaration(ns));
                    }
                }
            }

            throw new NotImplementedException("Probably need to subclass AuthoringScope again to provide namespace support separate from MemberSelect.");

//            BooDeclarations decs = new BooDeclarations(namespaces);


            return new BooScope(null);
        }
    }
}