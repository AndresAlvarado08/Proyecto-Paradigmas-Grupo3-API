using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Cards_Products_API.Models
{
    public class Purchase
    {
        [Key, Required]
        public int Purchase_Id { get; set; } //Llave Primaria

        //Navigation Properties
        [ForeignKey(nameof(Card_Id))]
        public int Card_Id { get; set; } //Llave Foranea
        public Card? Card { get; set; }
        //[ForeignKey(nameof(User_Id))]    //PONER RELACION EN FUTURO
        public int User_Id { get; set; } //Llave Foranea
        //public User? User { get; set; }   //PONER RELACION EN EL FUTURO
        public ICollection<PurchaseDetail>? PurchaseDetails { get; set; }

        //Purchase Details
        [Required]
        public int SubTotal { get; set; }
        [Required]
        public DateTime Purchase_Date { get; set; }
    }
}
