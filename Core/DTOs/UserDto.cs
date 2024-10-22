﻿using Core.Models;

namespace Core.DTOs
{
	public class UserDto
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public Role Role { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateUpdated { get; set; }
	}
}
