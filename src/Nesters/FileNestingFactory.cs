using System;
using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace MadsKristensen.FileNesting
{
    static class FileNestingFactory
    {
        private static List<IFileNester> _nesters = new List<IFileNester>()
        {
            //new KnownFileTypeNester(),
            //new VsDocNester(),
            //new BundleNester(),
            //new InterfaceImplementationNester(),
            new PathSegmentNester(),
            //new SpriteNester(),
            new AddedExtensionNester(),

        };

        //private static ProjectItemsEvents _events;
        public static bool Enabled { get; set; } = true;

        public static void Enable()
        {
            IdeApp.Workspace.FileAddedToProject += OnFileAddedToProject;
            IdeApp.Workspace.FileRenamedInProject += OnFileRenamedInProject;
        }

        static void OnFileAddedToProject(object sender, ProjectFileEventArgs e)
        {
            foreach (ProjectFileEventInfo info in e)
            {
                ItemAdded(info.ProjectFile);
            }
        }

        static void OnFileRenamedInProject(object sender, ProjectFileRenamedEventArgs e)
        {
            foreach (ProjectFileRenamedEventInfo info in e)
            {
                ItemAdded(info.ProjectFile);
            }
        }

        static void ItemAdded(ProjectFile item)
        {
            if (FileNestingOptions.EnableAutoNesting)
            {
                try
                {
                    RunNesting(item);
                }
                catch (Exception ex)
                {
                    LoggingService.LogError("Automatic nesting failed.", ex);
               }
           }
        }

        public static void RunNesting(ProjectFile item)
        {
            if (!Enabled)
                return;

            foreach (var nester in _nesters.Where(n => n.IsEnabled()))
            {
                NestingResult result = nester.Nest(item);

                if (result == NestingResult.StopProcessing)
                    break;
            }
        }
    }
}
