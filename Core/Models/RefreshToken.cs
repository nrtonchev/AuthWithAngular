using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
	public class RefreshToken
	{
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsExpired => DateTime.UtcNow > ExpiryDate;
    }
}
