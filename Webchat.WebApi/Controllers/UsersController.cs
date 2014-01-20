using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Webchat.Data;
using Webchat.Models;
using Webchat.Repositories;
using Webchat.WebApi.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Webchat.WebApi.Controllers
{
    [EnableCors(
        origins: "http://vdobrev.com",
        headers: "Accept, Overwrite, Destination, Content-Type, Depth, User-Agent, X-File-Size, X-Requested-With, If-Modified-Since, X-File-Name, Cache-Control",
        methods: "ACCEPT, PROPFIND, PROPPATCH, COPY, MOVE, DELETE, MKCOL, LOCK, UNLOCK, PUT, GETLIB, VERSION-CONTROL, CHECKIN, CHECKOUT, UNCHECKOUT, REPORT, UPDATE, CANCELUPLOAD, HEAD, OPTIONS, GET, POST")]
    [HttpHeader("Access-Control-Allow-Origin", "http://vdobrev.com")]
    [HttpHeader("Access-Control-Allow-Credentials", "true")]
    [HttpHeader("Access-Control-Allow-Methods", "ACCEPT, PROPFIND, PROPPATCH, COPY, MOVE, DELETE, MKCOL, LOCK, UNLOCK, PUT, GETLIB, VERSION-CONTROL, CHECKIN, CHECKOUT, UNCHECKOUT, REPORT, UPDATE, CANCELUPLOAD, HEAD, OPTIONS, GET, POST")]
    [HttpHeader("Access-Control-Allow-Headers", "Accept, Overwrite, Destination, Content-Type, Depth, User-Agent, X-File-Size, X-Requested-With, If-Modified-Since, X-File-Name, Cache-Control")]
    [HttpHeader("Access-Control-Max-Age", "3600")]
    public class UsersController : BaseController
    {
        private static Account m_account;
        private static Cloudinary m_cloudinary;
        private static string m_testImagePath;

        private readonly IUserRepository data;

        public UsersController()
        {
            this.data = new DbUsersRepository(new WebchatContext());
        }

        public UsersController(IUserRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("Invalid repository! It cannot be null!");
            }

            this.data = repository;
        }

        [HttpPost]
        [HttpOptions]
        [ActionName("register")]
        public HttpResponseMessage RegisterUser([FromBody]UserRegisterModel model)
        {
            return base.PerformOperationAndHandleExceptions(() =>
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Invalid user register model!");
                }

                User user = model.ToUser();

                if (!string.IsNullOrWhiteSpace(user.ImageUrl) && user.ImageUrl != string.Empty)
                {
                    string cloudImageUrl = this.UploadImageToCloudinary(user.ImageUrl);
                    user.ImageUrl = cloudImageUrl;
                }
                
                var dbUser = this.data.Add(user);
                var userLoggedModel = UserLoggedModel.CreateModel(dbUser);

                var response = this.Request.CreateResponse<UserLoggedModel>(HttpStatusCode.Created, userLoggedModel);
                var resourceLink = Url.Link("UsersGetApi", new { id = userLoggedModel.Id });
                response.Headers.Location = new Uri(resourceLink);
                return response;
            });
        }

        [HttpPost]
        [HttpOptions]
        [ActionName("login")]
        public HttpResponseMessage LoginUser([FromBody]UserLoginModel model)
        {
            return base.PerformOperationAndHandleExceptions(() =>
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Invalid user login model!");
                }

                User dbUser = this.data.Get(model.Nickname);

                if (dbUser == null || model.Password != dbUser.Password)
                {
                    throw new Exception("Invalid nickname or password!");
                }

                User loggedUser = this.data.LoginUser(dbUser);
                UserLoggedModel loggedModel = UserLoggedModel.CreateModel(loggedUser);

                var response = this.Request.CreateResponse<UserLoggedModel>(HttpStatusCode.OK, loggedModel);
                return response;
            });
        }

        [HttpGet]
        [HttpOptions]
        [ActionName("logout")]
        public HttpResponseMessage LogoutUser(string sessionKey)
        {
            return base.PerformOperationAndHandleExceptions(() =>
            {
                this.data.LogoutUser(sessionKey);

                var response = this.Request.CreateResponse(HttpStatusCode.OK, "User logged out successfully");
                return response;
            });
        }

        [HttpPost]
        [HttpOptions]
        [ActionName("add-contact")]
        public HttpResponseMessage AddContact(string sessionKey, [FromBody] string nickname)
        {
            return base.PerformOperationAndHandleExceptions(() =>
            {
                if (string.IsNullOrWhiteSpace(nickname) || nickname == string.Empty)
                {
                    throw new Exception("Invalid nickname!");
                }

                this.data.AddContact(sessionKey, nickname);

                var response = this.Request.CreateResponse(HttpStatusCode.OK, "Contact added successfully");
                return response;
            });
        }

        [HttpGet]
        [HttpOptions]
        [ActionName("get-contacts")]
        public IQueryable<UserModel> GetContacts(string sessionKey)
        {
            return base.PerformOperationAndHandleExceptions(() =>
            {
                var userContacts = this.data.GetContacts(sessionKey).Select(UserModel.FromUser);

                return userContacts;
            });
        }

        [HttpGet]
        [HttpOptions]
        [ActionName("get")]
        public UserDetails Get(int id)
        {
            return base.PerformOperationAndHandleExceptions(() =>
            {
                var user = this.data.Get(id);
                var userModel = UserDetails.CreateModel(user);

                return userModel;
            });
        }

        [HttpOptions]
        private string UploadImageToCloudinary(string url)
        {
            m_account = new Account("haiubldgg", "235176581338859", "LqSoe-u4wR4g8dFSOQGT3YBrCbM");
            m_cloudinary = new CloudinaryDotNet.Cloudinary(m_account);
            m_testImagePath = url;

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = "remote"
            };

            ImageUploadResult uploadResult = m_cloudinary.Upload(uploadParams);
            return uploadResult.Uri.AbsoluteUri;
        }
    }
}
