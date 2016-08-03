using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PowerBI.Api.V1;

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
                    await powerBI.CreateWorkspace();
                    await Run();
                    break;
                case '2':                    
                    await powerBI.UploadPBIXFile();
                    await Run();
                    break;
                case '3':
                    await powerBI.CreateWorkspace();
                    await powerBI.UploadPBIXFile();
                    await Run();
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
