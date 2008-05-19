using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Package;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using EnvDTE;

namespace Boo.BooLangProject
{
    [CLSCompliant(false)]
    [ComVisible(true)]
    [Guid(GuidList.guidBooProjectClassString)]
    public class BooProjectNode : ProjectNode
    {
        private ProjectPackage package;
        public BooProjectNode(ProjectPackage package)
        {
            this.package = package;
            imageIndex = this.ImageHandler.ImageList.Images.Count;
            booFileNodeImageIndex = imageIndex + 1;
            foreach (Image img in imageList.Images)
            {
                this.ImageHandler.AddImage(img);
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

        private static ImageList imageList;
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
                throw e;
            }
            if (booFileNodeImageList.Images.Count != 1)
                throw new FileNotFoundException("Cannot find Boo FileNode Icon at: " + booFileResourceString);
            else
                imageList.Images.Add(booFileNodeImageList.Images[0]);
            
        }

        protected override void Reload()
        {
            base.Reload();
            // setting the BooBinPath
            this.SetProjectProperty("BooBinPath", "dummy2");
            this.Save(this.FileName, 0, 0);         
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

        internal static int booFileNodeImageIndex;
        internal static int imageIndex;
        public override int ImageIndex
        {

            get { return imageIndex + 0; }

        }

    }
}