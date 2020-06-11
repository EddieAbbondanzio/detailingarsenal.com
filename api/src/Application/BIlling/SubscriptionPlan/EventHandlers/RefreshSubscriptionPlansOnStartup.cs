using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class RefreshSubscriptionPlansOnStartup : IBusEventHandler<StartupEvent> {


        public async Task Handle(StartupEvent busEvent) {
        }
    }
}