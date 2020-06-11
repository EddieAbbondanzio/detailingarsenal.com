using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;

public class SubscriptionPlanRepo : DatabaseInteractor, ISubscriptionPlanRepo {
    public SubscriptionPlanRepo(IDatabase database) : base(database) { }

    public async Task<SubscriptionPlan?> FindById(Guid id) {
        var plan = await Connection.QueryFirstOrDefaultAsync<SubscriptionPlan>(
            @"select * from subscription_plans where id = @Id;", new {
                Id = id
            }
        );

        if (plan == null) {
            return null;
        }

        plan.Prices = (await Connection.QueryAsync<SubscriptionPlanPrice>(
            @"select * from subscription_plan_prices where plan_id = @Id;",
            plan
        )).ToList();

        return plan;
    }

    public async Task<SubscriptionPlan?> FindByExternalId(string externalId) {
        var plan = await Connection.QueryFirstOrDefaultAsync<SubscriptionPlan>(
            @"select * from subscription_plans where external_id = @ExternalId;", new {
                ExternalId = externalId
            }
        );

        if (plan == null) {
            return null;
        }

        plan.Prices = (await Connection.QueryAsync<SubscriptionPlanPrice>(
            @"select * from subscription_plan_prices where plan_id = @Id;",
            plan
        )).ToList();

        return plan;
    }

    public async Task<List<SubscriptionPlan>> FindAll() {
        var plans = await Connection.QueryAsync<SubscriptionPlan>(
            @"select * from subscription_plans;"
        );

        foreach (var plan in plans) {
            plan.Prices = (await Connection.QueryAsync<SubscriptionPlanPrice>(
                @"select * from subscription_plan_prices where plan_id = @Id;",
                plan
            )).ToList();
        }

        return plans.ToList();
    }

    public async Task Add(SubscriptionPlan entity) {
        await Connection.ExecuteAsync(
            @"insert into subscription_plans (id, name, external_id) values (@Id, @Name, @ExternalId);",
            entity
        );

        var prices = entity.Prices.Select(p => new SubscriptionPlanPriceModel() {
            Id = p.Id,
            ExternalId = p.ExternalId,
            Interval = p.Interval,
            PlanId = entity.Id,
            Price = p.Price
        }).ToList();

        await Connection.ExecuteAsync(
            @"insert into subscription_plan_prices (id, plan_id, external_id, interval, price) values (@Id, @PlanId, @ExternalId, @Interval, @Price);",
            prices
        );
    }

    public async Task Update(SubscriptionPlan entity) {
        await Connection.ExecuteAsync(
            @"update subscription_plans set name = @Name where id = @Id;",
            entity
        );

        await Connection.ExecuteAsync(
            @"delete from subscription_plan_prices where plan_id = @Id;",
            entity
        );

        var prices = entity.Prices.Select(p => new SubscriptionPlanPriceModel() {
            Id = p.Id,
            ExternalId = p.ExternalId,
            Interval = p.Interval,
            PlanId = entity.Id,
            Price = p.Price
        }).ToList();

        await Connection.ExecuteAsync(
            @"insert into subscription_plan_prices (id, plan_id, external_id, interval, price) values (@Id, @PlanId, @ExternalId, @Interval, @Price);",
            prices
        );
    }

    public async Task Delete(SubscriptionPlan entity) {
        await Connection.ExecuteAsync(
            @"delete from subscription_plan_prices where plan_id = @Id;",
            entity
        );

        await Connection.ExecuteAsync(
            @"delete from subscription_plans where id = @Id;",
            entity
        );
    }

}