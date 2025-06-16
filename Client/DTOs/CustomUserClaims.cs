namespace Client.DTOs
{
    public class CustomUserClaims
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public CustomUserClaims(string id, string email, string role)
        {
            Id = id;
            Email = email;
            Role = role;
        }
    }
}
