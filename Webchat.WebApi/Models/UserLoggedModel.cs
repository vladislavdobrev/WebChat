using System;
using System.Linq;
using System.Runtime.Serialization;
using Webchat.Models;

namespace Webchat.WebApi.Models
{
    [DataContract]
    public class UserLoggedModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }

        [DataMember(Name = "sessionKey")]
        public string SessionKey { get; set; }

        [DataMember(Name = "imageUrl")]
        public string ImageUrl { get; set; }

        public static UserLoggedModel CreateModel(User user)
        {
            UserLoggedModel model = new UserLoggedModel()
            {
                Id = user.Id,
                Nickname = user.Nickname,
                SessionKey = user.SessionKey,
                ImageUrl = user.ImageUrl
            };

            return model;
        }
    }
}