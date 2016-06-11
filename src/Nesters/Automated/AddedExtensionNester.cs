using System.IO;
using MonoDevelop.Projects;

namespace MadsKristensen.FileNesting
{
    internal class AddedExtensionNester : IFileNester
    {
        public NestingResult Nest(ProjectFile file)
        {
            string trimmed = Path.GetFileNameWithoutExtension(file.FilePath);
            string trimmedFullPath = file.FilePath.ParentDirectory.Combine(trimmed);
            ProjectFile parent = file.Project.GetProjectFile(trimmedFullPath);

            if (parent != null)
            {
                file.DependsOn = parent.FilePath;
                return NestingResult.StopProcessing;
            }

            return NestingResult.Continue;
        }

        public bool IsEnabled()
        {
            return true;
            //return VSPackage.Options.EnableExtensionRule;
        }
    }
}
