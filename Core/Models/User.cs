using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
		[Required]
		[MaxLength(100)]
		public string LastName { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
		public List<RefreshToken> RefreshTokens { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
