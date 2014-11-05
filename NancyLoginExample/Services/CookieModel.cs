namespace NancyLoginExample.Services
{
    using System;

    /// <summary>
    /// Simple model class to store values from a cookie.
    /// </summary>
    public class CookieModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieModel"/> class.
        /// </summary>
        public CookieModel()
        {
            this.ExpirationDate = DateTime.UtcNow.AddSeconds(-1);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the user name value.
        /// </summary>
        /// <value>The user name value.</value>
        public string UserName
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

        /// <summary>
        /// Gets or sets the expiration date value.
        /// </summary>
        /// <value>The expiration date value.</value>
        public DateTime ExpirationDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this cookie is valid.
        /// </summary>
        /// <value>Whether this cookie is valid.</value>
        public bool IsValid
        {
            get
            {
                return !String.IsNullOrEmpty(this.UserName) && DateTime.UtcNow < this.ExpirationDate;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this is an administrative user.
        /// </summary>
        /// <value>Whether this user is an administrative user.</value>
        public bool IsAdmin
        {
            get
            {
                return String.Equals("admin", this.UserRole, StringComparison.InvariantCultureIgnoreCase) && this.IsValid;
            }
        }        

        #endregion
    }
}