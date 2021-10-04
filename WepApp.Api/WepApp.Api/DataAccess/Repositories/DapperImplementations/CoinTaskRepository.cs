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
    public class CoinTaskRepository : ICoinTaskRepository<CoinTask>
    {
        public string connectionString { get; set; }
        public CoinTaskRepository(DbConfig config)
        {
            connectionString = config.ConnectionString;
        }
        public async Task<int> AddAsync(CoinTask coin)
        {
            
            using (var connection = new SqliteConnection(connectionString))
            {
                string query = @"INSERT INTO CoinTask(Name,Description,UserId,CoinId,CronExpression,LastTime) 
                                VALUES(@Name,@Description,@UserId,@CoinId,@CronExpression,@LastTime)";
                return await connection.ExecuteAsync(query,coin);
            }
        }

        public async Task<IEnumerable<CoinTask>> GetAllAsync()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string query = @"SELECT * FROM CoinTask
                                 INNER JOIN User
                                 ON CoinTask.UserId=User.Id";
                return await connection.QueryAsync<CoinTask,User,CoinTask>(query,(coin,person)=> { coin.Email = person.Email;return coin; });
            }
        }

        public async Task<CoinTask> GetAsync(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string query = @"SELECT * FROM CoinTask
                                 INNER JOIN User
                                 ON User.Id=CoinTask.UserId
                                 WHERE CoinTask.Id=@Id";
                return (await connection.QueryAsync<CoinTask, User, CoinTask>(query, (coin, person) => { coin.Email = person.Email; return coin; },new { Id=id }))
                                        .FirstOrDefault();
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string deleteQuery = @"DELETE FROM CoinTask
                                       WHERE CoinTask.Id=@Id";
                return await connection.ExecuteAsync(deleteQuery,new { Id=id });
            }
        }

        public async Task<int> UpdateAsync(CoinTask coin)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string updateQuery = @"
    UPDATE CoinTask 
    SET Name=@Name, 
    Description=@Description, 
    CoinId=@CoinId, 
    CronExpression=@CronExpression, 
    LastTime=@LastTime 
    WHERE CoinTask.Id=@Id";
                return await connection.ExecuteAsync(updateQuery,coin);

            }
        }

        public async Task<IEnumerable<CoinTask>> GetUserTasksAsync(int userId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string query = @"SELECT * FROM CoinTask
                                 INNER JOIN User
                                 ON User.Id=CoinTask.UserId
                                 WHERE CoinTask.UserId=@Id";
                return (await connection.QueryAsync<CoinTask, User, CoinTask>(query, (coin, person) => { coin.Email = person.Email; return coin; }, new { Id = userId }));
            }
        }
    }
}
