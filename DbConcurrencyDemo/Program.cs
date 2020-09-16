using DbConcurrencyDemo.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbConcurrencyDemo
{
    class Program
    {
        static void Main()
        {
            new ImplicitTransactionWithIsolationLevel()
                 .Execute();

            //new ExplicitTransactionWithIsolationLevel()
            //    .Execute();

            //new OptimisticConcurrencyWithTimestampCheck()
            //    .Execute();

            //new OptimisticConcurrencyWithValueCheck()
            //    .Execute();


        }
    }
}
