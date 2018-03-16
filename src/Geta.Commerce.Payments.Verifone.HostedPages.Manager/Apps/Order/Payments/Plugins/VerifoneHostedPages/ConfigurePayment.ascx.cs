using Geta.Commerce.Payments.Verifone.HostedPages.Extensions;
using Geta.Verifone;
using Mediachase.Commerce.Orders.Dto;
using Mediachase.Web.Console.Interfaces;

namespace Geta.Commerce.Payments.Verifone.HostedPages.Manager.Apps.Order.Payments.Plugins.VerifoneHostedPages
{
    public partial class ConfigurePayment : System.Web.UI.UserControl, IGatewayControl
    {
        public string ValidationGroup { get; set; }

        public void LoadObject(object dto)
        {
            var paymentMethod = dto as PaymentMethodDto;
            if (paymentMethod == null)
                return;

            var isProduction = bool.Parse(paymentMethod.GetParameter(VerifoneConstants.Configuration.IsProduction, "false"));

            txtMerchantAgreementCode.Text = paymentMethod.GetParameter(VerifoneConstants.Configuration.MerchantAgreementCode, "");
            txtSuccessUrl.Text = paymentMethod.GetParameter(VerifoneConstants.Configuration.SuccessUrl, "");
            txtCancelUrl.Text = paymentMethod.GetParameter(VerifoneConstants.Configuration.CancelUrl, "");
            txtErrorUrl.Text = paymentMethod.GetParameter(VerifoneConstants.Configuration.ErrorUrl, "");
            txtExpiredUrl.Text = paymentMethod.GetParameter(VerifoneConstants.Configuration.ExpiredUrl, "");
            txtRejectedUrl.Text = paymentMethod.GetParameter(VerifoneConstants.Configuration.RejectedUrl, "");
            txtWebShopName.Text = paymentMethod.GetParameter(VerifoneConstants.Configuration.WebShopName, "My web shop");
            cbIsProduction.Checked = isProduction;
            txtProductionUrl.Text = paymentMethod.GetParameter(VerifoneConstants.Configuration.ProductionUrl, VerifoneConstants.Configuration.PaymentProductionNode1Url);
            txtProductionUrl2.Text = paymentMethod.GetParameter(VerifoneConstants.Configuration.ProductionUrl2, VerifoneConstants.Configuration.PaymentProductionNode2Url);
        }

        public void SaveChanges(object dto)
        {
            if (!Visible)
                return;

            var paymentMethod = dto as PaymentMethodDto;
            if (paymentMethod == null)
                return;

            paymentMethod.SetParameter(VerifoneConstants.Configuration.IsProduction, cbIsProduction.Checked ? "true" : "false");
            paymentMethod.SetParameter(VerifoneConstants.Configuration.MerchantAgreementCode, txtMerchantAgreementCode.Text);
            paymentMethod.SetParameter(VerifoneConstants.Configuration.SuccessUrl, txtSuccessUrl.Text);
            paymentMethod.SetParameter(VerifoneConstants.Configuration.CancelUrl, txtCancelUrl.Text);
            paymentMethod.SetParameter(VerifoneConstants.Configuration.ErrorUrl, txtErrorUrl.Text);
            paymentMethod.SetParameter(VerifoneConstants.Configuration.ExpiredUrl, txtExpiredUrl.Text);
            paymentMethod.SetParameter(VerifoneConstants.Configuration.RejectedUrl, txtRejectedUrl.Text);
            paymentMethod.SetParameter(VerifoneConstants.Configuration.WebShopName, txtWebShopName.Text);
            paymentMethod.SetParameter(VerifoneConstants.Configuration.ProductionUrl, txtProductionUrl.Text);
            paymentMethod.SetParameter(VerifoneConstants.Configuration.ProductionUrl2, txtProductionUrl2.Text);
        }
    }
}