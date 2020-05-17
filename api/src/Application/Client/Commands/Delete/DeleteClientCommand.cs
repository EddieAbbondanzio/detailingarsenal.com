using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class DeleteClientCommand : IAction {
        public Guid Id { get; set; }
    }
}