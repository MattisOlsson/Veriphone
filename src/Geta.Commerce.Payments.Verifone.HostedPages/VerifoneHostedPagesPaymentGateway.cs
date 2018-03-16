using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Plugins.Payment;

namespace Geta.Commerce.Payments.Verifone.HostedPages
{
    public class VerifoneHostedPagesPaymentGateway : AbstractPaymentGateway
    {
        public override bool ProcessPayment(Payment payment, ref string message)
        {
            return true;
        }
    }
}