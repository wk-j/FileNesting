using System.IO;
using System.Linq;
using MonoDevelop.Projects;

namespace MadsKristensen.FileNesting
{
    internal class PathSegmentNester : IFileNester
    {
        public NestingResult Nest(ProjectFile file)
        {
            if (!IsSupported(file.FilePath))
                return NestingResult.Continue;

            string name = Path.GetFileNameWithoutExtension(file.FilePath);

            int index = name.LastIndexOf('.');
            if (index > -1)
            {
                string directory = Path.GetDirectoryName(file.FilePath);
                string extension = Path.GetExtension(file.FilePath);
                string firstName = name.Substring(0, index);
                string parentFileName = Path.Combine(directory, firstName + extension);

                ProjectFile parent = file.Project.GetProjectFile(parentFileName);
                if (parent != null)
                {
                    file.DependsOn = parent.FilePath;
                    return NestingResult.StopProcessing;
                }
            }

            return NestingResult.Continue;
        }

        private static bool IsSupported(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            string[] allowed = new[] { ".js", ".css", ".html", ".htm", ".less", ".scss", ".coffee", ".iced", ".config", ".cs", "vb", ".sql" };

            return allowed.Contains(extension);
        }

        public bool IsEnabled()
        {
            return FileNestingOptions.EnablePathSegmentRule;
        }
    }
}
