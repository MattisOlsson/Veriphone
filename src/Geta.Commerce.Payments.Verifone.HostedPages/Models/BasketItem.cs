namespace Geta.Commerce.Payments.Verifone.HostedPages.Models
{
    public class BasketItem
    {
        public string Name { get; set; }
        public decimal UnitCost { get; set; }
        public decimal UnitCount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal VatPercentage { get; set; }
        public decimal DiscountPercentage { get; set; }
    }
}