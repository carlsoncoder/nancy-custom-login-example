namespace NancyLoginExample.Modules
{
    using Nancy;
    using Nancy.Extensions;
    using NancyLoginExample.Services;
    using System;

    /// <summary>
    /// Nancy module used to handle requests to /.
    /// </summary>
    public class HomeModule : NancyModule
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeModule"/> class.
        /// </summary>
        /// <remarks>Handles requests to / relative to the root path.</remarks>
        public HomeModule()
        {
            this.Get["/"] = this.Index;
            this.Get["/about"] = this.About;
            this.Get["/pictures"] = this.Pictures;
            this.Get["/products/listing/featured"] = this.ProductListing;

            this.Get["/login/{redirecturl?}/{alreadyAttempted?}"] = this.Login;
            this.Post["/trylogin"] = this.TryLogin;
            this.Get["/logout"] = this.Logout;

            this.CookieService = new CookieService();
            this.LoginService = new LoginService();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="LoginService"/> used to handle authentication.
        /// </summary>
        /// <value>The <see cref="LoginService"/> used to handle authentication.</value>
        public LoginService LoginService
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="CookieService"/> used to handle cookies.
        /// </summary>
        /// <value>The <see cref="CookieService"/> used to handle cookies.</value>
        public CookieService CookieService
        {
            get;
            private set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles GET requests to the main home page.
        /// </summary>
        /// <param name="parameters">The dynamic page parameters.</param>
        /// <returns>The Home view.</returns>
        /// <remarks>
        /// GET /
        /// </remarks>
        private dynamic Index(dynamic parameters)
        {
            this.ViewBag.PageTitle = "Home";
            return this.View["Home"];
        }

        /// <summary>
        /// Handles GET requests to the about page.
        /// </summary>
        /// <param name="parameters">The dynamic page parameters.</param>
        /// <returns>The About view.</returns>
        /// <remarks>
        /// GET /about
        /// </remarks>        
        private dynamic About(dynamic parameters)
        {
            this.ViewBag.PageTitle = "About";
            return this.View["About"];
        }

        /// <summary>
        /// Handles GET requests to the pictures page.
        /// </summary>
        /// <param name="parameters">The dynamic page parameters.</param>
        /// <returns>The Pictures view.</returns>
        /// <remarks>
        /// GET /pictures
        /// </remarks>
        [NancyLoginAttribute("/pictures", false)]
        private dynamic Pictures(dynamic parameters)
        {
            this.ViewBag.PageTitle = "Pictures";
            return this.View["Pictures"];
        }

        /// <summary>
        /// Handles GET requests to the product listing page.
        /// </summary>
        /// <param name="parameters">The dynamic page parameters.</param>
        /// <returns>The Product Listing view.</returns>
        /// <remarks>
        /// GET /products/listing/featured
        /// </remarks>
        [NancyLoginAttribute("/products/listing/featured", true)]
        private dynamic ProductListing(dynamic parameters)
        {
            this.ViewBag.PageTitle = "Product Listing";
            return this.View["ProductListing"];
        }

        /// <summary>
        /// Handles GET requests to the Login page.
        /// </summary>
        /// <param name="parameters">The dynamic page parameters.</param>
        /// <returns>The login view.</returns>
        /// <remarks>
        /// GET /login
        /// </remarks>
        private dynamic Login(dynamic parameters)
        {
            this.ViewBag.PageTitle = "Login";
            this.ViewBag.IsInitialLogin = "true";

            if (parameters.alreadyAttempted != null)
            {
                this.ViewBag.IsInitialLogin = "false";
            }

            if (parameters.redirectUrl != null)
            {
                this.ViewBag.RedirectUrlFromLogin = parameters.redirectUrl.ToString();
            }

            return this.View["Login"];
        }

        /// <summary>
        /// Handles POST requests to the Login page.
        /// </summary>
        /// <param name="parameters">The dynamic page parameters.</param>
        /// <returns>The login view.</returns>
        /// <remarks>
        /// POST /trylogin
        /// </remarks>
        private dynamic TryLogin(dynamic parameters)
        {
            LoginModel model = new LoginModel()
            {
                UserName = this.Request.Form["UserName"].ToString(),
                Password = this.Request.Form["Password"].ToString()
            };

            string redirectUrl = this.Request.Form["RedirectUrlField"].ToString();

            if (this.LoginService.Login(model))
            {
                var encryptedTicket = this.CookieService.GenerateEncryptedTicket(model);
                this.Request.Session[Constants.LoginCookieSessionKey] = encryptedTicket;

                if (!String.IsNullOrEmpty(redirectUrl))
                {
                    string formattedRedirect = redirectUrl.Replace('|', '/');
                    return this.Context.GetRedirect(formattedRedirect);
                }
                else
                {
                    return this.Index(parameters);
                }
            }
            else
            {
                return this.Context.GetRedirect(String.Format("/login/{0}/true", redirectUrl));
            }
        }

        /// <summary>
        /// Handles GET requests to the LogOut page.
        /// </summary>
        /// <param name="parameters">The dynamic page parameters.</param>
        /// <returns>The home view, after the user has been logged out.</returns>
        /// <remarks>
        /// GET /logout
        /// </remarks>
        private dynamic Logout(dynamic parameters)
        {
            this.Request.Session[Constants.LoginCookieSessionKey] = String.Empty;
            return this.Index(parameters);
        }

        #endregion
    }
}