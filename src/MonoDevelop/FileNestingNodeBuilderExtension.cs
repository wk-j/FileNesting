
using System;
using System.Linq;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using MonoDevelop.Projects;

namespace MonoDevelop.FileNesting
{
    class FileNestingNodeBuilderExtension : NodeBuilderExtension
    {
        static Type[] supportedTypes = new [] {
            typeof(ProjectFile),
            typeof(Project),
            typeof(ProjectFolder)
        };

        public override bool CanBuildNode(Type dataType)
        {
            return supportedTypes.Any(type => type.IsAssignableFrom(dataType));
        }

        public override Type CommandHandlerType
        {
            get
            {
                return typeof(FileNestingNodeCommandHandler);
            }
        }
    }
}

