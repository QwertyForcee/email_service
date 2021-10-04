using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WepApp.Api.Entities;
using WepApp.Api.Models;

namespace WepApp.Api.DataAccess.Repositories.Implementations
{
    public class UserRepository : IUserRepository<User>
    {
        public string connectionString { get; set; }

        private string getUserQuery = @"SELECT * FROM User WHERE Email = @Email AND Password = @Password";
        private string get_urole = @"SELECT * FROM Role WHERE Name = 'User'";
        public UserRepository(DbConfig config)
        {
            this.connectionString = config.ConnectionString;
        }
        public IEnumerable<User> GetAll()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string query = @"SELECT * FROM User";
                return connection.Query<User>(query);
            };
        }

        public User Get(int id)
        {
            using (var connection = new SqliteConnection(connectionString)) {
                string query = @"SELECT * FROM User 
                                INNER JOIN UserRole 
                                ON UserRole.UserId=User.Id
                                INNER JOIN Role
                                ON UserRole.RoleId=Role.Id
                                WHERE User.Id = @Id";

                return connection.Query<User,Role,User>(query,(u,r)=> { u.Roles = new List<Role> { r }; return u; },new { Id=id }).FirstOrDefault();
            }
        }

        public User LogIn(LoginModel credentials)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return connection.Query<User>(this.getUserQuery, credentials).FirstOrDefault();
            }
        }

        public User Register(LoginModel credentials)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string userExists = "SELECT * FROM User WHERE Email = @Email";
                var res = connection.Query<User>(userExists, credentials).FirstOrDefault();
                if (res == null)
                {
                    string addUser = "INSERT INTO User( Email , Password ) VALUES( @Email , @Password );";
                    connection.Execute(addUser, credentials);

                    var role = this.GetRole_User();
                    var user = connection.Query<User>(this.getUserQuery, credentials).FirstOrDefault();
                    
                    string addUserRole = "INSERT INTO UserRole(UserId,RoleId) VALUES (@UserId,@RoleId)";
                    connection.Execute(addUserRole,new { UserId=user.Id,RoleId=role.Id });
                    user.Roles = new List<Role> { role };
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }
        private Role GetRole_User()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                var urole = connection.Query<Role>(this.get_urole).FirstOrDefault();
                if (urole == null)
                {
                    string setupRoles = "INSERT INTO Role(Name) VALUES ('User');";
                    connection.Execute(setupRoles);
                    urole = connection.Query<Role>(this.get_urole).FirstOrDefault();
                }
                return urole;
            }
        }
    }
}
