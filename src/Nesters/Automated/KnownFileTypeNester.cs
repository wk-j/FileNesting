using System.Collections.Generic;
using System.IO;
using MonoDevelop.Projects;

namespace MadsKristensen.FileNesting
{
    internal class KnownFileTypeNester : IFileNester
    {
        private static Dictionary<string, string[]> _mapping = new Dictionary<string, string[]>(){
            {".js", new [] {".coffee", ".litcoffee", ".iced", ".ts", ".tsx", ".dart", ".html", ".cshtml", ".vbhtml", ".aspx", ".master", ".ascx"}},
            {".css", new [] {".less", ".scss", ".sass", ".styl", ".html", ".cshtml", ".vbhtml", ".aspx", ".master", ".ascx"}},
            {".ts", new [] {".html", ".cshtml", ".vbhtml", ".aspx", ".master", ".ascx"}},
            {".map", new [] {".js", ".css"}},
        };

        public NestingResult Nest(ProjectFile file)
        {
            string extension = Path.GetExtension(file.FilePath).ToLowerInvariant();

            if (!_mapping.ContainsKey(extension))
                return NestingResult.Continue;

            foreach (string ext in _mapping[extension])
            {
                string parent = Path.ChangeExtension(file.FilePath, ext);
                ProjectFile item = file.Project.GetProjectFile(parent);

                if (item != null)
                {
                    file.DependsOn = item.FilePath;
                    return NestingResult.StopProcessing;
                }
            }

            return NestingResult.Continue;
        }

        public bool IsEnabled()
        {
            return FileNestingOptions.EnableKnownFileTypeRule;
        }
    }
}
