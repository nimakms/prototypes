using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;

namespace CdsODataFunctionAppV2
{
    public class CdsODataFunctionV2
    {
        public static IActionResult Run(HttpRequest req, TraceWriter log)
        {           
            log.Info("C# HTTP trigger function processed a request.");

            var context = CdsProxyLibraryDotNetFwk.CdsConfigHelper.GetCdsContext();
            var accounts = context.accounts.EndExecute(context.accounts.BeginExecute(null, null));

            foreach (var account in accounts)
            {
                log.Info($"Name: {account.name}");
            }

            return new OkResult();
        }
    }
}
