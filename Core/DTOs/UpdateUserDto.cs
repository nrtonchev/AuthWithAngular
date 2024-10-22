using Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
	public class UpdateUserDto
	{
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		[Compare("Password")]
		public string ConfirmPassword { get; set; }
		[EnumDataType(typeof(Role))]
		public Role Role { get; set; }
	}
}
