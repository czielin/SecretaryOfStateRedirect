using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace SecretaryOfStateRedirect
{
    public static class SosRedirectFunction
    {
        [FunctionName("Get")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Get/{*path}")] string path, HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string sosUrl = req.Path.ToString().Replace("/api/Get", "");
                sosUrl = $"https://aoprals.state.gov{sosUrl}";

                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(sosUrl);
                return response;
            }
            catch(Exception ex)
            {
                log.LogError(ex, "An error occurred attempting to process the request.");
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
    }
}
