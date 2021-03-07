using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Persistence.Settings {
    [DependencyInjection(RegisterAs = typeof(IServiceRepo))]
    public class ServiceRepo : DatabaseInteractor, IServiceRepo {
        public ServiceRepo(IDatabase database) : base(database) { }

        public async Task<Service?> FindById(Guid id) {
            using (var conn = OpenConnection()) {
                var service = await conn.QueryFirstOrDefaultAsync<Service>(@"select * from services where id = @Id", new { Id = id });

                if (service == null) {
                    return null;
                }

                service.Configurations = (await conn.QueryAsync<ServiceConfiguration>(@"select * from service_configurations where service_id = @Id", service)).ToList();
                return service;
            }
        }

        public async Task<Service?> FindByName(string name) {
            using (var conn = OpenConnection()) {
                var service = await conn.QueryFirstOrDefaultAsync<Service>(@"select * from services where name = @Name", new { Name = name });

                if (service == null) {
                    return null;
                }

                service.Configurations = (await conn.QueryAsync<ServiceConfiguration>(@"select * from service_configurations where service_id = @Id", service)).ToList();
                return service;
            }
        }

        public async Task<List<Service>> FindByUser(User user) {
            using (var conn = OpenConnection()) {
                var services = await conn.QueryAsync<Service>(@"select * from services where user_id = @Id", user);

                if (services == null) {
                    return new List<Service>();
                }

                foreach (Service service in services) {
                    // Slow!
                    service.Configurations = (await conn.QueryAsync<ServiceConfiguration>(@"select * from service_configurations where service_id = @Id", service)).ToList();
                }

                return services.ToList();
            }
        }

        public async Task Add(Service entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"insert into services (id, user_id, name, description, pricing_method) values (@Id, @UserId, @Name, @Description, @PricingMethod);",
                        entity
                    );

                    await conn.ExecuteAsync(
                        @"insert into service_configurations (id, service_id, vehicle_category_id, price, duration) values (@Id, @ServiceId, @VehicleCategoryId, @Price, @Duration);",
                         entity.Configurations
                    );

                    t.Commit();
                }
            }
        }

        public async Task Update(Service entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"update services set user_id = @UserId, name = @Name, description = @Description, pricing_method = @PricingMethod where id = @Id",
                        entity
                    );

                    // update the configs
                    await conn.ExecuteAsync(
                        @"delete from service_configurations where service_id = @Id",
                        entity
                    );

                    await conn.ExecuteAsync(
                        @"insert into service_configurations (id, service_id, vehicle_category_id, price, duration) values (@Id, @ServiceId, @VehicleCategoryId, @Price, @Duration);",
                        entity.Configurations
                    );

                    t.Commit();
                }
            }
        }

        public async Task Delete(Service entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"delete from service_configurations where service_id = @Id",
                        entity
                    );

                    await conn.ExecuteAsync(
                        @"delete from services where id = @Id",
                        entity
                    );

                    t.Commit();
                }
            }
        }
    }
}