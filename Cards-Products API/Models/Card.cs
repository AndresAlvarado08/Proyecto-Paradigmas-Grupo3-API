using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Cards_Products_API.Models
{
    public class Card
    {
        [Key]
        public int Card_Id { get; set; }

        public int User_Id { get; set; }

        [JsonIgnore]
        [ForeignKey("User_Id")]
        public User User { get; set; }

        public string Card_Type { get; set; }
        public string Card_Number { get; set; }
        public int Money { get; set; }
        public DateOnly Expiration_Date { get; set; }
    }
}
