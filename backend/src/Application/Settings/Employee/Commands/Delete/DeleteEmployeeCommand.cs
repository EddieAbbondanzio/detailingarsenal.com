using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class DeleteEmployeeCommand : IAction {
        public Guid Id { get; set; } = Guid.Empty;
    }
}