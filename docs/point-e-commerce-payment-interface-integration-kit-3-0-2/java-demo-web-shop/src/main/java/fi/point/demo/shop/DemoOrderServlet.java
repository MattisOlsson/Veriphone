package fi.point.demo.shop;

import org.apache.commons.codec.binary.Hex;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.io.PrintWriter;
import java.nio.charset.Charset;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.security.PrivateKey;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Map;
import java.util.TimeZone;
import java.util.TreeMap;

/**
 * Order handling servlet class.
 * @author  Janis Stasans.
 */
public class DemoOrderServlet extends HttpServlet {

    private static final long serialVersionUID = 1L;

    private final SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");

    public DemoOrderServlet() {
        dateFormat.setTimeZone(TimeZone.getTimeZone("GMT"));
    }
    
    @Override
    public final void doGet(final HttpServletRequest request, final HttpServletResponse response)
            throws ServletException, IOException {
        render(request, response);
    }

    @Override
    protected final void doPost(final HttpServletRequest request, final HttpServletResponse response)
            throws ServletException, IOException {
        render(request, response);
    }

    protected final void render(final HttpServletRequest request, final HttpServletResponse response)
            throws ServletException, IOException {
        response.setContentType("text/html; charset=UTF-8");

        final PrintWriter out = response.getWriter();
        final String paymentSiteUrl = (String) getServletContext().getAttribute("payment-page-url");
        final PrivateKey privateKey = (PrivateKey) getServletContext().getAttribute("shop-private-key");
        final String shopSiteUrl = (String) getServletContext().getAttribute("shop-url");
        final String merchantAgreementCode = (String) getServletContext().getAttribute("merchant-agreement-code");
        final TreeMap<String, String> parameters = fillParemeters(shopSiteUrl, merchantAgreementCode);
        
        signParameters(parameters, privateKey);
        
        out.println("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional\" \"http://www.w3.org/TR/html4/loose.dtd\">");
        out.println("<html>");
        out.println("<head>");
        out.println("<title>");
        out.println("Demo Order Page");
        out.println("</title>");
        out.println("</head>");
        out.println("<body>");        
        out.println("<form id=\"integration-form\" action=\"" + paymentSiteUrl + "\" method=\"post\">");
        out.println("<h1>Demo Shop Order</h1>");
        out.println("<table>");
        for (final Map.Entry<String, String> entry : parameters.entrySet()) {
            outputTableLine(out, entry.getKey(), entry.getKey(), entry.getValue());
        }
        out.println("</table>");        
        out.println("<input type=\"submit\" name=\"s-t-1-40_submit\" value=\"Submit\" />");
        out.println("</form>");
        out.println("</body>");
        out.println("</html>");

        out.close();
    }

    private void signParameters(final TreeMap<String, String> parameters, final PrivateKey privateKey) {
        parameters.remove("s-t-256-256_signature-one");
        parameters.remove("s-t-256-256_signature-two");
        parameters.remove("s-t-1-40_submit");

        final byte[] parameterData = PointSignatureUtil.formatParameters(parameters);
        final String signatureOne = new String(Hex.encodeHex((PointSignatureUtil.sign(parameterData, privateKey,
                "RSA", "SHA-1")))).toUpperCase();
        parameters.put("s-t-256-256_signature-one", signatureOne);
        //final String signatureTwo = new String(Hex.encodeHex((PointSignatureUtil.sign(parameterData, privateKey, "RSA", "SHA-512")))).toUpperCase();
        //parameters.put("s-t-256-256_signature-two", signatureTwo);
    }

    private void outputTableLine(final PrintWriter out,
                                 final String header,
                                 final String name,
                                 final String value) {

        final StringBuilder builder = new StringBuilder();
        builder.append("<tr><td>");
        builder.append(header);
        builder.append("</td><td><input type=\"text\" readonly name=\"");
        builder.append(name);
        builder.append("\"value=\"");
        builder.append(value);
        builder.append("\" /></td></tr>");
        out.println(builder.toString());
    }

    /**
     * Fill parameter data.
     * @param shopSiteUrl shop site URL
     * @return parameter map
     */
    private TreeMap<String, String> fillParemeters(final String shopSiteUrl, final String merchantAgreementCode) {
        final TreeMap<String, String> parameters = new TreeMap<String, String>();
        final Date now = new Date();

        parameters.put("locale-f-2-5_payment-locale", "fi_FI");
        parameters.put("t-f-14-19_payment-timestamp", dateFormat.format(now));
        
        parameters.put("s-f-1-36_merchant-agreement-code", merchantAgreementCode);
        parameters.put("s-f-1-36_order-number", Long.toString(System.currentTimeMillis()));
        parameters.put("t-f-14-19_order-timestamp", dateFormat.format(now));
        parameters.put("s-t-1-36_order-note", "x213");
        parameters.put("i-f-1-3_order-currency-code", "978");
        parameters.put("l-f-1-20_order-net-amount", "1000");
        parameters.put("l-f-1-20_order-gross-amount", "1230");
        parameters.put("l-f-1-20_order-vat-amount", "230");
        parameters.put("i-t-1-4_order-vat-percentage", "2300");
        parameters.put("s-f-1-30_buyer-first-name", "Matti");
        parameters.put("s-f-1-30_buyer-last-name", "Meikäläinen");
        parameters.put("s-t-1-30_buyer-phone-number", "+358 50 234234");
        parameters.put("s-f-1-100_buyer-email-address", "john.smith@example.com");
        parameters.put("s-t-1-30_delivery-address-line-one", "Street Address #1");
        parameters.put("s-t-1-30_delivery-address-line-two", "Street Address #2");
        parameters.put("s-t-1-30_delivery-address-line-three", "Street Address #3");
        parameters.put("s-t-1-30_delivery-address-city", "City");
        parameters.put("s-t-1-30_delivery-address-postal-code", "00234");
        parameters.put("i-t-1-3_delivery-address-country-code", "246");
        
        parameters.put("s-t-1-30_payment-method-code", "");            
        parameters.put("l-t-1-20_saved-payment-method-id", "");
        parameters.put("s-t-1-30_style-code", "");
        parameters.put("i-t-1-1_recurring-payment", "0");
        parameters.put("i-t-1-1_deferred-payment", "0");     
        parameters.put("i-t-1-1_save-payment-method", "0");  
        parameters.put("i-t-1-1_skip-confirmation-page", "0");
        
        parameters.put("s-f-5-128_success-url", shopSiteUrl + "/receipt");
        parameters.put("s-f-5-128_rejected-url", shopSiteUrl + "/cancel");
        parameters.put("s-f-5-128_cancel-url", shopSiteUrl + "/cancel");
        parameters.put("s-f-5-128_expired-url", shopSiteUrl + "/cancel");
        parameters.put("s-f-5-128_error-url", shopSiteUrl + "/cancel"); 

        parameters.put("s-t-1-30_bi-name-0", "test-basket-item-0");
        parameters.put("l-t-1-20_bi-unit-cost-0", "100");
        parameters.put("i-t-1-11_bi-unit-count-0", "1");
        parameters.put("l-t-1-20_bi-net-amount-0", "100");
        parameters.put("l-t-1-20_bi-gross-amount-0", "123");
        parameters.put("i-t-1-4_bi-vat-percentage-0", "2300");
        parameters.put("i-t-1-4_bi-discount-percentage-0", "0");

        parameters.put("s-f-1-30_software", "My Web Shop");
        parameters.put("s-f-1-10_software-version", "1.0.1");
        parameters.put("i-f-1-11_interface-version", "3");

        parameters.put("t-f-14-19_payment-timestamp", dateFormat.format(new Date()));
        final String paymentTokenContent = parameters.get("s-f-1-36_merchant-agreement-code") + ";"
                + parameters.get("s-f-1-36_order-number") + ";"
                + parameters.get("t-f-14-19_payment-timestamp");
        try {
            final MessageDigest digest = MessageDigest.getInstance("SHA-256");
            final String digestvalue = new String(Hex.encodeHex(digest.digest(
                    paymentTokenContent.getBytes(Charset.forName("UTF8"))))).substring(0, 32);
            parameters.put("s-f-32-32_payment-token", digestvalue.toUpperCase());

        } catch (NoSuchAlgorithmException e) {
            throw new RuntimeException("SHA-256 algorithm not available.");
        }
        
        return parameters;
    }

}
