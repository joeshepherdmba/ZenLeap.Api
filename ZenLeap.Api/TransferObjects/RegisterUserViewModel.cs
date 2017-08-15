using System;
namespace ZenLeap.Api.ViewModels
{
    public class RegisterUserViewModel
    {
        public RegisterUserViewModel()
        {
        }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
