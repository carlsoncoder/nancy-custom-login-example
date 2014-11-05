namespace NancyLoginExample
{
    using Nancy;
    using Nancy.Bootstrapper;
    using Nancy.Conventions;
    using Nancy.Extensions;
    using Nancy.Session;
    using Nancy.TinyIoc;
    using NancyLoginExample.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Bootstrapper to load Nancy configuration on startup.
    /// </summary>
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {

        #region Properties

        /// <summary>
        /// Gets the dictionary holding the list of paths that require login.
        /// </summary>
        /// <value>The dictionary holding the list of paths that require login.</value>
        public Dictionary<string, bool> PathsRequiringLogin
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
        /// Handles the Nancy application startup logic.
        /// </summary>
        /// <param name="container">The <see cref="TinyIoCContainer"/> Nancy DI container - not used in this implementation.</param>
        /// <param name="pipelines">The application pipelines.</param>
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            this.CookieService = new CookieService();
            this.LoadLoginAttributes();

            CookieBasedSessions.Enable(pipelines);

            pipelines.BeforeRequest += (context) =>
            {
                string requestedPath = context.Request.Url.Path;
                if (this.PathsRequiringLogin.ContainsKey(requestedPath))
                {
                    bool requiresAdmin = this.PathsRequiringLogin[requestedPath];
                    CookieModel cookieModel = this.CookieService.ValidateLogin(context.Request.Session);

                    bool isValidLogin = cookieModel.IsValid;
                    if (requiresAdmin)
                    {
                        isValidLogin = cookieModel.IsAdmin;
                    }

                    if (!isValidLogin)
                    {
                        // Re-route to login page with redirect URL - replace / with | so we don't mess up Nancy routing
                        return context.GetRedirect(String.Format("/login/{0}", requestedPath.Replace('/', '|')));
                    }
                }

                return null;
            };
        }

        /// <summary>
        /// Configures the Nancy conventions.
        /// </summary>
        /// <param name="nancyConventions">The <see cref="NancyConventions"/> to be configured.</param>
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Scripts", @"Scripts"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Stylesheets", @"Stylesheets"));
            base.ConfigureConventions(nancyConventions);
        }

        /// <summary>
        /// Loads the paths that require login by scanning methods for the <see cref="NancyLoginAttribute"/> attribute through reflection.
        /// </summary>
        /// <remarks>Reflection is expensive, but this is only done on application load and then cached for the lifetime of the application.</remarks>
        private void LoadLoginAttributes()
        {
            this.PathsRequiringLogin = new Dictionary<string, bool>();

            // W00T - LINQ Reflection Magic!
            var pathsRequiringLogin =
                (from type in Assembly.GetExecutingAssembly().GetTypes()
                 from method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                 let attributes = method.GetCustomAttributes(typeof(NancyLoginAttribute), false)
                 where attributes != null && attributes.Length > 0
                 let nancyAttributes = (NancyLoginAttribute)attributes[0]
                 select new { Path = nancyAttributes.RequestedPath.ToLower(), RequiresAdmin = nancyAttributes.RequiresAdmin })
                 .ToList();

            foreach (var pathModel in pathsRequiringLogin)
            {
                // need to do this check since GET and POST methods can have the same path
                if (!this.PathsRequiringLogin.ContainsKey(pathModel.Path))
                {
                    this.PathsRequiringLogin.Add(pathModel.Path, pathModel.RequiresAdmin);
                }
            }
        }

        #endregion
    }
}