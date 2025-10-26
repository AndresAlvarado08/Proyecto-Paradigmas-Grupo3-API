using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cards_Products_API.Models
{
    public class PurchaseDetail
    {
        [Key]
        public int PurchaseDetail_Id { get; set; }
        [Required]
        public int Purchase_Id { get; set; }
        [Required]
        public int Product_Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int SubTotal { get; set; }

        [ForeignKey(nameof(Purchase_Id))]
        public Purchase? Purchase { get; set; }

        [ForeignKey(nameof(Product_Id))]
        public Product? Product { get; set; }
    }
}
