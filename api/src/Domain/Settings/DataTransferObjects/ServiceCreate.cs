using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Settings {
    public class ServiceCreate : IDataTransferObject {
        public Guid UserId { get; }
        public string Name { get; }
        public string? Description { get; }
        public ServicePricingMethod PricingMethod { get; }
        public List<ServiceConfigurationCreate> Configurations { get; }

        public ServiceCreate(Guid userId, string name, string? description, ServicePricingMethod pricingMethod, List<ServiceConfigurationCreate> configurations) {
            UserId = userId;
            Name = name;
            Description = description;
            PricingMethod = pricingMethod;
            Configurations = configurations;
        }
    }
}