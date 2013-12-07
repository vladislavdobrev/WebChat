namespace Webchat.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Webchat.Models;

    public class DbImagesRepository : IRepository<Image>
    {
        public DbImagesRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("Invalid database context! It cannot be null!");
            }

            this.Context = context;
            this.DbSet = this.Context.Set<Image>();
        }

        protected DbContext Context { get; set; }
        protected DbSet<Image> DbSet { get; set; }

        public IQueryable<Image> All()
        {
            var allFiles = this.DbSet;

            return allFiles;
        }

        public Image Get(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("Id must be positive integer number!");
            }

            var file = this.DbSet.Where(x => x.Id == id).FirstOrDefault();

            return file;
        }

        public Image Get(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == string.Empty)
            {
                throw new ArgumentOutOfRangeException("Image title must be non-null, not empty or containing only white spaces!");
            }

            var file = this.DbSet.Where(x => x.Title == value).FirstOrDefault();

            return file;
        }

        public Image Add(Image file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("Invalid file! It cannot be null!");
            }

            var dbSender = this.Context.Set<User>().Where(x => x.Nickname == file.User.Nickname).FirstOrDefault();
            file.User = dbSender;

            this.DbSet.Add(file);
            this.Context.SaveChanges();

            return file;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, Image item)
        {
            throw new NotImplementedException();
        }


        
    }
}
