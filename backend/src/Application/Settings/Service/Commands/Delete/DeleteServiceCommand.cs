using System;

namespace DetailingArsenal.Application {
    public class DeleteServiceCommand : IAction {
        public Guid Id { get; set; }
    }
}