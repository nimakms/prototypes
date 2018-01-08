#r ".\bin\ClassLibrary1.dll"
#r ".\bin\Microsoft.OData.Client.dll"
#r ".\bin\Microsoft.OData.Core.dll"
#r ".\bin\Microsoft.OData.Edm.dll"
#r ".\bin\Microsoft.Spatial.dll"
#r ".\bin\Microsoft.IdentityModel.Clients.ActiveDirectory.dll"

using System.Net;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
 var context = new Microsoft.Dynamics.CRM.System("https://m365x6522210.api.crm.dynamics.com/api/data/v9.0/");

            // TODO Substitute your correct CRM root service address, 
            string resource = "https://m365x6522210.api.crm.dynamics.com";

            // TODO Substitute your app registration values that can be obtained after you
            // register the app in Active Directory on the Microsoft Azure portal.
            string clientId = "b416039c-2fe3-470c-b84a-79e7504110bc";
            //string redirectUrl = "http://localhost/";


            // Authenticate the registered application with Azure Active Directory.
            AuthenticationContext authContext =
                new AuthenticationContext("https://login.windows.net/common", false);
            AuthenticationResult result = authContext.AcquireTokenAsync(resource,
                clientId,
                new UserPasswordCredential("SomeUserName", "SomePassword")).Result;


            context.BuildingRequest += (sender, eventArgs)=>
            {
                eventArgs.Headers["Authorization"] = "Bearer " + result.AccessToken;
            };

            var accounts = context.accounts.Execute();

            foreach(var account in accounts)
            {
                log.Info($"Name: {account.name}");
            }

            //Console.ReadLine();

            return req.CreateResponse(HttpStatusCode.OK);

}
