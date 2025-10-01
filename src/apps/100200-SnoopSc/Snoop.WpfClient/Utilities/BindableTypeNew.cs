namespace Snoop.WpfClient.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public class BindableTypeNew : INotifyPropertyChanged
    {
        public BindableTypeNew(Type type)
        {
            this.Type = type;
        }

        public Type Type { get; }

        public Type UnderlyingSystemType => this.Type.UnderlyingSystemType;

        public string Name => this.Type.Name;

        public Guid GUID => this.Type.GUID;

        public Module Module => this.Type.Module;

        public Assembly Assembly => this.Type.Assembly;

        public string? FullName => this.Type.FullName;

        public string? Namespace => this.Type.Namespace;

        public string? AssemblyQualifiedName => this.Type.AssemblyQualifiedName;

        public Type? BaseType => this.Type.BaseType;

        public bool IsEnum => this.Type.IsEnum;

        public bool IsGenericType => this.Type.IsGenericType;

        public bool IsValueType => this.Type.IsValueType;

        public event PropertyChangedEventHandler? PropertyChanged;

        // Not clear whts happening here with this attribute - commenting out for now
        // [return: NotNullIfNotNull("type")]
        public static implicit operator BindableTypeNew?(Type? type)
        {
            return ToBindableType(type);
        }

        // Not clear whts happening here with this attribute - commenting out for now
        // [return: NotNullIfNotNull("type")]
        public static implicit operator Type?(BindableTypeNew? type)
        {
            return ToType(type);
        }

        // Not clear whts happening here with this attribute - commenting out for now
        // [return: NotNullIfNotNull("type")]
        public static BindableTypeNew? FromType(Type? type)
        {
            return ToBindableType(type);
        }

        // Not clear whts happening here with this attribute - commenting out for now
        // [return: NotNullIfNotNull("type")]
        public static BindableTypeNew? ToBindableType(Type? type)
        {
            if (type is null)
            {
                return null;
            }

            return new(type);
        }

        // Not clear whts happening here with this attribute - commenting out for now
        // [return: NotNullIfNotNull("type")]
        public static Type? FromBindableType(BindableTypeNew? type)
        {
            return ToType(type);
        }

        // Not clear whts happening here with this attribute - commenting out for now
        // [return: NotNullIfNotNull("type")]
        public static Type? ToType(BindableTypeNew? type)
        {
            return type?.Type;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new(propertyName));
        }

        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return this.Type.GetCustomAttributes(attributeType, inherit);
        }
    }
}
