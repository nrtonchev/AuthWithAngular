namespace Core.Models
{
	public class AppSettings
	{
        public string Secret { get; set; }
        public int RefreshTokenExpiry { get; set; }
        public int TokenExpiry { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
