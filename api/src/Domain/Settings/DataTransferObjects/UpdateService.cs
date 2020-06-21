using System.Collections.Generic;

namespace DetailingArsenal.Domain.Settings {
    public class UpdateService : IDataTransferObject {
        public string Name { get; }
        public string? Description { get; }
        public ServicePricingMethod PricingMethod { get; }
        public List<UpdateServiceConfiguration> Configurations { get; }

        public UpdateService(string name, string? description, ServicePricingMethod pricingMethod, List<UpdateServiceConfiguration> configurations) {
            Name = name;
            Description = description;
            PricingMethod = pricingMethod;
            Configurations = configurations;
        }
    }
}