using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
	public class RegisterRequest
	{
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
		public string Password { get; set; }
		[Required]
		[Compare("Password")]
		public string ConfirmPassword { get; set; }
	}
}
