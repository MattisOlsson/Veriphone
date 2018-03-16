using System;

namespace Geta.Commerce.Payments.Verifone.HostedPages.Mvc
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class BindAliasAttribute : Attribute
    {
        public string Alias { get; private set; }

        public BindAliasAttribute(string alias)
        {
            Alias = alias;
        }
    }
}