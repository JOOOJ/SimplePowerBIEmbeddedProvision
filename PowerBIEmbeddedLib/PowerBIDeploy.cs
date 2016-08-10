using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PowerBI.Api.V1;
using Microsoft.PowerBI.Api.V1.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net;
using Microsoft.Rest;

namespace PowerBIEmbeddedLib
{
    public class PowerBIDeploy
    {
        const string powerBIApiEndpoint = "https://api.powerbi.com";
        public Workspace CreateWorkspace(string workspaceCollectionName, string accessToken)
        {
            if (string.IsNullOrEmpty(workspaceCollectionName) || string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException("workspaceCollectionName or accessToken", "The parameters are null or empty.");
            }
            using (IPowerBIClient client = CreateClient(accessToken))
            {
                return client.Workspaces.PostWorkspace(workspaceCollectionName);
            }
        }
        private IPowerBIClient CreateClient(string accessToken)
        {
            TokenCredentials token = new TokenCredentials(accessToken, "AppKey");
            IPowerBIClient client = new PowerBIClient(token);
            client.BaseUri = new Uri(powerBIApiEndpoint);
            return client;
        }
    }
}
