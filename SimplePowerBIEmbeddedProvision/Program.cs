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
            Run();            
        }       

        static void Run()
        {
            Console.WriteLine("Please input 1 or 2 to choose step 1 or step 2 to provision Power BI Embedded:");
            Console.WriteLine("1. Provision a new workspace in an existing workspace collection");
            Console.WriteLine("2. Import PBIX Desktop file into an existing workspace");
            Console.WriteLine("3. Import PBIX Desktop file into a new workspace");
            Console.WriteLine();

            var key = Console.ReadKey(true);
            bool exit = false;
            PowerBIEmbeddedGenerator powerBI = new PowerBIEmbeddedGenerator();
            switch (key.KeyChar)
            {
                case '1':
                    Workspace workspace = powerBI.CreateWorkspace();
                    if(workspace==null)
                    {
                        Console.WriteLine("Create workspace failed.");
                        exit = true;
                        break;
                    }
                    Console.WriteLine("Workspace id:{0}",workspace.WorkspaceId);
                    Run();
                    break;
                case '2':
                    Console.WriteLine("Please input file path which you want to import:");
                    string filepath = Console.ReadLine();
                    Console.WriteLine("Please create a report name, e.g. 'myreport':");
                    string dataset = Console.ReadLine();
                    Import import = powerBI.UploadPBIXFile(filepath, dataset);
                    Console.WriteLine("The file is imported, and the id is {0}",import.Id);
                    Run();
                    break;
                case '3':
                    Console.WriteLine("Please input file path which you want to import:");
                    string filepath2 = Console.ReadLine();
                    Console.WriteLine("Please create a report name, e.g. 'myreport':");
                    string dataset2 = Console.ReadLine();
                    Workspace newworkspace = powerBI.CreateWorkspace();
                    Import newimport = powerBI.UploadPBIXFile(filepath2, dataset2, newworkspace.WorkspaceId);
                    Console.WriteLine("The file is imported, and the id is {0}", newimport.Id);
                    Run();
                    break;
                default:
                    exit = true;
                    Console.WriteLine("Please enter any key to exit...");
                    break;
            }
            if (!exit)
            {
                Run();
            }
        }
    }
}
