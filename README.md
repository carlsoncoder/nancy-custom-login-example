nancy-custom-login-example
==========================

This is a simple example of how I implemented custom authentication in Nancy using method attributes.

As it is meant to be an example or learning tool, I did cut some corners.  There are no unit tests, interfaces are not used where they should be, things are hard-coded, etc.  When I did this in my *real* project, I had all those things : )   So don't be too hard on me for not having them in this sample application.

The jist of the sample is, you simply mark a method in your Module with the path it's requesting, and whether or not it requires an admin login:

        [NancyLoginAttribute("/pictures", false)]
        private dynamic Pictures(dynamic parameters)
        {
            this.ViewBag.PageTitle = "Pictures";
            return this.View["Pictures"];
        }
        
We then hook into the BeforeRequest pipeline in Nancy to re-route the user to the login page if required.  ASP.NET Forms authentication cookie logic is used to encrypt and store the cookie.

This is a work in progress - one thing it does NOT do that it really should, is path matching based on "closest match", instead of exact match.  For example, if you have a route like: "/products/{productId:int}", where the productID could be any value, this solution currently won't work, since we need an exact path (String.Equals(...), instead of String.StartsWith(...))

I've written a post on my blog that walks through some of the details on how to use this, but overall the solution is under 800 lines of code and it should be pretty easy to read through and figure out what's going on.

Check out [the blog post](http://www.carlsoncoder.com/blog/csharp/2014/11/06/Custom-login-authentication-with-Nancy-and-csharp "Carlson Coder Blog") here.

Feel free to reach out to me at Twitter via [@carlsoncoder](https://twitter.com/carlsoncoder "@carlsoncoder") with any questions!

Thanks,
Justin

License
----

This code sample is considered [Beerware](http://en.wikipedia.org/wiki/Beerware "Beerware") and licensed under the [IDGAF](http://dev.bukkit.org/licenses/2977-idgaf-v1-0-license/ "IDGAF") license.