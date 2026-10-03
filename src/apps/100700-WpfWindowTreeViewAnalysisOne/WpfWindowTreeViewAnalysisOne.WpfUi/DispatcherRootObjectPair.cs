using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfWindowTreeViewAnalysisOne.WpfUi
{
    public class DispatcherRootObjectPair : IEquatable<DispatcherRootObjectPair>
    {
        public DispatcherRootObjectPair(Dispatcher dispatcher, object rootObject)
        {
            this.Dispatcher = dispatcher;
            this.RootObject = rootObject;
        }

        public Dispatcher Dispatcher { get; }

        public object RootObject { get; }

        public bool Equals(DispatcherRootObjectPair? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Dispatcher.Equals(other.Dispatcher)
                   && this.RootObject.Equals(other.RootObject);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((DispatcherRootObjectPair)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Dispatcher.GetHashCode() * 397) ^ this.RootObject.GetHashCode();
            }
        }

        public static bool operator ==(DispatcherRootObjectPair? left, DispatcherRootObjectPair? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DispatcherRootObjectPair? left, DispatcherRootObjectPair? right)
        {
            return !Equals(left, right);
        }
    }
}
