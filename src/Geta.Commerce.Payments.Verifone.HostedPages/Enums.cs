namespace Geta.Commerce.Payments.Verifone.HostedPages
{
    public enum StatusCode
    {
        OK,
        OrderNotFound,
        TransactionNumberMissing,
        InterfaceVersionMissing,
        OrderCurrencyCodeMissing,
        OrderGrossAmountMissing,
        SoftwareVersionMissing,
        OrderTimestampMissing,
        OrderTimestampMismatch,
        OrderGrossAmountMismatch,
        OrderCurrencyCodeMismatch,
        SignatureInvalid
    }

    public enum CancelReason
    {
        CancelPaymentRejected
    }
}