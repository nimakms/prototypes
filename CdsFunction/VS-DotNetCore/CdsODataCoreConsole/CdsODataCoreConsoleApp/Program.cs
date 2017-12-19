using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdsODataCoreConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = CdsProxyLibraryDotNetFwk.CdsConfigHelper.GetCdsContext();
            var accounts = context.accounts.EndExecute(context.accounts.BeginExecute(null, null));

            foreach (var account in accounts)
            {
                Console.WriteLine($"Name: {account.name}");
            }

            Console.ReadLine();
        }
    }
}
