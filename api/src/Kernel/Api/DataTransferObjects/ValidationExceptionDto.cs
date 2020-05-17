using System.Collections.Generic;

public class ValidationExceptionDto : ExceptionDto {
    public override string Type => "validation";
    public bool Valid { get; set; }
    public IList<ValidationFailureDto> Errors { get; set; } = null!;
}