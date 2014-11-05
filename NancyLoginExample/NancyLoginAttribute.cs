namespace NancyLoginExample
{
    using System;

    /// <summary>
    /// Attribute decorator class to require a user to login when accessing certain paths.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class NancyLoginAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NancyLoginAttribute"/> class.
        /// </summary>
        /// <param name="requestedPath">The requested path.</param>
        /// <param name="requiresAdmin">Whether or not you must be an admin to access this path.</param>
        public NancyLoginAttribute(string requestedPath, bool requiresAdmin)
        {
            this.RequestedPath = requestedPath;
            this.RequiresAdmin = requiresAdmin;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the requested path of the request.
        /// </summary>
        /// <value>The requested path of the request.</value>
        public string RequestedPath
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets whether or not you must be an admin to access this path.
        /// </summary>
        /// <value>Whether or not you must be an admin to access this path.</value>
        public bool RequiresAdmin
        {
            get;
            private set;
        }

        #endregion
    }
}