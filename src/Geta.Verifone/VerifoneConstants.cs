namespace Geta.Verifone
{
    public class VerifoneConstants
    {
        public static class Configuration
        {
            public const string IsProduction = "IsProduction";
            public const string MerchantAgreementCode = "MerchantAgreementCode";
            public const string ProductionUrl = "ProductionUrl";
            public const string ProductionUrl2 = "ProductionUrl2";
            public const string CancelUrl = "CancelUrl";
            public const string ErrorUrl = "ErrorUrl";
            public const string ExpiredUrl = "ExpiredUrl";
            public const string RejectedUrl = "RejectedUrl";
            public const string SuccessUrl = "SuccessUrl";
            public const string WebShopName = "WebShopName";
            public const string PaymentProductionNode1Url = "https://epayment1.point.fi/pw/payment";
            public const string PaymentProductionNode2Url = "https://epayment2.point.fi/pw/payment";
            public const string PaymentTestUrl = "https://epayment.test.point.fi/pw/payment";
        }

        public static class MetaClass
        {
            public const string VerifoneCreditCardPayment = "VerifoneCreditCardPayment";
        }

        public static class ParameterName
        {
            public const string PaymentLocale = "locale-f-2-5_payment-locale";
            public const string PaymentTimestamp = "t-f-14-19_payment-timestamp";
            public const string MerchantAgreementCode = "s-f-1-36_merchant-agreement-code";
            public const string OrderNumber = "s-f-1-36_order-number";
            public const string OrderTimestamp = "t-f-14-19_order-timestamp";
            public const string OrderCurrencyCode = "i-f-1-3_order-currency-code";
            public const string OrderNetAmount = "l-f-1-20_order-net-amount";
            public const string OrderNote = "s-t-1-36_order-note";
            public const string OrderGrossAmount = "l-f-1-20_order-gross-amount";
            public const string OrderVatAmount = "l-f-1-20_order-vat-amount";
            public const string OrderVatPercentage = "i-t-1-4_order-vat-percentage";
            public const string BuyerFirstName = "s-f-1-30_buyer-first-name";
            public const string BuyerLastName = "s-f-1-30_buyer-last-name";
            public const string BuyerPhoneNumber = "s-t-1-30_buyer-phone-number";
            public const string BuyerEmailAddress = "s-f-1-100_buyer-email-address";
            public const string DeliveryAddressLineOne = "s-t-1-30_delivery-address-line-one";
            public const string DeliveryAddressLineTwo = "s-t-1-30_delivery-address-line-two";
            public const string DeliveryAddressLineThree = "s-t-1-30_delivery-address-line-three";
            public const string DeliveryAddressCity = "s-t-1-30_delivery-address-city";
            public const string DeliveryAddressPostalCode = "s-t-1-30_delivery-address-postal-code";
            public const string DeliveryAddressCountryCode = "i-t-1-3_delivery-address-country-code";
            public const string PaymentMethodCode = "s-t-1-30_payment-method-code";
            public const string SavedPaymentMethodId = "l-t-1-20_saved-payment-method-id";
            public const string StyleCode = "s-t-1-30_style-code";
            public const string RecurringPayment = "i-t-1-1_recurring-payment";
            public const string DeferredPayment = "i-t-1-1_deferred-payment";
            public const string SavePaymentMethod = "i-t-1-1_save-payment-method";
            public const string SkipConfirmationPage = "i-t-1-1_skip-confirmation-page";
            public const string ShortSuccessUrl = "s-f-5-128_success-url";
            public const string LongSuccessUrl = "s-f-5-256_success-url";
            public const string ShortRejectedUrl = "s-f-5-128_rejected-url";
            public const string LongRejectedUrl = "s-f-5-256_rejected-url";
            public const string ShortCancelUrl = "s-f-5-128_cancel-url";
            public const string LongCancelUrl = "s-f-5-256_cancel-url";
            public const string ShortExpiredUrl = "s-f-5-128_expired-url";
            public const string LongExpiredUrl = "s-f-5-256_expired-url";
            public const string ShortErrorUrl = "s-f-5-128_error-url";
            public const string LongErrorUrl = "s-f-5-256_error-url";
            public const string BasketItemNamePrefix = "s-t-1-30_bi-name-";
            public const string BasketItemUnitCostPrefix = "l-t-1-20_bi-unit-cost-";
            public const string BasketItemUnitCountPrefix = "i-t-1-11_bi-unit-count-";
            public const string BasketItemNetAmountPrefix = "l-t-1-20_bi-net-amount-";
            public const string BasketItemGrossAmountPrefix = "l-t-1-20_bi-gross-amount-";
            public const string BasketItemVatPercentagePrefix = "i-t-1-4_bi-vat-percentage-";
            public const string BasketItemDiscountPercentagePrefix = "i-t-1-4_bi-discount-percentage-";
            public const string Software = "s-f-1-30_software";
            public const string SoftwareVersion = "s-f-1-10_software-version";
            public const string InterfaceVersion = "i-f-1-11_interface-version";
            public const string PaymentToken = "s-f-32-32_payment-token";
            public const string SignatureOne = "s-t-256-256_signature-one";
            public const string SignatureTwo = "s-t-256-256_signature-two";
            public const string TransactionNumber = "l-f-1-20_transaction-number";
            public const string PaymentMethodCodeResponse = "s-f-1-30_payment-method-code";
            public const string ReferenceNumber = "s-f-1-20_reference-number";
            public const string Token = "s-t-1-256_token";
            public const string FilingCode = "s-t-1-26_filing-code";
            public const string SocialSecurityNumber = "s-t-0-11_social-security-number";
            public const string CardExpectedValidity = "s-t-1-6_card-expected-validity";
            public const string CancelReason = "s-t-1-30_cancel-reason";
            public const string Submit = "s-t-1-40-submit";
        }
    }
}