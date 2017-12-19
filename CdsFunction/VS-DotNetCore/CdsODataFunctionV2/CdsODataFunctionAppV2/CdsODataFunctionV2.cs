using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace CdsODataFunctionAppV2
{
    public static class CdsODataFunctionV2
    {
        //[FunctionName("CdsODataFunctionV2")]
        //public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)


        public static Microsoft.AspNetCore.Mvc.IActionResult Run(Microsoft.AspNetCore.Http.HttpRequest req, TraceWriter log)
        {

            log.Info("C# HTTP trigger function code is running ...");
            var context = CdsProxyLibraryDotNetFwk.CdsConfigHelper.GetCdsContext();

            var accountsQuery = context.accounts;
            var accounts = accountsQuery.EndExecute(accountsQuery.BeginExecute(null, null));

            foreach (var account in accounts)
            {
                log.Info($"Name: {account.name}");
            }

            //return req.CreateResponse(HttpStatusCode.OK);
            return new Microsoft.AspNetCore.Mvc.OkResult();
        }
    }
}
