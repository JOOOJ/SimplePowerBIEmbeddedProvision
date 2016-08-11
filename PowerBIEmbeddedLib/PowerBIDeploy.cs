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
            return Upload(workspaceCollectionName, filePath, reportName, accessToken, workspaceId);
        }

        /// <summary>
        /// Upload PowerBI multiple files into Windows Azure
        /// </summary>
        /// <param name="worksapceCollectionName"></param>
        /// <param name="accessToken"></param>
        /// <param name="workspaceId"></param>
        /// <param name="fileReportPairs">This is a dictionary, key is file full path, value is report name</param>
        /// <returns></returns>
        public IList<Import> UploadMultiPbixFiles(string worksapceCollectionName,string accessToken,string workspaceId,Dictionary<string,string> fileReportPairs)
        {
            CheckParameters(worksapceCollectionName, accessToken, workspaceId);
            if(fileReportPairs==null || fileReportPairs.Count==0)
            {
                throw new ArgumentException("The argument fileReportPairs is incorrect", "fileReportPairs");
            }
            IList<Import> importList = new List<Import>();
            foreach (KeyValuePair<string,string> item in fileReportPairs)
            {
                Import import = Upload(worksapceCollectionName, item.Key, item.Value, accessToken, workspaceId);
                importList.Add(import);
            }
            return importList;
        }
        private IPowerBIClient CreateClient(string accessToken)
        {
            CheckParameters(accessToken);
            TokenCredentials token = new TokenCredentials(accessToken, "AppKey");
            IPowerBIClient client = new PowerBIClient(token);
            client.BaseUri = new Uri(powerBIApiEndpoint);
            return client;
        }

        private Import Upload(string workspaceCollectionName, string filePath, string reportName, string accessToken, string workspaceId)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The file cannot be found, please input correct file path", filePath);
            }
            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                using (IPowerBIClient client = CreateClient(accessToken))
                {
                    return client.Imports.PostImportWithFile(workspaceCollectionName, workspaceId, stream, reportName);
                }
            }
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
