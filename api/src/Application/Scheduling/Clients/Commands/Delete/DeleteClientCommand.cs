using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Clients {
    public class DeleteClientCommand : IAction {
        public Guid Id { get; set; }
    }
}