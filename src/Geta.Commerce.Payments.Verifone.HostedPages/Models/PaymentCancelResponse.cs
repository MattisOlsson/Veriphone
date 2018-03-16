using System.Web.Mvc;
using Geta.Commerce.Payments.Verifone.HostedPages.Mvc;
using Geta.Verifone;

namespace Geta.Commerce.Payments.Verifone.HostedPages.Models
{
    [ModelBinder(typeof(VerifoneModelBinder))]
    public class PaymentCancelResponse
    {
        [BindAlias(VerifoneConstants.ParameterName.OrderNumber)]
        public string OrderNumber { get; set; }

        [BindAlias(VerifoneConstants.ParameterName.CancelReason)]
        public string CancelReason { get; set; }

        [BindAlias(VerifoneConstants.ParameterName.SoftwareVersion)]
        public string SoftwareVersion { get; set; }

        [BindAlias(VerifoneConstants.ParameterName.InterfaceVersion)]
        public string InterfaceVersion { get; set; }

        [BindAlias(VerifoneConstants.ParameterName.SignatureOne)]
        public string SignatureOne { get; set; }

        [BindAlias(VerifoneConstants.ParameterName.SignatureTwo)]
        public string SignatureTwo { get; set; }
    }
}