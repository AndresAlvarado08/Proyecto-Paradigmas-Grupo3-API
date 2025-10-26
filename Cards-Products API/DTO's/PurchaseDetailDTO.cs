namespace Cards_Products_API.DTO_s
{
    public class PurchaseDetailDTO
    {
        public int Purchase_Detail_Id { get; set; }
        public int Purchase_Id { get; set; }
        public int Product_Id { get; set; }
        public int Quantity { get; set; }
        public int SubTotal { get; set; }
    }
}
