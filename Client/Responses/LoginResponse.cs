namespace Client.Responses
{
    // C# 7.3 does not support records or nullable reference types.
    // Use a standard class with properties and a constructor.
    public class LoginResponse
    {
        public bool Flag { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }

        public LoginResponse(bool flag, string message, int userId)
        {
            Flag = flag;
            Message = message;
            UserId = userId;
        }
    }
}
