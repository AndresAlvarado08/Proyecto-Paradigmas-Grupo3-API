namespace Cards_Products_API.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        //FK Keys
        public int CardId { get; set; }
        public int ProductId { get; set; }

        //Navigation Properties
        public Card? Card { get; set; }
        public Product? Product { get; set; }

        //Purchase Details
        public int Amount { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
