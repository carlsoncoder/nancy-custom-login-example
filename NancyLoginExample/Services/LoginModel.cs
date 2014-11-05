namespace NancyLoginExample.Services
{
    using System;

    /// <summary>
    /// Simple model class to log in.
    /// </summary>
    public class LoginModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginModel"/> class.
        /// </summary>
        public LoginModel()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the user name of the model.
        /// </summary>
        /// <value>The user name of the model.</value>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password of the model.
        /// </summary>
        /// <value>The password of the model.</value>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the role of the model.
        /// </summary>
        /// <value>The role of the model.</value>
        public string UserRole
        {
            get;
            set;
        }

        #endregion
    }
}