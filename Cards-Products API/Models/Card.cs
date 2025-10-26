using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Cards_Products_API.Models
{
    public class Card
    {
        [Key, Required]
        public int Card_Id { get; set; } //Llave Primaria
        [AllowNull]
        public int User_Id { get; set; } //Llave Foranea
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
