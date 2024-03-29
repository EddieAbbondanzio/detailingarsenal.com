using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Clients {
    public interface IClientService : IService {
        Task<Client> GetById(Guid id);
        Task<List<Client>> GetByUser(User user);
        Task<Client> Create(ClientCreate create, User user);
        Task Update(Client client, ClientUpdate update);
        Task Delete(Client client);
    }

    [DependencyInjection(RegisterAs = typeof(IClientService))]
    public class ClientService : IClientService {
        IClientRepo repo;
        ClientHasNoAppointmentsSpecification noAppointmentsSpec;

        public ClientService(IClientRepo repo, ClientHasNoAppointmentsSpecification noAppointmentsSpec) {
            this.repo = repo;
            this.noAppointmentsSpec = noAppointmentsSpec;
        }

        public async Task<Client> GetById(Guid id) {
            return await repo.FindById(id) ?? throw new EntityNotFoundException();
        }

        public async Task<List<Client>> GetByUser(User user) {
            return await repo.FindByUser(user);
        }

        public async Task<Client> Create(ClientCreate create, User user) {
            var c = Client.Create(
                user.Id,
                create.Name,
                create.Phone,
                create.Email
            );

            await repo.Add(c);
            return c;
        }

        public async Task Update(Client client, ClientUpdate update) {
            client.Name = update.Name;
            client.Phone = update.Phone;
            client.Email = update.Phone;

            await repo.Update(client);
        }

        public async Task Delete(Client client) {
            await noAppointmentsSpec.CheckAndThrow(client);
            await repo.Delete(client);
        }
    }
}