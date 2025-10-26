namespace Cards_Products_API.DTO_s
{
    public class PurchaseDTO
    {
        public int Purchase_Id { get; set; }
        public int Total { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int User_Id { get; set; }
    }
}
