using System.Collections.Generic;

public class SpecifcationExceptionDto : ExceptionDto {
    public override string Type => "specification";
    public bool IsSatisfied { get; set; }
}