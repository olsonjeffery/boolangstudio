using System;
using System.Collections.Generic;
using System.Reflection;
using Boo.BooLangService.Members;
using Boo.BooLangService.VSInterop;
using BooLangService;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Boo.BooLangService
{
    /// <summary>
    /// Badly named.
    /// Will parse the document for MemberSelect, but also handles querying the available namespaces
    /// for intellisense. Probably needs splitting and renaming.
    /// </summary>
    public class DocumentParser
    {
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

            BooDeclarations decs = new BooDeclarations(namespaces);

            return new BooScope(decs);
        }
    }
}