using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace ElementTreeComparisionConsoleApp
{
    static class VisualTreeDumper
    {
        public static void Dump(DependencyObject originalElement)
        {
            DependencyObject closestVisualAncestor = FindClosestVisualAncestor(originalElement);
            DependencyObject visualRoot = FindVisualTreeRoot(originalElement);

            Console.WriteLine("DUMPING VISUAL TREE:");
            Console.WriteLine("Original Element: " + originalElement.GetType().Name);

            string closestParentText = closestVisualAncestor == originalElement ? "(self)" : closestVisualAncestor.GetType().Name;
            Console.WriteLine("Closest Visual Ancestor to Original Element: " + closestParentText);

            DependencyObject templatedParent = Utils.GetTemplatedParent(closestVisualAncestor);
            string templatedParentType = templatedParent == null ? "(null)" : templatedParent.GetType().Name;
            Console.WriteLine("TemplatedParent of Closest Visual Ancestor: " + templatedParentType);

            Console.WriteLine();

            // Write out the visual tree to the console.
            DoDump(visualRoot, closestVisualAncestor, 0);

            Console.WriteLine("************************************************************\n");
        }

        /// <summary>
        /// This method is necessary in case the user clicks on a ContentElement,
        /// which is not part of the visual tree.  It will walk up the logical
        /// tree, if necessary, to find the first ancestor in the visual tree.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        static DependencyObject FindClosestVisualAncestor(DependencyObject initial)
        {
            if (initial is Visual || initial is Visual3D)
                return initial;

            DependencyObject current = initial;
            DependencyObject result = initial;

            while (current != null)
            {
                result = current;
                if (current is Visual || current is Visual3D)
                {
                    result = current;
                    break;
                }
                else
                {
                    // If we're in Logical Land then we must walk up the logical tree
                    // until we find a Visual/Visual3D to get us back to Visual Land.
                    current = LogicalTreeHelper.GetParent(current);
                }
            }

            return result;
        }

        static DependencyObject FindVisualTreeRoot(DependencyObject initial)
        {
            DependencyObject current = initial;
            DependencyObject result = initial;

            while (current != null)
            {
                result = current;
                if (current is Visual || current is Visual3D)
                {
                    current = VisualTreeHelper.GetParent(current);
                }
                else
                {
                    // If we're in Logical Land then we must walk up the logical tree
                    // until we find a Visual/Visual3D to get us back to Visual Land.
                    current = LogicalTreeHelper.GetParent(current);
                }
            }

            return result;
        }

        static void DoDump(DependencyObject current, DependencyObject initial, int indentLevel)
        {
            Utils.DumpElement(current, current == initial, indentLevel);

            int visualChildrenCount = VisualTreeHelper.GetChildrenCount(current);

            for (int i = 0; i < visualChildrenCount; ++i)
            {
                DependencyObject visualChild = VisualTreeHelper.GetChild(current, i);
                DoDump(visualChild, initial, indentLevel + 1);
            }
        }
    }
}