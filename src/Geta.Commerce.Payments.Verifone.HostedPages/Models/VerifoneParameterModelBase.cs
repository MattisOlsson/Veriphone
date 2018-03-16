using System.Collections.Generic;

namespace Geta.Commerce.Payments.Verifone.HostedPages.Models
{
    public class VerifoneParameterModelBase
    {
        protected readonly SortedDictionary<string, string> _parameters;

        public VerifoneParameterModelBase()
        {
            _parameters = new SortedDictionary<string, string>();
        }

        public virtual SortedDictionary<string, string> GetParameters()
        {
            return _parameters;
        }

        public virtual string GetParameterValue(string parameterName)
        {
            return _parameters.ContainsKey(parameterName)
                ? _parameters[parameterName]
                : null;
        }

        public virtual void SetParameterValue(string parameterName, string parameterValue)
        {
            _parameters[parameterName] = parameterValue;
        }
    }
}