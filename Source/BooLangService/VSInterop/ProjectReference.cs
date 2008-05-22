namespace Boo.BooLangService.VSInterop
{
    public class ProjectReference
    {
        private string target;
        private bool isAssembly;

        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        public bool IsAssembly
        {
            get { return isAssembly; }
            set { isAssembly = value; }
        }
    }
}