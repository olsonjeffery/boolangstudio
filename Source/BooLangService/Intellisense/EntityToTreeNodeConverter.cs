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
                return new InterfaceTreeNode(new EntitySourceOrigin(type), type.FullName) { Name = type.Name };
            if (type.IsClass || type.IsEnum)
                return new ClassTreeNode(new EntitySourceOrigin(type), type.FullName) { Name = type.Name };
            
            return new ValueTypeTreeNode(new EntitySourceOrigin(type), type.FullName) { Name = type.Name };
        }

        private IBooParseTreeNode ToTreeNode(INamespace @namespace)
        {
            var name = @namespace.ToString();

            // get the last part of the namespace name, because it's returned as
            // fully qualified
            if (name.Contains("."))
                name = name.Substring(name.LastIndexOf('.') + 1);

            return new ImportedNamespaceTreeNode(new EntitySourceOrigin((IEntity)@namespace)) { Name = name };
        }

        private IBooParseTreeNode ToTreeNode(IMethod method)
        {
            var member = new MethodTreeNode(new EntitySourceOrigin(method), method.ReturnType.ToString(), method.DeclaringType.FullName)
            {
                Name = method.Name
            };

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
            return new MethodTreeNode(new EntitySourceOrigin(property), property.GetGetMethod().ReturnType.ToString(), property.DeclaringType.FullName)
            {
                Name = property.Name
            };
        }
    }
}