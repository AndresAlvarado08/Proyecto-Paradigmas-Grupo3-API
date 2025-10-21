using System.Data.SqlTypes;

namespace Cards_Products_API.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Card_Type { get; set; } = string.Empty;
        public string Card_Number { get; set; } = string.Empty;
        public int Money { get; set; }
        public DateTime Expiration_Date { get; set; }
    }
}
