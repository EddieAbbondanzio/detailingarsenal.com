using System;

namespace DetailingArsenal.Application.Settings {
    public class DeleteServiceCommand : IAction {
        public Guid Id { get; set; }
    }
}