﻿
using MonoDevelop.Core;
using Xwt;

namespace MonoDevelop.FileNesting
{
    partial class ProjectItemSelector : Dialog
    {
        ComboBox filesComboBox;
        DialogButton okButton;

        void Build()
        {
            Title = GettextCatalog.GetString("File Nesting");
            Width = 410;
            Resizable = false;

            var mainVBox = new VBox();
            Content = mainVBox;

            var label = new Label();
            label.Text = GettextCatalog.GetString("Select parent");

            mainVBox.PackStart(label);

            filesComboBox = new ComboBox();
            mainVBox.PackStart(filesComboBox);

            okButton = new DialogButton(GettextCatalog.GetString("OK"), Command.Ok);
            Buttons.Add(okButton);
        }
    }
}

