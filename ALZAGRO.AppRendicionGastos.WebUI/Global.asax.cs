using ALZAGRO.AppRendicionGastos.WebUI.App_Start;
using ALZAGRO.Reports.Data;
using Newtonsoft.Json.Serialization;
using System;
using System.Data.Entity;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace ALZAGRO.AppRendicionGastos.WebUI {

    public class Global : HttpApplication {
        void Application_Start(object sender, EventArgs e) {

            var config = GlobalConfiguration.Configuration;

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(config);
            Bootstrapper.Run();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            GlobalConfiguration.Configuration.EnsureInitialized();

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var formatters = GlobalConfiguration.Configuration.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            Database.SetInitializer(new NullDatabaseInitializer<ReportsContext>());


        }
    }
}