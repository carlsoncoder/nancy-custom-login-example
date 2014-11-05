namespace NancyLoginExample.Services
{
    using Nancy;
    using Nancy.Session;
    using System;
    using System.Web.Security;

    public partial class CookieService
    {
        #region Fields

        /// <summary>
        /// String used to identify the cookie that holds the authentication ticket.
        /// </summary>
        private const string CookieName = "YourAssemblyNameHere.CookieIdentifier";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieService"/>.
        /// </summary>
        public CookieService()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generates an encrypted forms authentication ticket.
        /// </summary>
        /// <param name="model">The <see cref="LoginModel"/> used to generate the ticket.</param>
        /// <returns>An encrypted string representing the valid ticket.</returns>
        public string GenerateEncryptedTicket(LoginModel model)
        {
            DateTime now = DateTime.UtcNow;
            var ticket = new FormsAuthenticationTicket(
                1,
                CookieService.CookieName,
                now,
                now.AddDays(7),
                true,
                String.Format("{0}|{1}", model.UserName, model.UserRole));

            return FormsAuthentication.Encrypt(ticket);
        }

        /// <summary>
        /// Validates a login.
        /// </summary>
        /// <param name="session">The <see cref="ISession"/> Nancy web session.</param>
        /// <returns>The <see cref="CookieModel"/>, if it was found.</returns>
        public CookieModel ValidateLogin(ISession session)
        {
            CookieModel model = new CookieModel();
            if (session != null)
            {
                object cookie = session[Constants.LoginCookieSessionKey];
                if (cookie != null)
                {
                    var encryptedTicket = cookie.ToString();

                    if (!String.IsNullOrEmpty(encryptedTicket))
                    {
                        var ticket = FormsAuthentication.Decrypt(encryptedTicket);

                        if (String.Equals(ticket.Name, CookieService.CookieName))
                        {
                            string[] userData = ticket.UserData.Split('|');
                            model.UserName = userData[0];
                            model.UserRole = userData[1];
                            model.ExpirationDate = ticket.Expiration;
                        }
                    }
                }
            }

            return model;
        }

        #endregion
    }
}