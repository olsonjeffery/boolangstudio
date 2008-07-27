using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.Document.Origins;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Intellisense
{
    /// <summary>
    /// Converts between the Boo Antlr entity nodes and our tree-node structure.
    /// </summary>
    public class EntityToTreeNodeConverter
    {
        public IBooParseTreeNode ToTreeNode(IEntity entity)
        {
            if (entity is IType)
                return ToTreeNode((IType)entity);
            if (entity is IProperty)
                return ToTreeNode((IProperty)entity);
            if (entity is IMethod)
                return ToTreeNode((IMethod)entity);
            if (entity is INamespace)
                return ToTreeNode((INamespace)entity);

            // fallback for unhandled types
            return new ClassTreeNode(new EntitySourceOrigin(entity), entity.FullName);
        }

        private IBooParseTreeNode ToTreeNode(IType type)
        {
            if (type.IsInterface)
                return new InterfaceTreeNode(new EntitySourceOrigin(type), type.FullName);
            if (type.IsClass || type.IsEnum)
                return new ClassTreeNode(new EntitySourceOrigin(type), type.FullName);
            
            return new ValueTypeTreeNode(new EntitySourceOrigin(type), type.FullName);
        }

        private IBooParseTreeNode ToTreeNode(INamespace @namespace)
        {
            return new ImportedNamespaceTreeNode(new EntitySourceOrigin((IEntity)@namespace));
        }

        private IBooParseTreeNode ToTreeNode(IMethod method)
        {
            var member = new MethodTreeNode(new EntitySourceOrigin(method), method.ReturnType.ToString(), method.DeclaringType.FullName);

            foreach (var parameter in method.GetParameters())
            {
                member.Parameters.Add(new MethodParameter
                {
                    Name = parameter.Name,
                    Type = parameter.Type.ToString()
                });
            }

            return member;
        }

        private IBooParseTreeNode ToTreeNode(IProperty property)
        {
            return new MethodTreeNode(new EntitySourceOrigin(property), property.GetGetMethod().ReturnType.ToString(), property.DeclaringType.FullName);
        }
    }
}