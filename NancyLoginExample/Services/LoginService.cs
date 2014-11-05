namespace NancyLoginExample.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class LoginService
    {
        #region Fields

        private Dictionary<string, Tuple<string, string>> sampleData;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginService"/> class.
        /// </summary>
        public LoginService()
        {
            this.sampleData = new Dictionary<string, Tuple<string, string>>();
            this.sampleData.Add("justin", new Tuple<string, string>("password", "admin"));
            this.sampleData.Add("jenny", new Tuple<string, string>("password1", "user"));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Attempts to log in.
        /// </summary>
        /// <param name="model">The <see cref="LoginModel"/> to attempt to log in with.</param>
        /// <returns>Whether or not the login was successful.</returns>
        public bool Login(LoginModel model)
        {
            Tuple<string, string> loginData;
            if (!this.sampleData.TryGetValue(model.UserName, out loginData))
            {
                // user does not exist
                return false;
            }

            // validate password
            if (!String.Equals(model.Password, loginData.Item1))
            {
                // invalid password
                return false;
            }

            // assign user property
            model.UserRole = loginData.Item2;
            return true;
        }

        #endregion
    }
}