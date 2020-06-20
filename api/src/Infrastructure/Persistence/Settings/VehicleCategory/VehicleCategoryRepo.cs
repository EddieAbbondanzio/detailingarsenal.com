using System.Threading.Tasks;
using DetailingArsenal.Domain;
using Dapper;
using System.Linq;
using System;
using System.Collections.Generic;
using DetailingArsenal.Domain.Settings;

namespace DetailingArsenal.Infrastructure.Persistence {
    public class VehicleCategoryRepo : DatabaseInteractor, IVehicleCategoryRepo {
        public VehicleCategoryRepo(IDatabase database) : base(database) { }

        public async Task<VehicleCategory?> FindById(Guid id) {
            return await Connection.QueryFirstOrDefaultAsync<VehicleCategory>(
                @"select * from vehicle_categories where id = @Id;",
                new { Id = id }
            );
        }

        public async Task<VehicleCategory?> FindByName(string name) {
            return await Connection.QueryFirstOrDefaultAsync<VehicleCategory>(
                @"select * from vehicle_categories where name = @Name",
                new { Name = name }
            );
        }

        public async Task<List<VehicleCategory>> FindByUser(User user) {
            return (await Connection.QueryAsync<VehicleCategory>(
                @"select * from vehicle_categories where user_id = @Id;", user
            )).ToList();
        }

        public async Task Add(VehicleCategory entity) {
            await Connection.ExecuteAsync(
                @"insert into vehicle_categories (id, user_id, name, description) values (@Id, @UserId, @Name, @Description);",
                entity
            );
        }

        public async Task Update(VehicleCategory entity) {
            await Connection.ExecuteAsync(
                @"update vehicle_categories set name = @Name, description = @Description where id = @Id",
                entity
            );
        }

        public async Task Delete(VehicleCategory entity) {
            await Connection.ExecuteAsync(
                @"delete from vehicle_categories where id = @Id;", entity
            );
        }

        public async Task<bool> IsInUse(VehicleCategory entity) {
            int configCount = await Connection.ExecuteScalarAsync<int>(@"select count(*) from service_configurations where vehicle_category_id = @Id", entity);
            return configCount > 0;
        }
    }
}