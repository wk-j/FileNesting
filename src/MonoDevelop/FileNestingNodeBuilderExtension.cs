
using System;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Projects;

namespace MonoDevelop.FileNesting
{
    class FileNestingNodeBuilderExtension : NodeBuilderExtension
    {
        public override bool CanBuildNode(Type dataType)
        {
            return typeof(ProjectFile).IsAssignableFrom(dataType);
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

