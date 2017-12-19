using System;

namespace CdsODataCoreConsole
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
