using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace SimpleTreeViewProcInjectorOne.MalDll.Infrastructure
{
    [DebuggerDisplay("TotalCount: {TotalCount}, Name: {Name}")]
    public class CustomTreeNode
    {
        public FrameworkElement Value { get; set; }
        public List<CustomTreeNode> Children { get; set; }

        public CustomTreeNode()
        {
            Value = default!;
            Children = [];
            Parent = null!;
        }

        public DependencyObject Parent { get; set; }

        public CustomTreeNode(System.Windows.Controls.UserControl value)
        {
            Value = value;
            Children = [];
            Parent = null!;
        }

        public void AddChild(CustomTreeNode child, DependencyObject dependencyObject)
        {
            child.Parent = Value;
            Children.Add(child);
        }

        /// <summary>
        /// Gets the total count of nodes in the tree, including this node and all its descendants.
        /// </summary>
        public int TotalCount
        {
            get
            {
                int count = 0;

                if (Value != null)
                    count++;

                foreach (var child in Children)
                {
                    count += child.TotalCount; // Recursively count children
                }
                return count;
            }
        }

        public void Clear()
        {
            Children.Clear();
        }

        public string Name
        {
            get
            {
                return ToString();
            }
        }

        override public string ToString()
        {
            // Use runtime type checking for Value, not T
            if (Value is FrameworkElement frameworkElement)
            {
                // You can customize how FrameworkElement is represented here if needed
                return frameworkElement.GetType().Name;
            }

            return Value?.ToString() ?? "root";
        }
    }
}
