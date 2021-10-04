using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WepApp.Api.Entities;
using Dapper;

namespace WepApp.Api.DataAccess.Repositories.DapperImplementations
{
    public class QuoteTaskRepository : IQuoteTaskRepository<QuoteTask>
    {
        public string connectionString { get; set; }
        public QuoteTaskRepository(DbConfig config)
        {
            connectionString = config.ConnectionString;
        }
        public async Task<int> AddAsync(QuoteTask quoteTask)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string query = @"INSERT INTO QuoteTask(Name,Description,UserId,Lang,CronExpression,LastTime) 
                                VALUES(@Name,@Description,@UserId,@Lang,@CronExpression,@LastTime)";
                return await connection.ExecuteAsync(query, quoteTask);
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string deleteQuery = @"DELETE FROM QuoteTask
                                       WHERE QuoteTask.Id=@Id";
                return await connection.ExecuteAsync(deleteQuery, new { Id = id });
            }
        }

        public async Task<IEnumerable<QuoteTask>> GetAllAsync()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string query = @"SELECT * FROM QuoteTask
                                 INNER JOIN User
                                 ON User.Id=QuoteTask.UserId";
                return await connection.QueryAsync<QuoteTask, User, QuoteTask>(query, (quote, person) => { quote.Email = person.Email; return quote; });
            }
        }

        public async Task<IEnumerable<QuoteTask>> GetUserTasksAsync(int userId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string query = @"SELECT * FROM QuoteTask
                                 INNER JOIN User
                                 ON User.Id=QuoteTask.UserId
                                 WHERE QuoteTask.UserId=@Id";
                return (await connection.QueryAsync<QuoteTask, User, QuoteTask>(query, (quote, person) => { quote.Email = person.Email; return quote; }, new { Id = userId }));
            }
        }
        public async Task<QuoteTask> GetAsync(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string query = @"SELECT * FROM QuoteTask
                                 INNER JOIN User
                                 ON User.Id=QuoteTask.UserId
                                 WHERE QuoteTask.Id=@id";
                return (await connection.QueryAsync<QuoteTask, User, QuoteTask>(query, (quote, person) => { quote.Email = person.Email; return quote; }, new { Id = id }))
                                        .FirstOrDefault();
            }
        }

        public async Task<int> UpdateAsync(QuoteTask quoteTask)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string updateQuery = @"UPDATE QuoteTask
                                       SET Name=@Name,
                                           Description=@Description,
                                           Lang=@Lang,
                                           CronExpression=@CronExpression,
                                           LastTime=@LastTime
                                       WHERE QuoteTask.Id=@Id";
                return await connection.ExecuteAsync(updateQuery, quoteTask);

            }
        }
    }
}
