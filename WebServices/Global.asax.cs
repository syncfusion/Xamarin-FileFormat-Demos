using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.Routing;
using WebServices;
using Microsoft.AspNet.SignalR;
using System.Web.Http.Dispatcher;
using System.Net.Http;
using System.Web.Http.Routing;

namespace WebServices
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {

            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector),
            new ApiControllerSelector(GlobalConfiguration.Configuration));
			
            System.Web.Http.GlobalConfiguration.Configuration.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{action}/{id}", defaults: new { id = RouteParameter.Optional });

            RouteTable.Routes.MapHubs(new HubConfiguration { EnableCrossDomain = true });
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "POST, PUT, DELETE, GET");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "ejAuthenticationToken, Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers");
                HttpContext.Current.Response.End();
            }
        }
    }
    public class ApiControllerSelector : DefaultHttpControllerSelector
    {
        public ApiControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
        }

        public override string GetControllerName(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            IHttpRouteData routeData = request.GetRouteData();

            if (routeData == null)
                return null;

            object controllerName;
            routeData.Values.TryGetValue("controller", out controllerName);

            return (string)controllerName;
        }
    }
}