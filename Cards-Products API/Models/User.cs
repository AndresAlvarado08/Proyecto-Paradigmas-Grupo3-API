using System.ComponentModel.DataAnnotations;

namespace Cards_Products_API.Models
{
    public class User
    {
        [Key]
        public int User_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
    }
}
