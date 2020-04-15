using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Infrastructure.Persistence {
    public class ServiceRepo : DatabaseInteractor, IServiceRepo {
        public ServiceRepo(IDatabase database) : base(database) { }

        public async Task<Service?> FindById(Guid id) {
            var service = await Connection.QueryFirstOrDefaultAsync<Service>(@"select * from services where id = @Id", new { Id = id });

            if (service == null) {
                return null;
            }

            service.Configurations = (await Connection.QueryAsync<ServiceConfiguration>(@"select * from service_configurations where service_id = @Id", service)).ToList();
            return service;
        }

        public async Task<Service?> FindByName(string name) {
            var service = await Connection.QueryFirstOrDefaultAsync<Service>(@"select * from services where name = @Name", new { Name = name });

            if (service == null) {
                return null;
            }

            service.Configurations = (await Connection.QueryAsync<ServiceConfiguration>(@"select * from service_configurations where service_id = @Id", service)).ToList();
            return service;
        }

        public async Task<List<Service>> FindByUser(User user) {
            var services = await Connection.QueryAsync<Service>(@"select * from services where user_id = @Id", user);

            if (services == null) {
                return new List<Service>();
            }

            foreach (Service service in services) {
                // Slow!
                service.Configurations = (await Connection.QueryAsync<ServiceConfiguration>(@"select * from service_configurations where service_id = @Id", service)).ToList();
            }

            return services.ToList();
        }

        public async Task Add(Service entity) {
            await Connection.ExecuteAsync(@"insert into services (id, user_id, name, description) values (@Id, @UserId, @Name, @Description);", entity);
            await Connection.ExecuteAsync(@"insert into service_configurations (id, service_id, vehicle_category_id, price, duration) values (@Id, @ServiceId, @VehicleCategoryId, @Price, @Duration);", entity.Configurations);
        }

        public async Task Update(Service entity) {
            await Connection.ExecuteAsync(@"update services set user_id = @UserId, name = @Name, description = @Description where id = @Id", entity);

            // update the configs
            await Connection.ExecuteAsync(@"delete from service_configurations where service_id = @Id", entity);
            await Connection.ExecuteAsync(@"insert into service_configurations (id, service_id, vehicle_category_id, price, duration) values (@Id, @ServiceId, @VehicleCategoryId, @Price, @Duration);", entity.Configurations);
        }

        public async Task Delete(Service entity) {
            await Connection.ExecuteAsync(@"delete from service_configurations where service_id = @Id", entity);
            await Connection.ExecuteAsync(@"delete from services where id = @Id", entity);
        }
    }
}