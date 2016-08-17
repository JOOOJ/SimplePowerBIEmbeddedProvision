using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PowerBI.Api.V1.Models;


namespace SimplePowerBIEmbeddedProvision
{
    class Program
    {
        static void Main(string[] args)
        {
            Run().GetAwaiter().GetResult();
        }

        static async Task Run()
        {
            Console.WriteLine("Please input choose number, such as 1 or 2 to choose steps to provision Power BI Embedded:");
            Console.WriteLine("1. Provision a new workspace in an existing workspace collection");
            Console.WriteLine("2. Import PBIX Desktop file into an existing workspace");
            Console.WriteLine("3. Import PBIX Desktop file into a new workspace");
            Console.WriteLine("4. Delete reports from worksapce");
            Console.WriteLine();

            var key = Console.ReadKey(true);
            bool exit = false;
            PowerBIEmbeddedGenerator powerBI = new PowerBIEmbeddedGenerator();
            switch (key.KeyChar)
            {
                case '1':
                    Workspace workspace = await powerBI.CreateWorkspace();
                    if (workspace == null)
                    {
                        Console.WriteLine("Create workspace failed.");
                        exit = true;
                        break;
                    }

                    Console.WriteLine("Workspace id:{0}", workspace.WorkspaceId);
                    await Run();
                    break;
                case '2':
                    Console.WriteLine("Please input file path which you want to import:");
                    string filepath = Console.ReadLine();
                    Console.WriteLine("Please create a report name, e.g. 'myreport':");
                    string dataset = Console.ReadLine();
                    Import import = await powerBI.UploadPBIXFile(filepath, dataset);
                    Console.WriteLine("The file is imported, and the id is {0}", import.Id);
                    Console.WriteLine();
                    await Run();
                    break;
                case '3':
                    Console.WriteLine("Please input file path which you want to import:");
                    string filepath2 = Console.ReadLine();
                    Console.WriteLine("Please create a report name, e.g. 'myreport':");
                    string dataset2 = Console.ReadLine();
                    Workspace newworkspace = await powerBI.CreateWorkspace();
                    Import newimport = await powerBI.UploadPBIXFile(filepath2, dataset2, newworkspace.WorkspaceId);
                    Console.WriteLine("The file is imported, and the id is {0}", newimport.Id);
                    Console.WriteLine();
                    await Run();
                    break;
                case '4':
                    Dictionary<string, string> dict = await powerBI.GetAllResports();
                    if (dict.Count > 0)
                    {
                        Console.WriteLine("The below is all reports, please choose correct report id you want to delete.");
                        foreach (KeyValuePair<string, string> item in dict)
                        {
                            Console.WriteLine("Workspace Id {0}", item.Key);
                            Console.WriteLine("   {0}", item.Value);
                        }
                        Console.WriteLine("Please input a workspace id:");
                        string workspaceId = Console.ReadLine();
                        Console.WriteLine("Please input a report id you want to delete:");
                        string reportId = Console.ReadLine();
                        await powerBI.DeleteReport(reportId, workspaceId);
                        Console.WriteLine("Dataset deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("There is no any report exist on this workspace collection.");
                        Console.WriteLine();
                        await Run();
                    }
                    break;
                default:
                    exit = true;
                    Console.WriteLine("Please enter any key to exit...");
                    break;
            }
            if (!exit)
            {
                await Run();
            }
        }
    }
}
