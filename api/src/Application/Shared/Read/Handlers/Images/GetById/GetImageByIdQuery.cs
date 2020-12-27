using System;

namespace DetailingArsenal.Application.Shared {
    public record GetImageByIdQuery(Guid Id) : IAction;
}