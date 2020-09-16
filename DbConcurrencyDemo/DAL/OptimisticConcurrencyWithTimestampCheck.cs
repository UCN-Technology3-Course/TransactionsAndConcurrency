using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ZooModel;

namespace DbConcurrencyDemo.DAL
{
    public class OptimisticConcurrencyWithTimestampCheck
    {
        public void Execute()
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ZooDatabase"].ConnectionString))
            {
                conn.Open();

                var selectCmd = new SqlCommand("SELECT * FROM Animal", conn);
                var reader = selectCmd.ExecuteReader();
                if (reader.Read())
                {
                    var animal = new Animal
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"] as string,
                        LastUpdate = reader["Updated"] as DateTime?
                    };
                    reader.Close();

                    // Insert execution halt here to simulate long running work...
                    Console.WriteLine("Data is read from database, try and change data from another client and continue this operation by pressing <ENTER>...");
                    Console.ReadLine();

                    var updateCmd = new SqlCommand("UPDATE Animal SET Name = @Name, Updated = @Updated WHERE Id = @Id AND Updated = @LastUpdate", conn);
                    updateCmd.Parameters.AddWithValue("Name", animal.Name);
                    updateCmd.Parameters.AddWithValue("LastUpdate", animal.LastUpdate);
                    updateCmd.Parameters.AddWithValue("Updated", DateTime.Now);
                    updateCmd.Parameters.AddWithValue("Id", animal.Id);
                    var rowsUpdated = updateCmd.ExecuteNonQuery();

                    if (rowsUpdated == 0)
                    {
                        throw new DBConcurrencyException("Data in table ANIMAL is out of sync");
                    }
                }
            }
        }
    }
}
