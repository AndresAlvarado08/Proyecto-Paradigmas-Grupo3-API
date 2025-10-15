namespace Cards_Products_API.Models
{
    public class Product
    {
        public int Id { get; set; } // Clave primaria
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
