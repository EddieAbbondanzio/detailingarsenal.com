using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Settings {
    public class BusinessDto : IDataTransferObject {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}