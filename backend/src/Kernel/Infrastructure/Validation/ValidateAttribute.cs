using System;

/// <summary>
/// Attribute to auto validate an interactor via a validator.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public sealed class ValidateAttribute : System.Attribute {
    #region Properties
    /// <summary>
    /// The type of validator that should be used.
    /// </summary>
    /// <value></value>
    public Type Validator { get; }
    #endregion

    #region Constructor(s)
    public ValidateAttribute(Type validatorType) {
        Validator = validatorType;

        if (!typeof(IValidator).IsAssignableFrom(validatorType)) {
            throw new ArgumentException("Validator type must implement IValidator");
        }
    }
    #endregion
}