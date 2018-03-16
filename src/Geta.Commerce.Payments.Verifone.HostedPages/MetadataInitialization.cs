using System.Security.Cryptography;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using Geta.Verifone;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Orders;
using Mediachase.MetaDataPlus;
using Mediachase.MetaDataPlus.Configurator;

namespace Geta.Commerce.Payments.Verifone.HostedPages
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    public class MetadataInitialization : IInitializableModule
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(MetadataInitialization));

        public void Initialize(InitializationEngine context)
        {
            MetaDataContext mdContext = CatalogContext.MetaDataContext;

            //var orderTimestamp = GetOrCreateMetaField(mdContext, MetadataConstants.OrderTimestamp, "Order timestamp", MetaDataType.ShortString);
            //JoinField(mdContext, orderTimestamp, MetadataConstants.PurchaseOrderClass);

            //var orderCurrencyCode = GetOrCreateMetaField(mdContext, MetadataConstants.OrderCurrencyCode, "Order currency code", MetaDataType.ShortString);
            //JoinField(mdContext, orderCurrencyCode, MetadataConstants.PurchaseOrderClass);

            var filingCode = GetOrCreateMetaField(mdContext, MetadataConstants.FilingCode, "Filing code", MetaDataType.ShortString);
            JoinField(mdContext, filingCode, MetadataConstants.OtherPaymentClass);

            var referenceNumber = GetOrCreateMetaField(mdContext, MetadataConstants.ReferenceNumber, "Reference number", MetaDataType.ShortString);
            JoinField(mdContext, referenceNumber, MetadataConstants.OtherPaymentClass);

            var paymentMethodCode = GetOrCreateMetaField(mdContext, MetadataConstants.PaymentMethodCode, "Payment method code", MetaDataType.ShortString);
            JoinField(mdContext, paymentMethodCode, MetadataConstants.OtherPaymentClass);

            //var signatureOne = GetOrCreateMetaField(mdContext, MetadataConstants.SignatureOne, "Signature one", MetaDataType.LongString);
            //JoinField(mdContext, signatureOne, MetadataConstants.PurchaseOrderClass);

            //var signatureTwo = GetOrCreateMetaField(mdContext, MetadataConstants.SignatureTwo, "Signature two", MetaDataType.LongString);
            //JoinField(mdContext, signatureTwo, MetadataConstants.PurchaseOrderClass);
        }

        private MetaField GetOrCreateMetaField(MetaDataContext mdContext, string fieldName, string friendlyName, MetaDataType metadataType)
        {
            var f = MetaField.Load(mdContext, fieldName);

            if (f == null)
            {
                Logger.Debug(string.Format("Adding meta field '{0}' for Verifone payment integration.", fieldName));
                f = MetaField.Create(mdContext, MetadataConstants.OrderNamespace, fieldName, friendlyName, string.Empty, metadataType, 0, true, false, false, false);
            }

            return f;
        }

        private void JoinField(MetaDataContext mdContext, MetaField field, string metaClassName)
        {
            var cls = MetaClass.Load(mdContext, metaClassName);

            if (MetaFieldIsNotConnected(field, cls))
            {
                cls.AddField(field);
            }
        }

        private static bool MetaFieldIsNotConnected(MetaField field, MetaClass cls)
        {
            return cls != null && !cls.MetaFields.Contains(field);
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }
    }
}
