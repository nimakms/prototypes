using System;
using System.Collections.Generic;
using System.Linq;

namespace CdsProxyLibraryDotNetFwk
{
    public static class CdsConfigHelper
    {
        public static Microsoft.Dynamics.CRM.System GetCdsContext()
        {
            var cdsConfig = GetCdsConfig();
            var cdsContext = new Microsoft.Dynamics.CRM.System(new Uri($"{cdsConfig.UriString}/api/data/v9.0/"));

            var client = new System.Net.Http.HttpClient();
            var dictionary = new Dictionary<string, string>();
            dictionary["grant_type"] = "password";
            dictionary["resource"] = cdsConfig.UriString;
            dictionary["username"] = cdsConfig.User;
            dictionary["password"] = cdsConfig.Password;
            dictionary["client_id"] = cdsConfig.NativeAppId;
            
            var response = client.PostAsync("https://login.microsoftonline.com/common/oauth2/token", new System.Net.Http.FormUrlEncodedContent(dictionary)).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK) throw new InvalidOperationException("Unable to get token.");
            var responseString = response.Content.ReadAsStringAsync().Result;
            var token = Newtonsoft.Json.Linq.JObject.Parse(responseString);
            var accessToken = (string)token.SelectToken("access_token");           

            cdsContext.BuildingRequest += (sender, eventArgs) =>
            {
                eventArgs.Headers["Authorization"] = "Bearer " + accessToken;
            };
            return cdsContext;
        }


        public const string ConfigFileName = "cds-config.csv";
        public const string ConfigDirectoryName = "local-config";
        public static CdsConfig GetCdsConfig()
        {
            var assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = new System.IO.FileInfo(assemblyLocation).Directory;
            while (true)
            {
                var configDirectory = directory.GetDirectories(ConfigDirectoryName).FirstOrDefault();
                if (configDirectory != null)
                {
                    var configFile = configDirectory.GetFiles(ConfigFileName).FirstOrDefault();
                    if (configFile != null)
                    {
                        var configDictionary = System.IO.File.ReadAllLines(configFile.FullName).
                            Select(line => line.Split(',')).
                            ToDictionary(splitLine => splitLine[0], splitLine => splitLine[1]);
                        return new CdsConfig
                        {
                            User = configDictionary["user"],
                            Password = configDictionary["password"],
                            UriString = configDictionary["uriString"],
                            NativeAppId = configDictionary["nativeAppId"]
                        };
                    }
                    else
                    {
                        throw new InvalidOperationException($"{ConfigFileName} file was not found inside '{configDirectory.FullName}'.");
                    }
                }
                directory = directory.Parent;
                if (directory == null) throw new InvalidOperationException($"{ConfigDirectoryName} file was not found.");
            }
        }

        public class CdsConfig
        {
            public string User { get; set; }
            public string Password { get; set; }
            public string UriString { get; set; }
            public string NativeAppId { get; set; }
        }
    }
}
