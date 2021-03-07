using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;

namespace DetailingArsenal.Persistence.Scheduling.Billing {
    [DependencyInjection(RegisterAs = typeof(ISubscriptionPlanRepo))]
    public class SubscriptionPlanRepo : DatabaseInteractor, ISubscriptionPlanRepo {
        IBillingConfig billingConfig;

        public SubscriptionPlanRepo(IDatabase database, IBillingConfig billingConfig) : base(database) {
            this.billingConfig = billingConfig;
        }



        public async Task<SubscriptionPlan?> FindById(Guid id) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
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

                    var sp = reader.Read<SubscriptionPlanRow, BillingReferenceRow, SubscriptionPlan>(
                    (sp, br) => new SubscriptionPlan(
                        sp.Id,
                        sp.Name,
                        sp.Description,
                        BillingReference.Product(br.BillingId),
                        roleId: sp.RoleId
                    )).SingleOrDefault();

                    if (sp == null) {
                        return null;
                    }

                    sp.Prices = reader.Read<SubscriptionPlanPriceRow, BillingReferenceRow, SubscriptionPlanPrice>(
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
        }


        public async Task<SubscriptionPlan?> FindByName(string name) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                    @"select sp.*, br.* from subscription_plans sp
                    left join billing_references br on sp.billing_reference_id = br.id
                    where sp.name = @Name;
                    
                  select spp.*, br.* from subscription_plan_prices spp
                    left join billing_references br on spp.billing_reference_id = br.id
                    left join subscription_plan sp on spp.plan_id = sp.id
                    where sp.name = @Name",
                    new {
                        Name = name
                    }
                )) {

                    var sp = reader.Read<SubscriptionPlanRow, BillingReferenceRow, SubscriptionPlan>(
                    (sp, br) => new SubscriptionPlan(
                        sp.Id,
                        sp.Name,
                        sp.Description,
                        BillingReference.Product(br.BillingId),
                        roleId: sp.RoleId
                    )).SingleOrDefault();

                    if (sp == null) {
                        return null;
                    }

                    sp.Prices = reader.Read<SubscriptionPlanPriceRow, BillingReferenceRow, SubscriptionPlanPrice>(
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
        }

        public async Task<SubscriptionPlan?> FindByBillingReference(BillingReference reference) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                    @"select sp.*, br.* from subscription_plans sp
                    join billing_references br on sp.billing_reference_id = br.id
                    where br.billing_id = @BillingId;

                  select spp.*, br1.* from subscription_plan_prices spp
                    join subscription_plans sp on spp.plan_id = sp.id
                    join billing_references br1 on spp.billing_reference_id = br1.id
                    join billing_references br2 on sp.billing_reference_id = br2.id
                    where br2.billing_id = @BillingId;        
                ",
                    reference)
                ) {

                    var sp = reader.Read<SubscriptionPlanRow, BillingReferenceRow, SubscriptionPlan>(
                    (sp, br) => new SubscriptionPlan(
                        sp.Id,
                        sp.Name,
                        sp.Description,
                        BillingReference.Product(br.BillingId),
                        roleId: sp.RoleId
                    )).SingleOrDefault();

                    if (sp == null) {
                        return null;
                    }

                    sp.Prices = reader.Read<SubscriptionPlanPriceRow, BillingReferenceRow, SubscriptionPlanPrice>(
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
        }

        public async Task<List<SubscriptionPlan>> FindAll() {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                    @"select sp.*, br.* from subscription_plans sp
                    join billing_references br on sp.billing_reference_id = br.id;

                  select spp.*, br2.* from subscription_plan_prices spp
                    join subscription_plans sp on spp.plan_id = sp.id
                    join billing_references br2 on sp.billing_reference_id = br2.id;
                ")
                ) {

                    var plans = reader.Read<SubscriptionPlanRow, BillingReferenceRow, SubscriptionPlan>(
                        (sp, br) => new SubscriptionPlan(
                            sp.Id,
                            sp.Name,
                            sp.Description,
                            BillingReference.Product(br.BillingId),
                            roleId: sp.RoleId
                        )
                    );

                    var planDict = new Dictionary<Guid, SubscriptionPlan>(plans.Select(p => KeyValuePair.Create(p.Id, p)));

                    var prices = reader.Read<SubscriptionPlanPriceRow, BillingReferenceRow, SubscriptionPlanPrice>(
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
        }

        public async Task Add(SubscriptionPlan entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    // insert plan billing ref first
                    var planBillingRef = new BillingReferenceRow() {
                        Id = Guid.NewGuid(),
                        BillingId = entity.BillingReference.BillingId,
                        Type = entity.BillingReference.Type
                    };

                    await conn.ExecuteAsync(
                        @"insert into billing_references (id, billing_id, type) values (@Id, @BillingId, @Type);",
                        planBillingRef,
                        t
                    );

                    // insert plan
                    await conn.ExecuteAsync(
                        @"insert into subscription_plans (id, name, description, role_id, billing_reference_id) values (@Id, @Name, @Description, @RoleId, @BillingReferenceId);",
                        new SubscriptionPlanRow() {
                            Id = entity.Id,
                            Name = entity.Name,
                            Description = entity.Description,
                            BillingReferenceId = planBillingRef.Id,
                            RoleId = entity.RoleId
                        },
                        t
                    );

                    var prices = new List<SubscriptionPlanPriceRow>();
                    var billingReferences = new List<BillingReferenceRow>();

                    foreach (var p in entity.Prices) {
                        var billingRefModel = new BillingReferenceRow() {
                            Id = Guid.NewGuid(),
                            BillingId = p.BillingReference.BillingId,
                            Type = p.BillingReference.Type
                        };

                        var priceModel = new SubscriptionPlanPriceRow() {
                            Id = Guid.NewGuid(),
                            Interval = p.Interval,
                            Price = p.Amount,
                            PlanId = entity.Id,
                            BillingReferenceId = billingRefModel.Id
                        };

                        prices.Add(priceModel);
                        billingReferences.Add(billingRefModel);
                    }


                    await conn.ExecuteAsync(
                        @"insert into billing_references (id, billing_id, type) values (@Id, @BillingId, @Type);",
                        billingReferences,
                        t
                    );

                    // insert price s
                    await conn.ExecuteAsync(
                        @"insert into subscription_plan_prices (id, plan_id, billing_reference_id, interval, price) values (@Id, @PlanId, @BillingReferenceId, @Interval, @Price);",
                        prices,
                        t
                    );

                    t.Commit();
                }
            }
        }

        public async Task Update(SubscriptionPlan entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"update subscription_plans set name = @Name, description = @Description, role_id = @RoleId where id = @Id;",
                        new SubscriptionPlanRow {
                            Id = entity.Id,
                            Name = entity.Name,
                            Description = entity.Description,
                            RoleId = entity.RoleId
                        }
                    );

                    // TODO: Implement update for prices
                    Console.WriteLine("SubscriptionPlanRepo.Update(): Hey bud, you never finished implementing this function.");

                    t.Commit();
                }
            }
        }

        public async Task Delete(SubscriptionPlan entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"delete from subscription_plan_prices where plan_id = @Id;",
                        entity,
                        t
                    );

                    await conn.ExecuteAsync(
                        @"delete from subscription_plans where id = @Id;",
                        entity,
                        t
                    );

                    await conn.ExecuteAsync(
                        @"delete from billing_references where billing_id = @BillingId;",
                        entity.BillingReference,
                        t
                    );

                    await conn.ExecuteAsync(
                        @"delete from billing_references where billing_id = @BillingId;",
                        entity.Prices.Select(p => p.BillingReference).ToList(),
                        t
                    );
                    t.Commit();
                }
            }
        }

        public async Task DeleteAll() {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"delete from subscription_plan_prices;"
                    );

                    await conn.ExecuteAsync(
                        @"delete from subscription_plans;"
                    );

                    await conn.ExecuteAsync(
                        @"delete from billing_references where type in (0, 1);"
                    );

                    await conn.ExecuteAsync(
                        @"delete from billing_references;"
                    );

                    t.Commit();
                }
            }
        }

        public async Task<SubscriptionPlan> FindDefault() {
            var p = await FindByBillingReference(BillingReference.Product(billingConfig.DefaultPlan));
            return p ?? throw new EntityNotFoundException();
        }
    }
}