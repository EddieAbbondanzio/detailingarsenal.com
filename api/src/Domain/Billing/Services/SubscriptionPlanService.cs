using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Domain.Billing {
    public interface ISubscriptionPlanService : IService {
        Task<SubscriptionPlan> GetById(Guid id);
        Task<List<SubscriptionPlan>> GetAllPlans();
        Task<SubscriptionPlan> GetTrialPlan();
        Task<List<SubscriptionPlan>> RefreshPlans();
        Task Update(SubscriptionPlan plan, UpdateSubscriptionPlan update);
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

        public async Task<SubscriptionPlan> GetTrialPlan() {
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

            await repo.DeleteAll();

            foreach (var plan in plans) {
                await repo.Add(plan);
            }

            return plans;
        }

        public async Task Update(SubscriptionPlan plan, UpdateSubscriptionPlan update) {
            plan.RoleId = update.RoleId;
            await repo.Update(plan);
        }
    }
}