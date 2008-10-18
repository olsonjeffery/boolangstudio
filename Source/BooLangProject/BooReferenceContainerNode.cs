using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Project.Automation;

namespace Boo.BooLangProject
{
    public class BooReferenceContainerNode : ReferenceContainerNode
    {
        private readonly BooProjectNode project;

        public BooReferenceContainerNode(BooProjectNode root) : base(root)
        {
            this.project = root;
        }

        public override void AddChild(HierarchyNode node)
        {
            base.AddChild(node);

            IReference reference = null;

            if (node is AssemblyReferenceNode)
            {
                reference = new AssemblyReference();
                reference.Path = ((OAAssemblyReference)node.Object).Path;

                project.Sources.References.Add(reference);
            }
            else if (node is ProjectReferenceNode)
            {
                reference = new ProjectReference();
                reference.Path = ((OAProjectReference)node.Object).Path;

                project.Sources.References.Add(reference);
            }
        }

    }
}