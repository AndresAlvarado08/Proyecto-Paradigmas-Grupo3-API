using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Cards_Products_API.Models
{
    public class Purchase
    {
        [Key, Required]
        public int Purchase_Id { get; set; } //Llave Primaria
        [AllowNull]
        public int User_Id { get; set; } //Llave Foranea
        public int Card_Id { get; set; } //Llave Foranea

        //Navigation Properties
        [ForeignKey(nameof(Card_Id))]
        public Card? Card { get; set; }
        public ICollection<PurchaseDetail>? PurchaseDetails { get; set; }

        //Purchase Details
        [Required]
        public int Total { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
    }
}
