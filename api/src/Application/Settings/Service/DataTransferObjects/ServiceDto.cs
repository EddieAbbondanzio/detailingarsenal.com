using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class ServiceDto : IDataTransferObject {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public ServicePricingMethod PricingMethod { get; set; }
        public IEnumerable<ServiceConfigurationDto> Configurations { get; set; } = null!;
    }
}