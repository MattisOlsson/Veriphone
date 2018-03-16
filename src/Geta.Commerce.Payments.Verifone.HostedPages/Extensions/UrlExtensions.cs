using System;
using System.Web;

namespace Geta.Commerce.Payments.Verifone.HostedPages.Extensions
{
    public static class UrlExtensions
    {

        public static string ToExternalUrl(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            Uri externalUri;

            if (Uri.TryCreate(input, UriKind.Absolute, out externalUri))
            {
                return input;
            }

            var siteUri = HttpContext.Current != null
                            ? HttpContext.Current.Request.Url
                            : null;

            if (siteUri == null)
            {
                return input;
            }

            if (!input.StartsWith("/"))
            {
                input = "/" + input;
            }

            var uriBuilder = new UriBuilder
            {
                Scheme = siteUri.Scheme,
                Host = siteUri.Host,
                Path = input
            };

            if (siteUri.IsDefaultPort == false)
            {
                uriBuilder.Port = siteUri.Port;
            }

            return uriBuilder.ToString();
        }
    }
}