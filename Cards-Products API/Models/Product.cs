using System.ComponentModel.DataAnnotations;

namespace Cards_Products_API.Models
{
    public class Product
    {
        [Key, Required]
        public int Product_Id { get; set; } //Llave Primaria
        [Required]
        public string Product_Name { get; set; } = string.Empty;
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Price { get; set; }
    }
}
