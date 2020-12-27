using System;

namespace DetailingArsenal.Application.Shared {
    public record GetThumbnailByIdQuery(Guid Id) : IAction;
}