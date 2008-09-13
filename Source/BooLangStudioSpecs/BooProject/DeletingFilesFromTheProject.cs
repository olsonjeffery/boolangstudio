using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Boo.BooLangProject;
using Boo.BooLangStudio;
using Boo.BooLangService;
using Xunit;
using Microsoft.VisualStudio.Shell.Interop;

namespace Boo.BooLangStudioSpecs
{
  public class when_creating_a_new_BooProjectNode_object
  {
    protected BooProjectNode node;
    public when_creating_a_new_BooProjectNode_object()
    {
      var package = new BooLangStudioPackage();
      var service = new Boo.BooLangService.BooLangService();
      
      node = new BooProjectNode(package, service);
    }

    [Fact]
    public void should_have_its_CanProjectDeleteItems_set_to_true()
    {
      Assert.True(node.CanDeleteItemsInProject);
    }
  }
}