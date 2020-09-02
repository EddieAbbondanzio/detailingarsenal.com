using System.Threading.Tasks;
using DetailingArsenal.Domain;
using Dapper;
using System.Linq;
using System;
using System.Collections.Generic;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Persistence.Settings {
    public class VehicleCategoryRepo : DatabaseInteractor, IVehicleCategoryRepo {
        public VehicleCategoryRepo(IDatabase database) : base(database) { }

        public async Task<VehicleCategory?> FindById(Guid id) {
            using (var conn = OpenConnection()) {
                return await conn.QueryFirstOrDefaultAsync<VehicleCategory>(
                    @"select * from vehicle_categories where id = @Id;",
                    new { Id = id }
                );
            }
        }

        public async Task<VehicleCategory?> FindByName(string name) {
            using (var conn = OpenConnection()) {
                return await conn.QueryFirstOrDefaultAsync<VehicleCategory>(
                    @"select * from vehicle_categories where name = @Name",
                    new { Name = name }
                );
            }
        }

        public async Task<List<VehicleCategory>> FindByUser(User user) {
            using (var conn = OpenConnection()) {
                return (await conn.QueryAsync<VehicleCategory>(
                    @"select * from vehicle_categories where user_id = @Id;", user
                )).ToList();
            }
        }

        public async Task Add(VehicleCategory entity) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(
                    @"insert into vehicle_categories (id, user_id, name, description) values (@Id, @UserId, @Name, @Description);",
                    entity
                );
            }
        }

        public async Task Update(VehicleCategory entity) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(
                    @"update vehicle_categories set name = @Name, description = @Description where id = @Id",
                    entity
                );
            }
        }

        public async Task Delete(VehicleCategory entity) {
            using (var conn = OpenConnection()) {
                await conn.ExecuteAsync(
                    @"delete from vehicle_categories where id = @Id;", entity
                );
            }
        }

        public async Task<bool> IsInUse(VehicleCategory entity) {
            using (var conn = OpenConnection()) {
                int configCount = await conn.ExecuteScalarAsync<int>(@"select count(*) from service_configurations where vehicle_category_id = @Id", entity);
                return configCount > 0;
            }
        }
    }
}