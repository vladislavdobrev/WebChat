

namespace Webchat.WebApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Webchat.Models;

    public class UserDetails : UserModel
    {

        public UserDetails()
        {
            this.Contacts = new HashSet<UserModel>();
        }

        public UserDetails(int id, string nickname, string imageUrl, string email)
        {
            this.Id = id;
            this.Nickname = nickname;
            this.ImageUrl = imageUrl;
            this.Email = email;
        }

        public ICollection<UserModel> Contacts { get; set; }

        public static Expression<Func<User, UserDetails>> FromUser
        {
            get
            {
                return x => new UserDetails
                {
                    Id = x.Id,
                    Nickname = x.Nickname,
                    Email = x.Email,
                    ImageUrl = x.ImageUrl,
                    Contacts = x.Contacts.AsQueryable<User>().Select(UserModel.FromUser).ToList()
                };
            }
        }

        public static UserDetails CreateModel(User user)
        {
            UserDetails model = new UserDetails(user.Id, user.Nickname, user.ImageUrl, user.Email);

            model.Contacts = user.Contacts.AsQueryable().Select(UserModel.FromUser).ToList();

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

            foreach (var userModel in this.Contacts)
            {
                user.Contacts.Add(userModel.ToUser());
            }

            return user;
        }
    }
}