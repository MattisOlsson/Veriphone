using EPiServer.Core;

namespace Geta.Commerce.Payments.Verifone.HostedPages
{
    public interface IVerifoneSettings
    {
        ContentReference PaymentRedirectPage { get; }
        string PaymentRedirectAction { get; }
    }
}