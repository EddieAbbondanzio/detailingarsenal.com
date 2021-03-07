using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Clients {
    [Authorization(Action = "read", Scope = "clients")]
    [DependencyInjection]
    public class GetClientsHandler : ActionHandler<GetClientsQuery, List<ClientView>> {
        IClientService service;

        public GetClientsHandler(IClientService service) {
            this.service = service;
        }

        public async override Task<List<ClientView>> Execute(GetClientsQuery input, User? user) {
            List<Client> clients = await service.GetByUser(user!);
            throw new NotImplementedException();
            // return mapper.Map<List<Client>, List<ClientView>>(clients);
        }
    }
}