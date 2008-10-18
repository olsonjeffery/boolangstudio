using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Shell.Interop;

namespace Boo.BooLangProject
{
  [CLSCompliant(false)]
  [ComVisible(true)]
  [Guid(GuidList.guidBooProjectClassString)]
  public class BooProjectNode : ProjectNode
  {
    private static ImageList imageList;
    internal static int booFileNodeImageIndex;
    internal static int imageIndex;

    private ProjectPackage package;
    private BooVSProject vsProject;
    private BooProjectSources projectSources;
    private readonly BooLangService.BooLangService languageService;

    public bool CanDeleteItemsInProject
    {
        get
        {
            return this.CanProjectDeleteItems;
        }
        set
        {
            this.CanProjectDeleteItems = value;
        }
    }

    static BooProjectNode()
    {
      imageList =
          Utilities.GetImageList(
          typeof(BooProjectNode).Assembly.GetManifestResourceStream(
          "Boo.BooLangProject.Resources.BooProjectNode.bmp"));

      ImageList booFileNodeImageList;
      string booFileResourceString = "Boo.BooLangProject.Resources.BooFileNode.bmp";
      try
      {

        booFileNodeImageList =
            Utilities.GetImageList(
            typeof(BooProjectNode).Assembly.GetManifestResourceStream(
            booFileResourceString));
      }
      catch (Exception e)
      {
        throw;
      }

      if (booFileNodeImageList.Images.Count != 1)
        throw new FileNotFoundException("Cannot find Boo FileNode Icon at: " + booFileResourceString);
      else
        imageList.Images.Add(booFileNodeImageList.Images[0]);

    }

    public BooProjectNode(ProjectPackage package, BooLangService.BooLangService languageService)
    {
      this.package = package;
      this.languageService = languageService;
      imageIndex = this.ImageHandler.ImageList.Images.Count;
      booFileNodeImageIndex = imageIndex + 1;

      this.CanDeleteItemsInProject = true;

      foreach (Image img in imageList.Images)
      {
        this.ImageHandler.AddImage(img);
      }
    }

    internal override object Object
    {
      get
      {
        if (vsProject == null)
          vsProject = new BooVSProject(this);

        return vsProject;
      }
    }

    /// <summary>
    /// This is a very poor workaround. Until I can figure out how
    /// to get our GlobalProperies into the ProjectShim that the
    /// base version uses...
    /// </summary>
    /// <returns></returns>
    protected override ProjectLoadOption IsProjectSecure()
    {
      return ProjectLoadOption.LoadNormally;
    }

    public override Guid ProjectGuid
    {
      get { return typeof(BooLangProjectFactory).GUID; }
    }

    public override string ProjectType
    {
      get
      {
        //return this.GetType().Name;
        return "BooProjectType";
      }
    }

    public override void AddFileFromTemplate(string source, string target)
    {
      string ns = this.FileTemplateProcessor.GetFileNamespace(target, this);
      this.FileTemplateProcessor.AddReplace("$nameSpace$", ns);
      this.FileTemplateProcessor.UntokenFile(source, target);
      this.FileTemplateProcessor.Reset();
    }

    public override FileNode CreateFileNode(ProjectElement item)
    {
      return new BooFileNode(this, item, booFileNodeImageIndex) as FileNode;
    }

    /// <summary>
    /// If you don't override this, then all new files you add to your
    /// project will be of type "content" by default and not "compile".
    /// You'll also get AIDS.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public override bool IsCodeFile(string fileName)
    {
      if (new FileInfo(fileName).Extension.ToLower().Contains("boo"))
        return true;
      else
        return false;
    }

    /// <summary>
    /// Overriding to provide project general property page
    /// </summary>
    /// <returns></returns>
    protected override Guid[] GetConfigurationIndependentPropertyPages()
    {
      Guid[] result = new Guid[1];
      result[0] = typeof(GeneralPropertyPage).GUID;
      return result;
    }

    /// <summary>
    /// provides the guid for configuration dependent settings
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    protected override Guid[] GetConfigurationDependentPropertyPages()
    {
      Guid[] result = new Guid[1];
      result[0] = typeof(BooBuildPropertyPage).GUID;
      return result;
    }

    public override int ImageIndex
    {

      get { return imageIndex + 0; }

    }

    public override object GetAutomationObject()
    {
      return new BooOAProject(this);
    }

    public override void Load(string fileName, string location, string name, uint flags, ref Guid iidProject, out int canceled)
    {
      // this needs to be instantiated before the base call, and then start watching
      // the hierarchy after. This is because the base.Load call hits the AddChild
      // method of the references, which add to the project sources. The watching
      // needs to start after the base call because if it's started before, then there
      // aren't any files added yet!
      projectSources = new BooProjectSources(languageService);

      base.Load(fileName, location, name, flags, ref iidProject, out canceled);

      projectSources.StartWatchingHierarchy(InteropSafeHierarchy);

      BooProjectSources.LoadedProjects.Add(projectSources);
    }

    IVsHierarchy InteropSafeHierarchy
    {
      get
      {
        IntPtr unknownPtr = Utilities.QueryInterfaceIUnknown(this);

        if (unknownPtr == IntPtr.Zero)
          return null;

        return (IVsHierarchy)Marshal.GetObjectForIUnknown(unknownPtr);
      }
    }

    public BooProjectSources Sources
    {
      get { return projectSources; }
    }

    protected override ReferenceContainerNode CreateReferenceContainerNode()
    {
      return new BooReferenceContainerNode(this);
    }
  }
}
