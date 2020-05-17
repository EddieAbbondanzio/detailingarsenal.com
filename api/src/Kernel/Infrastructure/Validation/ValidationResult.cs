using System.Collections.Generic;

public sealed class ValidationResult {
    #region Properties
    /// <summary>
    /// If the validation completed successfully.
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    /// Validation failures that occured if any.
    /// </summary>
    public IList<ValidationFailure> Errors { get; }

    public ValidationFailure First => Errors[0];
    #endregion

    #region Constructor(s)
    public ValidationResult() {
        IsValid = true;
        Errors = new List<ValidationFailure>();
    }

    public ValidationResult(IList<ValidationFailure> failures) {
        IsValid = false;
        Errors = failures;
    }
    #endregion

    #region Statics
    /// <summary>
    /// Create a new successful validation result with no errors.
    /// </summary>
    public static ValidationResult Success() => new ValidationResult();

    /// <summary>
    /// Create a new validation result that failed.
    /// </summary>
    /// <param name="failures">The errors of why it failed.</param>
    public static ValidationResult Fail(IList<ValidationFailure> failures) => new ValidationResult(failures);
    #endregion
}