namespace Webchat.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Webchat.Models;

    public class DbUsersRepository : IUserRepository
    {
        private const string SessionKeyChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int SessionKeyLen = 50;
        protected static Random rand = new Random();

        public DbUsersRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("Invalid database context! It cannot be null!");
            }

            this.Context = context;
            this.DbSet = this.Context.Set<User>();
        }

        protected DbContext Context { get; set; }
        protected DbSet<User> DbSet { get; set; }

        public IQueryable<User> All()
        {
            var allUsers = this.DbSet;

            return allUsers;
        }

        public User Get(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("Id must be positive integer number!");
            }

            var user = this.DbSet.Where(x => x.Id == id).FirstOrDefault();

            return user;
        }

        public User Get(string nickname)
        {
            if (string.IsNullOrWhiteSpace(nickname) || nickname == string.Empty)
            {
                throw new ArgumentOutOfRangeException("Nickname must be non-null, not empty or containing only white spaces!");
            }

            nickname = nickname.ToLower();
            var user = this.DbSet.Where(x => x.Nickname == nickname).FirstOrDefault();

            return user;
        }

        public User Add(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("Invalid user! It cannot be null!");
            }

            user.Nickname = user.Nickname.ToLower();

            if (this.Get(user.Nickname) != null)
            {
                throw new InvalidOperationException(string.Format("User with nickname {0} already exists!", user.Nickname));
            }

            this.DbSet.Add(user);

            try
            {
                this.Context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            this.LoginUser(user);

            return user;
        }

        public User LoginUser(User user)
        {
            var dbUser = this.Get(user.Nickname);
            string sessionKey = this.GenerateSessionKey(dbUser.Id);
            dbUser.SessionKey = sessionKey;
            this.Context.SaveChanges();

            return dbUser;
        }

        public void LogoutUser(string sessionKey)
        {
            var dbUser = this.DbSet.Where(x => x.SessionKey == sessionKey).FirstOrDefault();
            dbUser.SessionKey = null;

            this.Context.SaveChanges();
        }

        public void AddContact(string sessionKey, string nickname)
        {
            var hostUser = this.DbSet.Where(x => x.SessionKey == sessionKey).FirstOrDefault();

            var contact = this.DbSet.Where(x => x.Nickname == nickname).FirstOrDefault();

            if (contact == null)
            {
                throw new InvalidOperationException(string.Format("User with nickname {0} does not exist!", nickname));
            }

            hostUser.Contacts.Add(contact);
            contact.Contacts.Add(hostUser);
            this.Context.SaveChanges();
        }

        public IQueryable<User> GetContacts(string sessionKey)
        {
            var userContacts = this.DbSet.Where(x => x.SessionKey == sessionKey).FirstOrDefault().Contacts.AsQueryable();

            return userContacts;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, User item)
        {
            throw new NotImplementedException();
        }

        public string GenerateSessionKey(int userId)
        {
            StringBuilder keyChars = new StringBuilder(50);
            keyChars.Append(userId.ToString());
            while (keyChars.Length < SessionKeyLen)
            {
                int randomCharNum;
                lock (rand)
                {
                    randomCharNum = rand.Next(SessionKeyChars.Length);
                }
                char randomKeyChar = SessionKeyChars[randomCharNum];
                keyChars.Append(randomKeyChar);
            }
            string sessionKey = keyChars.ToString();
            return sessionKey;
        }
    }
}
