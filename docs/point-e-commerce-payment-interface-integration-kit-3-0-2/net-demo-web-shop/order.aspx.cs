using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Text;

public partial class order : System.Web.UI.Page
{
    private const String PAYMENT_SITE_URL = "https://epayment-test.point.fi/pw/payment";
    //private const String PAYMENT_SITE_URL = "http://127.0.0.1:8080/pw/payment";
    private const String SHOP_SITE_URL = "http://localhost:65061/net-demo-web-shop";
    private const String MERCHANT_AGREEMENT_CODE = "demo-merchant-agreement";
    private const String DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

    protected override void Render(HtmlTextWriter writer)
    {
        SortedDictionary<String, String> parameters = new SortedDictionary<String, String>();
        DateTime now = DateTime.Now.ToUniversalTime();

        parameters.Add("locale-f-2-5_payment-locale", "fi_FI");
        parameters.Add("t-f-14-19_payment-timestamp", now.ToString(DATE_FORMAT));
        parameters.Add("s-f-1-36_merchant-agreement-code", MERCHANT_AGREEMENT_CODE);
        parameters.Add("s-f-1-36_order-number", now.Ticks.ToString());
        parameters.Add("t-f-14-19_order-timestamp", now.ToString(DATE_FORMAT));
        parameters.Add("s-t-1-36_order-note", "x213");
        parameters.Add("i-f-1-3_order-currency-code", "978");
        parameters.Add("l-f-1-20_order-net-amount", "1000");
        parameters.Add("l-f-1-20_order-gross-amount", "1230");
        parameters.Add("l-f-1-20_order-vat-amount", "230");
        parameters.Add("i-t-1-4_order-vat-percentage", "2300");
        parameters.Add("s-f-1-30_buyer-first-name", "John");
        parameters.Add("s-f-1-30_buyer-last-name", "Smith");
        parameters.Add("s-t-1-30_buyer-phone-number", "+358 50 234234");
        parameters.Add("s-f-1-100_buyer-email-address", "john.smith@example.com");
        parameters.Add("s-t-1-30_delivery-address-line-one", "Street Address #1");
        parameters.Add("s-t-1-30_delivery-address-line-two", "Street Address #2");
        parameters.Add("s-t-1-30_delivery-address-line-three", "Street Address #3");
        parameters.Add("s-t-1-30_delivery-address-city", "City");
        parameters.Add("s-t-1-30_delivery-address-postal-code", "00234");
        parameters.Add("i-t-1-3_delivery-address-country-code", "246");

        parameters.Add("s-t-1-30_payment-method-code", "");
        parameters.Add("l-t-1-20_saved-payment-method-id", "");
        parameters.Add("s-t-1-30_style-code", "");
        parameters.Add("i-t-1-1_recurring-payment", "0");
        parameters.Add("i-t-1-1_deferred-payment", "0");
        parameters.Add("i-t-1-1_save-payment-method", "0");
        parameters.Add("i-t-1-1_skip-confirmation-page", "0");

        parameters.Add("s-f-5-128_success-url", SHOP_SITE_URL + "/receipt.aspx");
        parameters.Add("s-f-5-128_rejected-url", SHOP_SITE_URL + "/cancel.aspx");
        parameters.Add("s-f-5-128_cancel-url", SHOP_SITE_URL + "/cancel.aspx");
        parameters.Add("s-f-5-128_expired-url", SHOP_SITE_URL + "/cancel.aspx");
        parameters.Add("s-f-5-128_error-url", SHOP_SITE_URL + "/cancel.aspx");

        parameters.Add("s-t-1-30_bi-name-0", "test-basket-item-0");
        parameters.Add("l-t-1-20_bi-unit-cost-0", "100");
        parameters.Add("i-t-1-11_bi-unit-count-0", "1");
        parameters.Add("l-t-1-20_bi-net-amount-0", "100");
        parameters.Add("l-t-1-20_bi-gross-amount-0", "123");
        parameters.Add("i-t-1-4_bi-vat-percentage-0", "2300");
        parameters.Add("i-t-1-4_bi-discount-percentage-0", "0");

        parameters.Add("s-f-1-30_software", "My Web Shop");
        parameters.Add("s-f-1-10_software-version", "1.0.1");
        parameters.Add("i-f-1-11_interface-version", "3");

        String paymentToken = PointSignatureUtil.CreatePaymentToken(parameters);
        parameters.Add("s-f-32-32_payment-token", paymentToken);

        String content = PointSignatureUtil.FormatParameters(parameters);
        Debug.WriteLine("Signing content: " + content);
        String signatureOne = PointSignatureUtil.CreateSignature(PointCertificateUtil.GetMerchantCertificate(), content, HashAlgorithm.SHA1);
        parameters.Add("s-t-256-256_signature-one", signatureOne);

        Debug.WriteLine("Signature one: " + signatureOne);
        Debug.WriteLine("Signature one verified: " + PointSignatureUtil.VerifySignature(PointCertificateUtil.GetMerchantCertificate(), signatureOne, content, HashAlgorithm.SHA1));

        //String signatureTwo = Signer.CreateSignature(StoreName.My, StoreLocation.CurrentUser, "dnQualifier=123456", content, Signer.HashAlgorithm.Sha512);
        //Debug.WriteLine("Signature two: " + signatureTwo);
        //Debug.WriteLine("Signature twp verified: " + Signer.VerifySignature(StoreName.My, StoreLocation.CurrentUser, "dnQualifier=123456", signatureTwo, content, Signer.HashAlgorithm.Sha512));

        writer.WriteLine("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional\" \"http://www.w3.org/TR/html4/loose.dtd\">");
        writer.WriteLine("<html>");
        writer.WriteLine("<head>");
        writer.WriteLine("<title>");
        writer.WriteLine("Demo Order Page");
        writer.WriteLine("</title>");
        writer.WriteLine("</head>");
        writer.WriteLine("<body>");

        writer.WriteLine("<form id=\"integration-form\" action=\"" + PAYMENT_SITE_URL + "\" method=\"post\">");
        writer.WriteLine("<h1>Test Shop Order</h1>");

        writer.WriteLine("<table>");

        foreach (KeyValuePair<string, string> kvp in parameters)
        {
            writer.WriteLine("<tr><td>" + kvp.Key + "</td><td><input readonly type=\"text\" name=\"" + kvp.Key + "\" value=\"" + kvp.Value + "\" /></td></tr>");
        }

        writer.WriteLine("</table>");

        writer.WriteLine("<input type=\"submit\" name=\"s-t-1-40_submit\" value=\"Submit\" />");
        writer.WriteLine("</form>");

        writer.WriteLine("<p>" + content + "</p>");
        writer.WriteLine("</body>");
        writer.WriteLine("</html>");

    }
        
}