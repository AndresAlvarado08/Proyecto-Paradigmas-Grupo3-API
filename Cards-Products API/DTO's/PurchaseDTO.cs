namespace Cards_Products_API.DTO_s
{
    public class PurchaseDTO
    {
        public int Purchase_Id { get; set; }
        public int Card_Id { get; set; }
        public int SubTotal { get; set; }
        public DateTime Purchase_Date { get; set; }
        public int User_Id { get; set; }
    }
}
