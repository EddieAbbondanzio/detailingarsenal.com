using System;

namespace DetailingArsenal.Application {
    public class EmployeeDto : IDataTransferObject {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Position { get; set; }
    }
}