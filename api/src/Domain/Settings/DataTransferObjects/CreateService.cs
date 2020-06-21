using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Settings {
    public class CreateService : IDataTransferObject {
        public Guid UserId { get; }
        public string Name { get; }
        public string? Description { get; }
        public ServicePricingMethod PricingMethod { get; }
        public List<CreateServiceConfiguration> Configurations { get; }

        public CreateService(Guid userId, string name, string? description, ServicePricingMethod pricingMethod, List<CreateServiceConfiguration> configurations) {
            UserId = userId;
            Name = name;
            Description = description;
            PricingMethod = pricingMethod;
            Configurations = configurations;
        }
    }
}