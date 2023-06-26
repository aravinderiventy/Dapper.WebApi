using Dapper.Application.Repositories;
using Dapper.Core.Entities;
using Dapper.FastCrud;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        public string connectionString { get; set; }
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("UserDbConnection");
        }

        public async Task<int> AddAsync(User entity)
        {
            var sql = "Insert into Users (Name,Email,Address) VALUES (@Name,@Email,@Address)";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = "select *from Users where Id=@id";
                var usr = await db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
                if (usr == null) return 0;

                var res = await db.DeleteAsync(usr);
                return Convert.ToInt32(res);
            }
        }

        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var users= await db.FindAsync<User>();
                return users.ToList();
            }
        }

        public async Task<User> GetByIdAsync(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = "select *from Users where Id=@id";
                var result = await db.QueryFirstOrDefaultAsync<User>(sql, new {Id = id});
                return result;
            }
        }

        public async Task<int> UpdateAsync(User entity)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var user = await db.UpdateAsync(entity);
                return 1;
            }
        }
    }
}
