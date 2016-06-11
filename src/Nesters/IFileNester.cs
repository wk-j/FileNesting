
using MonoDevelop.Projects;

namespace MadsKristensen.FileNesting
{
    public interface IFileNester
    {
        NestingResult Nest(ProjectFile file);
        bool IsEnabled();
    }

    public enum NestingResult
    {
        Continue,
        StopProcessing,
    }
}
