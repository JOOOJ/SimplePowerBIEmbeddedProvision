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
using System.IO;

namespace PowerBIEmbeddedLib
{
    public class PowerBIDeploy
    {
        const string powerBIApiEndpoint = "https://api.powerbi.com";
        public Workspace CreateWorkspace(string workspaceCollectionName, string accessToken)
        {
            CheckParameters(workspaceCollectionName, accessToken);
            using (IPowerBIClient client = CreateClient(accessToken))
            {
                return client.Workspaces.PostWorkspace(workspaceCollectionName);
            }
        }

        public IList<Workspace> RetrieveAllWorkspaceByCollectionName(string workspaceCollectionName, string accessToken)
        {
            CheckParameters(workspaceCollectionName, accessToken);
            using (IPowerBIClient client = CreateClient(accessToken))
            {
                return client.Workspaces.GetWorkspacesByCollectionName(workspaceCollectionName).Value;
            }
        }

        public Import UploadPbixSingleFile(string workspaceCollectionName, string filePath, string reportName, string accessToken, string workspaceId)
        {
            CheckParameters(workspaceCollectionName, filePath, reportName, accessToken, workspaceId);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The file cannot be found, please input correct file path", filePath);
            }
            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                using (IPowerBIClient client = CreateClient(accessToken))
                {
                    return client.Imports.PostImportWithFile(workspaceCollectionName, workspaceId, stream);
                }
            }
        }
         
        private IPowerBIClient CreateClient(string accessToken)
        {
            CheckParameters(accessToken);
            TokenCredentials token = new TokenCredentials(accessToken, "AppKey");
            IPowerBIClient client = new PowerBIClient(token);
            client.BaseUri = new Uri(powerBIApiEndpoint);
            return client;
        }

        private void CheckParameters(params string[] parameters)
        {
            foreach (string item in parameters)
            {
                if (string.IsNullOrEmpty(item))
                {
                    throw new ArgumentNullException(item, string.Format("{0} is incorrect", item));
                }
            }
        }
    }
}
