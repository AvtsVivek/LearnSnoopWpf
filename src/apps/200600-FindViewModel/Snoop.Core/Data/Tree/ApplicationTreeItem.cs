// (c) Copyright Cory Plotts.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

namespace Snoop.Data.Tree;

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Snoop.Infrastructure;

public class ApplicationTreeItem : ResourceContainerTreeItem
{
    private readonly Application application;

    public ApplicationTreeItem(Application application, TreeItem? parent, TreeService treeService)
        : base(application, parent, treeService)
    {
        this.application = application;
        this.IsExpanded = true;
    }

    public override Visual? MainVisual => this.application.MainWindow;

    protected override IEnumerable<ResourceDictionaryWrapper?> ResourceDictionary
    {
        get { yield return this.application.Resources; }
    }

    protected override void ReloadCore()
    {
        // having the call to base.ReloadCore here ... puts the application resources at the very top of the tree view
        base.ReloadCore();

        // MessageBox.Show($"this.application.Windows count is {this.application.Windows.Count}");

        foreach (Window? window in this.application.Windows)
        {
            // MessageBox.Show($"Window type is {window.GetType().FullName}");

            if (window is null)
            {
                // MessageBox.Show($"Window null");
                continue;
            }

            if (window.IsInitialized == false
                || window.CheckAccess() == false
                || window.IsPartOfSnoopVisualTree())
            {
                // MessageBox.Show($"Window IsInitialized is null");
                continue;
            }

            // windows which have an owner are added as child items in VisualItem, so we have to skip them here
            if (window.Owner is not null)
            {
                // MessageBox.Show($"Window owner is not null");
                continue;
            }

            // MessageBox.Show($"Window type is {window.GetType().FullName}");

            this.AddChild(this.TreeService.Construct(window, this));
        }
    }
}