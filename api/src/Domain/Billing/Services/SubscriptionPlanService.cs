using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Domain.Billing {
    public interface ISubscriptionPlanService : IService {
        Task<SubscriptionPlan> GetById(Guid id);
        Task<List<SubscriptionPlan>> GetAllPlans();
        Task<SubscriptionPlan> GetDefaultPlan();
        Task<List<SubscriptionPlan>> RefreshPlans();
    }

    public class SubscriptionPlanService : ISubscriptionPlanService {
        ISubscriptionPlanGateway gateway;
        ISubscriptionPlanRepo repo;
        ISubscriptionConfig config;

        public SubscriptionPlanService(ISubscriptionPlanGateway gateway, ISubscriptionPlanRepo repo, ISubscriptionConfig config) {
            this.gateway = gateway;
            this.repo = repo;
            this.config = config;
        }

        public async Task<List<SubscriptionPlan>> GetAllPlans() {
            return await repo.FindAll();
        }

        public async Task<SubscriptionPlan> GetById(Guid id) {
            return await repo.FindById(id) ?? throw new EntityNotFoundException();
        }

        public async Task<SubscriptionPlan> GetDefaultPlan() {
            return await repo.FindByBillingReference(
                new BillingReference(
                    config.DefaultPlan,
                    BillingReferenceType.Product
                )
            )
                ?? throw new EntityNotFoundException();
        }

        public async Task<List<SubscriptionPlan>> RefreshPlans() {
            var plans = await gateway.GetAll();

            foreach (var plan in plans) {
                var existingPlan = await repo.FindById(plan.Id);

                if (existingPlan == null) {
                    await repo.Add(plan);
                } else {
                    await repo.Update(plan);
                }
            }

            return plans;
        }
    }
}