using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PowerBI.Api.V1;
using System.Configuration;
using Microsoft.Rest;
using Microsoft.PowerBI.Api.V1.Models;
using System.IO;

namespace SimplePowerBIEmbeddedProvision
{
    internal class PowerBIEmbeddedGenerator
    {
        static string AccessKey = ConfigurationManager.AppSettings["AccessKey"];
        static string WorkspaceCollectionName = ConfigurationManager.AppSettings["WorkspaceCollectionName"];
        static string WorkspaceId = ConfigurationManager.AppSettings["WorkspaceId"];
        static string PowerBIApiEndpoint = ConfigurationManager.AppSettings["PowerBIApiEndpoint"];

        private IPowerBIClient CreateClient()
        {
            if (string.IsNullOrEmpty(AccessKey))
            {
                Console.WriteLine("Please provide correct Access Key to create Power BI Client.");
                return null;
            }
            if (string.IsNullOrEmpty(PowerBIApiEndpoint))
            {
                Console.WriteLine("Please provide correct Power BI endpoint.");
                return null;
            }
            TokenCredentials token = new TokenCredentials(AccessKey, "AppKey");

            IPowerBIClient client = new PowerBIClient(token);
            client.BaseUri = new Uri(PowerBIApiEndpoint);
            return client;
        }

        public async Task<Workspace> CreateWorkspace()
        {
            using (var client = CreateClient())
            {
                return  await client.Workspaces.PostWorkspaceAsync(WorkspaceCollectionName);
            }
        }
        
        public async Task<Import> UploadPBIXFile(string filePath,string datasetName)
        {
            if(string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(WorkspaceId) || string.IsNullOrEmpty(datasetName) || string.IsNullOrEmpty(WorkspaceCollectionName))
            {
                return null;
            }
            using (FileStream file = File.OpenRead(filePath))
            {
                using (var client = CreateClient())
                {
                    return await client.Imports.PostImportWithFileAsync(WorkspaceCollectionName, WorkspaceId, file, datasetName);
                }
            }
        }

        public async Task<Import> UploadPBIXFile(string filePath, string datasetName,string workspaceId)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(workspaceId) || string.IsNullOrEmpty(datasetName) || string.IsNullOrEmpty(WorkspaceCollectionName))
            {
                return null;
            }
            using (FileStream file = File.OpenRead(filePath))
            {
                using (var client = CreateClient())
                {
                    return await client.Imports.PostImportWithFileAsync(WorkspaceCollectionName, workspaceId, file,datasetName);
                }
            }
        }

        public async Task<Dictionary<string,string>> GetAllResports()
        {
            ODataResponseListWorkspace odataWorkspaces = await RetrieveAllWorkspaces();
            IList<Workspace> workspaces = odataWorkspaces.Value;
            Dictionary<string, string> dict = new Dictionary<string, string>(workspaces.Count);
            using (var client = CreateClient())
            {
                foreach (Workspace item in workspaces)
                {
                    ODataResponseListDataset odataSet = await client.Datasets.GetDatasetsAsync(WorkspaceCollectionName, item.WorkspaceId);
                    if(odataSet.Value.Count==0)
                    {
                        continue;
                    }
                    List<string> list = new List<string>(odataSet.Value.Count);
                    foreach (Dataset set in odataSet.Value)
                    {
                        list.Add(string.Format("{0} {1}", set.Name, set.Id));
                    }
                    dict.Add(item.WorkspaceId, string.Join(",", list));
                }
            }
            return dict;
        }

        public async Task DeleteReport(string reportId,string workspaceId)
        {
            using (var client = CreateClient())
            {
                await client.Datasets.DeleteDatasetByIdAsync(WorkspaceCollectionName,workspaceId,reportId);
            }
        }

        private async Task<ODataResponseListWorkspace> RetrieveAllWorkspaces()
        {
            using (var client = CreateClient())
            {
                return await client.Workspaces.GetWorkspacesByCollectionNameAsync(WorkspaceCollectionName);
            }
        }
    }
}
