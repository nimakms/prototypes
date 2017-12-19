using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdsODataConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = CdsProxyLibraryDotNetFwk.CdsConfigHelper.GetCdsContext();
            var accounts = context.accounts.Execute();

            foreach (var account in accounts)
            {
                Console.WriteLine($"Name: {account.name}");
            }

            Console.ReadLine();
        }
    }
}
