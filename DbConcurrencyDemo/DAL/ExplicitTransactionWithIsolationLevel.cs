using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using ZooModel;

namespace DbConcurrencyDemo.DAL
{
    /// <summary>
    /// Demonstrates the implementation of explicit transactions using the CommittableTransaction class.
    /// </summary>
    public class ExplicitTransactionWithIsolationLevel
    {
        public void Execute()
        {
            var transaction = new CommittableTransaction(new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            });

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ZooDatabase"].ConnectionString))
            {
                conn.Open();
                conn.EnlistTransaction(transaction);

                try
                {
                    var animals = new List<Animal>();

                    Console.WriteLine("Starting transaction...");

                    // Getting animals from database...
                    using (var selectCmd = new SqlCommand("SELECT * FROM Animal", conn)) // Puts a lock on every row in the table. A better approach is to select only the row that you need...
                    {
                        var reader = selectCmd.ExecuteReader();
                        while (reader.Read())
                        {
                            animals.Add(new Animal
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"] as string,
                                LastUpdate = reader["Updated"] as DateTime?
                            });
                        }
                        reader.Close();
                    }

                    // Insert execution halt here to simulate long running work...
                    Console.WriteLine("Data is read from database, try and change data from another client and continue this operation by pressing <ENTER>...");
                    Console.ReadLine();

                    var animal = animals.FirstOrDefault();

                    if (animal != null)
                    {
                        animal.Name = "Julien";

                        using (var updateCmd = new SqlCommand("UPDATE Animal SET Name = @Name, Updated = @Updated WHERE Id = @Id", conn))
                        {
                            updateCmd.Parameters.AddWithValue("Name", animal.Name);
                            updateCmd.Parameters.AddWithValue("Updated", DateTime.Now);
                            updateCmd.Parameters.AddWithValue("Id", animal.Id);
                            var rowsUpdated = updateCmd.ExecuteNonQuery();
                        }
                    }
                    Console.WriteLine("Committing transaction...");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
        }
    }
}
