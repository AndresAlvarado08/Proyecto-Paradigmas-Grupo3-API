using System.ComponentModel.DataAnnotations;

namespace Cards_Products_API.Models
{
    public class User
    {
        [Key, Required]
        public int User_Id { get; set; }
        public string Username { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
    }
}
