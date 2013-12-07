namespace Webchat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        private int id;
        private string nickname;
        private string password;
        private string imageUrl;
        private string email;
        private ICollection<User> contacts;
        private ICollection<User> users;
        private string sessionKey;

        public User()
        {
            this.contacts = new HashSet<User>();
        }

        public User(int id, string nickname, string password, string imageUrl, string email)
        {
            this.Id = id;
            this.Nickname = nickname;
            this.Password = password;
            this.ImageUrl = imageUrl;
            this.Email = email;

            this.contacts = new HashSet<User>();
        }

        public int Id
        {
            get
            {
                return this.id;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Invalid id! It must be a positive integer value!");
                }

                this.id = value;
            }
        }

        [MinLength(5, ErrorMessage = "Invalid nickname! It must contain more than 5 characters!")]
        public string Nickname
        {
            get
            {
                return this.nickname;
            }

            set
            {
                ValidateStringField(value, "nickname");

                this.nickname = value;
            }
        }

        [MinLength(8, ErrorMessage = "Invalid password! It must contain more than 8 characters!")]
        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                ValidateStringField(value, "password");

                this.password = value;
            }
        }

        public string ImageUrl
        {
            get
            {
                return this.imageUrl;
            }

            set
            {
                this.imageUrl = value;
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }

            set
            {
                ValidateStringField(value, "email");

                this.email = value;
            }
        }

        public string SessionKey
        {
            get
            {
                return this.sessionKey;
            }

            set
            {
                this.sessionKey = value;
            }
        }

        public virtual ICollection<User> Contacts
        {
            get
            {
                return this.contacts;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Invalid contacts collection! It cannot be null!");
                }

                this.contacts = value;
            }
        }

        public virtual ICollection<User> Users
        {
            get
            {
                return this.users;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Invalid contacts collection! It cannot be null!");
                }

                this.users = value;
            }
        }

        private void ValidateStringField(string value, string field)
        {
            if (string.IsNullOrWhiteSpace(value) || value == string.Empty)
            {
                string message = string.Format("Invalid {0}! It must be an existing non-empty string that contains not only white spaces!",
                    field);
                throw new ArgumentException(message, field);
            }
        }
    }
}
