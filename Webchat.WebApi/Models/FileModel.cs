namespace Webchat.WebApi.Models
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Webchat.Models;

    public class FileModel
    {
        public FileModel()
        {
        }

        public FileModel(int id, string title, string url)
        {
            this.Id = id;
            this.Title = title;
            this.Url = url;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public int SenderId { get; set; }

        public UserModel Sender { get; set; }

        public static Expression<Func<Image, FileModel>> FromFile
        {
            get
            {
                return x => new FileModel { Id = x.Id, Title = x.Title, Url = x.Url, Sender = UserModel.CreateModel(x.User), SenderId = x.UserId };
            }
        }

        public Image ToFile()
        {
            Image user = new Image()
            {
                Id = this.Id,
                Title = this.Title,
                Url = this.Url,
                User = this.Sender.ToUser(),
                UserId = Sender.Id,
            };

            return user;
        }

        public static FileModel CreateModel(Image file)
        {
            FileModel model = new FileModel(file.Id, file.Title, file.Url);
            model.Sender = UserModel.CreateModel(file.User);
            model.SenderId = file.UserId;

            return model;
        }
    }
}