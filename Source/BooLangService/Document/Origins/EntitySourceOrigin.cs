using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Origins
{
    /// <summary>
    /// Represents any kind of declaration whom was found in an IEntity from the boo parser.
    /// </summary>
    public class EntitySourceOrigin : ISourceOrigin
    {
        private readonly List<string> excludedMembers = new List<string> { ".ctor", "constructor" };

        private readonly IEntity entity;

        public EntitySourceOrigin(IEntity entity)
        {
            this.entity = entity;
        }

        public string Name
        {
            get { return entity.Name; }
        }

        public List<ISourceOrigin> GetMembers(bool constructor)
        {
            var namespaceEntity = entity as INamespace;
            var instance = constructor;

            if (entity is IMethod)
            {
                namespaceEntity = ((IMethod)entity).ReturnType;
                instance = true;
            }
            else if (entity is ITypedEntity && !(entity is IType))
            {
                namespaceEntity = ((ITypedEntity)entity).Type;
                instance = true;
            }

            var members = new List<IEntity>(TypeSystemServices.GetAllMembers(namespaceEntity));

            // remove any static members for instances, and any instance members for types
            members.RemoveAll(e =>
            {
                if (excludedMembers.Contains(e.Name)) return true;
                if (e is NamespaceEntity || e is NullNamespace || e is SimpleNamespace) return false;

                var member = (IMember)e;

                if (!member.IsPublic) return true;
                return (instance && member.IsStatic) || (!instance && !member.IsStatic);
            });

            return ConvertEntitiesToSourceOrigins(members);
        }

        public IBooParseTreeNode ToTreeNode()
        {
            var converter = new EntityToTreeNodeConverter();

            return converter.ToTreeNode(entity);
        }

        private List<ISourceOrigin> ConvertEntitiesToSourceOrigins(List<IEntity> members)
        {
            var origins = new List<ISourceOrigin>();

            members.ForEach(m => origins.Add(new EntitySourceOrigin(m)));

            return origins;
        }
    }
}