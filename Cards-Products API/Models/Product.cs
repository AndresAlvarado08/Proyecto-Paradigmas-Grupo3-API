namespace Cards_Products_API.Models
{
    public class Product
    {
        public int Id { get; set; } // Clave primaria
        public string Product_Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
