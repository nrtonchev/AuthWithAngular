using Core.Models;
using System.Text.Json.Serialization;

namespace Core.DTOs
{
	public class AuthResponse
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public Role Role { get; set; }
        public string JwtToken { get; set; }
        [JsonIgnore]
		public string RefreshToken { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateUpdated { get; set; }
	}
}
