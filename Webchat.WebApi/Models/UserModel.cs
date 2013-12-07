namespace Webchat.WebApi.Models
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;
    using Webchat.Models;

    [DataContract]
    public class UserModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "imageUrl")]
        public string ImageUrl { get; set; }

        public static Expression<Func<User, UserModel>> FromUser
        {
            get
            {
                return x => new UserModel { Id = x.Id, Nickname = x.Nickname, Email = x.Email, ImageUrl = x.ImageUrl };
            }
        }

        public static UserModel CreateModel(User user)
        {
            UserModel model = new UserModel()
            {
                Id = user.Id,
                Nickname = user.Nickname,
                Email = user.Email,
                ImageUrl = user.ImageUrl
            };

            return model;
        }

        public User ToUser()
        {
            User user = new User()
            {
                Id = this.Id,
                Nickname = this.Nickname,
                Email = this.Email,
                ImageUrl = this.ImageUrl
            };

            return user;
        }
    }
}