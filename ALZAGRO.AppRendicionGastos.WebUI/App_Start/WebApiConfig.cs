using FluentValidation.WebApi;
using ALZAGRO.AppRendicionGastos.WebUI.MessageHandlers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ALZAGRO.AppRendicionGastos.WebUI
{

    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {

            config.MessageHandlers.Add(new AuthHandler());

            config.MapHttpAttributeRoutes();

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );



            FluentValidationModelValidatorProvider.Configure(config);
        }
    }
}