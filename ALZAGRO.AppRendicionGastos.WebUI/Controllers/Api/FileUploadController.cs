using ALZAGRO.AppRendicionGastos.Application.Dtos;
using ALZAGRO.AppRendicionGastos.Fwk.Controllers;
using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using ALZAGRO.AppRendicionGastos.Fwk.Domain;
using ALZAGRO.AppRendicionGastos.WebUI.Infrastructure.Core;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SECCO.AppInspeccionPlanta.WebUI.Controllers.Api {
    [RoutePrefix("api/fileupload")]
    public class FileUploadController : ApiControllerBase {
        private readonly IUnitOfWork unitOfWork;
        private IErrorAppService errorAppService;
        public FileUploadController(IUnitOfWork unitOfWork, IErrorAppService errorAppService)
            : base(errorAppService) {
            this.unitOfWork = unitOfWork;
        }

        [MimeMultipart]
        [Route("upload")]
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request, String entity) {
                var uploadPath = HttpContext.Current.Server.MapPath("~/Resources/" + entity);
                Directory.CreateDirectory(uploadPath);
                var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);
                // Read the MIME multipart asynchronously 
                await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

                var _localFileName = multipartFormDataStreamProvider.FileData.
                                                                     Select(multiPartData => multiPartData.LocalFileName).
                                                                     FirstOrDefault();

                FileUploadResult fileUploadResult = new FileUploadResult {
                    LocalFilePath = _localFileName,
                    FileName = Path.GetFileName(_localFileName),
                    FileLength = new FileInfo(_localFileName).Length
                };

                var response = request.CreateResponse(HttpStatusCode.OK, fileUploadResult);
                return response;
            
           
        }
        //[AllowAnonymous]
       // [WebApiValidateAntiForgeryToken]
        [HttpPost]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, ImageDto dto) {


            if (File.Exists(HttpContext.Current.Server.MapPath("~/" + dto.Path))) {
                try {
                    File.Delete(HttpContext.Current.Server.MapPath("~/" + dto.Path));
                    var response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                    return response;
                }
                catch (IOException) {
                    var response = request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Algo salio mal durante la operación." });
                    return response;
                }
            }
            else {
                var response = request.CreateResponse(HttpStatusCode.BadRequest, new { message = "El archivo no existe." });
                return response;
            }       
        }

        [HttpGet]
        // [WebApiValidateAntiForgeryToken]
        //[AllowAnonymous]
        [Route("exist")]
        public HttpResponseMessage Exist(HttpRequestMessage request, [FromUri] ImageExistDto dto) {


            if (File.Exists(HttpContext.Current.Server.MapPath("~/Resources/"+dto.Entity+"/"+ dto.Name))) {
                    var response = request.CreateResponse(HttpStatusCode.OK, new { exist = true });
                    return response;
            }
            else {
                var response = request.CreateResponse(HttpStatusCode.OK, new { exist = false });
                return response;
            }



        }

        [Route("getfile")]
        [HttpGet]
        public HttpResponseMessage GetTestFile(HttpRequestMessage request, [FromUri] String route) {
            HttpResponseMessage result = null;
            var localFilePath = HttpContext.Current.Server.MapPath("~" + route);

            if (!File.Exists(localFilePath)) {
                result = Request.CreateResponse(HttpStatusCode.Gone);
            }
            else {
                // Serve the file to the client
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") {
                    FileName = "downloadedFile"
                };
            }

            return result;
        }
    }
}