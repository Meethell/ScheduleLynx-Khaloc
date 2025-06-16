namespace Client.Responses
{
    public class GeneralResponse
    {
        public bool Flag { get; set; }
        public string Message { get; set; }

        public GeneralResponse(bool flag, string message)
        {
            Flag = flag;
            Message = message;
        }
    }
}
