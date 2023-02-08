namespace BusinessLayer.DTOs
{
    public class UserLoginInfoDTO
    {
        public UserLoginInfoDTO(bool isLogon)
        {
            IsLogon = isLogon;
        }
        public string? UserEmail { get; set; } = string.Empty;
        public bool IsLogon { get; set; }


    }
}
