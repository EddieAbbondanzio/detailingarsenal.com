using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Infrastructure.Persistence {
    public class EmployeeRepo : DatabaseInteractor, IEmployeeRepo {
        public EmployeeRepo(IDatabase database) : base(database) {
        }

        public async Task<Employee?> FindById(Guid id) {
            return await Connection.QueryFirstOrDefaultAsync<Employee>(
                @"select * from employees where id = @Id;", new { Id = id }
            );
        }

        public async Task<List<Employee>> FindByUser(User user) {
            return (await Connection.QueryAsync<Employee>(
                @"select * from employees where user_id = @Id;", user
            )).ToList();
        }

        public async Task Add(Employee entity) {
            await Connection.ExecuteAsync(
                @"insert into employees (id, user_id, name, position) values (@Id, @UserId, @Name, @Position);", entity
            );
        }

        public async Task Update(Employee entity) {
            await Connection.ExecuteAsync(
                @"update employees set name = @Name, position = @Position where id = @Id", entity
            );
        }

        public async Task Delete(Employee entity) {
            await Connection.ExecuteAsync(
                @"delete from employees where id = @Id", entity
            );
        }
    }
}