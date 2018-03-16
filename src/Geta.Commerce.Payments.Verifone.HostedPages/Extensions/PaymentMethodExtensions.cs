using System;
using Geta.Verifone;
using Mediachase.Commerce.Orders.Dto;
using Mediachase.Commerce.Orders.Exceptions;

namespace Geta.Commerce.Payments.Verifone.HostedPages.Extensions
{
    public static class PaymentMethodsExtensions
    {
        public static string GetParameter(this PaymentMethodDto.PaymentMethodParameterRow row, string name, string defaultValue = null)
        {
            if (row == null || string.IsNullOrWhiteSpace(row.Value))
            {
                if (defaultValue != null)
                    return defaultValue;

                throw new PaymentException(
                    PaymentException.ErrorType.ConfigurationError,
                    "NO_SETTING",
                    "Verifone payment provider: Required setting '" + name + "' is not specified.");
            }

            return row.Value;
        }

        public static string GetParameter(this PaymentMethodDto paymentMethod, string name, string defaultValue = null)
        {
            var row = GetParameterRow(paymentMethod, name);

            return row.GetParameter(name, defaultValue);
        }

        public static void SetParameter(this PaymentMethodDto paymentMethod, string name, string value)
        {
            var row = GetParameterRow(paymentMethod, name);

            if (row != null)
            {
                row.Value = value;
            }
            else
            {
                row = paymentMethod.PaymentMethodParameter.NewPaymentMethodParameterRow();
                row.PaymentMethodId = paymentMethod.PaymentMethod.Count > 0 ? paymentMethod.PaymentMethod[0].PaymentMethodId : Guid.Empty;
                row.Parameter = name;
                row.Value = value;

                paymentMethod.PaymentMethodParameter.Rows.Add(row);
            }
        }

        public static string GetPaymentUrl(this PaymentMethodDto paymentMethod)
        {
            return paymentMethod.GetParameter(VerifoneConstants.Configuration.IsProduction, "false") == "true"
                ? paymentMethod.GetParameter(VerifoneConstants.Configuration.ProductionUrl, VerifoneConstants.Configuration.PaymentProductionNode1Url)
                : VerifoneConstants.Configuration.PaymentTestUrl;
        }

        public static bool IsVerifoneProduction(this PaymentMethodDto paymentMethod)
        {
            return paymentMethod.GetParameter(VerifoneConstants.Configuration.IsProduction, "false") == "true";
        }

        public static string GetPaymentNode1Url(this PaymentMethodDto paymentMethod)
        {
            return paymentMethod.GetParameter(VerifoneConstants.Configuration.ProductionUrl, VerifoneConstants.Configuration.PaymentProductionNode1Url);
        }

        public static string GetPaymentNode2Url(this PaymentMethodDto paymentMethod)
        {
            return paymentMethod.GetParameter(VerifoneConstants.Configuration.ProductionUrl2, VerifoneConstants.Configuration.PaymentProductionNode2Url);
        }

        public static string GetPaymentTestUrl(this PaymentMethodDto paymentMethod)
        {
            return VerifoneConstants.Configuration.PaymentTestUrl;
        }

        public static string GetMerchantAgreementCode(this PaymentMethodDto paymentMethod)
        {
            return paymentMethod.GetParameter(VerifoneConstants.Configuration.MerchantAgreementCode, "demo-agreement-code");
        }

        internal static PaymentMethodDto.PaymentMethodParameterRow GetParameterRow(this PaymentMethodDto paymentMethod, string name)
        {
            var rows = (PaymentMethodDto.PaymentMethodParameterRow[])paymentMethod.PaymentMethodParameter.Select(string.Format("Parameter = '{0}'", name));

            return rows.Length > 0 ? rows[0] : null;
        }
    }
}
