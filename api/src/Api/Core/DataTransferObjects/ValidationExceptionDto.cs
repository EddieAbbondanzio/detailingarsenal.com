using System.Collections.Generic;
using DetailingArsenal.Application;

namespace DetailingArsenal.Api {
    public class ValidationExceptionDto : ExceptionDto {
        public override string Type => "validation";
        public bool Valid { get; set; }
        public IList<ValidationFailureDto> Errors { get; set; } = null!;
    }
}