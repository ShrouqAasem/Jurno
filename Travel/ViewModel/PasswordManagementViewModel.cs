namespace Travel.ViewModel
{
    public class PasswordManagementViewModel
    {
        public string Email { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public bool IsReset { get; set; } // True if resetting password, false if forgetting password
    }

}
