using BoardAndBarber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace BoardAndBarber.Data
{
    //Ado.net example

    //public class CustomerRepository
    //{
    //    static List<Customer> _customers = new List<Customer>();

    //    const string _connectionString = "Server = localhost; Database = BoardAndBarber; Trusted_Connection = True;";

    //    public void Add(Customer customerToAdd)
    //    {
    //        var sql = $@"INSERT INTO [dbo].[Customers]
    //                           ([Name]
    //                           ,[Birthday]
    //                           ,[FavoriteBarber]
    //                           ,[Notes])
    //                     OUTPUT inserted.Id
    //                     VALUES
    //                           (@name,@birthday,@FavoriteBarber,@Notes)";

    //        using var connection = new SqlConnection(_connectionString);
    //        connection.Open();

    //        var cmd = connection.CreateCommand();
    //        cmd.CommandText = sql;

    //        cmd.Parameters.AddWithValue("name", customerToAdd.Name);
    //        cmd.Parameters.AddWithValue("birthday", customerToAdd.Birthday);
    //        cmd.Parameters.AddWithValue("FavoriteBarber", customerToAdd.FavoriteBarber);
    //        cmd.Parameters.AddWithValue("Notes", customerToAdd.Notes);

    //        var newId = (int) cmd.ExecuteScalar();

    //        customerToAdd.Id = newId;

    //    }

    //    public List<Customer> GetAll()
    //    {
    //        using var connection = new SqlConnection(_connectionString);
    //        connection.Open();

    //        var command = connection.CreateCommand();
    //        var sql = $"select * from customers";

    //        command.CommandText = sql;

    //        var reader = command.ExecuteReader();
    //        var customers = new List<Customer>();

    //        while (reader.Read())
    //        {
    //            var customer = MapToCustomer(reader);
    //            customers.Add(customer);
    //        }

    //        return customers;
    //    }

    //    public Customer GetById(int id)
    //    {
    //        using var connection = new SqlConnection(_connectionString);
    //        connection.Open();

    //        var command = connection.CreateCommand();
    //        var query = $@"select *
    //                        from Customers
    //                        where id = {id}";

    //        command.CommandText = query;

    //        //command.ExecuteNonQuery();  <-- run this query and I don't care about the results (returns the number of rows affected)
    //        //command.ExecuteScalar();  <-- run this query and only return the top leftmost cell
    //        var reader = command.ExecuteReader(); // run this query and give me the results one row at a time
    //        // sql server has executed the command and is waiting to give us results


    //        if (reader.Read())
    //        {
    //            return MapToCustomer(reader);
    //        }
    //        else
    //        {
    //            //no results, what do we do?
    //            return null;
    //        }

    //    }

    //    public Customer Update(int id, Customer customer)
    //    {
    //        var sql = @"UPDATE [dbo].[Customers]
    //                       SET [Name] = @name
    //                          ,[Birthday] = @birthday
    //                          ,[FavoriteBarber] = @favoriteBarber
    //                          ,[Notes] = @notes
    //                     OUTPUT inserted.*
    //                     WHERE id = @id";

    //        using var connection = new SqlConnection(_connectionString);
    //        connection.Open();

    //        var cmd = connection.CreateCommand();
    //        cmd.CommandText = sql;

    //        cmd.Parameters.AddWithValue("id", id);
    //        cmd.Parameters.AddWithValue("name", customer.Name);
    //        cmd.Parameters.AddWithValue("birthday", customer.Birthday);
    //        cmd.Parameters.AddWithValue("favoriteBarber", customer.FavoriteBarber);
    //        cmd.Parameters.AddWithValue("notes", customer.Notes);

    //        var reader = cmd.ExecuteReader();

    //        if (reader.Read())
    //        {
    //            return MapToCustomer(reader);
    //        }

    //        return null;

    //    }

    //    public void Remove(int customerId)
    //    {
    //        var sql = @"DELETE 
    //                    FROM [dbo].[Customers]
    //                    WHERE Id = @Id";

    //        using var connection = new SqlConnection(_connectionString);
    //        connection.Open();

    //        var cmd = connection.CreateCommand();
    //        cmd.CommandText = sql;

    //        cmd.Parameters.AddWithValue("id", customerId);

    //        var rows = cmd.ExecuteNonQuery();

    //        if (rows != 1)
    //        {
    //            //do something because that is bad
    //        }
    //    }

    //    Customer MapToCustomer(SqlDataReader reader)
    //    {
    //        var customerFromDb = new Customer();
    //        //do something with the result
    //        customerFromDb.Id = (int)reader["Id"]; // explicit conversion/cast, throws an exception on failure
    //        customerFromDb.Name = reader["Name"] as string; // implicit conversion/cast, returns a null on failure
    //        customerFromDb.Birthday = DateTime.Parse(reader["Birthday"].ToString()); // parsing
    //        customerFromDb.FavoriteBarber = reader["FavoriteBarber"].ToString(); // make it a string
    //        customerFromDb.Notes = reader["Notes"].ToString();

    //        return customerFromDb;
    //    }
    //}

    // Dapper example 
    public class CustomerRepository
    {
        readonly string _connectionString;
        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BoardAndBarber");
        }

        //const string _connectionString = "Server = localhost; Database = BoardAndBarber; Trusted_Connection = True;";

        public void Add(Customer customerToAdd)
        {
            var sql = @"INSERT INTO [dbo].[Customers]
                               ([Name]
                               ,[Birthday]
                               ,[FavoriteBarber]
                               ,[Notes])
                        Output inserted.id
                        VALUES
                               (@name,@birthday,@favoritebarber,@notes)";

            using var db = new SqlConnection(_connectionString);

            var newId = db.ExecuteScalar<int>(sql, customerToAdd);

            customerToAdd.Id = newId;
        }

        public IEnumerable<Customer> GetAll()
        {
            using var db = new SqlConnection(_connectionString);

            var sql = $"select * from customers";

            var customers = db.Query<Customer>(sql);

            return customers;
        }

        public Customer GetById(int customerId)
        {
            using var db = new SqlConnection(_connectionString);

            var query = @"select *
                            from Customers
                            where id = @cid";

            var parameters = new { cid = customerId };

            var customer = db.QueryFirstOrDefault<Customer>(query, parameters);

            return customer;
        }

        public Customer Update(int id, Customer customer)
        {
            var sql = @"UPDATE [dbo].[Customers]
                           SET [Name] = @name
                              ,[Birthday] = @birthday
                              ,[FavoriteBarber] = @favoriteBarber
                              ,[Notes] = @notes
                         OUTPUT inserted.*
                         WHERE id = @id";

            using var db = new SqlConnection(_connectionString);

            var parameters = new
            {
                customer.Name,
                customer.Birthday,
                customer.FavoriteBarber,
                customer.Notes,
                id
            };

            var updatedCustomer = db.QueryFirstOrDefault<Customer>(sql, parameters);

            return updatedCustomer;
        }

        public void Remove(int customerId)
        {
            var sql = @"DELETE 
                        FROM [dbo].[Customers]
                        WHERE Id = @id";

            using var db = new SqlConnection(_connectionString);

            db.Execute(sql, new { id = customerId });
        }
    }
}
