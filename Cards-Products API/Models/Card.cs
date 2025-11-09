using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Cards_Products_API.Models
{
    public class Card
    {
        [Key, Required]
        public int Card_Id { get; set; } //Llave Primaria
        //[ForeignKey(nameof(User_Id))]    //PONER RELACION EN FUTURO
        public int User_Id { get; set; } //Llave Foranea
        //public User User { get; set; }   //PONER RELACION EN FUTURO
        [Required]
        public string Card_Type { get; set; } = string.Empty;
        [Required]
        public string Card_Number { get; set; } = string.Empty;
        [Required]
        public int Money { get; set; }
        [Required]
        public DateTime Expiration_Date { get; set; }
    }
}
