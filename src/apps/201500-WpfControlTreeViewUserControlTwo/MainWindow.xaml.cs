using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
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

            ExtractVisualTree(rootObjects.First(), ControlTreeView);

            if (presentationSourceCount == 0)
            {
                MessageBox.Show("No presentation sources found!!");
            }

            ExpandAllTreeViewItems(ControlTreeView);
        }

        private void ExtractVisualTree(object parent, ItemsControl parentTreeViewItem)
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

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);

                // Only add UserControls to the tree view
                if (child is UserControl userControl)
                {
                    var childTreeViewItem = new TreeViewItem
                    {
                        Header = userControl.GetType().Name,
                        Tag = userControl
                    };

                    parentTreeViewItem.Items.Add(childTreeViewItem);

                    // Optionally, recurse into the UserControl to find nested UserControls
                    ExtractVisualTree(userControl, childTreeViewItem);
                }
                else
                {
                    // If you want to find nested UserControls, you can still recurse
                    ExtractVisualTree(child, parentTreeViewItem);
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