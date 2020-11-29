using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Scheduling.Billing {
    public record SubscriptionPlanReadModel : IDataTransferObject {
        public Guid Id { get; }
        public string Name { get; }
        public string? Description { get; }
        public Guid? RoleId { get; }
        public List<SubscriptionPlanPriceReadModel> Prices { get; }

        public SubscriptionPlanReadModel(Guid id, string name, string? description, Guid? roleId, List<SubscriptionPlanPriceReadModel>? prices = null) {
            Id = id;
            Name = name;
            Description = description;
            RoleId = roleId;
            Prices = prices ?? new();
        }
    }
}