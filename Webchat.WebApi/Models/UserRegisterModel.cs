namespace Webchat.WebApi.Models
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using Webchat.Models;


    [DataContract(Name="userRegisterModel")]
    public class UserRegisterModel
    {
        [DataMember(Name="nickname")]
        public string Nickname { get; set; }

        [DataMember(Name = "password")]
        public string Password { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "imageUrl")]
        public string ImageUrl { get; set; }

        public User ToUser()
        {
            User user = new User(0, this.Nickname, this.Password, this.ImageUrl, this.Email);

            return user;
        }
    }
}