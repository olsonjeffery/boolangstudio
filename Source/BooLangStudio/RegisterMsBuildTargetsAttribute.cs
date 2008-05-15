using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell;

namespace Boo.BooLangStudio
{
    public class RegisterMsBuildTargetsAttribute : RegistrationAttribute
    {
        private string _regLabel, _targetPath = "";
        private const string _safeImportPath = @"MSBuild\SafeImports";
        public RegisterMsBuildTargetsAttribute(string regLabel, string targetPath)
        {
            _regLabel = regLabel;
            _targetPath = targetPath;
        }

        public override void Register(RegistrationContext context)
        {
            if (context == null) throw new ArgumentException("Bad context to register Boo MsBuild task.");
            Key key = context.CreateKey(_safeImportPath);
            key.SetValue(_regLabel, _targetPath);
            key.Close();
        }

        public override void Unregister(RegistrationContext context)
        {
            if (context == null) throw new ArgumentException("Bad context to unregister Boo MsBuild task.");
            context.RemoveValue(_safeImportPath,_regLabel);
        }
    }
}
