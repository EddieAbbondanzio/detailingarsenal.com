using System.Collections.Generic;

namespace DetailingArsenal.Application {
    public class SpecifcationExceptionDto : ExceptionDto {
        public override string Type => "specification";
        public bool IsSatisfied { get; set; }
    }
}