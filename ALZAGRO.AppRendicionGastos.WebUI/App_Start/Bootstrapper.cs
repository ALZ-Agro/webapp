using System.Web.Http;

namespace ALZAGRO.AppRendicionGastos.WebUI.App_Start {

    public class Bootstrapper {

        public static void Run() {

            AutofacWebapiConfig.Initialize(GlobalConfiguration.Configuration);
            Mappings.AutoMapperConfiguration.Configure();
            //Reports.Data.Mappings.AutoMapperConfiguration.Configure();
        }
    }
}