using System.ComponentModel.DataAnnotations;

namespace TMP_API.Models.Users
{
    public class UserRegisterDTO
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        public String? Phone { get; set; }
        public String? Address { get; set; }

        public String UserType { get; set; }
    }
}
