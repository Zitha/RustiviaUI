namespace IntroductionMVC5.Models.ArsloTrading
{
    public class ArsloInvoiceItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
