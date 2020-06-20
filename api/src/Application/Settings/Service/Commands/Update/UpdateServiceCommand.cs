using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;

namespace DetailingArsenal.Application.Settings {
    public class UpdateServiceCommand : IAction {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public ServicePricingMethod PricingMethod { get; set; }
        public List<ServiceConfigurationDto> Configurations { get; set; } = new List<ServiceConfigurationDto>();
    }
}