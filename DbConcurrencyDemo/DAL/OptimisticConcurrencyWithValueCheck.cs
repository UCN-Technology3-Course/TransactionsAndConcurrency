using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ZooModel;

namespace DbConcurrencyDemo.DAL
{
    public class OptimisticConcurrencyWithValueCheck
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
                    var oldAnimal = new Animal
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"] as string,
                        LastUpdate = reader["Updated"] as DateTime?
                    };
                    reader.Close();

                    if (oldAnimal.Clone() is Animal updatedAnimal)
                    {
                        updatedAnimal.Name = "Julien";
                        var updateCmd = new SqlCommand("UPDATE Animal SET Name = @NewName WHERE Id = @Id AND Name = @OldName", conn);
                        updateCmd.Parameters.AddWithValue("NewName", updatedAnimal.Name);
                        updateCmd.Parameters.AddWithValue("OldName", oldAnimal.Name);
                        updateCmd.Parameters.AddWithValue("Id", oldAnimal.Id);
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
}
