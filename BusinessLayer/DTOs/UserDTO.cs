namespace BusinessLayer.DTOs
{
    public class UserDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int Year { get; set; }
    }
}
