
using System;
using System.Collections.Generic;
using System.Linq;
using MadsKristensen.FileNesting;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using MonoDevelop.Projects;

namespace MonoDevelop.FileNesting
{
    class FileNestingNodeCommandHandler : NodeCommandHandler
    {
        static IEnumerable<ProjectFile> nested;

        [AllowMultiSelection]
        [CommandUpdateHandler(Commands.Nest)]
        void BeforeNest(CommandInfo info)
        {
            info.Enabled = true;
            //_items = Helpers.GetSelectedItems().Where(i => (i.Kind.Equals(VSConstants.ItemTypeGuid.PhysicalFile_string, StringComparison.OrdinalIgnoreCase) && i.ProjectItems != null));
            //button.Enabled = _items.Any();
        }

        [AllowMultiSelection]
        [CommandHandler(Commands.Nest)]
        void Nest()
        {
            var files = CurrentNodes.Select(node => node.DataItem).OfType<ProjectFile>().ToList();
            var siblings = GetSiblings().ToList();

            ManualNester.Nest(files, siblings);
        }

        IEnumerable<ProjectFile> GetSiblings()
        {
            var files = new List<ProjectFile>();

            var node = CurrentNodes[0];
            var folderNode = MoveToParentFolderOrProjectNode(node);
            if (folderNode == null)
                return files;

            if (node.MoveToFirstChild())
            {
                do
                {
                    var file = CurrentNode.DataItem as ProjectFile;
                    if (file != null)
                        files.Add(file);

                } while (CurrentNode.MoveNext());
            }

            return files;
        }

        ITreeNavigator MoveToParentFolderOrProjectNode(ITreeNavigator node)
        {
            while (node.MoveToParent())
            {
                var parentItem = CurrentNode.DataItem;
                if (parentItem is ProjectFolder ||
                    parentItem is Project)
                {
                    return node;
                }

                node = CurrentNode;
            }

            return null;
        }

        [AllowMultiSelection]
        [CommandUpdateHandler(Commands.UnNest)]
        void BeforeUnNest(CommandInfo info)
        {
            nested = GetNestedFiles();

            info.Enabled = nested.Any();
        }

        [AllowMultiSelection]
        [CommandHandler(Commands.UnNest)]
        void UnNest()
        {
            //FileNestingFactory.Enabled = false;

            foreach (ProjectFile item in nested)
            {
                ManualNester.UnNest(item);
            }

            //FileNestingFactory.Enabled = true;
        }

        IEnumerable<ProjectFile> GetNestedFiles()
        {
            return CurrentNodes
                .Select(node => node.DataItem)
                .OfType<ProjectFile>()
                .Where(file => file.DependsOnFile != null);
        }
    }
}

