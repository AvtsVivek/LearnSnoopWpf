using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;

namespace ElementTreeComparisionConsoleApp
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Element Tree Comparision Tool");

            var currentPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);

            var windowFileName = "DemoWindow.xaml";

            var windowXamlPath = Path.Combine(currentPath!, windowFileName);

            if (!File.Exists(windowXamlPath))
            {
                var message = $"Cannot find {windowFileName} " + Environment.NewLine +
                    $"in {currentPath}. " + Environment.NewLine + Environment.NewLine +
                    $"Copy {windowFileName} " + Environment.NewLine +
                    $"to the {currentPath} " + Environment.NewLine +
                    $"and try again.";

                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine(message);

                MessageBox.Show(message);

                return;
            }

            using (XmlReader xmlReader = XmlReader.Create(windowFileName))
            {
                Window wnd = (XamlReader.Load(xmlReader) as Window)!;

                if (wnd == null)
                    return;

                wnd.PreviewMouseLeftButtonDown += HandlePreviewMouseLeftButtonDown;
                wnd.PreviewMouseRightButtonDown += HandlePreviewMouseRightButtonDown;
                wnd.AddHandler(ButtonBase.ClickEvent, (RoutedEventHandler)HandleClick);

                wnd.ShowDialog();
            }
        }

        static void HandlePreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                LogicalTreeDumper.Dump(e.OriginalSource as DependencyObject);
                e.Handled = true;
            }
        }

        static void HandlePreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                VisualTreeDumper.Dump(e.OriginalSource as DependencyObject);
                e.Handled = true;
            }
        }

        static void HandleClick(object sender, RoutedEventArgs e)
        {
            // Since CheckBox and Button both derive from ButtonBase, which exposes
            // the Click event, we need to make sure that the user is clicking on the
            // Button and not the CheckBox control.
            if (e.Source is Button)
                Console.Clear();
        }
    }
}