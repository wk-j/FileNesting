
using System;
using MadsKristensen.FileNesting;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Dialogs;

namespace MonoDevelop.FileNesting
{
    public class FileNestingOptionsPanel : OptionsPanel
    {
        Gtk.CheckButton enableAutomaticFileNestingCheckBox;
        Gtk.CheckButton enableExtensionRuleCheckBox;
        Gtk.CheckButton enableInterfaceImplementationRuleCheckBox;
        Gtk.CheckButton enableKnownFileTypeRuleCheckBox;
        Gtk.CheckButton enablePathSegmentRuleCheckBox;

        public override Control CreatePanelWidget()
        {
            var vbox = new Gtk.VBox();
            vbox.Spacing = 6;

            var restoreSectionLabel = new Gtk.Label(GetBoldMarkup (GettextCatalog.GetString ("File Nesting")));
            restoreSectionLabel.UseMarkup = true;
            restoreSectionLabel.Xalign = 0;
            vbox.PackStart(restoreSectionLabel, false, false, 0);

            enableAutomaticFileNestingCheckBox = new Gtk.CheckButton (GettextCatalog.GetString("Enable auto-nesting"));
            enableAutomaticFileNestingCheckBox.TooltipText = GettextCatalog.GetString("Enables automatic nesting when files are added or renamed");
            enableAutomaticFileNestingCheckBox.Active = FileNestingOptions.EnableAutoNesting;
            enableAutomaticFileNestingCheckBox.BorderWidth = 10;
            vbox.PackStart(enableAutomaticFileNestingCheckBox, false, false, 0);

            var outputSectionLabel = new Gtk.Label(GetBoldMarkup(GettextCatalog.GetString ("Nesting Rules")));
            outputSectionLabel.UseMarkup = true;
            outputSectionLabel.Xalign = 0;
            vbox.PackStart(outputSectionLabel, false, false, 0);

            var rulesVBox = new Gtk.VBox ();
            vbox.PackStart (rulesVBox);
            rulesVBox.Spacing = 0;

            enableExtensionRuleCheckBox = new Gtk.CheckButton (GettextCatalog.GetString("Enable extension rule"));
            enableExtensionRuleCheckBox.TooltipText = GettextCatalog.GetString ("Files with an added extension nests under parent. Example: foo.js.map nests under foo.js");
            enableExtensionRuleCheckBox.Active = FileNestingOptions.EnableExtensionRule;
            enableExtensionRuleCheckBox.BorderWidth = 10;
            rulesVBox.PackStart(enableExtensionRuleCheckBox, false, false, 0);

            enableInterfaceImplementationRuleCheckBox = new Gtk.CheckButton(GettextCatalog.GetString("Enable interface implementation rule"));
            enableInterfaceImplementationRuleCheckBox.TooltipText = GettextCatalog.GetString("Nest C# interface implementations under their respective interfaces (by filename only)");
            enableInterfaceImplementationRuleCheckBox.Active = FileNestingOptions.EnableInterfaceImplementationRule;
            enableInterfaceImplementationRuleCheckBox.BorderWidth = 10;
            rulesVBox.PackStart(enableInterfaceImplementationRuleCheckBox, false, false, 0);

            enableKnownFileTypeRuleCheckBox = new Gtk.CheckButton(GettextCatalog.GetString("Enable known file type rule"));
            enableKnownFileTypeRuleCheckBox.TooltipText = GettextCatalog.GetString("Some known file types will be nested. Example: foo.css will nest under foo.less");
            enableKnownFileTypeRuleCheckBox.Active = FileNestingOptions.EnableKnownFileTypeRule;
            enableKnownFileTypeRuleCheckBox.BorderWidth = 10;
            rulesVBox.PackStart(enableKnownFileTypeRuleCheckBox, false, false, 0);

            enablePathSegmentRuleCheckBox = new Gtk.CheckButton(GettextCatalog.GetString("Enable path segment rule"));
            enablePathSegmentRuleCheckBox.TooltipText = GettextCatalog.GetString("Files with an added path segment nests under parent. Example: foo.min.js nests under foo.js");
            enablePathSegmentRuleCheckBox.Active = FileNestingOptions.EnablePathSegmentRule;
            enablePathSegmentRuleCheckBox.BorderWidth = 10;
            rulesVBox.PackStart(enablePathSegmentRuleCheckBox, false, false, 0);

            vbox.ShowAll();

            return vbox;
        }

        public override void ApplyChanges()
        {
            FileNestingOptions.EnableAutoNesting = enableAutomaticFileNestingCheckBox.Active;
            FileNestingOptions.EnableExtensionRule = enableExtensionRuleCheckBox.Active;
            FileNestingOptions.EnablePathSegmentRule = enablePathSegmentRuleCheckBox.Active;
            FileNestingOptions.EnableKnownFileTypeRule = enableKnownFileTypeRuleCheckBox.Active;
            FileNestingOptions.EnableInterfaceImplementationRule = enableInterfaceImplementationRuleCheckBox.Active;
        }

        static string GetBoldMarkup (string text)
        {
            return "<b>" + text + "</b>";
        }
    }
}

