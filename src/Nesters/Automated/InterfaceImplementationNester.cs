using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonoDevelop.Projects;

namespace MadsKristensen.FileNesting
{
    internal class InterfaceImplementationNester : IFileNester
    {
        public NestingResult Nest(ProjectFile file)
        {
            if (!IsSupported(file.FilePath))
                return NestingResult.Continue;

            IEnumerable<string> possibleInterfaceNames = PossibleInterfaceNames(file.FilePath);

            foreach (string interfaceName in possibleInterfaceNames)
            {
                string directory = Path.GetDirectoryName(file.FilePath);
                string parentFileName = Path.Combine(directory, interfaceName);

                ProjectFile parent = file.Project.GetProjectFile(parentFileName);
                if (parent != null)
                {
                    file.DependsOn = parent.FilePath;
                    return NestingResult.StopProcessing;
                }
            }

            return NestingResult.Continue;
        }

        private static IEnumerable<string> PossibleInterfaceNames(string fileName)
        {
            string fileNameOnly = Path.GetFileNameWithoutExtension(fileName);

            List<string> possibleNames = new List<string>();

            for (int i = 0; i < fileNameOnly.Length; i++)
            {
                string letter = fileNameOnly.Substring(i, 1);

                if (letter == letter.ToUpperInvariant())
                {
                    possibleNames.Add(fileNameOnly.Substring(i, fileNameOnly.Length - i));
                }
            }

            string extension = Path.GetExtension(fileName);

            return possibleNames.Select(n => "I" + n + extension);
        }

        private static bool IsSupported(string fileName)
        {
            return (IsAllowedFileType(fileName)) && (!IsInterface(fileName));
        }

        private static bool IsAllowedFileType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            string[] allowed = { ".cs" };

            return allowed.Contains(extension);
        }

        private static bool IsInterface(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            bool firstLetterIsI = ('I' == fileName[0]);

            string secondLetter = fileName.Substring(1, 1);
            bool secondLetterPeriodOrLowercase = (("." == secondLetter) || (secondLetter == secondLetter.ToLowerInvariant()));

            return ((firstLetterIsI) && (!secondLetterPeriodOrLowercase));
        }

        public bool IsEnabled()
        {
            return FileNestingOptions.EnableInterfaceImplementationRule;
        }
    }
}