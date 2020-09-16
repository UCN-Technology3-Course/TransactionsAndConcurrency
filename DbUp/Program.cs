using DbUp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DbUpDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            #region MyRegion
            //// Enter DataSorce
            //Console.Write("Enter servername: ->");
            //var ds = Console.ReadLine();

            //// Enter initial catalog (database name)
            //Console.Write("Enter database name: ->");
            //var ic = Console.ReadLine();

            //// Enter username for database access
            //Console.Write("Enter username: ->");
            //var un = Console.ReadLine();

            //// Enter password for database access and masking input...
            //var pw = string.Empty;
            //Console.Write("Enter password: ->");
            //do
            //{
            //    var input = Console.ReadKey(true);
            //    if (input.KeyChar >= 32 && input.KeyChar <= 122)
            //    {
            //        pw += input.KeyChar;
            //        Console.Write("*");
            //    }
            //    if (input.Key == ConsoleKey.Enter)
            //    {
            //        Console.WriteLine("\n\n");
            //        break;
            //    }

            //} while (true); 
            #endregion


            // Builds connection string
            var connBldr = new SqlConnectionStringBuilder
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "ZooDatabase", 
                IntegratedSecurity = true
            };

            try
            {
                EnsureDatabase.For.SqlDatabase(connBldr.ConnectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}\n\n", ex.Message);
            }

            Console.WriteLine("Continuing with database upgrade...\n\n");

            // Creating database upgrader
            var upgrader = DeployChanges.To
                .SqlDatabase(connBldr.ConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            // Executes upgrade of database
            var result = upgrader.PerformUpgrade();

            // Writes upgrade result to console
            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success!");
                Console.ResetColor();
            }

#if DEBUG
            Console.ReadLine();
#endif

        }
    }
}
