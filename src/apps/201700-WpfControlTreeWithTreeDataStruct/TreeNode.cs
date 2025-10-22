using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlTreeWithTreeDataStruct
{
    [DebuggerDisplay("TotalCount: {TotalCount}, Name: {Name}")]
    public class TreeNode
    {
        public FrameworkElement Value { get; set; }
        public List<TreeNode> Children { get; set; }

        public TreeNode()
        {
            Value = default!;
            Children = [];
            Parent = null!;
        }

        public DependencyObject Parent { get; set; }

        public TreeNode(UserControl value)
        {
            Value = value;
            Children = [];
            Parent = null!;
        }

        public void AddChild(TreeNode child, DependencyObject dependencyObject)
        {
            child.Parent = Value;
            Children.Add(child);
        }

        /// <summary>
        /// Gets the total count of nodes in the tree, including this node and all its descendants.
        /// </summary>
        public int TotalCount
        {
            get {
                int count = 0;

                if(Value != null)
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
