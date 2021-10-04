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
    public class CurrencyTaskRepository : ICurrencyTaskRepository<CurrencyTask>
    {
        public string connectionString { get; set; }
        public CurrencyTaskRepository(DbConfig config)
        {
            connectionString = config.ConnectionString;
        }
        public async Task<int> AddAsync(CurrencyTask currency)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string query = @"INSERT INTO CurrencyTask(Name,Description,UserId,CronExpression,'From','To','Count',LastTime) 
                                VALUES(@Name,@Description,@UserId,@CronExpression,@From,@To,@Count,@LastTime)";
                return await connection.ExecuteAsync(query, currency);
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string deleteQuery = @"DELETE FROM CurrencyTask
                                       WHERE CurrencyTask.Id=@Id";
                return await connection.ExecuteAsync(deleteQuery, new { Id = id });
            }
        }

        public async Task<IEnumerable<CurrencyTask>> GetAllAsync()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string query = @"SELECT * FROM CurrencyTask
                                 INNER JOIN User
                                 ON User.Id=CurrencyTask.UserId";
                return await connection.QueryAsync<CurrencyTask, User, CurrencyTask>(query, (currency, person) => { currency.Email = person.Email; return currency; });
            }
        }


        public async Task<IEnumerable<CurrencyTask>> GetUserTasksAsync(int userId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string query = @"SELECT * FROM CurrencyTask
                                 INNER JOIN User
                                 ON User.Id=CurrencyTask.UserId
                                 WHERE CurrencyTask.UserId=@Id";
                return (await connection.QueryAsync<CurrencyTask, User, CurrencyTask>(query, (currency, person) => { currency.Email = person.Email; return currency; }, new { Id = userId }));
            }
        }
        public async Task<CurrencyTask> GetAsync(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string query = @"SELECT * FROM CurrencyTask
                                 INNER JOIN User
                                 ON User.Id=CurrencyTask.UserId
                                 WHERE CurrencyTask.Id=@id";
                return (await connection.QueryAsync<CurrencyTask, User, CurrencyTask>(query, (currency, person) => { currency.Email = person.Email; return currency; }, new { Id = id }))
                                        .FirstOrDefault();
            }
        }

        public async Task<int> UpdateAsync(CurrencyTask currency)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string updateQuery = @"UPDATE CurrencyTask
                                       SET Name=@Name,
                                           Description=@Description,
                                           'From'=@From,
                                           'To'=@To,
                                           'Count'=@Count,
                                           CronExpression=@CronExpression,
                                           LastTime=@LastTime
                                       WHERE CurrencyTask.Id=@Id";
                return await connection.ExecuteAsync(updateQuery, currency);

            }
        }
    }
}
