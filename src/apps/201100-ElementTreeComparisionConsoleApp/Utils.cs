using System;
using System.Windows;

namespace ElementTreeComparisionConsoleApp
{
    static class Utils
    {
        /// <summary>
        /// Writes information about 'elem' to the console.
        /// </summary>
        public static void DumpElement(object elem, bool isInitialElement, int indentLevel)
        {
            string indentation = new string(' ', indentLevel);
            string typeName = elem == null ? "(null)" : elem.GetType().Name;
            string text = String.Format("{0}{1}) {2}", indentation, indentLevel, typeName);

            if (isInitialElement)
                text += " [YOU CLICKED HERE]";

            Console.WriteLine(text);
        }

        /// <summary>
        /// Returns the TemplatedParent property value if the object is a
        /// FrameworkElement or FrameworkContentElement.
        /// </summary>
        public static DependencyObject GetTemplatedParent(DependencyObject depObj)
        {
            FrameworkElement fe = depObj as FrameworkElement;
            FrameworkContentElement fce = depObj as FrameworkContentElement;

            DependencyObject result;

            if (fe != null)
                result = fe.TemplatedParent;
            else if (fce != null)
                result = fce.TemplatedParent;
            else
                result = null;

            return result;
        }
    }
}
