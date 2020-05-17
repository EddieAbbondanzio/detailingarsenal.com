using System.Collections.Generic;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class CreateServiceCommand : IAction {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public ServicePricingMethod PricingMethod { get; set; }
        public List<ServiceConfigurationDto> Configurations { get; set; } = new List<ServiceConfigurationDto>();
    }
}