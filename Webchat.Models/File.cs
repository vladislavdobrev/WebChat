namespace Webchat.Models
{
    using System;

    public class Image
    {
        private int id;
        private string title;
        private int userId;
        private User user;
        private string url;

        public int Id
        {
            get
            {
                return this.id;
            }

            set
            {
                ValidateIndex(value, "id");

                this.id = value;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                ValidateStringField(value, "title");

                this.title = value;
            }
        }

        public int UserId
        {
            get
            {
                return this.userId;
            }

            set
            {
                ValidateIndex(value, "sender id");

                this.userId = value;
            }
        }

        public virtual User User
        {
            get
            {
                return this.user;
            }

            set
            {
                ValidateUser(value, "sender");

                this.user = value;
            }
        }

        public string Url
        {
            get
            {
                return this.url;
            }

            set
            {
                ValidateStringField(value, "url");

                this.url = value;
            }
        }
        private void ValidateIndex(int value, string field)
        {
            if (value < 0)
            {
                string message = string.Format("Invalid {0}! It must be a positive integer number!", field);
                throw new ArgumentOutOfRangeException(field, value, message);
            }
        }

        private void ValidateUser(User user, string field)
        {
            if (user == null)
            {
                string message = string.Format("Invalid {0}! It cannot be null!", field);
                throw new ArgumentNullException(field, message);
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
