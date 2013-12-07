namespace Webchat.WebApi.Models
{
    using System;
    using System.Linq;

    public class UserLoginModel
    {
        public string Nickname { get; set; }

        public string Password { get; set; }

    }
}