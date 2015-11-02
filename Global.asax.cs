using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using SisPark.Models;
using SisPark.Controllers;


namespace SisPark
{
    // Nota: para obtener instrucciones sobre cómo habilitar el modo clásico de IIS6 o IIS7, 
    // visite http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                try
                {
                    //lets take out the username now
                    string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                    string roles = string.Empty;

                    using (SisparkModel db = new SisparkModel())
                    {
                        User user =  db.User.SingleOrDefault(u => u.Usuario == username);

                        roles = user.Usertype.description;
                    }

                    //let us extract the roles from our own custom cookie

                    //Let us set the Principal with our user specific details

                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                        new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(','));
                }
                catch (Exception)
                {
                    //something went wrong
                }
            }
        }

    }
}