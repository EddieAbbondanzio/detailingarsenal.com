using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;

namespace DetailingArsenal.Persistence.Billing {
    public class SubscriptionPlanRepo : DatabaseInteractor, ISubscriptionPlanRepo {
        public SubscriptionPlanRepo(IDatabase database) : base(database) { }

        public async Task<SubscriptionPlan?> FindById(Guid id) {
            using (var reader = await Connection.QueryMultipleAsync(
                @"select sp.*, br.* from subscription_plans sp
                    left join billing_references br on sp.billing_reference_id = br.id
                    where sp.id = @Id;
                    
                  select spp.*, br.* from subscription_plan_prices spp
                    left join billing_references br on spp.billing_reference_id = br.id
                    where spp.plan_id = @Id;",
                new {
                    Id = id
                }
            )) {

                var sp = reader.Read<SubscriptionPlanModel, BillingReferenceModel, SubscriptionPlan>(
                (sp, br) => new SubscriptionPlan() {
                    Id = sp.Id,
                    Name = sp.Name,
                    RoleId = sp.RoleId,
                    BillingReference = new BillingReference(
                            br.BillingId, br.Type
                        )
                }
                ).SingleOrDefault();

                if (sp == null) {
                    return null;
                }

                sp.Prices = reader.Read<SubscriptionPlanPriceModel, BillingReferenceModel, SubscriptionPlanPrice>(
                    (spp, br) => new SubscriptionPlanPrice(
                            spp.Price,
                            spp.Interval,
                            new BillingReference(
                                br.BillingId,
                                br.Type
                            )
                        )
                ).ToList();

                return sp;
            }
        }

        public async Task<SubscriptionPlan?> FindByBillingReference(BillingReference reference) {
            using (var reader = await Connection.QueryMultipleAsync(
                @"select sp.* from subscription_plans sp
                    join billing_references br on sp.billing_reference_id = br.id
                    where br.billing_id = @BillingId;

                  select spp.* from subscription_plan_prices spp
                    join subscription_plans sp on spp.plan_id = sp.id
                    join billing_references br2 on sp.billing_reference_id = br2.id
                    where br2.billing_id = @BillingId;        
                ",
                reference)
            ) {

                var sp = reader.Read<SubscriptionPlanModel, BillingReferenceModel, SubscriptionPlan>(
                (sp, br) => new SubscriptionPlan() {
                    Id = sp.Id,
                    Name = sp.Name,
                    RoleId = sp.RoleId,
                    BillingReference = new BillingReference(
                            br.BillingId, br.Type
                        )
                }
                ).SingleOrDefault();

                if (sp == null) {
                    return null;
                }

                sp.Prices = reader.Read<SubscriptionPlanPriceModel, BillingReferenceModel, SubscriptionPlanPrice>(
                    (spp, br) => new SubscriptionPlanPrice(
                            spp.Price,
                            spp.Interval,
                            new BillingReference(
                                br.BillingId,
                                br.Type
                            )
                        )
                ).ToList();

                return sp;
            }
        }

        public async Task<List<SubscriptionPlan>> FindAll() {
            using (var reader = await Connection.QueryMultipleAsync(
                @"select sp.* from subscription_plans sp
                    join billing_references br on sp.billing_reference_id = br.id;

                  select spp.* from subscription_plan_prices spp
                    join subscription_plans sp on spp.plan_id = sp.id
                    join billing_references br2 on sp.billing_reference_id = br2.id;
                ")
            ) {

                var plans = reader.Read<SubscriptionPlanModel, BillingReferenceModel, SubscriptionPlan>(
                    (sp, br) => new SubscriptionPlan() {
                        Id = sp.Id,
                        Name = sp.Name,
                        RoleId = sp.RoleId,
                        BillingReference = new BillingReference(
                                br.BillingId, br.Type
                            )
                    }
                );

                var planDict = new Dictionary<Guid, SubscriptionPlan>(plans.Select(p => KeyValuePair.Create(p.Id, p)));

                var prices = reader.Read<SubscriptionPlanPriceModel, BillingReferenceModel, SubscriptionPlanPrice>(
                    (spp, br) => {
                        var price = new SubscriptionPlanPrice(
                            spp.Price,
                            spp.Interval,
                            new BillingReference(
                                br.BillingId,
                                br.Type
                            )
                        );

                        planDict[spp.PlanId].Prices.Add(price);

                        return price;
                    }
                );

                return plans.ToList();
            }
        }

        public async Task Add(SubscriptionPlan entity) {
            using (var t = Connection.BeginTransaction()) {
                // insert plan billing ref first
                var planBillingRef = new BillingReferenceModel() {
                    Id = Guid.NewGuid(),
                    BillingId = entity.BillingReference.BillingId,
                    Type = entity.BillingReference.Type
                };

                await Connection.ExecuteAsync(
                    @"insert into billing_references (id, billing_id, type) values (@Id, @BillingId, @Type);",
                    planBillingRef,
                    t
                );

                // insert plan
                await Connection.ExecuteAsync(
                    @"insert into subscription_plans (id, name, billing_reference_id, role_id) values (@Id, @Name, @BillingReferenceId, @RoleId);",
                    new SubscriptionPlanModel() {
                        Id = entity.Id,
                        Name = entity.Name,
                        BillingReferenceId = planBillingRef.Id,
                        RoleId = entity.RoleId
                    },
                    t
                );

                var prices = new List<SubscriptionPlanPriceModel>();
                var billingReferences = new List<BillingReferenceModel>();

                foreach (var p in entity.Prices) {
                    var billingRefModel = new BillingReferenceModel() {
                        Id = Guid.NewGuid(),
                        BillingId = p.BillingReference.BillingId,
                        Type = p.BillingReference.Type
                    };

                    var priceModel = new SubscriptionPlanPriceModel() {
                        Id = Guid.NewGuid(),
                        Interval = p.Interval,
                        Price = p.Price,
                        PlanId = entity.Id,
                        BillingReferenceId = billingRefModel.Id
                    };

                    prices.Add(priceModel);
                    billingReferences.Add(billingRefModel);
                }


                await Connection.ExecuteAsync(
                    @"insert into billing_references (id, billing_id, type) values (@Id, @BillingId, @Type);",
                    billingReferences,
                    t
                );

                // insert price s
                await Connection.ExecuteAsync(
                    @"insert into subscription_plan_prices (id, plan_id, billing_reference_id, interval, price) values (@Id, @PlanId, @BillingReferenceId, @Interval, @Price);",
                    prices,
                    t
                );

                t.Commit();
            }
        }

        public async Task Update(SubscriptionPlan entity) {
            using (var t = Connection.BeginTransaction()) {
                await Connection.ExecuteAsync(
                    @"update subscription_plans set name = @Name, role_id = @RoleId where id = @Id;",
                    new SubscriptionPlanModel {
                        Id = entity.Id,
                        Name = entity.Name,
                        RoleId = entity.RoleId == Guid.Empty ? (Nullable<Guid>)null : entity.RoleId
                    }
                );

                // TODO: Implement update for prices

                t.Commit();
            }
        }

        public async Task Delete(SubscriptionPlan entity) {
            using (var t = Connection.BeginTransaction()) {
                await Connection.ExecuteAsync(
                    @"delete from subscription_plan_prices where plan_id = @Id;",
                    entity,
                    t
                );

                await Connection.ExecuteAsync(
                    @"delete from subscription_plans where id = @Id;",
                    entity,
                    t
                );

                await Connection.ExecuteAsync(
                    @"delete from billing_references where billing_id = @BillingId;",
                    entity.BillingReference,
                    t
                );

                await Connection.ExecuteAsync(
                    @"delete from billing_references where billing_id = @BillingId;",
                    entity.Prices.Select(p => p.BillingReference).ToList(),
                    t
                );
                t.Commit();
            }
        }

        public async Task DeleteAll() {
            using (var t = Connection.BeginTransaction()) {
                await Connection.ExecuteAsync(
                    @"delete from subscription_plan_prices;"
                );

                await Connection.ExecuteAsync(
                    @"delete from subscription_plans;"
                );

                await Connection.ExecuteAsync(
                    @"delete from billing_references where type in (0, 1);"
                );

                await Connection.ExecuteAsync(
                    @"delete from billing_references;"
                );

                t.Commit();
            }
        }
    }
}