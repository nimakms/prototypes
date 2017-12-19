using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace CdsODataFunctionApp
{
    public static class CdsODataFunction
    {
        [FunctionName("CdsODataFunction")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function code is running ...");
            var context = CdsProxyLibraryDotNetFwk.CdsConfigHelper.GetCdsContext();

            var accounts = context.accounts.Execute();

            foreach (var account in accounts)
            {
                log.Info($"Name: {account.name}");
            }

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
