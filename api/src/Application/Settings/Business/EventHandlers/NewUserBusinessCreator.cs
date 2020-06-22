using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;

namespace DetailingArsenal.Application.Settings {
    /// <summary>
    /// When a new user is generated, go ahead and create an empty business for the user.
    /// </summary>
    public class NewUserBusinessCreator : IBusEventHandler<NewUserEvent> {
        IBusinessService service;

        public NewUserBusinessCreator(IBusinessService service) {
            this.service = service;
        }

        public async Task Handle(NewUserEvent busEvent) {
            await service.CreateDefault(busEvent.User);
        }
    }
}