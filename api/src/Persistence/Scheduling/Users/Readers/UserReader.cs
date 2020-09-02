using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Persistence.Users {
    public class UserReader : DatabaseInteractor, IUserReader {
        public UserReader(IDatabase database) : base(database) { }

        public async Task<UserReadModel> ReadById(Guid id) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                    @"select * from users where id = @Id;
                  
                  select count(*) from users u 
                    join user_roles ur on u.id = ur.user_id
                    join roles r on ur.role_id = r.id
                    where r.name = 'Admin' and u.Id = @Id;
                  
                  select p.* from users u 
                    join user_roles ur on u.id = ur.user_id 
                    join roles r on ur.role_id = r.id 
                    join role_permissions rp on r.id = rp.role_id 
                    join permissions p on rp.permission_id = p.id 
                    where u.id = @Id;
                ",
                    new { Id = id }
                )) {
                    var user = reader.ReadFirst();
                    var isAdmin = reader.ReadFirst<bool>();
                    var permissions = reader.Read().Select(r => new UserPermissionReadModel(r.action, r.scope));

                    return new UserReadModel(user.email, user.name, user.joined_date, isAdmin, permissions.ToList());
                }
            }
        }
    }
}