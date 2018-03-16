using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography.X509Certificates;
using System.Text;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void Render(HtmlTextWriter writer)
    {
        String dataString = "000102030405060708";
        byte[] data = PointSignatureUtil.HexStringToByteArray(dataString);
        String signatureString = PointSignatureUtil.CreateSignature(PointCertificateUtil.GetMerchantCertificate(), Encoding.UTF8.GetString(data), HashAlgorithm.SHA1);

        writer.WriteLine("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional\" \"http://www.w3.org/TR/html4/loose.dtd\">");
        writer.WriteLine("<html>");
        writer.WriteLine("<head>");
        writer.WriteLine("<title>");
        writer.WriteLine("Test Order Page");
        writer.WriteLine("</title>");
        writer.WriteLine("</head>");
        writer.WriteLine("<body>");

        writer.WriteLine("<h1>Signature Test</h1>");

        writer.WriteLine("<p>");
        writer.WriteLine("data hex: '" + dataString + "'<br/>");
        writer.WriteLine("sha-1 with rsa signqature hex: '" + signatureString + "'<br/>");
        writer.WriteLine("</p>");

        writer.WriteLine("</body>");
        writer.WriteLine("</html>");
    }
}