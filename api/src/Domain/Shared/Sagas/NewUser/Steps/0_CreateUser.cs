using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain {
    public class CreateUserStep : SagaStep<string> {
        IUserRepo userRepo;
        IUserGateway userGateway;
        IDomainEventPublisher eventPublisher;

        public CreateUserStep(IUserRepo userRepo, IUserGateway userGateway, IDomainEventPublisher eventPublisher) {
            this.userRepo = userRepo;
            this.userGateway = userGateway;
            this.eventPublisher = eventPublisher;
        }

        public async override Task Execute(SagaContext<string> context) {
            var cachedUser = await userRepo.FindByAuth0Id(context.Input);

            if (cachedUser != null) {
                context.Data.User = cachedUser;
                return;
            }

            var newUser = await userGateway.GetUserByAuth0Id(context.Input);

            // TODO: Switch to webhooks to avoid this.
            try {
                await userRepo.Add(newUser);
            } catch {
                Console.WriteLine("0_CreateUser: Duplicate user attempted to be created. Just use webhooks already");
            }

            // refactor this. Events should come from entity?
            _ = eventPublisher.Dispatch(new NewUserCreatedEvent(newUser));

            context.Data.User = newUser;
        }

        public async override Task Compensate(SagaContext<string> context) {
            await userRepo.Delete(context.Data.User);
        }
    }
}