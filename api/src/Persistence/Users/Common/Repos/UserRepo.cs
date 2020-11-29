using System;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Persistence.Users {
    public class UserRepo : DatabaseInteractor, IUserRepo {
        public UserRepo(IDatabase database) : base(database) { }

        public async Task<User?> FindById(Guid id) {
            using (var conn = OpenConnection()) {
                var model = await conn.QueryFirstOrDefaultAsync<UserRow>(
                    @"select * from users where id = @Id", new { Id = id }
                );

                return Rebuild(model);
            }
        }

        public async Task<User?> FindByAuth0Id(string id) {
            using (var conn = OpenConnection()) {
                var model = await conn.QueryFirstOrDefaultAsync<UserRow>(
                    @"select * from users where auth_0_id = @Id", new { Id = id }
                );

                return Rebuild(model);
            }
        }

        public async Task<User?> FindByEmail(string email) {
            using (var conn = OpenConnection()) {
                var model = await conn.QueryFirstOrDefaultAsync<UserRow>(
                    @"select * from users where email = @Email", new { Email = email }
                );

                return Rebuild(model);
            }
        }

        public async Task Add(User entity) {
            using (var conn = OpenConnection()) {
                var model = Map(entity);

                await conn.ExecuteAsync(
                    @"insert into users (id, auth_0_id, name, email, username, joined_date) VALUES (@Id, @Auth0Id, @Name, @Email, @Username, @JoinedDate);",
                    model
                );
            }
        }

        public async Task Update(User entity) {
            using (var conn = OpenConnection()) {
                var model = Map(entity);

                await conn.ExecuteAsync(
                    @"update users set auth_0_id = @Auth0Id, name = @Name, email = @Email, username = @Username, joined_date = @JoinedDate where id = @Id;",
                    model
                );
            }
        }

        public async Task Delete(User entity) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(
                    @"delete from users where id = @Id;",
                    entity
                );
            }
        }

        User? Rebuild(UserRow? model) => model == null ? null : new User(model.Id, model.Auth0Id, model.Email, model.Username, model.JoinedDate, model.Name);
        UserRow Map(User entity) => new UserRow() {
            Id = entity.Id,
            Auth0Id = entity.Auth0Id,
            Email = entity.Email,
            Username = entity.Username,
            JoinedDate = entity.JoinedDate,
            Name = entity.Name
        };
    }
}