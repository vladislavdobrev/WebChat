namespace Webchat.Repositories
{
    using System.Linq;
using Webchat.Models;

    public interface IUserRepository : IRepository<User>
    {
        User LoginUser(User user);

        void LogoutUser(string sessionkey);

        void AddContact(string sessionKey, string nickname);

        IQueryable<User> GetContacts(string sessionKey);

        string GenerateSessionKey(int id);
    }
}
