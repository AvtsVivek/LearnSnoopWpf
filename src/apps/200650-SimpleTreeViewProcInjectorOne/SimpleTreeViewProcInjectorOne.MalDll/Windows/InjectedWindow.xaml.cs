using SimpleTreeViewProcInjectorOne.MalDll.Infrastructure;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace SimpleTreeViewProcInjectorOne.MalDll.Windows
{
    /// <summary>
    /// Interaction logic for InjectedWindow.xaml
    /// </summary>
    public partial class InjectedWindow
    {
        private readonly Process _currentProcess;
        public InjectedWindow()
        {
            target = new();
            InitializeComponent();
            _currentProcess = Process.GetCurrentProcess();
            resultTextBox.Text = _currentProcess.Id.ToString();
        }

        private object target;

        public override object Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        protected override void LoadRootObject(object rootToInspect)
        {
            ShowControlTree();
        }

        // Create the root node
        CustomTreeNode rootTreeNode = new();

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ShowControlTree();
        }

        private void FindViewModelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearTreeView();
        }

        private void ExpandAllButton_Click(object sender, RoutedEventArgs e)
        {
            ExpandAll(ControlTreeView, true);
        }

        private void CollapseAllButton_Click(object sender, RoutedEventArgs e)
        {
            ExpandAll(ControlTreeView, false);
        }

        private void ClearTreeView()
        {
            ControlTreeView.Items.Clear();
            rootTreeNode = new();
        }

        public static void ExpandAll(ItemsControl itemsControl, bool expand)
        {
            foreach (object item in itemsControl.Items)
            {
                TreeViewItem treeViewItem = (itemsControl.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem)!;
                if (treeViewItem != null)
                {
                    treeViewItem.IsExpanded = expand;
                    if (treeViewItem.HasItems)
                    {
                        ExpandAll(treeViewItem, expand); // Recursively call for child items
                    }
                }
            }
        }

        private void ShowControlTree()
        {
            ClearTreeView();

            var presentationSourceCount = 0;

            List<object> rootObjects = new List<object>();

            foreach (PresentationSource? presentationSource in PresentationSource.CurrentSources)
            {
                if (presentationSource is null)
                {
                    MessageBox.Show("Presentation source is null!!");
                    continue;
                }

                var presentationSourceDispatcher = presentationSource.Dispatcher;
                var rootVisual = presentationSource.RunInDispatcher(() => presentationSource.RootVisual);

                object rootObject = rootVisual;

                if (Application.Current is not null
                    && Application.Current.Dispatcher == presentationSourceDispatcher)
                {
                    rootObject = Application.Current;
                }

                if (rootObject is null)
                {
                    MessageBox.Show("Presentation source is null!!");
                    continue;
                }

                presentationSourceCount++;

                if (!rootObjects.Exists(obj => obj == rootObject))
                {
                    rootObjects.Add(rootObject);
                }

                var dispatcher = (rootObject as DispatcherObject)?.Dispatcher ?? presentationSourceDispatcher;
            }

            var firstRootObject = rootObjects.First();

            ExtractVisualTree(firstRootObject, ControlTreeView, rootTreeNode, -1);

            if (presentationSourceCount == 0)
            {
                MessageBox.Show("No presentation sources found!!");
            }

            ExpandAllTreeViewItems(ControlTreeView);
        }

        private void ExtractVisualTree(object parent, ItemsControl parentTreeViewItem,
            CustomTreeNode parentTreeNode, int level)
        {
            DependencyObject dependencyObject = null!;

            if (parent is null)
            {
                return;
            }

            if (parent is Application)
            {
                var app = parent as Application;
                dependencyObject = app!.MainWindow;
                parentTreeNode.Value = app!.MainWindow;
            }
            else if (parent is DependencyObject)
            {
                dependencyObject = (parent as DependencyObject)!;
            }
            else
            {
                return;
            }

            var childrenCount = VisualTreeHelper.GetChildrenCount(dependencyObject);

            bool levelIncremented = false;

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);

                // Only add UserControls to the tree view
                if (child is UserControl userControl)
                {
                    if (!levelIncremented)
                        level++;

                    levelIncremented = true;

                    var childTreeViewItem = new TreeViewItem
                    {
                        Header = userControl.GetType().Name + " " + level,
                        Tag = userControl,
                        ToolTip = level + " " + userControl.ToString(),
                    };

                    if (userControl.DataContext != null)
                    {
                        childTreeViewItem.ToolTip += "\nDataContext: " + userControl.DataContext.ToString();
                        childTreeViewItem.Header += " (VM: " + userControl.DataContext.GetType().Name + ")";
                    }
                    else
                    {
                        childTreeViewItem.ToolTip += "\nDataContext: null";
                    }

                    var childTreeNode = new CustomTreeNode(userControl);

                    parentTreeViewItem.Items.Add(childTreeViewItem);

                    parentTreeNode.AddChild(childTreeNode, dependencyObject);

                    // Optionally, recurse into the UserControl to find nested UserControls
                    ExtractVisualTree(userControl, childTreeViewItem, childTreeNode, level);
                }
                else
                {
                    // If you want to find nested UserControls, you can still recurse
                    ExtractVisualTree(child, parentTreeViewItem, parentTreeNode, level);
                }
            }
        }

        private void ExpandAllTreeViewItems(ItemsControl parent)
        {
            foreach (object item in parent.Items)
            {
                if (parent.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem treeViewItem)
                {
                    treeViewItem.IsExpanded = true;
                    // Force UI to update so child containers are generated
                    treeViewItem.Dispatcher.Invoke(() => { }, DispatcherPriority.Background);
                    ExpandAllTreeViewItems(treeViewItem);
                }
            }
        }
    }
}
