using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Application.Scheduling.Billing;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;

namespace DetailingArsenal.Persistence.Scheduling.Billing {
    [DependencyInjection(RegisterAs = typeof(ISubscriptionPlanReader))]
    public class SubscriptionPlanReader : DatabaseInteractor, ISubscriptionPlanReader {
        IBillingConfig config;

        public SubscriptionPlanReader(IDatabase database, IBillingConfig config) : base(database) {
            this.config = config;
        }

        public async Task<List<SubscriptionPlanReadModel>> ReadAll() {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                    @"select sp.* from subscription_plans sp;

                  select spp.*, br2.* from subscription_plan_prices spp
                    join subscription_plans sp on spp.plan_id = sp.id
                    join billing_references br2 on sp.billing_reference_id = br2.id;
                "
                )) {
                    var plans = reader.Read<SubscriptionPlanRow>().Select(
                        (sp, br) => new SubscriptionPlanReadModel(
                            sp.Id,
                            sp.Name,
                            sp.Description,
                            sp.RoleId
                        // No billing reference since we don't send it over to the client
                        )
                    );

                    var planDict = new Dictionary<Guid, SubscriptionPlanReadModel>(plans.Select(p => KeyValuePair.Create(p.Id, p)));
                    var prices = reader.Read<SubscriptionPlanPriceRow, BillingReferenceRow, SubscriptionPlanPriceReadModel>(
                                    (spp, br) => {
                                        var price = new SubscriptionPlanPriceReadModel(
                                            spp.Price,
                                            spp.Interval,
                                                br.BillingId
                                        );

                                        planDict[spp.PlanId].Prices.Add(price);

                                        return price;
                                    }
                                );

                    return planDict.Values.ToList();
                }
            }
        }

        public async Task<SubscriptionPlanReadModel?> ReadById(Guid id) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                    @"select sp.* from subscription_plans sp 
                        where sp.id = @Id;
                        
                    select spp.*, br1.* from subscription_plan_prices spp
                        join subscription_plans sp on spp.plan_id = sp.id
                        join billing_references br1 on spp.billing_reference_id = br1.id
                        where sp.id = @Id;",
                        new { Id = id }
                )) {
                    var raw = reader.ReadFirst<SubscriptionPlanRow>();
                    var plan = new SubscriptionPlanReadModel(
                        raw.Id, raw.Name, raw.Description, raw.RoleId,
                        reader.Read<SubscriptionPlanPriceRow, BillingReferenceRow, SubscriptionPlanPriceReadModel>(
                        (spp, br) => new SubscriptionPlanPriceReadModel(
                                spp.Price,
                                spp.Interval,
                                br.BillingId
                            )
                            ).ToList()
                    );

                    return plan;
                }
            }
        }

        public async Task<SubscriptionPlanReadModel> ReadDefault() {
            var param = new { BillingId = config.DefaultPlan };

            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                    @"select sp.* from subscription_plans sp 
                        join billing_references br on sp.billing_reference_id = br.id
                        where br.billing_id = @BillingId;
                        
                    select spp.*, br1.* from subscription_plan_prices spp
                        join subscription_plans sp on spp.plan_id = sp.id
                        join billing_references br1 on spp.billing_reference_id = br1.id
                        join billing_references br2 on sp.billing_reference_id = br2.id
                        where br2.billing_id = @BillingId;",
                        param
                )) {
                    var raw = reader.ReadFirst<SubscriptionPlanRow>();
                    var plan = new SubscriptionPlanReadModel(
                        raw.Id, raw.Name, raw.Description, raw.RoleId,
                        reader.Read<SubscriptionPlanPriceRow, BillingReferenceRow, SubscriptionPlanPriceReadModel>(
                        (spp, br) => new SubscriptionPlanPriceReadModel(
                                spp.Price,
                                spp.Interval,
                                br.BillingId
                            )
                            ).ToList()
                    );

                    return plan;
                }
            }
        }
    }
}