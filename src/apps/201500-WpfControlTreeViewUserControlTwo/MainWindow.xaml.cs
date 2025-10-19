using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfControlTreeViewUserControlTwo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnShowControlTree_Click(object sender, RoutedEventArgs e)
        {
            ShowControlTree();
        }

        private void BtnAnalyzeUserControl_Click(object sender, RoutedEventArgs e)
        {
            ShowControlTree();

            if (ControlTreeView.Items.Count > 0)
            {
                var firstItem = ControlTreeView.Items[0] as TreeViewItem;
                if (firstItem is not null)
                {
                    AnalyzeUserControlTreeViewItem(firstItem);
                }
            }
        }

        private void AnalyzeUserControlTreeViewItem(TreeViewItem treeViewItem)
        {
            if (treeViewItem.Tag is UserControl userControl)
            {
                // Perform analysis on the UserControl
                // MessageBox.Show($"Analyzing UserControl: {userControl.GetType().Name}");
                AnalyzeUserControl(userControl);
            }

            foreach (object item in treeViewItem.Items)
            {
                if (treeViewItem.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem childTreeViewItem)
                {
                    AnalyzeUserControlTreeViewItem(childTreeViewItem);
                }
            }
        }

        private void AnalyzeUserControl(UserControl userControl) 
        {
            var dataContext = userControl.DataContext;
            if (dataContext != null)
            {
                MessageBox.Show($"UserControl {userControl.GetType().Name} has DataContext of type {dataContext.GetType().Name}");
            }
            else
            {
                MessageBox.Show($"UserControl {userControl.GetType().Name} has no DataContext.");
            }
        }

        private void ShowControlTree()
        {
            ControlTreeView.Items.Clear();

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

            ExtractVisualTree(rootObjects.First(), ControlTreeView, -1);

            if (presentationSourceCount == 0)
            {
                MessageBox.Show("No presentation sources found!!");
            }

            ExpandAllTreeViewItems(ControlTreeView);
        }

        private void ExtractVisualTree(object parent, ItemsControl parentTreeViewItem, int level)
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
                    if(!levelIncremented)
                        level++;

                    levelIncremented = true;

                    var childTreeViewItem = new TreeViewItem
                    {
                        Header = userControl.GetType().Name + " " + level,
                        Tag = userControl,
                        ToolTip = level + " " + userControl.ToString(),
                    };

                    parentTreeViewItem.Items.Add(childTreeViewItem);

                    // Optionally, recurse into the UserControl to find nested UserControls
                    ExtractVisualTree(userControl, childTreeViewItem, level);
                }
                else
                {
                    // If you want to find nested UserControls, you can still recurse
                    ExtractVisualTree(child, parentTreeViewItem, level);
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