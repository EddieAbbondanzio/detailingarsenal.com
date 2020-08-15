using System.Collections.Generic;

namespace DetailingArsenal.Domain.Settings {
    public class ServiceUpdate : IDataTransferObject {
        public string Name { get; }
        public string? Description { get; }
        public ServicePricingMethod PricingMethod { get; }
        public List<ServiceConfigurationUpdate> Configurations { get; }

        public ServiceUpdate(string name, string? description, ServicePricingMethod pricingMethod, List<ServiceConfigurationUpdate> configurations) {
            Name = name;
            Description = description;
            PricingMethod = pricingMethod;
            Configurations = configurations;
        }
    }
}