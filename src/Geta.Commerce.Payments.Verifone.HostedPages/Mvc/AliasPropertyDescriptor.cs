using System;
using System.ComponentModel;
using System.Linq;

namespace Geta.Commerce.Payments.Verifone.HostedPages.Mvc
{
    public class AliasPropertyDescriptor : PropertyDescriptor
    {
        private readonly PropertyDescriptor _inner;

        public AliasPropertyDescriptor(string alias, PropertyDescriptor inner) : base(alias, inner.Attributes.OfType<Attribute>().ToArray())
        {
            _inner = inner;
        }

        public override bool CanResetValue(object component)
        {
            return _inner.CanResetValue(component);
        }

        public override object GetValue(object component)
        {
            return _inner.GetValue(component);
        }

        public override void ResetValue(object component)
        {
            _inner.ResetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            _inner.SetValue(component, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return _inner.ShouldSerializeValue(component);
        }

        public override Type ComponentType { get { return _inner.ComponentType; } }
        public override bool IsReadOnly { get { return _inner.IsReadOnly; } }
        public override Type PropertyType { get { return _inner.PropertyType; } }
    }
}