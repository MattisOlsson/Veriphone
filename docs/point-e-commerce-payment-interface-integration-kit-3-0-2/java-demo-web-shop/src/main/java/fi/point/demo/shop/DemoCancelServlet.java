package fi.point.demo.shop;

import org.apache.commons.codec.binary.Hex;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.io.PrintWriter;
import java.security.PublicKey;
import java.util.Map;
import java.util.TreeMap;

/**
 * Cancel request handling servlet.
 */
public class DemoCancelServlet extends HttpServlet {

    private static final long serialVersionUID = 1L;

    @Override
    protected final void doPost(final HttpServletRequest request, final HttpServletResponse response)
            throws ServletException, IOException {
        response.setContentType("text/html");
        response.setCharacterEncoding("utf-8");
        response.setHeader("Pragma", "no-cache");
        response.setHeader("Cache-Control", "no-cache,no-store,max-age=0");
        response.setDateHeader("Expires", 1);
        response.setContentType("text/html; charset=UTF-8");

        final PrintWriter out = response.getWriter();
        final TreeMap<String, String> parameters = gatherRequestParameters(request);        
        verifySignedParameters(request, parameters);

        out.println("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional\" \"http://www.w3.org/TR/html4/loose.dtd\">");
        out.println("<html>");
        out.println("<head>");
        out.println("<title>");
        out.println("Demo Cancel Page");
        out.println("</title>");
        out.println("</head>");
        out.println("<body>");
        out.println("<form action=\"order\" method=\"post\">");
        out.println("<h1>Demo Cancel Receipt</h1>");
        out.println("<table>");
        for (final Map.Entry<String, String> entry : parameters.entrySet()) {
            outputTableLine(out, entry.getKey(), entry.getKey(), entry.getValue());
        }
        out.println("</table>");
        out.println("<input class=\"forward-button\" type=\"submit\" name=\"submit\" value=\"New Order\" />");
        out.println("</form>");
        out.println("</body>");
        out.println("</html>");

        out.close();

        System.out.println(String.format("DemoCancelServlet Post from %s : %s ", request.getRemoteAddr(), request.getRemotePort()));        
        System.out.println("Request Parameters");
        for (Object key : request.getParameterMap().keySet()) {
            System.out.println(String.format("%s %s", key, request.getParameter((String) key)));
        }
    }

    private void verifySignedParameters(final HttpServletRequest request, final TreeMap<String,
            String> parameters) throws ServletException { 
        
        final PublicKey paypagePublicKey = (PublicKey) getServletContext().getAttribute("payment-page-public-key");
        final String signatureOne = request.getParameter("s-t-256-256_signature-one");
        final String signatureTwo = request.getParameter("s-t-256-256_signature-two");
        final byte[] paramData = PointSignatureUtil.formatParameters(parameters);
        parameters.put("s-t-256-256_signature-one", signatureOne);
        parameters.put("s-t-256-256_signature-two", signatureTwo);

        try {
            final byte[] signatureDataOne = Hex.decodeHex(signatureOne.toCharArray());
            final byte[] signatureDataTwo = Hex.decodeHex(signatureTwo.toCharArray());

            boolean sigResultOne = PointSignatureUtil.verify(paramData, signatureDataOne,
                    paypagePublicKey, "RSA", "SHA-1");
            boolean sigResultTwo = PointSignatureUtil.verify(paramData, signatureDataTwo,
                    paypagePublicKey, "RSA", "SHA-512");
            parameters.put("Signature 1 is valid", Boolean.toString(sigResultOne));
            parameters.put("Signature 2 is valid", Boolean.toString(sigResultTwo));
        } catch (Exception e) {
            throw new ServletException(e);
        }

    }


    private TreeMap<String, String> gatherRequestParameters(final HttpServletRequest request) {
        
        final TreeMap<String, String> parameters = new TreeMap<String, String>();
        String parameterKey = "";
        parameterKey = "s-f-1-36_order-number";
        parameters.put(parameterKey , request.getParameter(parameterKey));
        parameterKey = "s-t-1-30_cancel-reason";
        parameters.put(parameterKey , request.getParameter(parameterKey));
        parameterKey = "s-f-1-10_software-version";
        parameters.put(parameterKey , request.getParameter(parameterKey));
        parameterKey = "i-f-1-11_interface-version";
        parameters.put(parameterKey , request.getParameter(parameterKey));
        return parameters;
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

}
