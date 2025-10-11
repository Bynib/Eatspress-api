namespace Eatspress.ServiceModels
{
    public class RegistrationRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone_No { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
