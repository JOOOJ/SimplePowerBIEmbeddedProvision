using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PowerBI.Api.V1;
using System.Configuration;


namespace SimplePowerBIEmbeddedProvision
{
    internal class PowerBIEmbeddedGenerator
    {
        static string AccessKey = ConfigurationManager.AppSettings["AccessKey"];
        static string WorkspaceCollectionName = ConfigurationManager.AppSettings["WorkspaceCollectionName"];
        static string WorkspaceId = ConfigurationManager.AppSettings["WorkspaceId"];
        private async Task<IPowerBIClient> CreateClient()
        {
            return null;
        }

        public async Task<Workspaces> CreateWorkspace()
        {
            return null;
        }

        public async Task<Imports> UploadPBIXFile()
        {
            return null;
        }
    }
}
