using System.Collections.Generic;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;

namespace DetailingArsenal.Application.Settings {
    public class CreateServiceCommand : IAction {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public ServicePricingMethod PricingMethod { get; set; }
        public List<ServiceConfigurationView> Configurations { get; set; } = new List<ServiceConfigurationView>();
    }
}