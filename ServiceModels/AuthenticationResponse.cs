namespace Eatspress.ServiceModels
{
    public class AuthenticationResponse
    {
        public string token { get; set; }
        public UserResponse user { get; set; }
    }
}
