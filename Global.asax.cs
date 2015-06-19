using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WitBird.XiaoChangHe
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
        }

        protected void Application_Error()
        {
            var error = Server.GetLastError();
            var code = (error is HttpException) ? (error as HttpException).GetHttpCode() : 500;
            string path = Request.Path;
            #region LogException
            var logPath = HttpContext.Current.Server.MapPath("~/Error.txt");
            using (StreamWriter tw = new StreamWriter(logPath))
            {
                tw.WriteLine("Path:" + path);
                tw.WriteLine("Code:" + code);
                tw.WriteLine("ExecptionMessage:" + error.Message);
                tw.WriteLine(error.Source);
                tw.WriteLine(error.StackTrace);
                //tw.WriteLine("InnerExecptionMessage:" + ex.InnerException.Message);

                if (error.InnerException != null)
                {
                    tw.WriteLine("========= InnerException =========");
                    tw.WriteLine(error.InnerException.Message);
                    tw.WriteLine(error.InnerException.Source);
                    tw.WriteLine(error.InnerException.StackTrace);
                }

                tw.Flush();
                tw.Close();
            }
            #endregion

        }
    }
}