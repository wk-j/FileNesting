
using System.Collections.Generic;
using System.Linq;
using MadsKristensen.FileNesting;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using MonoDevelop.Projects;

namespace MonoDevelop.FileNesting
{
    class FileNestingNodeCommandHandler : NodeCommandHandler
    {
        [AllowMultiSelection]
        [CommandUpdateHandler(Commands.Nest)]
        void BeforeNest(CommandInfo info)
        {
            info.Enabled = OnlyProjectFilesSelected();
        }

        bool OnlyProjectFilesSelected()
        {
            return CurrentNodes.All(node => node.DataItem is ProjectFile);
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
            info.Enabled = OnlyProjectFilesSelected();
            if (!info.Enabled)
                return;

            var nested = GetNestedFiles();

            info.Enabled = nested.Any();
        }

        [AllowMultiSelection]
        [CommandHandler(Commands.UnNest)]
        void UnNest()
        {
            FileNestingFactory.Enabled = false;

            foreach (ProjectFile item in GetNestedFiles())
            {
                ManualNester.UnNest(item);
            }

            FileNestingFactory.Enabled = true;
        }

        IEnumerable<ProjectFile> GetNestedFiles()
        {
            return CurrentNodes
                .Select(node => node.DataItem)
                .OfType<ProjectFile>()
                .Where(file => file.DependsOnFile != null);
        }

        [AllowMultiSelection]
        [CommandHandler(Commands.AutoNest)]
        void AutoNest()
        {
            var selected = GetSelectedItemsRecursive().Distinct().ToArray();
            var projects = selected.Select(item => item.Project).Distinct().ToArray();

            using (ProgressMonitor monitor = CreateProgressMonitor())
            {
                foreach (ProjectFile item in selected)
                {
                    FileNestingFactory.RunNesting(item);
                }

                foreach (var project in projects)
                {
                    project.SaveAsync(new ProgressMonitor());
                }
            }
        }

        IEnumerable<ProjectFile> GetSelectedItemsRecursive()
        {
            foreach (var node in CurrentNodes)
            {
                var provider = ProjectFileProvider.Create(node.DataItem);
                if (provider != null)
                {
                    foreach (ProjectFile child in provider.GetFiles())
                    {
                        if (child != null)
                            yield return child;
                    }
                }
                else
                {
                    var projectFile = node.DataItem as ProjectFile;
                    if (projectFile != null)
                        yield return projectFile;
                }
            }
        }

        ProgressMonitor CreateProgressMonitor()
        {
            return IdeApp.Workbench.ProgressMonitors.GetStatusProgressMonitor
            (
                GettextCatalog.GetString ("Nesting files..."),
                Stock.StatusSolutionOperation,
                false
            );
        }
    }
}

