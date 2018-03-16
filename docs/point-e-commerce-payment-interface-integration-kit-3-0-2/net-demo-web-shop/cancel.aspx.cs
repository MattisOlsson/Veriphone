using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

public partial class failure : System.Web.UI.Page
{

    protected override void Render(HtmlTextWriter writer)
    {
        SortedDictionary<String, String> parameters = new SortedDictionary<String, String>();
        foreach (String key in this.Request.Form.Keys)
        {
            parameters.Add(key, this.Request.Form[key]);
        }

        writer.WriteLine("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional\" \"http://www.w3.org/TR/html4/loose.dtd\">");
        writer.WriteLine("<html>");
        writer.WriteLine("<head>");
        writer.WriteLine("<title>");
        writer.WriteLine("Demo Order Page");
        writer.WriteLine("</title>");
        writer.WriteLine("</head>");
        writer.WriteLine("<body>");

        writer.WriteLine("<form id=\"integration-form\" action=\"order.aspx\" method=\"post\">");
        writer.WriteLine("<h1>Test Shop Cancel</h1>");

        writer.WriteLine("<table>");

        foreach (KeyValuePair<string, string> kvp in parameters)
        {
            writer.WriteLine("<tr><td>" + kvp.Key + "</td><td><input readonly type=\"text\" name=\"" + kvp.Key + "\" value=\"" + kvp.Value + "\" /></td></tr>");
        }

        String signatureOne = parameters["s-t-256-256_signature-one"];
        String signatureTwo = parameters["s-t-256-256_signature-two"];
        parameters.Remove("s-t-256-256_signature-one");
        parameters.Remove("s-t-256-256_signature-two");
        parameters.Remove("s-t-1-40_shop-order__phase");
        String content = PointSignatureUtil.FormatParameters(parameters);

        Debug.WriteLine("Verifying content: " + content);
        bool signatureOneVerified = PointSignatureUtil.VerifySignature(PointCertificateUtil.GetPointCertificate(), signatureOne, content, HashAlgorithm.SHA1);
        bool signatureTwoVerified = PointSignatureUtil.VerifySignature(PointCertificateUtil.GetPointCertificate(), signatureTwo, content, HashAlgorithm.SHA512);

        writer.WriteLine("<tr><td>Signature One Verified</td><td><input type=\"text\" value=\"" + signatureOneVerified + "\" /></td></tr>");
        writer.WriteLine("<tr><td>Signature Two Verified</td><td><input type=\"text\" value=\"" + signatureTwoVerified + "\" /></td></tr>");

        writer.WriteLine("</table>");
        writer.WriteLine("<input class=\"forward-button\" type=\"submit\" name=\"submit\" value=\"New Order\" />");
        writer.WriteLine("</form>");


        writer.WriteLine("</body>");
        writer.WriteLine("</html>");
    }

}