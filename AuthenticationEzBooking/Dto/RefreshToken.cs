namespace AuthenticationEzBooking.Dto
{
    public class RefreshToken
    {
        public string? token { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }
    }
}
